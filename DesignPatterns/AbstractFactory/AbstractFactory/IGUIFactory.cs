namespace AbstractFactory
{
    public interface IGUIFactory
    {
        IButton CreateButton();

        ICheckBox CreateCheckBox();
    }
}
