using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TestTourViewModels
    {
        private ObservableCollection<TourItem> _tours;
        private AddTourViewModel _VM;

        [SetUp]
        public void SetUp()
        {
            _tours = new ObservableCollection<TourItem>(){
                new TourItem ("WienBerlin", "nice tour", "Wien", "Berlin", "Car"),
                new TourItem ("WienPrag", "awsome tour", "Wien", "Prag", "Car"),
                new TourItem ("WienKlagenfurt", "cool tour", "Wien", "Klagenfurt", "Car")
            };

            _VM = new AddTourViewModel(_tours);
        }

        [Test]
        public void TestInputTourName_InvalidLength()
        {
            try
            {
                _VM.TourName = "Wi";
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Tourname should be between 5 and 15 characters long");
            }
        }

        [Test]
        public void TestInputTourName_ValidLength()
        {
            try
            {
                _VM.TourName = "WienSalzburg";
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourDescription_InvalidLength()
        {
            try
            {
                _VM.TourDescription = "ni";
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Description should be between 5 and 50 characters long");
            }
        }

        [Test]
        public void TestInputTourDescription_ValidLength()
        {
            try
            {
                _VM.TourDescription = "nice tour";
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourFrom_InvalidLength()
        {
            try
            {
                _VM.TourFrom = "Wi";
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Starting point should be between 3 and 20 characters long");
            }
        }

        [Test]
        public void TestInputTourFrom_ValidLength()
        {
            try
            {
                _VM.TourFrom = "Wien";
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourTo_InvalidLength()
        {
            try
            {
                _VM.TourTo = "Sa";
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "End point should be between 3 and 20 characters long");
            }
        }

        [Test]
        public void TestInputTourTo_ValidLength()
        {
            try
            {
                _VM.TourTo = "WienSalzburg";
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourTransportMode_NotSet()
        {
            try
            {
                _VM.TourTransportMode = "";
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Please choose transport mode");
            }
        }

        [Test]
        public void TestInputTourTransportMode_Set()
        {
            try
            {
                _VM.TourTransportMode = "Car";
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }
    }
}
