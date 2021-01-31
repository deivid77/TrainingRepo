using Core.Common.Contracts;

namespace CarRental.Data
{
    public interface IDataRepositoryFactory
    {
        T GetDataRepository<T>() where T : IDataRepository;
    }
}
