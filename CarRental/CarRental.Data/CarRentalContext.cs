using CarRental.Business.Entities;
using Core.Common.Contracts;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Runtime.Serialization;

namespace CarRental.Data
{
    public class CarRentalContext : DbContext
    {

        public DbSet<Account> AccountSet { get; set; }
        public DbSet<Car> CarSet { get; set; }
        public DbSet<Rental> RentalSet { get; set; }
        public DbSet<Reservation> ReservationSet { get; set; }

        public CarRentalContext() : base("name=CarRental")
        {
            Database.SetInitializer<CarRentalContext>(null);
        }

        /// <summary>
        /// Aquí especificamos las convenciones y los mappings de clases a tablas.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Borramos las convenciones de pluralizar las tablas
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Ignoramos los ExtensionDataObjects de las clases EntityBase
            modelBuilder.Ignore<ExtensionDataObject>();

            //Ignoramos el EntityId de los objetos que implementan IIdentifiableEntity
            modelBuilder.Ignore<IIdentifiableEntity>();

            //Mapping 1: Key
            //Mapping 2: Qué propiedades NO mapear a columnas
            modelBuilder.Entity<Account>().HasKey<int>(k => k.AccountId).Ignore(c => c.EntityId);
            modelBuilder.Entity<Car>().HasKey<int>(k => k.CarId).Ignore(c => c.EntityId);
            modelBuilder.Entity<Rental>().HasKey<int>(k => k.RentalId).Ignore(c => c.EntityId);
            modelBuilder.Entity<Reservation>().HasKey<int>(k => k.ReservationId).Ignore(c => c.EntityId);
            modelBuilder.Entity<Car>().Ignore(c => c.CurrentlyRented);

            //Mapping 3: Mapear a tablas con nombre distinto a la clase
            //modelBuilder.Entity<Account>().ToTable("Whatever");
        }

    }
}
