using CarRental.Business.Common;
using CarRental.Business.Contracts;
using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Data;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;

namespace CarRental.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService
    {
        [Import]
        IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _businessEngineFactory;

        #region Constructores
        public InventoryManager()
        {
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        public InventoryManager(IBusinessEngineFactory businessEngineFactory)
        {
            _businessEngineFactory = businessEngineFactory;
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory,
                                IBusinessEngineFactory businessEngineFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
            _businessEngineFactory = businessEngineFactory;
        }
        #endregion

        #region IInventoryService members

            
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)] //WCF autorizará esta operación sólo a usuarios con este grupo de Windows              
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]      //Y también a los que se acrediten con este usuario de Windows
        public Car GetCar(int carId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();

                Car carEntity = carRepository.Get(carId);
                if (carEntity == null)
                {
                    NotFoundException ex
                            = new NotFoundException(string.Format("Car with Id {0} is not in the database", carId));
                    //WCF gestiona las excepciones como mensaje SOAP con FaultException: FaultContract attribute
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                return carEntity;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)] //WCF autorizará esta operación sólo a usuarios con este grupo de Windows              
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]      //Y también a los que se acrediten con este usuario de Windows
        public Car[] GetAllCars()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();
                IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                IEnumerable<Car> cars = carRepository.Get();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars();
                foreach (Car car in cars)
                {
                    Rental rentedCar = rentedCars.Where(item => item.CarId == car.CarId).FirstOrDefault();
                    car.CurrentlyRented = (rentedCar != null);
                }

                return cars.ToArray();
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]                             //Transaction friendly decorator. Inicia una nueva transacción.  
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)] //WCF autorizará esta operación sólo a usuarios con este grupo de Windows
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();

                Car updatedEntity = null;
                if (car.CarId == 0)
                    updatedEntity = carRepository.Add(car);
                else
                    updatedEntity = carRepository.Update(car);

                return updatedEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]                             //Transaction friendly decorator. Inicia una nueva transacción.                                                                    
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)] //WCF autorizará esta operación sólo a usuarios con este grupo de Windows
        public void DeleteCar(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();
                carRepository.Remove(carId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)] //WCF autorizará esta operación sólo a usuarios con este grupo de Windows              
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]      //Y también a los que se acrediten con este usuario de Windows
        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();
                IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                ICarRentalEngine carRentalEngine = _businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();
                IEnumerable<Car> allCars = carRepository.Get();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars();
                IEnumerable<Reservation> reservedCars = reservationRepository.Get();

                List<Car> availableCars = new List<Car>();

                foreach (Car car in allCars)
                {
                    if (carRentalEngine.IsCarAvailableForRental(car.CarId, pickupDate, returnDate, rentedCars, reservedCars))
                        availableCars.Add(car);
                }

                return availableCars.ToArray();
            });
        }

        #endregion
    }
}
