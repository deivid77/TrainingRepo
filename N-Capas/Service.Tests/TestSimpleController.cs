using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Service.Tests
{
    [TestClass]
    public class TestSimpleController
    {

        [TestMethod]
        public void Test_CRUD()
        {
            var category = CreateCategory();
            var product = CreateProduct();
            DeleteProduct(product);
        }
  
        private Category CreateCategory()
        {
            Category cereales = new Category()
            {
                CategoryName = "Cereales",
                Description = "Productos de Maíz"
            };

            var controller = new NWindController();
            var result = controller.CreateCategory(cereales);
            Assert.IsNotNull(result);

            return result;
        }

        private Product CreateProduct()
        {
            Product avena = new Product
            {
                CategoryID = 1,
                UnitsInStock = 100,
                ProductName = "Avena",
                UnitPrice = 10
            };

            var controller = new NWindController();
            var result = controller.CreateProduct(avena);
            Assert.IsNotNull(result);

            return result;
        }

        private void DeleteProduct(Product product)
        {        
            var controller = new NWindController();
            var result = controller.DeleteProduct(product.ProductID);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Test_ShouldReturnAllProductsByCategory()
        {
            var testProducts = GetTestProducts();
            var controller = new NWindController();
            var result = controller.FilterProductsByCategoryID(7) as List<Product>;
            Assert.AreEqual(testProducts.Count, result.Count);
        }

        private List<Product> GetTestProducts()
        {
            var testProducts = new List<Product>();
            testProducts.Add(new Product { ProductID = 74, ProductName = "Longlife Tofu", UnitPrice = 1 });
            testProducts.Add(new Product { ProductID = 51, ProductName = "Manjimup Dried Apples", UnitPrice = 3.75M });
            testProducts.Add(new Product { ProductID = 28, ProductName = "Rössle Sauerkraut", UnitPrice = 16.99M });
            testProducts.Add(new Product { ProductID = 14, ProductName = "Tofu", UnitPrice = 11.00M });
            testProducts.Add(new Product { ProductID = 7, ProductName = "Uncle Bob's Organic Dried Pears", UnitPrice = 6.00M });
            
            return testProducts;
        }

        [TestMethod]
        public void GetProduct_ShouldNotFindProduct()
        {
            var controller = new NWindController();

            var result = controller.GetProduct(999);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetProductAsync_ShouldReturnCorrectProduct()
        {
            var testProducts = GetTestProducts();
            var controller = new NWindController();
            
            var result = await controller.GetProductAsync(7) as OkNegotiatedContentResult<Product>;
            Assert.IsNotNull(result);
            Assert.AreEqual(testProducts[4].ProductName, result.Content.ProductName);
        }

    }
}
