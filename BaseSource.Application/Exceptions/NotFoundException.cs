using BaseSource.Application.Wrappers;


namespace BaseSource.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key) : base($"{name} ({key}) is not found")
        {
            Data.Add("Message", new List<Message> { new Message("ResourceNotFound", $"{name} ({key}) is not found") });
        }
        public NotFoundException(Message message) : base(message.Code + "-" + message.Text)
        {
            Data.Add("Message", new List<Message> { message });
        }
    }
}
