using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using HCore.Domain.Entities;
using HCore.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCore.Infrastructure.Identity
{
    public sealed class UserQueries : IUserQueries
    {
        private readonly AppDbContext _db;

        public UserQueries(UserManager<User> userManager, AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResponse<UserSearchResponseDto>> SearchAsync(UserSearchInput input, CancellationToken ct = default)
        {
            // Chuẩn hóa paging
            var pageIndex = input.PageIndex <= 0 ? 1 : input.PageIndex;
            var pageSize = input.PageSize <= 0 ? 10 : Math.Min(input.PageSize, 200);

            // Base query (đọc)
            var q = _db.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.UserName))
            {
                var key = input.UserName.Trim().ToLower();
                q = q.Where(u => u.UserName!.ToLower().Contains(key.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                var key = input.Name.Trim().ToLower();
                q = q.Where(u => u.Name!.ToLower().Contains(key.ToLower()));
            }

            // Tổng trước khi paging
            var total = await q.CountAsync(ct);

            // Lấy trang Users trước 
            var usersPage = await q.OrderBy(u => u.UserName)
                                   .Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .Select(u => new UserSearchResponseDto
                                   {
                                       Id = u.Id,
                                       UserName = u.UserName!,
                                       Email = u.Email,
                                       Name = u.Name,
                                   })
                                   .ToListAsync(ct);

            if (usersPage.Count == 0)
                return PagedResponse<UserSearchResponseDto>.Create(usersPage, total, pageIndex, pageSize);

            // Lấy Roles cho đúng tập UserId của page (1 query, không N+1)
            var userIds = usersPage.Select(x => x.Id).ToList();

            var rolesByUser = await (from ur in _db.UserRoles.AsNoTracking()
                                     join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                                     where userIds.Contains(ur.UserId)
                                     select new { ur.UserId, r.Name })
                                    .ToListAsync(ct);

            var lookup = rolesByUser
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Name).Distinct().ToList());

            // Gán roles vào DTO của trang hiện tại
            foreach (var u in usersPage)
            {
                if (lookup.TryGetValue(u.Id, out var roles))
                    u.Roles = string.Join(", ", roles); // ngăn cách bằng dấu phẩy + khoảng trắng
                else
                    u.Roles = string.Empty;
            }

            // Trả PagedResponse chuẩn
            return PagedResponse<UserSearchResponseDto>.Create(usersPage, total, pageIndex, pageSize);
        }
    }

}
