using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Repository.EFCore.DataContext;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        IUnitOfWorkRepositories _unitOfWorkRepositories;

        public UnitOfWork(
            ApplicationDbContext applicationDbContext,
            IUnitOfWorkRepositories unitOfWorkRepositories)
        {
            _dbContext = applicationDbContext;
            _unitOfWorkRepositories = unitOfWorkRepositories;
        }

        public IUnitOfWorkRepositories UnitOfWorkRepositories { get => _unitOfWorkRepositories; }

        public async Task<bool> SaveAsync()
        {
            try
            {
                int result = await _dbContext.SaveChangesAsync();
                return result <= 0 ? false : true;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                int result = _dbContext.SaveChanges();
                return result <= 0 ? false : true;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
