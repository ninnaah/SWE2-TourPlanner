using NUnit.Framework;
using TourPlanner.BusinessLayer;

namespace TourPlanner.Test
{
    public class TestTourItemFactorycs
    {
        [Test]
        public void TestGetInstance_SameInstance()
        {
            ITourItemFactory tourFactory1 = TourItemFactory.GetInstance();
            ITourItemFactory tourFactory2 = TourItemFactory.GetInstance();

            Assert.AreEqual(tourFactory1, tourFactory2);
        }



    }
}
