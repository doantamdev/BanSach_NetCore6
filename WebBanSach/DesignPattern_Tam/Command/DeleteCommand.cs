using BanSach.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace WebBanSach.DesignPattern_Tam.Command
{
    public class DeleteCommand<T> : ICommand<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly int _itemId;
        private T _item;

        public DeleteCommand(ApplicationDbContext db, int itemId)
        {
            _db = db;
            _itemId = itemId;
        }

        public void Execute()
        {
            _item = _db.Set<T>().Find(_itemId);
            if (_item != null)
            {
                _db.Set<T>().Remove(_item);
                _db.SaveChanges();
            }
        }

         public void Undo()
        {
            if (_item != null)
            {
                using (var undo = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
                {
                    undo.Set<T>().Add(_item);
                    undo.SaveChanges();
                }
            }
        }
    }
}
