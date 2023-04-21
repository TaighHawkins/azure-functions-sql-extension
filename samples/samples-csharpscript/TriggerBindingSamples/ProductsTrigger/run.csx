#load "../../Common/product.csx"
#r "Newtonsoft.Json"
#r "Microsoft.Azure.WebJobs.Extensions.Sql"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.Sql;

public static void Run(IReadOnlyList<SqlChange<Product>> changes, ILogger log)
{
    log.LogInformation($"SQL Changes: {changes.Count}");
    foreach (SqlChange<Product> change in changes)
    {
        // The output is used to inspect the trigger binding parameter in test methods.
        log.LogInformation("SQL Change: " + JsonConvert.SerializeObject(change));
    }
}