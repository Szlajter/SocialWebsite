using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PaginatedList<MessageDto>> GetMessages(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetConversation(string currentUserName, string recipientUsername);
        Task<bool> SaveAllAsync();
    }
}