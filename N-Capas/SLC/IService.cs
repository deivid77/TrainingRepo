using Entities;
using System.Collections.Generic;

namespace SLC
{
    public interface IService
    {
        Product CreateProduct(Product newProduct);

        Product RetrieveProductByID(int ID);

        bool UpdateProduct(Product productToUpdate);

        bool DeleteProduct(int ID);

        List<Product> FilterProductsByCategoryID(int ID);

        Category CreateCategory(Category newCategory);
    }

}
