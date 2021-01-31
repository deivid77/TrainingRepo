using System;

namespace AbstractFactory
{
    internal class MacButton : IButton
    {
        public void Paint()
        {
            Console.WriteLine("Mac Button painted");
        }
    }
}