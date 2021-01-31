using System;

namespace FactoryMethod
{
    public abstract class DialogFactory
    {
        public abstract IButton CreateButton();

        public void Render()
        {
            IButton okButton = CreateButton();

            var ret = okButton.Render();
            Console.WriteLine(ret);
        }
               
    }
}
