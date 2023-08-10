using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }
        
        //messages between 2 users  
        public async Task<IEnumerable<MessageDto>> GetConversation(string currentUserName, string recipientUsername)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(
                    m => m.RecipientUsername == currentUserName &&
                    m.SenderUsername == recipientUsername ||
                    m.RecipientUsername == recipientUsername &&
                    m.SenderUsername == currentUserName
                )
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null 
                && m.RecipientUsername == currentUserName).ToList();
            
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                    message.DateRead = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PaginatedList<MessageDto>> GetMessages(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PaginatedList<MessageDto>.CreateAsync(messages, messageParams.PageIndex, messageParams.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        //returns a list of the latest messages in each conversation
        //I should refactor it later. Make it so it returns some smaller dto or something
        public async Task<IEnumerable<MessageDto>> GetRecentConversations(string username)
        {
            var messages = await (from m in _context.Messages
                           .Include(u => u.Sender).ThenInclude(p => p.Photos)
                           .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                           let msgTo = m.RecipientUsername == username
                           let msgFrom = m.SenderUsername == username
                           where msgTo || msgFrom
                           group m by msgTo ? m.SenderUsername : m.RecipientUsername into g
                           select g.OrderByDescending(x => x.MessageSent).First())
                           .ToListAsync();
                          
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }
    }
}