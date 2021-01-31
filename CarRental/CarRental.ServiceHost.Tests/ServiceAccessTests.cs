using CarRental.Business.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace CarRental.ServiceHost.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {

        [TestMethod]
        public void test_inventory_manager_as_service()
        {
            ChannelFactory<IInventoryService> channelFactory =
                new ChannelFactory<IInventoryService>("");  
                    //Las comillas son un workaraound porque el constructor por defecto no funciona. Bug de Microsoft

            IInventoryService proxy = channelFactory.CreateChannel();

            //Sólo quiero probar que haya conectividad
            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_rental_manager_as_service()
        {
            ChannelFactory<IRentalService> channelFactory =
                new ChannelFactory<IRentalService>("");

            IRentalService proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_account_manager_as_service()
        {
            ChannelFactory<IAccountService> channelFactory =
                new ChannelFactory<IAccountService>("");

            IAccountService proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }
    }

}