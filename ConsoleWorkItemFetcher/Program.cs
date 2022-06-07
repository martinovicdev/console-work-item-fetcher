using Microsoft.Extensions.Configuration;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using ConsoleWorkItemFetcher.Entities;

namespace ConsoleWorkItemFetcher;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("azure-config.json", false);

        IConfiguration config = builder.Build();
        var azureData = config.GetSection("AzureData").Get<AzureData>();

        var query = WiqlQueries.WiqlActive;
        switch (args.Length)
        {
            case 0:
                Console.WriteLine("No user-defined Work Item Query. Getting all active Work Items.");
                break;
            case > 0:
                switch (args[0])
                {
                    case "active":
                        query = WiqlQueries.WiqlActive;
                        Console.WriteLine("Active Work items:");
                        break;

                    case "closed":
                        query = WiqlQueries.WiqlClosed;
                        Console.WriteLine("Closed Work items:");
                        break;

                    default:
                        query = WiqlQueries.WiqlActive;
                        Console.WriteLine("Query not recognised. Getting active Work items:");
                        break;
                }

                break;
        }

        var list = GetWorkItems(query, azureData.Url ?? string.Empty, azureData.PersonalAccessToken ?? string.Empty);
        ShowActiveItemsInConsole(list);
    }

    private static void ShowActiveItemsInConsole(List<WorkItem> workItems)
    {
        if (workItems.Count == 0) Console.WriteLine("No items to show.");
        foreach (var item in workItems)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Work item ID: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(item.Fields["System.Id"]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" -- Title: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(item.Fields["System.Title"]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" -- State: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(item.Fields["System.State"] + "\n");
        }
    }

    private static List<WorkItem> GetWorkItems(Wiql query, string url, string pat)
    {
        var orgUrl = new Uri(url);
        var connection = new VssConnection(orgUrl, new VssBasicCredential(string.Empty, pat));
        var workItems = new List<WorkItem>();

        using var witClient = connection.GetClient<WorkItemTrackingHttpClient>();
        var workItemQueryResult = witClient.QueryByWiqlAsync(query).Result;

        if (!workItemQueryResult.WorkItems.Any()) return workItems;
        var arr = workItemQueryResult.WorkItems.Select(item => item.Id).ToArray();

        var fields = new string[5];
        fields[0] = "System.Id";
        fields[1] = "System.State";
        fields[2] = "System.Title";
        fields[3] = "System.AssignedTo";
        fields[4] = "Microsoft.VSTS.Scheduling.StartDate";

        workItems = witClient.GetWorkItemsAsync(arr, fields, workItemQueryResult.AsOf).Result;

        return workItems;
    }
}