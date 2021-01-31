using CarRental.Business.CarRental_Engines;
using CarRental.Data;
using System.ComponentModel.Composition.Hosting;

namespace CarRental.Business.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            //CarRental.Data Assembly
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));

            //CarRental.Business Assembly
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(CarRentalEngine).Assembly));
            
            CompositionContainer container = new CompositionContainer(catalog);
            return container;
        }

    }
}
