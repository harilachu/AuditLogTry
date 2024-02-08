using Audit.Core;
using Audit.Core.Providers;
using ConsoleAppTry;
using System.Runtime.CompilerServices;
using System.Text.Json;

// See https://aka.ms/new-console-template for more information


string? apiKeyName = string.Empty;

List<ChurnApiKey> churnApiKeyList = ApiKeyFactory.CreateApiKeys();

apiKeyName = GetApiKeyName(apiKeyName);

if (!string.IsNullOrEmpty(apiKeyName))
{
    var apiKey = churnApiKeyList.FirstOrDefault(x => x.Name == apiKeyName);
    PrintApiKey(apiKey);

    IAuditService auditService = new AuditService();
    UpdateApiKeyData(apiKey, auditService);

    //Delete ApiKey 
    RemoveApiKeyData(churnApiKeyList, apiKey, auditService);

}

Console.ReadLine();


void PrintApiKey(ChurnApiKey? apiKey)
{
    if (apiKey == null) return;
    Console.WriteLine(JsonSerializer.Serialize<ChurnApiKey>(apiKey).ToString());
}

static string GetApiKeyName(string? apiKeyName)
{
    while (string.IsNullOrEmpty(apiKeyName))
    {
        Console.Write("Enter the API Key name: ");
        apiKeyName = Console.ReadLine();
        if (string.IsNullOrEmpty(apiKeyName))
            Console.WriteLine("API key should not be empty.");
        else if (apiKeyName.Equals("exit"))
            break;
        else
            break;
    }

    return apiKeyName;
}

static void UpdateApiKey(ChurnApiKey apiKey, bool isActive, bool regenerateApiKey)
{
    if (apiKey == null) return;
    
    apiKey.IsActive = isActive;

    if (regenerateApiKey)
        apiKey.ApiKey = Guid.NewGuid();
}

static void DeleteApiKey(List<ChurnApiKey> churnApiKeyList, ChurnApiKey apiKey)
{
    churnApiKeyList.Remove(apiKey);
}

static void SaveAudit(IAuditService auditService)
{
    auditService.SaveAudit();
    auditService.DisposeAudit();
}

static void RemoveApiKeyData(List<ChurnApiKey> churnApiKeyList, ChurnApiKey? apiKey, IAuditService auditService)
{
    int retryCount;
    Console.Write("Delete API Key? : ");
    bool deleteApiKey = false;
    retryCount = 1;
    while (!bool.TryParse(Console.ReadLine(), out deleteApiKey))
    {
        Console.Write("Enter valid Delete API Key (bool): ");
        if (retryCount++ == 3)
            break;
    }

    if (deleteApiKey)
    {
        auditService.CreateAuditScope("APIKey", apiKey, Status.Deleted);
        DeleteApiKey(churnApiKeyList, apiKey);
        SaveAudit(auditService);
    }
}

static void UpdateApiKeyData(ChurnApiKey? apiKey, IAuditService auditService)
{
    auditService.CreateAuditScope("APIKey", apiKey, Status.Updated);

    bool isActive = apiKey.IsActive;
    Console.Write("Enter IsActive: ");

    int retryCount = 1;
    while (!bool.TryParse(Console.ReadLine(), out isActive))
    {
        Console.Write("Enter valid IsActive (bool): ");
        if (retryCount++ == 3)
            break;
    }

    bool regenerateApiKey = false;
    Console.Write("Enter Regenerate Api Key: ");
    retryCount = 1;
    while (!bool.TryParse(Console.ReadLine(), out regenerateApiKey))
    {
        Console.Write("Enter valid Regenerate Api Key (bool): ");
        if (retryCount++ == 3)
            break;
    }

    //Update ApiKey
    UpdateApiKey(apiKey, isActive, regenerateApiKey);
    SaveAudit(auditService);
}