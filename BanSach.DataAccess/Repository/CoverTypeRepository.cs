using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.EntityFrameworkCore;

namespace BanSach.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public CoverTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            //_db.Products.Include(u => u.Category).Include(u =>u.CoverType); //load Obj Category trong Product.cs
        }

        public void Update(CoverType coverType)
        {
            _db.CoverTypes.Update(coverType);
        }

    }
}
