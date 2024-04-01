using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CompanyRepository(ApplicationDbContext db) : Repository<Company>(db), ICompanyRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(Company objCompany)
        {
            _db.Companies.Update(objCompany);
        }
    }
}
