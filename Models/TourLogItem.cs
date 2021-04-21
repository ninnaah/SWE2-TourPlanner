using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourLogItem
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public float Duration { get; set; }
        public float Distance { get; set; }
        public string Report { get; set; }
        public int Rating { get; set; }

        public TourLogItem(string name, DateTime date, float duration, float distance, string report, int rating)
        {
            Name = name;
            Date = date;
            Duration = duration;
            Distance = distance;
            Report = report;
           
            if(rating <= 5 && rating >= 1)
                Rating = rating;
            else
                Rating = 0;
            
        }
        public TourLogItem()
        {

        }
    }
}
