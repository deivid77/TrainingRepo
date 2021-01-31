namespace FactoryMethod
{
    public class WebDialogFactory : DialogFactory
    {
        public override IButton CreateButton()
        {
            return new WebButton();
        }
    }
}
