using System;
using System.Security.Principal;
using System.Threading;
using System.Timers;
using System.Transactions;
using CarRental.Business.Bootstrapper;
using CarRental.Business.Entities;
using CarRental.Business.Managers;
using Core.Common.Core;
using SM = System.ServiceModel;

namespace CarRental.ServiceHost.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericPrincipal principal = new GenericPrincipal(
                                           new GenericIdentity("Dave"),
                                           new string[] { "CarRentalAdmin" });

            Thread.CurrentPrincipal = principal;

            ObjectBase.Container = MEFLoader.Init();

            System.Console.WriteLine("Starting up services...");
            System.Console.WriteLine("");

            SM.ServiceHost hostInventoryManager = new SM.ServiceHost(typeof(InventoryManager));
            SM.ServiceHost hostRentalManager = new SM.ServiceHost(typeof(RentalManager));
            SM.ServiceHost hostAccountManager = new SM.ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostRentalManager, "RentalManager");
            StartService(hostAccountManager, "AccountManager");

            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
            System.Console.WriteLine("Reservation monitor started.");

            System.Console.WriteLine("");
            System.Console.WriteLine("Press [Enter] to exit.");
            System.Console.ReadLine();

            timer.Stop();
            System.Console.WriteLine("Reservation monitor stopped.");

            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");

        }

        private static void StartService(SM.ServiceHost host, string serviceDescription)
        {
            host.Open();
            System.Console.WriteLine("Service {0} started.", serviceDescription);

            foreach (var endPoint in host.Description.Endpoints)
            {
                System.Console.WriteLine(string.Format("Listening on endpoint:"));
                System.Console.WriteLine(string.Format("Address: {0}", endPoint.Address.Uri.ToString()));
                System.Console.WriteLine(string.Format("Binding: {0}", endPoint.Binding.Name));
                System.Console.WriteLine(string.Format("Contract: {0}", endPoint.Contract.ConfigurationName));
            }

            System.Console.WriteLine();
        }

        private static void StopService(SM.ServiceHost host, string serviceDescription)
        {
            host.Close();
            System.Console.WriteLine("Service {0} stopped.", serviceDescription);
        }

        private static void OnTimerElapsed(Object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine("Looking for dead reservation at {0}", DateTime.Now.ToString());

            RentalManager rentalManager = new RentalManager();
            Reservation[] reservations = rentalManager.GetDeadReservations();

            if (reservations != null)
            {
                foreach (Reservation reservation in reservations)
                {
                    //Aádimos referencia Syste.Transactions
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            rentalManager.CancelReservation(reservation.ReservationId);
                            System.Console.WriteLine("Canceling reservation '{0}'.", reservation.ReservationId);

                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("There was an exception when attempting to cancel reservation '{0}.'", reservation.ReservationId);
                            System.Console.WriteLine(ex.Message.ToString());
                        }
                    }
                }
            }
        }

    }
}
