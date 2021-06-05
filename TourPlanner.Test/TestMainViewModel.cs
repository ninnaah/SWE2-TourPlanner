using NUnit.Framework;
using System.Collections.ObjectModel;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TestMainViewModel
    {
        private ObservableCollection<TourItem> _tours;

        [SetUp]
        public void SetUp()
        {
            _tours = new ObservableCollection<TourItem>(){
                new TourItem ("WienBerlin", "nice tour", "Wien", "Berlin", "Car"),
                new TourItem ("WienPrag", "awsome tour", "Wien", "Prag", "Car"),
                new TourItem ("WienKlagenfurt", "cool tour", "Wien", "Klagenfurt", "Car")
            };
        }

        [Test]
        public void TestCheckIfTourNameExists_NameDoesntExist()
        {
            MainViewModel VM = new MainViewModel(_tours);

            bool expectedResult = false;
            bool result = VM.CheckIfTourNameExists("WienBerlin");

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TestCheckIfTourNameExists_NameExists()
        {
            MainViewModel VM = new MainViewModel(_tours);

            bool expectedResult = true;
            bool result = VM.CheckIfTourNameExists("WienLinz");

            Assert.AreEqual(expectedResult, result);
        }
    }
}