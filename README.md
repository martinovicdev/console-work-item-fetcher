# Console Work Items Fetcher

This simple console app is a demonstration of how you can easily fetch work items from [Azure DevOps API](https://docs.microsoft.com/en-us/azure/devops/integrate/quickstarts/work-item-quickstart?view=azure-devops) from your console. 

Initial two modes are getting active items and getting closed items which are added as arguments when running the app from console. The default value is 'active'.

To try it out with your own configuration, go to azure-config.json and replace placeholder data in 

    "PersonalAccessToken": "your-personal-access-token",  
    "Url": "https://dev.azure.com/your-org-name" 

 with your PAT and URL.
