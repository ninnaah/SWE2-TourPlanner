using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Elements;
using QuestPDF.Helpers;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TourPlanner.Models;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace TourPlanner.DataAccessLayer
{
    public class SummarizeReport : IDocument
    {
        public float TotalTime;
        public float TotalDistance;

        public SummarizeReport(float totalTime, float totalDistance)
        {
            TotalTime = totalTime;
            TotalDistance = totalDistance;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IContainer container)
        {
            container
                .PaddingHorizontal(50)
                .PaddingVertical(50)
                .Page(page =>
                {
                    page.Content(ComposeContent);
                });

        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).PageableStack(stack =>
            {
                stack.Spacing(5);
                stack.Element().Text($"Summarize Report\n", TextStyle.Default.Size(20));
                stack.Element().Text("Total time: ");
                stack.Element().Text($"{TotalTime}");
                stack.Element().Text("Total distance: ");
                stack.Element().Text($"{TotalDistance}");
            });

        }
        


    }
}

