using CarRental.Business.Entities;
using Core.Common.Contracts;

namespace CarRental.Data.Contracts
{
    /// <summary>
    /// Necesaria para añadir miembros extra a nuestro repositorio, aparte de los CRUD
    /// </summary>
    public interface IAccountRepository : IDataRepository<Account>
    {
        Account GetByLogin(string login);
    }
}
