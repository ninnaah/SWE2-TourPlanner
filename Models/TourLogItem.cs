using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourLogItem
    {
        public string TourName { get; set; }
        public DateTime Date { get; set; }
        public string TransportMode { get; set; }
        public float Distance { get; set; } //mapQuest?
        public float Duration { get; set; } //mapQuest?
        public float AverageSpeed { get; set; } //mapQuest
        public float FuelUsed { get; set; } //mapQuest
        public string Weather { get; set; }
        public int Effort { get; set; }
        public string Report { get; set; }
        public int Rating { get; set; }

        public TourLogItem(string name, DateTime date, string mode, float distance, float duration, string report, int rating, float speed, float fuel, string weather, int effort)
        {
            TourName = name;
            Date = date;
            TransportMode = mode;
            Distance = distance;
            Duration = duration;
            Report = report;
            Rating = rating;
            AverageSpeed = speed;
            FuelUsed = fuel;
            Weather = weather;
            Effort=effort;
            
        }
        public TourLogItem(string name, DateTime date, string mode, string report, int rating, string weather, int effort)
        {
            TourName = name;
            Date = date;
            TransportMode = mode;
            Report = report;
            Rating = rating;
            Weather = weather;
            Effort = effort;
        }

        public TourLogItem()
        {

        }
    }
}
