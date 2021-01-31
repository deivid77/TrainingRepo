using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;

namespace CarRental.Business
{
    [Export(typeof(IBusinessEngineFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BusinessEngineFactory : IBusinessEngineFactory
    {
        public T GetBusinessEngine<T>() where T : IBusinessEngine
        {
            return ObjectBase.Container.GetExportedValue<T>();
        }
    }
}
