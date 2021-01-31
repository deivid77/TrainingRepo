using System;
using System.Runtime.Serialization;

namespace Core.Common.Core
{
    /// <summary>
    /// Permite a las Entidades tolerantes a versionado
    /// Si reciben datos que no pueden acomodar no explotan por el contrato
    /// Aceptan los datos de forma silenciosa y lo almacenan temporalmente en la propiedad "ExtensionData" 
    /// </summary>
    [DataContract]
    public abstract class EntityBase : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
