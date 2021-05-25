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
        public float Distance { get; set; }
        public float Duration { get; set; }
        public float AverageSpeed { get; set; } //calculate
        public float FuelUsed { get; set; }
        public string Weather { get; set; }
        public float Temperature { get; set; }
        public int Effort { get; set; }
        public string Report { get; set; }
        public int Rating { get; set; }

        public TourLogItem(string name, DateTime date, float distance, float duration, string report, int rating, float fuel, string weather, float temperature, int effort)
        {
            TourName = name;
            Date = date;
            Distance = distance;
            Duration = duration;
            Report = report;
            Rating = rating;
            AverageSpeed = (distance/(duration/60));
            FuelUsed = fuel;
            Weather = weather;
            Temperature = temperature;
            Effort=effort;
            
        }

        public TourLogItem()
        {

        }
    }
}
