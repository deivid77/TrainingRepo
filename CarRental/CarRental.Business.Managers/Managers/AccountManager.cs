using System.ComponentModel.Composition;
using System.ServiceModel;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Exceptions;
using System.Security.Permissions;
using CarRental.Common;
using CarRental.Data;
using CarRental.Business.Contracts;
using Core.Common.Contracts;

namespace CarRental.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     ReleaseServiceInstanceOnTransactionComplete = false)]
    public class AccountManager : ManagerBase, IAccountService
    {

        [Import]
        IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _businessEngineFactory;

        public AccountManager()
        {
        }

        public AccountManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        public AccountManager(IBusinessEngineFactory businessEngineFactory)
        {
            _businessEngineFactory = businessEngineFactory;
        }

        public AccountManager(IDataRepositoryFactory dataRepositoryFactory,
                                IBusinessEngineFactory businessEngineFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
            _businessEngineFactory = businessEngineFactory;
        }

        protected override Account LoadAuthorizationValidationAccount(string loginName)
        {
            IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
            Account authAcct = accountRepository.GetByLogin(loginName);
            if(authAcct!=null)
            {
                NotFoundException ex = new NotFoundException(string.Format("Cannot find account for login name {0} to use for security", loginName));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }
            return authAcct;
        }

        #region IAccountService operations

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();

                Account accountEntity = accountRepository.GetByLogin(loginEmail);
                if (accountEntity == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Account with login {0} is not in database", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(accountEntity);

                return accountEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public void UpdateCustomerAccountInfo(Account account)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();

                ValidateAuthorization(account);
                
                Account updatedAccount = accountRepository.Update(account);
            });
        }
        
        #endregion
    }
}
