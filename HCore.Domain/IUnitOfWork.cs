namespace HCore.Domain
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }

}
