using System;
using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Reservation : ObjectBase
    {
        int _reservationId;
        int _accountId;
        int _carId;
        DateTime _returnDate;
        DateTime _rentalDate;

        public int ReservationId
        {
            get { return _reservationId; }
            set
            {
                if (_reservationId != value)
                {
                    _reservationId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AccountId
        {
            get { return _accountId; }
            set
            {
                if (_accountId != value)
                {
                    _accountId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CarId
        {
            get { return _carId; }
            set
            {
                if (_carId != value)
                {
                    _carId = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime RentalDate
        {
            get { return _rentalDate; }
            set
            {
                if (_rentalDate != value)
                {
                    _rentalDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime ReturnDate
        {
            get { return _returnDate; }
            set
            {
                if (_returnDate != value)
                {
                    _returnDate = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
