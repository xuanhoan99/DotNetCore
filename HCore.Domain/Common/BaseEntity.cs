namespace HCore.Domain.Common
{
    public interface IDeletedEntity
    {
        bool IsDeleted { get; }
        void SoftDelete();
    }

    public interface IAuditEntity
    {
        string CreatedBy { get; }
        DateTime CreatedAt { get; }
        string? ApprovedBy { get; }
        DateTime? ApprovedAt { get; }
        string ApprovalStatus { get; }

        void Approve(string approvedBy, string approvalStatus);
    }

    public abstract class DeletedEntityBase : IDeletedEntity
    {
        public bool IsDeleted { get; private set; }

        protected DeletedEntityBase()
        {
            IsDeleted = false;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }
    }
    public abstract class AuditEntityBase : DeletedEntityBase, IAuditEntity
    {
        public string CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? ApprovedBy { get; private set; }
        public DateTime? ApprovedAt { get; private set; }
        public string ApprovalStatus { get; private set; }

        protected AuditEntityBase(string createdBy)
        {
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
            ApprovalStatus = "Pending";
        }

        public void Approve(string approvedBy, string approvalStatus)
        {
            ApprovedBy = approvedBy;
            ApprovedAt = DateTime.UtcNow;
            ApprovalStatus = approvalStatus;
        }
    }

}
