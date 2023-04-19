using BaseSource.Application.Wrappers;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace BaseSource.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public List<Message> ValidationErrors { get; set; }
        public ValidationException(Message message)
        {
            ValidationErrors = new List<Message>();
            ValidationErrors.Add(message);
            Data.Add("Message", message);

        }

        public ValidationException(ValidationResult validationResult)
        {
            ValidationErrors = new List<Message>();
            foreach (var validationError in validationResult.Errors)
            {
                var message = new Message { Code = validationError.ErrorCode, Text = validationError.ErrorMessage, Type = MessageType.Error };
                ValidationErrors.Add(message);
                Data.Add("Message", message);
            }
        }

        public ValidationException(List<ValidationFailure> errors)
        {
            ValidationErrors = new List<Message>();
            foreach (var validationError in errors)
            {
                var message = new Message { Code = validationError.ErrorCode, Text = validationError.ErrorMessage, Type = MessageType.Error };
                ValidationErrors.Add(message);
            }
            Data.Add("Message", ValidationErrors);
        }

    }
}
