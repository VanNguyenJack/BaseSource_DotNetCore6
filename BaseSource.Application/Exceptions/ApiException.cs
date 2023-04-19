using System;
using System.Globalization;
using BaseSource.Application.Wrappers;

namespace BaseSource.Application.Exceptions
{
    public class ApiException : ApplicationException
    {
        public ApiException() : base()
        {
        }
        public ApiException(string error) : base(error)
        {
            Data.Add("Message", new List<Message> { new Message("", error) });
        }

        public ApiException(Message message) : base(message.Code + "-" + message.Text)
        {
            Data.Add("Message", new List<Message> { message });
        }

        public ApiException(List<Message> messages) : base(messages.First().Code + "-" + messages.First().Text)
        {
            Data.Add("Message", messages);
        }

        public ApiException(Message message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message.Code + "-" + message.Text, args))
        {
            message.Text = String.Format(CultureInfo.CurrentCulture, message.Text, args);
            Data.Add("Message", new List<Message> { message });
        }
    }
}
