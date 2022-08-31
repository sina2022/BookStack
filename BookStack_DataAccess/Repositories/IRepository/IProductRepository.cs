using BookStack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStack_DataAccess.Repositories.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetT(Func<object, bool> value);
        void Update(Product product);
    }
}
