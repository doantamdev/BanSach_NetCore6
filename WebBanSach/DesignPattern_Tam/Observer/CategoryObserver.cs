namespace WebBanSach.DesignPattern_Tam.Observer
{
    public class CategoryObserver : ICategoryObserver
    {
        private readonly List<ICategoryObserver> _observers = new List<ICategoryObserver>();

        public void Add(ICategoryObserver observer)
        {
            _observers.Add(observer);
        }

        public void Remove(ICategoryObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }

        public void Update(string message)
        {
            Console.WriteLine("Received notification: " + message);
            // THÊM LỆNH LOG CHO KHUNG
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(message);
        }
    }
}
