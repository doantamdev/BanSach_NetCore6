using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;

namespace WebBanSach.Areas.IteratorPattern
{
    public class CompanyIterator : IIterator<Company>
    {
        private List<Company> _companys;
        private int _index = 0;

        public CompanyIterator(IUnitOfWork unitOfWork)
        {
            _companys = unitOfWork.Company.GetAll().ToList();
        }

        public bool HasNext()
        {
            return _index < _companys.Count;
        }

        public Company Next()
        {
            if (HasNext())
            {
                Company company = _companys[_index];
                _index++;
                return company;
            }
            return null;
        }
    }
}
