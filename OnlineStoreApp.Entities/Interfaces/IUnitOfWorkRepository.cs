namespace OnlineStoreApp.Entities.Interfaces
{
    public interface IUnitOfWork
    {
        IUnitOfWorkRepositories UnitOfWorkRepositories { get; }

        Task<bool> SaveAsync();
        bool Save();
    }
}
