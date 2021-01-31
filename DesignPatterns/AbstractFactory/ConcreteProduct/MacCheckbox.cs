using System;

namespace AbstractFactory
{
    internal class MacCheckbox : ICheckBox
    {  
        public void Paint()
        {
            Console.WriteLine("Mac Checkbox painted");
        }
    }
}