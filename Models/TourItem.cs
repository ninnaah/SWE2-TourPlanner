using System;

namespace TourPlanner.Models
{
    public class TourItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public float Distance { get; set; }

        public TourItem(string name, string description, string from, string to, int distance)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            Distance = distance;
        }

        public TourItem()
        {

        }

    }
}
