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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

         public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(MessageCreateDto messageCreate)
        {
            var username = User.GetUsername();

            if (username == messageCreate.RecipientUsername.ToLower())
                return BadRequest("You cannot send a message to yourself.");

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(messageCreate.RecipientUsername);
 
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

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<MessageDto>>> GetMessages([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _unitOfWork.MessageRepository.GetMessages(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(
                messages.PageIndex,
                messages.PageSize, 
                messages.TotalCount, 
                messages.TotalPages));

            return messages;
        }

        //maybe move it to the messageHub
        //but i'm not sure about it
        [HttpGet("conversations")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetRecentConversations()
        {
            var currentUsername = User.GetUsername(); 

            return Ok(await _unitOfWork.MessageRepository.GetRecentConversations(currentUsername));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var message = await _unitOfWork.MessageRepository.GetMessage(id);
            
            if(message.SenderUsername != username && message.RecipientUsername != username)
                return Unauthorized();
            
            if(message.SenderUsername == username) message.SenderDeleted = true;
            if(message.RecipientUsername == username) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
            {
                _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if(await _unitOfWork.Complete()) return Ok();

            return BadRequest("Deleting message has failed");
        }
    }
}