using Datos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PruebaCompleta
{
    public class MainWindowMV : INotifyPropertyChanged
    {

        private IEnumerable<Persona> _listaPersonas;
        private readonly ICommand _obtenerPersonasCommand;

        public IEnumerable<Persona> ListaPersonas
        {
            get
            {
                return _listaPersonas;
            }
            set
            {
                _listaPersonas = value;
                RaisePropertyChanged("ListaPersonas");
            }
        }

        public ICommand ObtenerPersonasCommand
        {
            get
            {             
                return _obtenerPersonasCommand;
            }
        }

        public MainWindowMV()
        {
            _obtenerPersonasCommand = new RelayCommand(o => { CargarPersonasAsync(); });            
        }

        public async void CargarPersonasAsync()
        {            
            //string s = await RequestData("http://localhost:55010/Servicio.svc/ListaPersonas");
            string s = await RequestData("http://localhost/ServicioREST/Servicio.svc/ListaPersonas");

            this.ListaPersonas = JsonConvert.DeserializeObject<JsonRes>(s).ObtenerListaPersonasResult; 
        }

        public void CargarPersonas()
        {            
            string s = GetHttpClient().DownloadString($"ListaPersonas");
            this.ListaPersonas = JsonConvert.DeserializeObject<JsonRes>(s).ObtenerListaPersonasResult;
        }

        private async Task<string> RequestData(string uri)
        {
            using (var client = new WebClient())
            {
                return await client.DownloadStringTaskAsync(uri);
            }
        }
                
        private WebClient GetHttpClient()
        {
            Uri uri = new Uri("http://localhost:55010/Servicio.svc/");
            WebClient client = new WebClient { BaseAddress = uri.ToString() };            
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Headers[HttpRequestHeader.Accept] = "application/json";
            return client;
        }


        private class JsonRes
        {
            public Persona[] ObtenerListaPersonasResult { get; set; }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
