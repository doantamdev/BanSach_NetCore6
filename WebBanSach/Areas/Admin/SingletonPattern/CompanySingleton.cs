using BanSach.Models;

namespace WebBanSach.Areas.Admin.SingletonPattern
{
    public class CompanySingleton
    {
        private static CompanySingleton? _instance;
        private static readonly object LockObject = new object();

        // Đối tượng Company để lưu trữ thông tin công ty
        private Company _company;

        // Tạo một private constructor để ngăn chặn việc tạo thể hiện mới từ bên ngoài lớp
        private CompanySingleton() { }

        // Tạo một public static method để lấy thể hiện duy nhất của lớp
        public static CompanySingleton Instance
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
                            _instance = new CompanySingleton();
                        }
                    }
                }

                return _instance;
            }
        }

        public Company CompanyInstance
        {
            get { return _company; }
            set
            {
                _company = value;
            }
        }
    }
}
