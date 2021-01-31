namespace AbstractFactory
{
    public class Application
    {
        private IGUIFactory _factory;
        private IButton _button;
        private ICheckBox _checkBox;

        public Application(IGUIFactory factory)
        {
            _factory = factory;
        }

        public void CreateUI()
        {
            _button = _factory.CreateButton();
            _checkBox = _factory.CreateCheckBox();
        }

        public void Paint()
        {           
            _button.Paint();          
            _checkBox.Paint();
        }
    }
}
