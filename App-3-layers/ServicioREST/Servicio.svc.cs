using Datos;
using System.Collections.Generic;
using System.Threading;

namespace ServicioREST
{
    public class Servicio : IServicio
    {
        public IEnumerable<Persona> ObtenerListaPersonas()
        {

            var lista = new List<Persona>()
            {
                new Persona() { Nombre="John", Apellidos="Koenig", Id = 1},
                new Persona() { Nombre="Dylan", Apellidos="Hunt", Id = 2},
                new Persona() { Nombre="John", Apellidos="Crichton", Id = 3},
                new Persona() { Nombre="Dave", Apellidos="Lister", Id = 4},
                new Persona() { Nombre="John", Apellidos="Sheridan", Id = 5},
                new Persona() { Nombre="Dante", Apellidos="Montana", Id = 6},
                new Persona() { Nombre="Isaac", Apellidos="Gampu", Id = 7},
                new Persona() { Nombre="David", Apellidos="Blanco", Id = 8}
            };

            Thread.Sleep(5000);

            return lista;

        }

    }

}
