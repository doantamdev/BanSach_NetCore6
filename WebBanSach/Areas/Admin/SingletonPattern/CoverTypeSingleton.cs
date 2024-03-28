using BanSach.Models;

namespace WebBanSach.Areas.Admin.SingletonPattern
{
    public class CoverTypeSingleton
    {
        private static CoverTypeSingleton? _instance;

        private static readonly object LockObject = new object();

        private CoverType _CoverType;

        private CoverTypeSingleton() { }

        public static CoverTypeSingleton Instance
        {
            get
            {
                // Sử dụng double-checked locking để đảm bảo hiệu suất và đồng bộ hóa
                if (_instance == null)
                {
                    lock (LockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new CoverTypeSingleton();
                        }
                    }
                }

                return _instance;
            }
        }

        public CoverType CoverTypeInstance
        {
            get { return _CoverType; }
            set
            {
                _CoverType = value;
            }
        }
    }
}
