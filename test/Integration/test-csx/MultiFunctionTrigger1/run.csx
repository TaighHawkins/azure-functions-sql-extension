#load "../Common/Product.csx"
#r "Newtonsoft.Json"
#r "Microsoft.Azure.WebJobs.Extensions.Sql"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.Sql;

public static void Run(IReadOnlyList<SqlChange<Product>> changes, ILogger log)
{
    foreach (SqlChange<Product> change in changes)
    {
        // The output is used to inspect the trigger binding parameter in test methods.
        log.LogInformation("Trigger1 Changes: " + Microsoft.Azure.WebJobs.Extensions.Sql.Utils.JsonSerializeObject(changes));
    }
}