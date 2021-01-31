﻿using CarRental.Business.Entities;
using Core.Common.Exceptions;
using System;
using System.ServiceModel;

namespace CarRental.Business.Contracts
{
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Car GetCar(int carId);

        [OperationContract]
        Car[] GetAllCars();

        [OperationContract]
        [TransactionFlow (TransactionFlowOption.Allowed)]   //Implica transacción. Ideal para concurrencia. Necesita OperationBehaviour decorator
        Car UpdateCar(Car car);

        [OperationContract]
        [TransactionFlow (TransactionFlowOption.Allowed)]   //Implica transacción. Ideal para concurrencia. Necesita OperationBehaviour decorator
        void DeleteCar(int carId);

        [OperationContract]
        Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate);
    }
}
