using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using System.Linq;

namespace DAL.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestDALSecuentially()
        {
            Test01_AddCategoryAndProduct();
            Test02_AddProduct();
            Test03_RetrieveAndUpdate();
            Test04_List();
            Test05_SearchAndDelete();
        }
       
        public void Test01_AddCategoryAndProduct()
        {
            Category cereales = new Category()
            {
                CategoryName = "Cereales",
                Description = "Productos de Maíz"
            };

            Product cereal = new Product
            {
                ProductName = "Cereal",
                UnitsInStock = 0,
                UnitPrice = 15
            };

            cereales.Products.Add(cereal);

            using (var repo = RepositoryFactory.CreateRepository())
            {
                var ret = repo.Create(cereales);
                Assert.IsNotNull(ret);
            }
        }
             
        public void Test02_AddProduct()
        {
            Product avena = new Product
            {
                CategoryID = 1,
                UnitsInStock = 100,
                ProductName = "Avena",
                UnitPrice = 10
            };

            using (var repo = RepositoryFactory.CreateRepository())
            {
                var ret = repo.Create(avena);
                Assert.IsNotNull(ret);
            }
        }
               
        public void Test03_RetrieveAndUpdate()
        {
            using (var repo = RepositoryFactory.CreateRepository())
            {
                // Buscar el último producto agregado
                var product = repo.Retrieve<Product>(p => p.ProductName == "Avena");
                Assert.IsNotNull(product);

                var oldProductName = product.ProductName;
                product.ProductName = product.ProductName + "***";
                var ret = repo.Update(product);
                Assert.IsTrue(ret);
            }
        }

        public void Test04_List()
        {
            using (var repo = RepositoryFactory.CreateRepository())
            {
                // Buscar una categoría agregada previamente
                var productos = repo.Filter<Product>(p => p.CategoryID == 1);
                Assert.IsNotNull(productos);
                Assert.IsTrue(productos.Any());
            }
        }

        public void Test05_SearchAndDelete()
        {
            using (var repo = RepositoryFactory.CreateRepository())
            {
                // Buscar el penúltimo producto agregado
                var product = repo.Retrieve<Product>(p => p.ProductName == "Avena***");
                Assert.IsNotNull(product);

                var ret = repo.Delete(product);
                Assert.IsTrue(ret);

                // Buscar el último producto agregado
                product = repo.Retrieve<Product>(p => p.ProductName == "Cereal");
                Assert.IsNotNull(product);

                ret = repo.Delete(product);
                Assert.IsTrue(ret);

                // Buscar la último categoría agregada
                var category = repo.Retrieve<Category>(c => c.CategoryName == "Cereales");
                Assert.IsNotNull(category);

                ret = repo.Delete(category);
                Assert.IsTrue(ret);
            }
        }
    }
}
