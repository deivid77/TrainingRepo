using System;

namespace FactoryMethod
{
    class Program
    {

        private static DialogFactory _dialog;

        static void Main(string[] args)
        {
            Initialize();
            _dialog.Render();

            Console.ReadLine();
        }

        private static void Initialize()
        {
            var config = ReadAppConfigFile();

            if (config == "Windows")
            {
                Console.WriteLine("App: Launched with the WindowsDialog");
                _dialog = new WindowsDialogFactory();
            }
            else if (config == "Web")
            {
                Console.WriteLine("App: Launched with the WebDialog");
                _dialog = new WebDialogFactory(); 
            }
            else
                throw new Exception("Error! Unknown operating system");
        }
        private static string ReadAppConfigFile()
        {
            return "Windows";
        }
    }
}
