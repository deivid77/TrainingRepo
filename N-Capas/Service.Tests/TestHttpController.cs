using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Service.Tests
{
    [TestClass]
    public class TestHttpController
    {

        [TestMethod]
        public void GetReturnsProduct()
        {
            // Arrange
            var controller = new NWindController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.Get(7);

            // Assert
            Product product;
            Assert.IsTrue(response.TryGetContentValue<Product>(out product));
            Assert.AreEqual(7, product.ProductID);
        }

    }
}
