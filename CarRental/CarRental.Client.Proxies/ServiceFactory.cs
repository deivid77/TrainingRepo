using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;

namespace CarRental.Client.Proxies
{
    [Export(typeof(IServiceFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]  //No la hacemos Singleton
    public class ServiceFactory : IServiceFactory
    {
        public T CreateClient<T>() where T : IServiceContract
        {
            return ObjectBase.Container.GetExportedValue<T>();
        }
    }
}
