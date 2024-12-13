namespace Sob_Need_Server.DTOs
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int? TotalCount { get; set; } // Optional field for pagination

        public ResponseDto(bool isSuccess, string message, T data, int? totalCount = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            TotalCount = totalCount;
        }
    }
}
