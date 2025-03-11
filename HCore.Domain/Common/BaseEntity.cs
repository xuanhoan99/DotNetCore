using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Domain.Common
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }

    public interface IDeletedEntity<TId> : IEntity<TId>
    {
        bool IsDeleted { get; }
        void SoftDelete();
    }

    public interface IAuditEntity<TId> : IDeletedEntity<TId>
    {
        string CreatedBy { get; }
        DateTime CreatedAt { get; }
        string? ApprovedBy { get; }
        DateTime? ApprovedAt { get; }
        string ApprovalStatus { get; }

        void Approve(string approvedBy, string approvalStatus);
    }

    public abstract class EntityBase<TId> : IEntity<TId>
    {
        public TId Id { get; private set; }

        protected EntityBase(TId id)
        {
            Id = id;
        }
    }

    public abstract class DeletedEntityBase<TId> : EntityBase<TId>, IDeletedEntity<TId>
    {
        public bool IsDeleted { get; private set; }

        protected DeletedEntityBase(TId id) : base(id)
        {
            IsDeleted = false;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }
    }
    public abstract class AuditEntityBase<TId> : DeletedEntityBase<TId>, IAuditEntity<TId>
    {
        public string CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? ApprovedBy { get; private set; }
        public DateTime? ApprovedAt { get; private set; }
        public string ApprovalStatus { get; private set; }

        protected AuditEntityBase(TId id, string createdBy) : base(id)
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
