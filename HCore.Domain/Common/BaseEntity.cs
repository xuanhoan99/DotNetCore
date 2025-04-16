namespace HCore.Domain.Common
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
    public interface IDeletedEntity<TId> : IEntity<TId>
    {
        bool IsDeleted { get; }
    }

    public interface IAuditEntity<TId> : IDeletedEntity<TId>
    {
        string CreatedBy { get; }
        DateTime CreatedAt { get; }
        string? ApprovedBy { get; }
        DateTime? ApprovedAt { get; }
        string ApprovalStatus { get; }
    }

    public abstract class EntityBase<TId> : IEntity<TId>
    {
        public virtual TId Id { get; set; }
    }

    public abstract class DeletedEntityBase<TId> : EntityBase<TId>, IDeletedEntity<TId>
    {
        public bool IsDeleted { get; set; }
    }
    public abstract class AuditEntityBase<TId> : DeletedEntityBase<TId>, IAuditEntity<TId>
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovalStatus { get; set; }
    }

}
