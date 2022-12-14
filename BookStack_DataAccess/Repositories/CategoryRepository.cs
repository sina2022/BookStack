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
    public class CategoryRepository : Repository<Category>,ICategoryRepository
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(Category category)
        {
            var categoryDb = _context.Categories.FirstOrDefault(x => x.Id==category.Id);

            if(categoryDb != null)
            {
                categoryDb.Name = category.Name;
                categoryDb.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
