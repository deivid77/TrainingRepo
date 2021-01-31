using CarRental.Client.Entities;
using CarRental.Common;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CarRental.Client.Contracts
{
    [ServiceContract]
    public interface IAccountService : IServiceContract
    {

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Account GetCustomerAccountInfo(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateCustomerAccountInfo(Account account);

        #region Async operations

        [OperationContract]
        Task<Account> GetCustomerAccountInfoAsync(string loginEmail);

        [OperationContract]
        Task UpdateCustomerAccountInfoAsync(Account account);

        #endregion

    }
}
