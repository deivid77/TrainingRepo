using System.Collections.Generic;
using System.Web.Http;
using Entities;
using BLL;
using SLC;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace Service
{
    public class NWindController : ApiController, IService
    {

        [HttpPost]
        public Category CreateCategory(Category newCategory)
        {
            var BL = new Categories();
            var NewCategory = BL.Create(newCategory);
            return NewCategory;
        }

        [HttpPost]
        public Product CreateProduct(Product newProduct)
        {
            var BL = new Products();
            var NewProduct = BL.Create(newProduct);
            return NewProduct;
        }

        [HttpGet]
        public bool DeleteProduct(int ID)
        {
            var BL = new Products();
            var Result = BL.Delete(ID);
            return Result;
        }

        [HttpGet]
        public List<Product> FilterProductsByCategoryID(int ID)
        {
            var BL = new Products();
            var Result = BL.FilterByCategoryID(ID);
            return Result;
        }

        [HttpGet]
        public Product RetrieveProductByID(int ID)
        {
            var BL = new Products();
            var Result = BL.RetrieveByID(ID);
            return Result;
        }

        #region NUEVOS DAVE

        [HttpGet]
        public IHttpActionResult GetProduct(int id)
        {
            var BL = new Products();
            var product = BL.RetrieveByID(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetProductAsync(int id)
        {
            return await Task.FromResult(GetProduct(id));
        }

        public HttpResponseMessage Get(int id)
        {
            var BL = new Products();
            var product = BL.RetrieveByID(id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(product);
        }
        #endregion

        [HttpPost]
        public bool UpdateProduct(Product productToUpdate)
        {
            var BL = new Products();
            var Result = BL.Update(productToUpdate);
            return Result;
        }

    }
}
