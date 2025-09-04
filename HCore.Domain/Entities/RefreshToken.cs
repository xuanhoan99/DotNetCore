using HCore.Domain.Common;

namespace HCore.Domain.Entities
{
    public class RefreshToken : EntityBase<int>
    {
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; } = null!;
    }

}
