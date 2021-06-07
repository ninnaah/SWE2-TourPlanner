using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

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
                stack.Element().Text("Total time (in min): ");
                stack.Element().Text($"{TotalTime}");
                stack.Element().Text("Total distance (in km): ");
                stack.Element().Text($"{TotalDistance}");
            });

        }
        


    }
}

