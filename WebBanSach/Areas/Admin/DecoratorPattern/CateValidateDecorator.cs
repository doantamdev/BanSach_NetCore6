using BanSach.Models;

namespace WebBanSach.Areas.Admin.DecoratorPattern
{
    public class CateValidateDecorator : BaseValidate
    {
        public CateValidateDecorator(IValidate component) : base(component)
        {

        }

        public bool CateValidate(Category cate)
        {
            if (!string.IsNullOrEmpty(cate.Name))
            {
                if (cate.Name.Length > 10)
                {
                    return false;
                }

                if (!int.TryParse(cate.DisplayOrder.ToString(), out _))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
