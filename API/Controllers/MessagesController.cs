using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController: BaseApiController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessagesController(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(MessageCreateDto messageCreate)
        {
            var username = User.GetUsername();

            if (username == messageCreate.RecipientUsername.ToLower())
                return BadRequest("You cannot send a message to yourself.");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(messageCreate.RecipientUsername);
 
            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                SenderId = sender.Id,
                SenderUsername = sender.UserName,
                Recipient = recipient,
                RecipientId = recipient.Id,
                RecipientUsername = recipient.UserName,
                Content = messageCreate.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Faield to send message");
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<MessageDto>>> GetMessages([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _messageRepository.GetMessages(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(
                messages.PageIndex,
                messages.PageSize, 
                messages.TotalCount, 
                messages.TotalPages));

            return messages;
        }

        [HttpGet("conversation/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetConversation(string username)
        {
            var currentUsername = User.GetUsername(); 

            return Ok(await _messageRepository.GetConversation(currentUsername, username));
        }

        [HttpGet("conversations")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetRecentConversations()
        {
            var currentUsername = User.GetUsername(); 

            return Ok(await _messageRepository.GetRecentConversations(currentUsername));
        }
    }
}