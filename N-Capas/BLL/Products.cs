using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;

namespace BLL
{
    public class Products
    {

        public Product Create(Product newProduct)
        {
            Product Result = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Buscar si el nombre de producto existe
                Product res = r.Retrieve<Product>(p => p.ProductName == newProduct.ProductName);
                if (res == null)
                {
                    // No existe, podemos crearlo
                    Result = r.Create(newProduct);
                }

                else
                {
                    // Podríamos aquí lanzar una excepción
                    // para notificar que el producto ya existe.
                    // Podríamos incluso crear una capa de Excepciones
                    // personalizadas y consumirla desde otras
                    // capas.
                }
            }
            return Result;
        }

        public Product RetrieveByID(int ID)
        {
            Product Result = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                Result = r.Retrieve<Product>(p => p.ProductID == ID);
            }
            return Result;
        }

        public bool Update(Product productToUpdate)
        {
            bool Result = false;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre de producto no exista
                Product temp =
                r.Retrieve<Product>(p => p.ProductName == productToUpdate.ProductName && p.ProductID != productToUpdate.ProductID);
                if (temp == null)
                {
                    // No existe
                    Result = r.Update(productToUpdate);
                }
                else
                {
                    // Podemos implementar alguna lógica para
                    // indicar que no se pudo modificar
                }
            }
            return Result;
        }

        public bool Delete(int ID)
        {
            bool Result = false;
            // Buscar el producto para ver si tiene existencias
            var Product = RetrieveByID(ID);
            if (Product != null)
            {
                //if (Product.UnitsInStock == 0)
                //{
                // Eliminar el producto
                using (var r = RepositoryFactory.CreateRepository())
                {
                    Result = r.Delete(Product);
                }
                //}
                //else
                //{
                //    // Podemos implementar alguna lógica adicional
                //    // para indicar que no se pudo eliminar el producto
                //}
            }
            else
            {
                // Podemos implementar alguna lógica
                // para indicar que el producto no existe
            }
            return Result;
        }
        public List<Product> FilterByCategoryID(int categoryID)
        {
            List<Product> Result = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                Result = r.Filter<Product>
                (p => p.CategoryID == categoryID);
            }
            return Result;
        }

    }
}
