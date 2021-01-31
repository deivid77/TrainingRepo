using CarRental.Business.Entities;
using CarRental.Common;
using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading;

namespace CarRental.Business.Managers
{
    public class ManagerBase
    {
        protected string _loginName = string.Empty;
        protected Account _authorizationAccount = null;

        public ManagerBase()
        {
            //Evaluamos la cabecera SOAP de WCF donde vendrá el login name del browser
            OperationContext context = OperationContext.Current;
            if (context != null)
            {
                try
                {
                    _loginName = context.IncomingMessageHeaders.GetHeader<string>("String", "System");
                    //Para saber si es un usuario desktop dentro de mi firewall o si es un web user.
                    //Para ello comprobamos si lleva backslash. Todos los usuarios de Windows lo llevan, 
                    //ya sea con nombre de Dominio o máquina local
                    if (_loginName.IndexOf(@"\") > 1)
                        _loginName = string.Empty;
                }

                catch
                {
                    _loginName = string.Empty;
                }
            }

            if (ObjectBase.Container != null)
                ObjectBase.Container.SatisfyImportsOnce(this);  //Resuelve todas las interfaces con Import de esta clase

            if (!string.IsNullOrWhiteSpace(_loginName))
                _authorizationAccount = LoadAuthorizationValidationAccount(_loginName);
        }

        protected virtual Account LoadAuthorizationValidationAccount(string loginName)
        {
            return null;
        }

        protected void ValidateAuthorization(IAccountOwnedEntity entity)
        {
            if (!Thread.CurrentPrincipal.IsInRole(Security.CarRentalAdminRole))
            {
                if (_authorizationAccount != null)
                {
                    if (_loginName != string.Empty && entity.OwnerAccountId != _authorizationAccount.AccountId)
                    {
                        AuthorizationValidationException ex = new AuthorizationValidationException("Attempt to access a secure record with improper user authorization validation.");
                        throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
                    }
                }
            }
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> codeToExecute)
        {
            try
            {
                return codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;   //Como FaultException<T> deriva de FaultException, metemos este wrapper y relanzamos
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected void ExecuteFaultHandledOperation(Action codeToExecute)
        {
            try
            {
                codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;   //Como FaultException<T> deriva de FaultException, metemos este wrapper y relanzamos
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}