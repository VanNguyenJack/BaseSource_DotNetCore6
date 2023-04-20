using BaseSource.Application.Wrappers;
using BaseSource.Domain.Constants;

namespace BaseSource.Domain.Wrappers
{
    public class PaginatedResult<T> : Result<T>
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public Uri FirstPage { get; set; }

        public Uri LastPage { get; set; }

        public int TotalPages { get; set; }

        public long TotalRecords { get; set; }

        public Uri NextPage { get; set; }

        public Uri PreviousPage { get; set; }

        public bool IsValidate { get; set; }

        public bool IsAddNew { get; set; }

        public PaginatedResult()
        {

        }

        public PaginatedResult(T data)
        {
            Data = data;
        }

        public PaginatedResult(bool succeeded, T data = default, List<Message> messages = null, long records = 0, int pageNumber = 1, int pageSize = Filter_Paramater.PageSize)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Succeeded = succeeded;
            TotalPages = (int)Math.Ceiling(records / (double)pageSize);
            TotalRecords = records;
        }

        public static PaginatedResult<T> Failure(List<Message> messages)
        {
            return new PaginatedResult<T>(false, default, messages);
        }

        public static PaginatedResult<T> Success(T data, long count, int pageNumber, int pageSize)
        {
            return new PaginatedResult<T>(true, data, null, count, pageNumber, pageSize);
        }
    }
}
