
namespace TourPlanner.BusinessLayer
{
    //Singleton
    public static class TourItemFactory
    {
        private static ITourItemFactory _instance;
        public static ITourItemFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TourItemFactoryImpl();
            }
            return _instance;
        }
    }
}
