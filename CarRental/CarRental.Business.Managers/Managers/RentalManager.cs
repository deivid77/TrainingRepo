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
    public class RentalManager : ManagerBase, IRentalService
    {
        [Import]
        IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _businessEngineFactory;

        #region Constructores
        public RentalManager()
        {
        }

        public RentalManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        public RentalManager(IBusinessEngineFactory businessEngineFactory)
        {
            _businessEngineFactory = businessEngineFactory;
        }

        public RentalManager(IDataRepositoryFactory dataRepositoryFactory,
                                IBusinessEngineFactory businessEngineFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
            _businessEngineFactory = businessEngineFactory;
        }
        #endregion

        protected override Account LoadAuthorizationValidationAccount(string loginName)
        {
            IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
            Account authAccount = accountRepository.GetByLogin(loginName);
            if (authAccount == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("Cannot find account for login name {0} to use for security trimming.", loginName));
                //WCF gestiona las excepciones como mensaje SOAP con FaultException: FaultContract attribute
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }

            return authAccount;
        }

        #region IRentalService operations
         
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime dateDueBack)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRentalEngine carRentalEngine = _businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(loginEmail, carId, DateTime.Now, dateDueBack);

                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRentalEngine carRentalEngine = _businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(loginEmail, carId, rentalDate, dateDueBack);

                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public void AcceptCarReturn(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                ICarRentalEngine carRentalEngine = _businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                Rental rental = rentalRepository.GetCurrentRentalByCar(carId);
                if (rental == null)
                {
                    CarNotRentedException ex = new CarNotRentedException(string.Format("Car {0} is not currently rented.", carId));
                    throw new FaultException<CarNotRentedException>(ex, ex.Message);
                }

                rental.DateReturned = DateTime.Now;

                Rental updatedRentalEntity = rentalRepository.Update(rental);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)] //WCF autorizará esta operación sólo a usuarios con este grupo de Windows              
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]      //Y también a los que se acrediten con este usuario de Windows
        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Not account found for login {0}", loginEmail));
                    //WCF gestiona las excepciones como mensaje SOAP con FaultException: FaultContract attribute
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);

                IEnumerable<Rental> rentalHistory = rentalRepository.GetRentalHistoryByAccount(account.AccountId);
                return rentalHistory;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Reservation GetReservation(int reservationId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No reservation found for id '{0}'.", reservationId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(reservation);

                return reservation;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for login '{0}'.", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);

                Reservation reservation = new Reservation()
                {
                    AccountId = account.AccountId,
                    CarId = carId,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };

                Reservation savedEntity = reservationRepository.Add(reservation);

                return savedEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public void ExecuteRentalFromReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();
                ICarRentalEngine carRentalEngine = _businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Reservation {0} is not found.", reservationId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                Account account = accountRepository.Get(reservation.AccountId);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for account ID '{0}'.", reservation.AccountId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                try
                {
                    //Call to business engine
                    Rental rental = carRentalEngine.RentCarToCustomer(account.LoginEmail, reservation.CarId, reservation.RentalDate, reservation.ReturnDate);
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                reservationRepository.Remove(reservation);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public void CancelReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No reservation found found for ID '{0}'.", reservationId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(reservation);

                reservationRepository.Remove(reservationId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public CustomerReservationData[] GetCurrentReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

                IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCurrentCustomerReservationInfo();
                foreach (CustomerReservationInfo reservationInfo in reservationInfoSet)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationId = reservationInfo.Reservation.ReservationId,
                        Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for login '{0}'.", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);

                List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

                IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCustomerOpenReservationInfo(account.AccountId);
                foreach (CustomerReservationInfo reservationInfo in reservationInfoSet)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationId = reservationInfo.Reservation.ReservationId,
                        Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Rental GetRental(int rentalId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                Rental rental = rentalRepository.Get(rentalId);
                if (rental == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No rental record found for id '{0}'.", rentalId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(rental);

                return rental;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public CustomerRentalData[] GetCurrentRentals()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                List<CustomerRentalData> rentalData = new List<CustomerRentalData>();

                IEnumerable<CustomerRentalInfo> rentalInfoSet = rentalRepository.GetCurrentCustomerRentalInfo();
                foreach (CustomerRentalInfo rentalInfo in rentalInfoSet)
                {
                    rentalData.Add(new CustomerRentalData()
                    {
                        RentalId = rentalInfo.Rental.RentalId,
                        Car = rentalInfo.Car.Color + " " + rentalInfo.Car.Year + " " + rentalInfo.Car.Description,
                        CustomerName = rentalInfo.Customer.FirstName + " " + rentalInfo.Customer.LastName,
                        DateRented = rentalInfo.Rental.DateRented,
                        ExpectedReturn = rentalInfo.Rental.DateDue
                    });
                }

                return rentalData.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Reservation[] GetDeadReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                IEnumerable<Reservation> reservations = reservationRepository.GetReservationsByPickupDate(DateTime.Now.AddDays(-1));

                return (reservations?.ToArray());
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public bool IsCarCurrentlyRented(int carId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRentalEngine carRentalEngine = _businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                //Call to business engine
                return carRentalEngine.IsCarCurrentlyRented(carId);
            });
        }
        #endregion

    }
}
