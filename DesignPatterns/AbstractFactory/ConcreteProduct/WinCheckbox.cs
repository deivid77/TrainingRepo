using System;

namespace AbstractFactory
{
    internal class WinCheckbox : ICheckBox
    {
        public void Paint()
        {
            Console.WriteLine("Windows CheckBox painted");
        }
    }
}