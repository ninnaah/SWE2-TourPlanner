using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourItemDirection
    {
        public string Text { get; set; }
        public string Image { get; set; }

        public TourItemDirection(string text, string image)
        {
            Text = text;
            Image = image;
        }
        public TourItemDirection()
        {
        }
    }
}
