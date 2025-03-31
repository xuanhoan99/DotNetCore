using HCore.Application.Modules.Auth.Dtos;

namespace HCore.Application.Modules.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    }
}
