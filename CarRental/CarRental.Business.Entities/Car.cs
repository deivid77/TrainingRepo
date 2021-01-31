using Core.Common.Contracts;
using Core.Common.Core;
using System.Runtime.Serialization;

namespace CarRental.Business.Entities
{

    [DataContract]
    public class Car : EntityBase, IIdentifiableEntity
    {
        [DataMember]
        public int CarId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Color { get; set; }
        
        [DataMember]
        public int Year { get; set; }
        
        [DataMember]
        public decimal RentalPrice { get; set; }

        [DataMember]
        public bool CurrentlyRented { get; set; }

        #region IIdentifiableEntity members
        public int EntityId
        {
            get { return CarId; }
            set { CarId = value; }
        }
        #endregion
    }
}
