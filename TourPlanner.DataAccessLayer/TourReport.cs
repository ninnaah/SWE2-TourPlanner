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
    public class TourReport : IDocument
    {
        public TourItem Tour { get; }
        public List<TourLogItem> TourLogs;
        public byte[] ImageData;

        public TourReport(TourItem tour, List<TourLogItem> logs, string filePath)
        {
            Tour = tour;
            TourLogs = logs;

            string fullFilePath = $"{filePath}/maps/{Tour.Name}.png";

            FileStream fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fullFilePath).Length;
            ImageData = br.ReadBytes((int)numBytes);

        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IContainer container)
        {
            container
                .PaddingHorizontal(50)
                .PaddingVertical(50)
                .Page(page =>
                {
                    page.Header(ComposeHeader);
                    page.Content(ComposeMap);
                    page.Footer(ComposeContent);
                });

        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Element().AlignCenter().Text($"Tour Report", TextStyle.Default.Size(20));
                    stack.Element().Text($"{Tour.Name}", TextStyle.Default.Size(16));
                    stack.Element().Text($"Description: {Tour.Description}");
                    stack.Element().Text($"Start: {Tour.From}");
                    stack.Element().Text($"End: {Tour.To}");
                    stack.Element().Text($"Distance: {Tour.Distance}");
                });

            });
        }

        void ComposeMap(IContainer container)
        {
            container.PaddingVertical(40).PageableStack(stack =>
            {
                stack.Spacing(5);
                stack.Element().Text($"Tour Map", TextStyle.Default.Size(20));
                stack.Element(ComposeImage);
            });

        }
        void ComposeImage(IContainer container)
        {
            container.Image(ImageData, ImageScaling.FitHeight);
        }

        void ComposeContent(IContainer container)
        {

            container.PaddingVertical(40).PageableStack(stack =>
            {
                stack.Spacing(5);
                stack.Element().Text($"Tour Logs", TextStyle.Default.Size(20));

                if (TourLogs.Count != 0)
                    stack.Element(ComposeTable);

                stack.Element().AlignRight().PageNumber("{number}");
            });
            

        }
        void ComposeTable(IContainer container)
        {
            container.Section(section =>
            {
                section.Header().BorderBottom(1).Padding(5).Row(row =>
                {
                    row.RelativeColumn().Text("Date");
                    row.RelativeColumn().Text("Transport mode");
                    row.RelativeColumn().Text("Duration");
                    row.RelativeColumn().Text("Average speed");
                    row.RelativeColumn().Text("Fuel used");
                    row.RelativeColumn().Text("Weather");
                    row.RelativeColumn().Text("Effort");
                    row.RelativeColumn().AlignRight().Text("Report");
                    row.RelativeColumn().AlignRight().Text("Rating");
                });

                section
                    .Content()
                    .PageableStack(stack =>
                    {
                        foreach (TourLogItem log in TourLogs)
                        {
                            stack.Element().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                            {
                                row.RelativeColumn().Text(log.Date);
                                row.RelativeColumn().Text(log.TransportMode);
                                row.RelativeColumn().Text(log.Duration);
                                row.RelativeColumn().Text(log.AverageSpeed);
                                row.RelativeColumn().Text(log.FuelUsed);
                                row.RelativeColumn().Text(log.Weather);
                                row.RelativeColumn().Text(log.Effort);
                                row.RelativeColumn().AlignRight().Text(log.Report);
                                row.RelativeColumn().AlignRight().Text(log.Rating);
                            });
                        }
                    });
            });
        }




    }
}

