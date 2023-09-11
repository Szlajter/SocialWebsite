using System.Security.AccessControl;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class MessageHub: Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<StatusHub> _statusHub;

        public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository,
            IMapper mapper, IHubContext<StatusHub> statusHub)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _statusHub = statusHub;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"];
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            //signalR group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //group tracker
            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _messageRepository.GetConversation(Context.User.GetUsername(), otherUser);

            await Clients.Caller.SendAsync("ReceiveConversation", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(MessageCreateDto messageCreateDto)
        {
            var username = Context.User.GetUsername();

            if (username == messageCreateDto.RecipientUsername.ToLower())
            {
                throw new HubException("You cannot send a message to yourself.");
            }

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(messageCreateDto.RecipientUsername);
 
            if (recipient == null) 
            {
                throw new HubException("User not found");
            }

            var message = new Message
            {
                Sender = sender,
                SenderId = sender.Id,
                SenderUsername = sender.UserName,
                Recipient = recipient,
                RecipientId = recipient.Id,
                RecipientUsername = recipient.UserName,
                Content = messageCreateDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _messageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = StatusTracker.GetUserConnections(recipient.UserName);
                if (connections != null)
                {
                    await _statusHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new {username = sender.UserName, nickname = sender.NickName});
                }
            }

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        public async Task DeleteMessage(int id)
        {
            var username = Context.User.GetUsername();

            var message = await _messageRepository.GetMessage(id);

            if(message == null)
            {
                throw new HubException("Message Not Found");
            }
            
            if(message.SenderUsername != username && message.RecipientUsername != username)
            {
                throw new HubException("Unauthorized");
            }
            
            if(message.SenderUsername == username) message.SenderDeleted = true;
            if(message.RecipientUsername == username) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
            {
                _messageRepository.DeleteMessage(message);
            }

            if(await _messageRepository.SaveAllAsync())
            {
                await Clients.Caller.SendAsync("MessageDeleted",  _mapper.Map<MessageDto>(message));
            }
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if(group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _messageRepository.SaveAllAsync()) 
            {
                return group;
            }

            throw new HubException("Failed to add to the group");

        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageRepository.GetGroupByConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _messageRepository.RemoveConnection(connection);

            if (await _messageRepository.SaveAllAsync())
            {
                return group;
            }

            throw new HubException("Failed to remove from the group");
        }
    }
}