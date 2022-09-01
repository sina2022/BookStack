using BookStack_DataAccess.Data;
using BookStack_DataAccess.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStack_DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }

        //public IcartRepository cart { get; private set; }
        //public IApplicationUserRepository ApplicationUser { get; private set; }
        //public IOrderHeaderRepository OrderHeader { get; private set; }
        //public IOrderDetailRepository Product { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category=new CategoryRepository(context);
            Product=new ProductRepository(context);
            ShoppingCart = new ShoppingCartRepository(context);

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
