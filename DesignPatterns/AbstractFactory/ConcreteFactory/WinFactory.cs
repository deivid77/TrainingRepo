namespace AbstractFactory
{
    public class WinFactory : IGUIFactory
    {
        public IButton CreateButton()
        {
            return new WinButton();
        }

        public ICheckBox CreateCheckBox()
        {
            return new WinCheckbox();
        }
    }
}
