using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db) //Passing to the base class that is Repository.cs
        {

            _db = db;
        }



        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(Category obj)
        {
            throw new NotImplementedException();
        }
    }
}
