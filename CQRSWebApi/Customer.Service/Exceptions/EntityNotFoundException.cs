using System;

namespace Customer.Service.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException() : base() { }

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(string message, Exception ex) : base(message, ex) { }
    }
}