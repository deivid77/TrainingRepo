using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web.Http.Dependencies;
using Core.Common.Extensions;

namespace CarRental.Web.Core
{
    public class MefAPIDependencyResolver : IDependencyResolver
    {
        public MefAPIDependencyResolver(CompositionContainer container)
        {
            _Container = container;
        }

        CompositionContainer _Container;

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _Container.GetExportedValue(serviceType.Name);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {        
            return _Container.GetExportedValues(serviceType);
        }

        public void Dispose()
        {
        }
    }
}
