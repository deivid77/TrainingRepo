using CarRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CarRental.Client.Contracts
{
    [ServiceContract]
    public interface IInventoryService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Car GetCar(int carId);
               
        [OperationContract]
        Car[] GetAllCars();

        [OperationContract]
        Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate);

        [OperationContract]
        [TransactionFlow (TransactionFlowOption.Allowed)]   //Implica transacción. Ideal para concurrencia. Necesita OperationBehaviour decorator
        Car UpdateCar(Car car);

        [OperationContract]
        [TransactionFlow (TransactionFlowOption.Allowed)]   //Implica transacción. Ideal para concurrencia. Necesita OperationBehaviour decorator
        void DeleteCar(int carId);

        #region Async operations

        [OperationContract]
        Task<Car> UpdateCarAsync(Car car);

        [OperationContract]
        Task DeleteCarAsync(int carId);

        [OperationContract]
        Task<Car> GetCarAsync(int carId);

        [OperationContract]
        Task<Car[]> GetAllCarsAsync();

        [OperationContract]
        Task<Car[]> GetAvailableCarsAsync(DateTime pickupDate, DateTime returnDate);

        #endregion

    }
}
