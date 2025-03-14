using HCore.Domain.Common;

namespace HCore.Domain.Entities
{
    public class User : AuditEntityBase
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public bool IsActive { get; private set; }
        public string EmployeeId { get; set; }

        // Quan hệ nhiều-nhiều với Role
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // Constructor
        public User(string fullName, string email, string password, string createdBy)
            : base(createdBy)
        {
            FullName = fullName;
            Email = email;
            Password = password;
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
        public void ChangePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Password không được để trống.");

            Password = newPassword;
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
