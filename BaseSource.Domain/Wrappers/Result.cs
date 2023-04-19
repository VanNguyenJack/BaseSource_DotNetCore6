using BaseSource.Domain.Shared;

namespace BaseSource.Application.Wrappers
{
    public class Result : IResult
    {
        public Result()
        {
        }

        public List<Message> Message { get; set; }

        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public static IResult Fail()
        {
            return new Result { Succeeded = false };
        }

        public static IResult Fail(Message message)
        {
            return new Result { Succeeded = false, Message = new List<Message>() { message } };
        }

        public static IResult Fail(List<string> errors)
        {
            return new Result { Succeeded = false, Errors = errors };
        }

        public static Task<IResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<IResult> FailAsync(Message message)
        {
            return Task.FromResult(Fail(message));
        }

        public static IResult Success()
        {
            return new Result { Succeeded = true };
        }

        public static IResult Success(Message message)
        {
            return new Result { Succeeded = true, Message = new List<Message>() { message } };
        }

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(Message message)
        {
            return Task.FromResult(Success(message));
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result()
        {
        }

        public T Data { get; set; }

        public new static Result<T> Fail()
        {
            return new Result<T> { Succeeded = false };
        }

        public new static Result<T> Fail(List<string> errors)
        {
            return new Result<T>
            {
                Succeeded = false,
                Errors = errors
            };
        }


        public new static Result<T> Fail(Message message)
        {
            return new Result<T> { Succeeded = false, Message = new List<Message>() { message } };
        }

        public new static Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public new static Task<Result<T>> FailAsync(Message message)
        {
            return Task.FromResult(Fail(message));
        }

        public new static Result<T> Success()
        {
            return new Result<T> { Succeeded = true };
        }

        public new static Result<T> Success(Message message)
        {
            return new Result<T> { Succeeded = true, Message = new List<Message>() { message } };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }

        public static Result<T> Success(T data, Message message)
        {
            return new Result<T> { Succeeded = true, Data = data, Message = new List<Message>() { message } };
        }
        public static Result<T> Success(T data, List<Message> messages)
        {
            return new Result<T> { Succeeded = true, Data = data, Message = messages };
        }

        public new static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public new static Task<Result<T>> SuccessAsync(Message message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, Message message)
        {
            return Task.FromResult(Success(data, message));
        }
        public static Task<Result<T>> SuccessAsync(T data, List<Message> messages)
        {
            return Task.FromResult(Success(data, messages));
        }

        public static Result<T> Fail(T data, Message message)
        {
            return new Result<T> { Succeeded = false, Data = data, Message = new List<Message>() { message } };
        }
    }
}
