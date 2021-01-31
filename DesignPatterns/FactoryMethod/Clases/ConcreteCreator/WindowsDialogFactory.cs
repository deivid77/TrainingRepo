namespace FactoryMethod
{
    class WindowsDialogFactory : DialogFactory
    {
        public override IButton CreateButton()
        {
            return new WindowsButton();
        }
    }
}
