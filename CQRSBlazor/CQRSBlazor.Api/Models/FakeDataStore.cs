using System.Collections.Generic;

namespace CQRSBlazor.Api.Models
{
    public class FakeDataStore
    {
        private static List<string> _values;

        public FakeDataStore()
        {
            _values = new List<string>  {
                                            "a",
                                            "b",
                                            "c"
                                        };
        }


        public void AddValue(string value)
        {
            _values.Add(value);
        }

        public IEnumerable<string> GetAllValues()
        {
            return _values;
        }

        //In real-world use cases, these would be implemented differently, likely taking external dependencies 
        //and doing something meaningful, but here we are just trying to demonstrate the behavior of Notifications.
        public void EventOccured(string value, string evt)
        {
            var indexOfValue = _values.FindIndex(val => val.StartsWith(value));
            _values[indexOfValue] = $"{_values[indexOfValue]}, event: {evt}";
        }
    }
}
