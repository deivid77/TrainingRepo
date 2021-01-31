using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepositoryFactory
    {
        public static IRepository CreateRepository()
        {
            {
                var Context = new Entities.NWindEntities();
                //Desconectamos para que no cree automáticamente clases proxy que Postman no sabe deserializar
                Context.Configuration.ProxyCreationEnabled = false;
                return new EFRepository(Context);

            }
        }
    }
}

