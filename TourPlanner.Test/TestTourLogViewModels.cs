using NUnit.Framework;
using System;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TestTourLogViewModels
    {
        private TourItem _currentTour;
        private AddTourLogViewModel _VM;

        [SetUp]
        public void SetUp()
        {
            _currentTour = new TourItem("WienBerlin", "nice tour", "Wien", "Berlin", "Car", 1111, 111, 11, null);
            _VM = new AddTourLogViewModel(_currentTour);
        }

        [Test]
        public void TestInputTourLogDistance_InvalidValue()
        {
            try
            {
                _VM.TourLogDistance = 1171;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Distance should be about (+-50 km) the same as the tour");
            }
        }

        [Test]
        public void TestInputTourLogDistance_ValidValue()
        {
            try
            {
                _VM.TourLogDistance = 1131;
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourLogDuration_InvalidValue()
        {
            try
            {
                _VM.TourLogDuration = 240;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Duration should be about (+-120 min) the same as the tour");
            }
        }

        [Test]
        public void TestInputTourLogDuration_ValidValue()
        {
            try
            {
                _VM.TourLogDuration = 121;
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourLogFuelUsed_InvalidValue()
        {
            try
            {
                _VM.TourLogFuelUsed = 31;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Used fuel should be about (+-10 liter) the same as the tour");
            }
        }

        [Test]
        public void TestInputTourLogFuelUsed_ValidValue()
        {
            try
            {
                _VM.TourLogFuelUsed = 10;
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourLogWeather_InvalidValue()
        {
            try
            {
                _VM.TourLogWeather = "su";
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Weather should be between 5 and 10 characters long");
            }
        }

        [Test]
        public void TestInputTourLogWeather_ValidValue()
        {
            try
            {
                _VM.TourLogWeather = "sunny";
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourLogTemperature_InvalidValue()
        {
            try
            {
                _VM.TourLogTemperature = -30;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Temperature should be between -20 and 40 °C");
            }
        }

        [Test]
        public void TestInputTourLogTemperature_ValidValue()
        {
            try
            {
                _VM.TourLogTemperature = 20;
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourLogReport_InvalidValue()
        {
            try
            {
                _VM.TourLogReport = "aw";
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Report should be between 5 and 50 characters long");
            }
        }

        [Test]
        public void TestInputTourLogReport_ValidValue()
        {
            try
            {
                _VM.TourLogReport = "awesome tour";
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }
    }
}
