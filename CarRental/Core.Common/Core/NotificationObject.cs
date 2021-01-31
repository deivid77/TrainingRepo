using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Core.Common.Core
{
    public class NotificationObject : INotifyPropertyChanged
    {

        //Esta colección nos servirá para comprobar si un subscriptor existe y evitar crear nuevo handling
        private List<PropertyChangedEventHandler> _propertyChangedSubscribers = new List<PropertyChangedEventHandler>();

        //Modificamos el evento como técnica para evitar dobles y triples subscripciones
        private event PropertyChangedEventHandler _propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!_propertyChangedSubscribers.Contains(value))
                {
                    _propertyChanged += value;
                    _propertyChangedSubscribers.Add(value);
                }
            }
            remove
            {
                _propertyChanged -= value;
                _propertyChangedSubscribers.Remove(value);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

    }
}
