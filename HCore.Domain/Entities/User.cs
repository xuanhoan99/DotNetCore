using HCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Domain.Entities
{
    public class User : AuditEntityBase<Guid>
    {
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }

        // Constructor
        public User(Guid id, string fullName, string email, string passwordHash, string createdBy)
            : base(id, createdBy)
        {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            IsActive = true; // Mặc định User mới sẽ Active
        }

        // Cập nhật Full Name
        public void UpdateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full Name không được để trống.");

            FullName = fullName;
        }

        // Đổi mật khẩu
        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password không được để trống.");

            PasswordHash = newPasswordHash;
        }

        // Deactivate User
        public void Deactivate()
        {
            IsActive = false;
            SoftDelete(); // Gọi phương thức Soft Delete
        }

        // Reactivate User
        public void Reactivate()
        {
            IsActive = true;
            // Khôi phục nếu cần (nếu có thuộc tính IsDeleted thì đặt lại thành false)
        }
    }

}
