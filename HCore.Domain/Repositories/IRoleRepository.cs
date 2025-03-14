using HCore.Domain.Entities;

namespace HCore.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllAsync();
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<Role> GetByNameAsync(string roleName);
    }
}
