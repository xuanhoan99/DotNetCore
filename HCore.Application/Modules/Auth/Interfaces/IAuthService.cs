using HCore.Application.Modules.Auth.Dtos;
using HCore.Application.Modules.Common.Responses;

namespace HCore.Application.Modules.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    }
}
