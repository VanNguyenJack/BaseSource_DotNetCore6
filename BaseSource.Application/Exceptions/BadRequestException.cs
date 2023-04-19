using BaseSource.Application.Wrappers;
using System;
using System.Collections.Generic;
namespace BaseSource.Application.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(Message message) : base(message.Code + "-" + message.Text)
        {
            Data.Add("Message", new List<Message> { message });
        }
    }
}
