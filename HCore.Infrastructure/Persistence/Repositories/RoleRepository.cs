using HCore.Domain.Entities;
using HCore.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Role role)
        {
            await _context.Set<Role>().AddAsync(role);
        }

        public async Task DeleteAsync(Role role)
        {
            _context.Set<Role>().Remove(role);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Set<Role>().ToListAsync();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Set<Role>().FindAsync(id);
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            return await _context.Set<Role>().FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Set<Role>().Update(role);
        }
    }
}
