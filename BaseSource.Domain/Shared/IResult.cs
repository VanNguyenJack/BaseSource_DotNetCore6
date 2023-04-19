using BaseSource.Application.Wrappers;

namespace BaseSource.Domain.Shared
{
    public interface IResult
    {
        List<Message> Message { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}
