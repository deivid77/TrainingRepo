using System;

namespace AbstractFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            IGUIFactory factory = new WinFactory();
            var app = new Application(factory);
            app.CreateUI();
            app.Paint();

            Console.ReadLine();
        }
    }
}
