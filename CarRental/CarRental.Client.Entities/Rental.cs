using System;
using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Rental : ObjectBase
    {
        int _rentalId;
        int _accountId;
        int _carId;
        DateTime _dateRented;
        DateTime _dateDue;
        DateTime? _dateReturned;

        public int RentalId
        {
            get { return _rentalId; }
            set
            {
                if (_rentalId != value)
                {
                    _rentalId = value;
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

        public DateTime DateRented
        {
            get { return _dateRented; }
            set
            {
                if (_dateRented != value)
                {
                    _dateRented = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateDue
        {
            get { return _dateDue; }
            set
            {
                if (_dateDue != value)
                {
                    _dateDue = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? DateReturned
        {
            get { return _dateReturned; }
            set
            {
                if (_dateReturned != value)
                {
                    _dateReturned = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
