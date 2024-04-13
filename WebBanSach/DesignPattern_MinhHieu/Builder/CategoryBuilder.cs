using BanSach.Models;

namespace WebBanSach.DesignPattern_MinhHieu.Builder
{
    public class CategoryBuilder
    {
        private Category _category = new Category();

        public CategoryBuilder BuilderWithName(string name)
        {
            _category.Name = name;
           
        }

        public CategoryBuilder BuilderWithDisplayOrder(int displayOrder)
        {
            _category.DisplayOrder = displayOrder;
            return this;
        }

        public CategoryBuilder BuilderWithCreateDatetimer(DateTime date)
        {
            _category.CreateDatetime = date;
            return this;
        }

        public Category Build()
        {
            return _category;
        }
    }
}
