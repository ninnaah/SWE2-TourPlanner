using System;

namespace TourPlanner.Models
{
    public class TourItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransportMode { get; set; }
        public float Distance { get; set; } //mapQuest
        public float Duration { get; set; } //mapQuest
        public float FuelUsed { get; set; } //mapQuest
  

        public TourItem(string name, string description, string from, string to, string mode)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportMode = mode;
        }

        public TourItem(string name, string description, string from, string to, string mode, float distance, float duration, float fuelUsed)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportMode = mode;
            Distance = distance;
            Duration = duration;
            FuelUsed = fuelUsed;
        }

        public TourItem()
        {

        }

    }
}
