namespace AbstractFactory
{
    public class MacFactory : IGUIFactory
    {
        public IButton CreateButton()
        {
            return new MacButton();
        }

        public ICheckBox CreateCheckBox()
        {
            return new MacCheckbox();
        }
    }
}
