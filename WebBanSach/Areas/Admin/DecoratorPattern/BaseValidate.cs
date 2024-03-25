using BanSach.Models;

namespace WebBanSach.Areas.Admin.DecoratorPattern
{
    public class BaseValidate : IValidate
    {
        private IValidate _component;

        public BaseValidate() { }

        public BaseValidate(IValidate component)
        {
            _component = component;
        }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
