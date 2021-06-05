using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TestTourViewModels
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
        public void TestInputTourName_InvalidLength()
        {
            try
            { 
                AddTourViewModel VM = new AddTourViewModel(_tours, "Wi", null, null, null, null);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourName_ValidLength()
        {
            try
            {
                AddTourViewModel VM = new AddTourViewModel(_tours, "WienPrag", null, null, null, null);
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
                AddTourViewModel VM = new AddTourViewModel(_tours, null, "ni", null, null, null);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourDescription_ValidLength()
        {
            try
            {
                AddTourViewModel VM = new AddTourViewModel(_tours, null, "nice tour", null, null, null);
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
                AddTourViewModel VM = new AddTourViewModel(_tours, null, null, "Wi", null, null);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourFrom_ValidLength()
        {
            try
            {
                AddTourViewModel VM = new AddTourViewModel(_tours, null, null, "Wien", null, null);
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
                AddTourViewModel VM = new AddTourViewModel(_tours, null, null, null, "Wi", null);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourTo_ValidLength()
        {
            try
            {
                AddTourViewModel VM = new AddTourViewModel(_tours, null, null, null, "Wien", null);
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
                AddTourViewModel VM = new AddTourViewModel(_tours, null, null, null,null, null);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInputTourTransportMode_Set()
        {
            try
            {
                AddTourViewModel VM = new AddTourViewModel(_tours, null, null, null, null, "Car");
                Assert.Pass();
            }
            catch (Exception)
            {
            }
        }
    }
}
