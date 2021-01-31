using Core.Common.Contracts;
using Core.Common.Data;

namespace CarRental.Data
{
    public abstract class DataRepositoryBase<T> : DataRepositoryBase<T, CarRentalContext>
        where T : class, IIdentifiableEntity, new()      //Con esta constraint, aseguramos que el new creará el DataContext
    {     
    }
}
