using Entities;
using NWindProxyService;
using System.Web.Mvc;

namespace NWind.MVCPLS
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int id)
        {
            // Obtener los productos de la categoría
            var Proxy = new Proxy();
            var Products = Proxy.FilterProductsByCategoryID(id);
            return View("ProductList", Products);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var Proxy = new Proxy();
            var Model = Proxy.RetrieveProductByID(id);
            return View(Model);
        }

        public ActionResult CUD(int id = 0)
        {
            var Proxy = new Proxy();
            var Model = new Product();
            if (id != 0)
            {
                Model = Proxy.RetrieveProductByID(id);
            }
            return View(Model);
        }

        [HttpPost]
        public ActionResult CUD(Product newProduct, string CreateBtn, string UpdateBtn, string DeleteBtn)
        {
            Product Product;
            var Proxy = new Proxy();
            ActionResult Result = View();
            if (CreateBtn != null) // ¿Crear un producto?
            {
                Product = Proxy.CreateProduct(newProduct);
                if (Product != null)
                {
                    Result = RedirectToAction("CUD", new { id = Product.ProductID });
                }
            }
            else if (UpdateBtn != null) // ¿Modificar un producto?
            {
                var IsUpdate = Proxy.UpdateProduct(newProduct);
                if (IsUpdate)
                {
                    Result = Content("El producto se ha actualizado");
                }
            }
            else if (DeleteBtn != null) // ¿Eliminar un producto?
            {
                var DeletedProduct = Proxy.DeleteProduct(newProduct.ProductID);
                if (DeletedProduct)
                {
                    Result = Content("El producto se ha eliminado");
                }
            }
            return Result;
        }


    }
}