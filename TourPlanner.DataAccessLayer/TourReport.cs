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

namespace TourPlanner.DataAccessLayer
{
    public class TourReport : IDocument
    {
        public TourItem Tour { get; }
        public List<TourLogItem> TourLogs;
        public object Model { get; private set; }

        public TourReport(TourItem tour, List<TourLogItem> logs)
        {
            Tour = tour;
            TourLogs = logs;
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
                    page.Content(ComposeContent);
                });

        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Element().Text($"Tour Report");
                    stack.Element().Text($"Name: {Tour.Name}");
                    stack.Element().Text($"Description: {Tour.Description}");
                    stack.Element().Text($"Start: {Tour.From}");
                    stack.Element().Text($"End: {Tour.To}");
                    stack.Element().Text($"Distance: {Tour.Distance}");
                });

            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).PageableStack(stack =>
            {
                stack.Spacing(5);
                stack.Element().Text($"Tour Log: ");
                stack.Element(ComposeTable);
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Section(section =>
            {
                section.Header().BorderBottom(1).Padding(5).Row(row =>
                {
                    row.RelativeColumn().Text("Date");
                    row.RelativeColumn().Text("Duration");
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
                                row.RelativeColumn().Text(log.Duration);
                                row.RelativeColumn().AlignRight().Text(log.Report);
                                row.RelativeColumn().AlignRight().Text(log.Rating);
                            });
                        }
                    });
            });
        }




    }
}

