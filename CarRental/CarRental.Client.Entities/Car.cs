using System;
using Core.Common.Core;
using FluentValidation;

namespace CarRental.Client.Entities
{
    public class Car : ObjectBase
    {

        private int _carId;
        private string _description;
        private string _color;
        private int _year;
        private decimal _rentalPrice;
        private bool _currentlyRented;

        public int CarId
        {
            get { return _carId; }
            set
            {
                _carId = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }

        public decimal RentalPrice
        {
            get { return _rentalPrice; }
            set
            {
                _rentalPrice = value;
                OnPropertyChanged();
            }
        }

        public bool CurrentlyRented
        {
            get { return _currentlyRented; }
            set
            {
                _currentlyRented = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Clase encapsulada no reusable, validadora de datos a través del Nuget FluentValidation
        /// </summary>
        class CarValidator : AbstractValidator<Car>
        {
            public CarValidator()
            {
                RuleFor(obj => obj.Description).NotEmpty();
                RuleFor(obj => obj.Color).NotEmpty();
                RuleFor(obj => obj.RentalPrice).GreaterThan(0);
                RuleFor(obj => obj.Year).GreaterThan(2000).LessThanOrEqualTo(DateTime.Now.Year + 1);
            }
        }

        protected override IValidator GetValidator()
        {
            return new CarValidator();
        }
    }
}