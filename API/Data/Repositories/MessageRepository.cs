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
             var query = _context.Messages
                .Where(
                    m => m.RecipientUsername == currentUserName &&
                    m.SenderUsername == recipientUsername &&
                    m.RecipientDeleted == false ||
                    m.RecipientUsername == recipientUsername &&
                    m.SenderUsername == currentUserName &&
                    m.SenderDeleted == false
                )
                .OrderBy(m => m.MessageSent)
                .AsQueryable();

            var unreadMessages = query.Where(m => m.DateRead == null 
                && m.RecipientUsername == currentUserName).ToList();
            
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
            }

            return await query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();
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

        //returns a list of the latest message in each conversation
        //I should refactor it later. Make it so it returns some smaller dto
        //groupBy and Select must be used by client. Trying to do it async caused a lot of issues
        public async Task<IEnumerable<MessageDto>> GetRecentConversations(string username)
        {
            var messages = await _context.Messages
                .Where(m => m.RecipientUsername == username && m.RecipientDeleted == false || 
                        m.SenderUsername == username && m.SenderDeleted == false)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var recentMessages = messages    
                .GroupBy(m => m.RecipientUsername == username ? m.SenderUsername : m.RecipientUsername)
                .Select(g => g.OrderByDescending(m => m.MessageSent).First())
                .ToList();

            return recentMessages; 
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups.Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<Group> GetGroupByConnection(string connectionId)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }
    }
}