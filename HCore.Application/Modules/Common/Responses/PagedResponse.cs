namespace HCore.Application.Modules.Common.Responses
{
    public class PagedResponse<T> : BaseResponse<List<T>>
    {
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public static PagedResponse<T> Create(List<T> data, int total, int pageIndex, int pageSize)
        {
            return new PagedResponse<T>
            {
                Success = true,
                Data = data,
                TotalItems = total,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
    }
}
