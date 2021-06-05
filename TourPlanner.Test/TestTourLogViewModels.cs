using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TestTourLogViewModels
    {
        private TourItem _currentTour;

        [SetUp]
        public void SetUp()
        {
            _currentTour = new TourItem("WienBerlin", "nice tour", "Wien", "Berlin", "Car", 1111, 111, 11, null);
        }

        [Test]
        public void TestInputTourName_InvalidLength()
        {
            try
            {
                AddTourLogViewModel VM = new AddTourLogViewModel(_currentTour);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

        }
    }
}
