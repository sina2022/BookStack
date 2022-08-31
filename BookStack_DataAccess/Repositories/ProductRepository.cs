using BookStack_DataAccess.Data;
using BookStack_DataAccess.Repositories.IRepository;
using BookStack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStack_DataAccess.Repositories
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Product product)
        {
            var ProductDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if (ProductDb != null)
            {
                ProductDb.Name = product.Name;
                ProductDb.Description = product.Description;
                ProductDb.Price = product.Price;
                if(product.ImageUrl != null)
                {
                    ProductDb.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
