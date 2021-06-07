using NUnit.Framework;
using TourPlanner.Models;
using System.IO;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace TourPlanner.Test
{
    public class TestFileSystem
    {
        DataAccessLayer.FileSystem _fileSystem;
        List<TourLogItem> _logs;
        List<TourItem> _tours;

        [SetUp]
        public void SetUp()
        {
            _fileSystem = new DataAccessLayer.FileSystem(); 
            _fileSystem.FilePath = "../../../tours/";

            _logs = new List<TourLogItem>(){
                new TourLogItem ("WienBerlin", DateTime.Now, 1111, 111, "cool tour", 5, 30, "sunny", 20, 2),
                new TourLogItem ("WienBerlin", DateTime.Now, 1121, 121, "nice tour", 4, 25, "cloudy", 15, 2),
                new TourLogItem ("WienBerlin", DateTime.Now, 1131, 131, "awesome tour", 5, 20, "ok", 20, 3)
            };

            _tours = new List<TourItem>(){
                new TourItem ("WienBerlin", "nice tour", "Wien", "Berlin", "Car"),
                new TourItem ("WienPrag", "awsome tour", "Wien", "Prag", "Car"),
                new TourItem ("WienKlagenfurt", "cool tour", "Wien", "Klagenfurt", "Car")
            };
        }

        [Test]
        public void TestCreateTourReportPDF_FileExists()
        {
            TourItem tour = new TourItem("WienBerlin", "nice tour", "Wien", "Berlin", "Car");

            //create empty tourMap 
            Bitmap tourMap = new Bitmap(300, 300);
            tourMap.Save($"{_fileSystem.FilePath}/maps/{tour.Name}.png", ImageFormat.Png);

            _fileSystem.CreateTourReportPDF(tour, _logs);

            if (File.Exists($"{tour.Name}.pdf"))
            {
                File.Delete($"{ tour.Name}.pdf");
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void TestCreateSummarizeReportPDF_FileExists()
        {
            _fileSystem.CreateSummarizeReportPDF(_logs);

            if (File.Exists("summarizeReport.pdf"))
            {
                File.Delete("summarizeReport.pdf");
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void TestExportTours_FileExists()
        {

            _fileSystem.ExportTours(_tours);

            if (File.Exists("tours.json"))
            {
                //File.Delete("tours.json");
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void TestImportTours_ValidTours()
        {
            bool listsAreEqual = false;
            IEnumerable<TourItem> importedTours = _fileSystem.ImportTours("tours.json");

            foreach (TourItem impTour in importedTours)
            {
                foreach (TourItem tour in _tours)
                {
                    if (tour.Name == impTour.Name)
                    {
                        listsAreEqual = true;
                    }
                }
            }

            File.Delete("tours.json");

            if(listsAreEqual == true)
                Assert.Pass();
        }

        [Test]
        public void TestSaveTourDirection_FileExists()
        {
            TourItem tour = new TourItem("WienBerlin", "nice tour", "Wien", "Berlin", "Car");
            tour.Direction = new List<TourItemDirection>()
            {
                new TourItemDirection("an instruction", "an imagePath"),
                new TourItemDirection("another instruction", "another imagePath")
            };

            _fileSystem.SaveTourDirection(tour);

            if (File.Exists($"{_fileSystem.FilePath}direction/{tour.Name}.json"))
            {
                File.Delete($"{_fileSystem.FilePath}direction/{tour.Name}.json");

                foreach (Process p in Process.GetProcessesByName("explorer"))
                {
                    p.Kill();
                }

                Assert.Pass();
            }

            Assert.Fail();
        }


    }
}
