using Datos;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ServicioREST
{

    [ServiceContract]
    public interface IServicio
    {

        [OperationContract]
        [WebInvoke(Method ="GET", 
                    UriTemplate ="/ListaPersonas/",
                    RequestFormat =WebMessageFormat.Json, 
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<Persona> ObtenerListaPersonas();
    }

}
