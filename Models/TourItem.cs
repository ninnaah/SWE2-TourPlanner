using System;

namespace TourPlanner.Models
{
    public class TourItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Distance { get; set; }

        public TourItem(string name, string description, int distance)
        {
            Name = name;
            Description = description;
            Distance = distance;
        }

        public TourItem()
        {

        }

    }
}