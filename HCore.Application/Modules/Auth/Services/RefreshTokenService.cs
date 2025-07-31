using HCore.Application.Modules.Auth.Interfaces;
using HCore.Domain.Common;
using HCore.Domain.Entities;
using HCore.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Application.Modules.Auth.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IGenericRepository<RefreshToken, int> _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RefreshTokenService(IGenericRepository<RefreshToken, int> refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RefreshToken> CreateRefreshTokenAsync(User user)
        {
            var token = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateSecureToken(),
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(token);
            await _unitOfWork.SaveChangesAsync();

            return token;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _refreshTokenRepository.FirstOrDefaultAsync(x => x.Token == token,x => x.User);
        }

        public async Task RevokeTokenAsync(RefreshToken token)
        {
            token.IsRevoked = true;
            _refreshTokenRepository.Update(token);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RevokeAllUserTokensAsync(int userId)
        {
            var tokens = await _refreshTokenRepository.FindAsync(x =>
                x.UserId == userId &&
                !x.IsRevoked &&
                x.ExpiryDate > DateTime.UtcNow);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                _refreshTokenRepository.Update(token);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private string GenerateSecureToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
