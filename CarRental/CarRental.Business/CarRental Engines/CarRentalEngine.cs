using CarRental.Business.Common;
using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Data;
using CarRental.Data.Contracts;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CarRental.Business.CarRental_Engines
{
    [Export(typeof(ICarRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRentalEngine : ICarRentalEngine
    {

        IDataRepositoryFactory _dataRepositoryFactory;

        [ImportingConstructor]
        public CarRentalEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        #region ICarRentalEngine

        public bool IsCarCurrentlyRented(int carId, int accountId)
        {
            bool rented = false;

            IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

            var currentRental = rentalRepository.GetCurrentRentalByCar(carId);
            if (currentRental != null && currentRental.AccountId == accountId)
                rented = true;

            return rented;
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            bool rented = false;

            IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

            Rental currentRental = rentalRepository.GetCurrentRentalByCar(carId);
            if (currentRental != null)
                rented = true;

            return rented;
        }

        public bool IsCarAvailableForRental(int carId,
                                            DateTime pickupDate,
                                            DateTime returnDate,
                                            IEnumerable<Rental> rentedCars,
                                            IEnumerable<Reservation> reservedCars)
        {
            bool available = true;

            Reservation reservation = reservedCars.Where(item => item.CarId == carId).FirstOrDefault();

            if (reservation != null && (
                (pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
                (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
            {
                available = false;
            }

            return available;
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            if (rentalDate > DateTime.Now)
                throw new UnableToRentForDateException(string.Format("Cannot rent for date {0} yet.", rentalDate.ToShortDateString()));

            IAccountRepository accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

            bool carIsRented = IsCarCurrentlyRented(carId);
            if (carIsRented)
                throw new CarCurrentlyRentedException(string.Format("Car {0} is already rented.", carId));

            Account account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
                throw new NotFoundException(string.Format("No account found for login '{0}'.", loginEmail));

            Rental rental = new Rental()
            {
                AccountId = account.AccountId,
                CarId = carId,
                DateRented = rentalDate,
                DateDue = dateDueBack
            };

            Rental savedEntity = rentalRepository.Add(rental);

            return savedEntity;
        }

        #endregion

    }
}
