using System.Collections.Generic;

namespace NWind.ViewModel
{
    public class Product : ViewModelBase
    {
        public Product()
        {
            InitializeViewModel();
        }

        void InitializeViewModel()
        {
            Products = new List<Entities.Product>();

            SearchProductsCommand = new CommandDelegate
            ((o) => { return true; },
            (o) =>
            {
                var Proxy = new NWindProxyService.Proxy();
                Products = Proxy.FilterProductsByCategoryID(CategoryID);
            });

            SearchProductByIDCommand = new CommandDelegate
            ((o) => { return true; },
            (o) =>
            {
                if (ProductSelected.ProductID != 0)
                {
                    var Proxy = new NWindProxyService.Proxy();
                    var p = Proxy.RetrieveProductByID(ProductSelected.ProductID);
                    ProductName = p.ProductName;
                    ProductID = p.ProductID;
                    UnitsInStock = p.UnitsInStock;
                    UnitPrice = p.UnitPrice;
                }
            });
        }

        #region Properties
        private int _categoryID;
        public int CategoryID
        {
            get { return _categoryID; }
            set
            {
                _categoryID = value;
                OnPropertyChanged();
            }
        }
        private List<Entities.Product> _products;
        public List<Entities.Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        private Entities.Product _productSelected;
        public Entities.Product ProductSelected
        {
            get { return _productSelected; }
            set
            {
                _productSelected = value;
                OnPropertyChanged();
            }
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }
        private int _productId;
        public int ProductID
        {
            get { return _productId; }
            set
            {
                _productId = value;
                OnPropertyChanged();
            }
        }
        private short? _unitsInStock;
        public short? UnitsInStock
        {
            get { return _unitsInStock; }
            set
            {
                _unitsInStock = value;
                OnPropertyChanged();
            }
        }
        private decimal? _unitPrice;
        public decimal? UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                _unitPrice = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public CommandDelegate SearchProductsCommand { get; set; }
        public CommandDelegate SearchProductByIDCommand { get; set; }
    }
}
