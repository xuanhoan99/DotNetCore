namespace HCore.Application.Modules.Common.Responses
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public string? ErrorCode { get; set; } // Mã lỗi (nếu có)
        public List<string>? Errors { get; set; } // Validation hoặc exception chi tiết
        public int? StatusCode { get; set; } // Dùng để map với HTTP StatusCode (tùy chọn)

        public static BaseResponse<T> Ok(T data, string? message = null)
            => new BaseResponse<T> { Success = true, Data = data, Message = message };

        public static BaseResponse<T> Fail(string message, string? errorCode = null, List<string>? errors = null)
            => new BaseResponse<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                Errors = errors
            };
    }

}
