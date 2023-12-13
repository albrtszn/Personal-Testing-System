using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class MessageService
    {
        private IMessageRepo MessageRepo;
        public MessageService(IMessageRepo _MessageRepo)
        {
            this.MessageRepo = _MessageRepo;
        }
        private MessageDto ConvertToMessageDto(Message Message)
        {
            return new MessageDto
            {
                Id = Message.Id,
                IdEmployee = Message.IdEmployee,
                MessageText = Message.MessageText,
                StatusRead = Message.StatusRead,
                DateAndTime = Message.DateAndTie.ToString()
            };
        }
        private Message ConvertToMessage(MessageDto MessageDto)
        {
            return new Message
            {
                Id = MessageDto.Id.Value,
                IdEmployee = MessageDto.IdEmployee,
                MessageText = MessageDto.MessageText,
                StatusRead = MessageDto.StatusRead.Value,
                DateAndTie = DateTime.Parse(MessageDto.DateAndTime)
            };
        }
        public async Task<bool> DeleteMessageById(int id)
        {
            return await MessageRepo.DeleteMessageById(id);
        }

        public async Task<bool> DeleteMessagesByEmployee(string idEmployee)
        {
            List<Message> list = (await GetAllMessages()).Where(x => x!=null && x.IdEmployee != null && x.IdEmployee.Equals(idEmployee)).ToList();
            foreach (Message Message in list)
            {
                await DeleteMessageById(Message.Id);
            }
            return true;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            return await MessageRepo.GetAllMessages();
        }

        public async Task<List<Message>> GetMessagesByEmployee(string idEmployee)
        {
            return (await GetAllMessages()).Where(x => x != null && x.IdEmployee != null && x.IdEmployee.Equals(idEmployee)).ToList();
        }

        public async Task<List<MessageDto>> GetAllMessageDtos()
        {
            List<MessageDto> Messages = new List<MessageDto>();
            List<Message> list = await MessageRepo.GetAllMessages();
            list.ForEach(x => Messages.Add(ConvertToMessageDto(x)));
            return Messages;
        }

        public async Task<List<MessageDto>> GetMessageDtosByEmployee(string idEmployee)
        {
            return (await GetAllMessageDtos()).Where(x => x.IdEmployee.Equals(idEmployee)).ToList();
        }

        public async Task<Message> GetMessageById(int id)
        {
            return await MessageRepo.GetMessageById(id);
        }

        public async Task<MessageDto> GetMessageDtoById(int id)
        {
            return ConvertToMessageDto(await MessageRepo.GetMessageById(id));
        }

        public async Task<bool> ChangeMessageStatus(int idMessage)
        {
            var message = await MessageRepo.GetMessageById(idMessage);
            message.StatusRead = !message.StatusRead;
            return await MessageRepo.SaveMessage(message);
        }

        public async Task<bool> SaveMessage(Message MessageToSave)
        {
            return await MessageRepo.SaveMessage(MessageToSave);
        }

        public async Task<bool> SaveMessage(AddMessageModel MessageToSave, string idEmployee)
        {
            return await MessageRepo.SaveMessage(new Message()
            {
                IdEmployee = idEmployee,
                MessageText = MessageToSave.MessageText,
                StatusRead = false,
                DateAndTie = DateTime.Now
            });
        }

        public async Task<bool> SaveMessage(MessageDto MessageToSave)
        {
            return await MessageRepo.SaveMessage(new Message()
            {
                Id = MessageToSave.Id.Value,
                IdEmployee = MessageToSave.IdEmployee,
                MessageText = MessageToSave.MessageText,
                StatusRead = MessageToSave.StatusRead.Value,
                DateAndTie = DateTime.Parse(MessageToSave.DateAndTime)
            });
        }
    }
}
