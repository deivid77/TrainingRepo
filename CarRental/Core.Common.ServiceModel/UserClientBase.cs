using System.ServiceModel;
using System.Threading;

namespace Core.Common.ServiceModel
{
    public abstract class UserClientBase<T>: ClientBase<T> where T: class 
    {
        public UserClientBase()
        {
            //Añadiremos el usuario actual (del IIS, WebForms, etc) a la cabecera del mensaje
            string userName = Thread.CurrentPrincipal.Identity.Name;
            MessageHeader<string> header = new MessageHeader<string>(userName);

            OperationContextScope contextScope = new OperationContextScope(InnerChannel);

            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "Syetem"));
        }
    }
}
