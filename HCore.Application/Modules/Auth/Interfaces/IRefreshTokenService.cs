using HCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Application.Modules.Auth.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateRefreshTokenAsync(User user);
        Task<RefreshToken?>  GetByTokenAsync(string token);
        Task RevokeTokenAsync(RefreshToken token);
        Task RevokeAllUserTokensAsync(int userId);
    }

}
