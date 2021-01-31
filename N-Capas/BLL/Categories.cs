using DAL;
using Entities;

namespace BLL
{
    public class Categories
    {
        public Category Create(Category newCategory)
        {
            using (var r = RepositoryFactory.CreateRepository())
            {
                newCategory = r.Create(newCategory);
            }
            return newCategory;
        }
    }

}
