using System;

namespace AbstractFactory
{
    internal class WinButton : IButton
    {
        public void Paint()
        {
            Console.WriteLine("Windows Button painted");
        }
    }
}