using BookStack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStack_DataAccess.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
         void Update(Category category);
    }
}
