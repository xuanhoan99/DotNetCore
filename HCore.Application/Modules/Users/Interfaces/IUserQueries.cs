using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Users.Dtos;

namespace HCore.Application.Modules.Users.Interfaces
{
    public interface IUserQueries
    {
        Task<PagedResponse<UserSearchResponseDto>> SearchAsync(UserSearchInput input, CancellationToken ct = default);
    }

}
