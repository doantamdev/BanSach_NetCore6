using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.EntityFrameworkCore;

namespace WebBanSach.DesignPattern_Tam.Command
{
    public class DeleteCommand<T> : ICommand<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _itemId;
        private Category _item;

        public DeleteCommand(ApplicationDbContext db, int itemId)
        {
            _db = db;
            _itemId =itemId;
        }
            public DeleteCommand(ApplicationDbContext db, int itemId, IUnitOfWork unitOfWork)
        {
            _db = db;
            _itemId = itemId;   
            _unitOfWork = unitOfWork;
        }

        public void Execute()
        {
            _item = _db.Set<Category>().Find(_itemId);
            if (_item != null)
            {
                _db.Set<Category>().Remove(_item);
                _db.SaveChanges();
            }
        }

        public void Undo()
        {
            if (_item != null)
            {
                _unitOfWork.Category.Add(_item);
                _unitOfWork.Save();
            }
        }
    }
}
