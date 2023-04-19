using System;

namespace BaseSource.Application.Wrappers
{
    public class Message
    {
        public string Code { get; set; }

        public string Text { get; set; }

        public MessageType Type { get; set; }

        public Message(string code, string text, MessageType type = MessageType.Message)
        {
            Code = code;
            Text = text;
            Type = type;
        }
        public Message(string code, Exception e, MessageType type = MessageType.Message)
        {
            Code = code;
            Text = e.InnerException != null ? e.InnerException.Message : e.Message;
            Type = type;
        }
        public Message() { }
    }

    public enum MessageType
    {
        Info = 'I',
        Message = 'M',
        Error = 'E',
        Warning = 'W'
    }
}