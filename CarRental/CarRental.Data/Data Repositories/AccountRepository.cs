using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using System.ComponentModel.Composition;

namespace CarRental.Data
{
    [Export(typeof(IAccountRepository))]                //Registramos el tipo con su interface
    [PartCreationPolicy(CreationPolicy.NonShared)]      //Quitamos el Singleton/Shared por defecto
    public class AccountRepository : DataRepositoryBase<Account>, IAccountRepository
    {

        #region IDataRepository

        protected override Account AddEntity(CarRentalContext entityContext, Account entity)
        {
            return entityContext.AccountSet.Add(entity);
        }

        protected override Account UpdateEntity(CarRentalContext entityContext, Account entity)
        {
            return (from e in entityContext.AccountSet
                    where e.AccountId == entity.AccountId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Account> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.AccountSet
                   select e;
        }

        protected override Account GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.AccountSet
                         where e.AccountId == id
                         select e);

            var results = query.FirstOrDefault();

            return results;
        }
           
        #endregion

        #region IAccountRepository
        
        public Account GetByLogin(string login)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                return (from a in entityContext.AccountSet
                        where a.LoginEmail == login
                        select a).FirstOrDefault();
            }
        }

        #endregion

    }
}
