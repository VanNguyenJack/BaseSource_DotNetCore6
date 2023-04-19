using BaseSource.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BaseSource.Application.Exceptions
{
    public class UnAuthorizeException : ApplicationException
    {
        public UnAuthorizeException() : base()
        {
        }

        public UnAuthorizeException(Message message) : base(message.Code + "-" + message.Text)
        {
            Data.Add("Message", new List<Message> { message });
        }

        public UnAuthorizeException(Message message, params object[] args)
           : base(String.Format(CultureInfo.CurrentCulture, message.Code + "-" + message.Text, args))
        {
            Data.Add("Message", new List<Message> { message });
        }
    }
}
