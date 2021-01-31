using CarRental.Business.Entities;
using Core.Common.Contracts;

namespace CarRental.Data.Contracts
{
    public interface ICarRepository : IDataRepository<Car>
    {
    }
}
