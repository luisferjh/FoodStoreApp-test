using OnlineStoreApp.Entities.Interfaces;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class UnitOfWorkAdapter : IUnitOfWorkAdapter
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkAdapter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork Create()
        {
            return _unitOfWork;
        }

    }
}
