using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace ConsoleWorkItemFetcher.Entities;

public static class WiqlQueries
{
    static WiqlQueries()
    {
        WiqlClosed = new Wiql
        {
            Query = "Select [State], [Title], [Assigned To]" +
                    "From WorkItems " +
                    "Where [State] = 'Closed' " +
                    "Order by [Changed Date] Desc"
        };

        WiqlActive = new Wiql
        {
            Query = "Select [State], [Title], [Assigned To]" +
                    "From WorkItems " +
                    "Where [State] = 'Active' " +
                    "Order by [Changed Date] Desc"
        };
    }

    public static Wiql WiqlClosed { get; }
    public static Wiql WiqlActive { get; }
}