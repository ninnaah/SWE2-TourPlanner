using NUnit.Framework;
using System.Collections.ObjectModel;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TestMainViewModel
    {
        private ObservableCollection<TourItem> _tours;
        private MainViewModel _VM;

        [SetUp]
        public void SetUp()
        {
            _tours = new ObservableCollection<TourItem>(){
                new TourItem ("WienBerlin", "nice tour", "Wien", "Berlin", "Car"),
                new TourItem ("WienPrag", "awsome tour", "Wien", "Prag", "Car"),
                new TourItem ("WienKlagenfurt", "cool tour", "Wien", "Klagenfurt", "Car")
            };

            _VM = new MainViewModel(_tours);
        }

        [Test]
        public void TestCheckIfTourNameExists_NameDoesntExist()
        {
            bool expectedResult = false;
            bool result = _VM.CheckIfTourNameExists("WienBerlin");

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TestCheckIfTourNameExists_NameExists()
        {
            bool expectedResult = true;
            bool result = _VM.CheckIfTourNameExists("WienLinz");

            Assert.AreEqual(expectedResult, result);
        }

    }
}