using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;

namespace WebBanSach.Areas.IteratorPattern
{
    public class CateIterator : IIterator<Category>
    {
        private IEnumerator<Category> _cates;
        private readonly IUnitOfWork _unitOfWork;

        public CateIterator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cates = _unitOfWork.Category.GetAll().GetEnumerator();
        }

        public bool HasNext()
        {
            return _cates.MoveNext();
        }

        public Category Next()
        {
            if(HasNext())
            {
                return _cates.Current;
            }

            return null;
        }
    }
}
