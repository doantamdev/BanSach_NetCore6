using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.EntityFrameworkCore;

namespace BanSach.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            //_db.Products.Include(u => u.Category).Include(u =>u.CoverType); //load Obj Category trong Product.cs
        }

        public void Update(Company company)
        {
            throw new NotImplementedException();
        }
    }
}
