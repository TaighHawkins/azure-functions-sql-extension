// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using DotnetIsolatedTests.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;


namespace DotnetIsolatedTests
{
    /// <summary>
    /// Used to ensure correct functionality with multiple user functions tracking the same table.
    /// </summary>
    public static class MultiFunctionTrigger
    {

        [Function(nameof(MultiFunctionTrigger1))]
        public static void MultiFunctionTrigger1(
            [SqlTrigger("[dbo].[Products]", "SqlConnectionString")]
            IReadOnlyList<SqlChange<Product>> changes,
            FunctionContext context)
        {
            ILogger logger = context.GetLogger("MultiFunctionTrigger1");
            foreach (SqlChange<Product> change in changes)
            {
                // The output is used to inspect the trigger binding parameter in test methods.
                logger.LogInformation("Trigger1 Change: " + Utils.JsonSerializeObject(change));
            }
        }

        [Function(nameof(MultiFunctionTrigger2))]
        public static void MultiFunctionTrigger2(
            [SqlTrigger("[dbo].[Products]", "SqlConnectionString")]
            IReadOnlyList<SqlChange<Product>> changes,
            FunctionContext context)
        {
            ILogger logger = context.GetLogger("MultiFunctionTrigger2");
            foreach (SqlChange<Product> change in changes)
            {
                // The output is used to inspect the trigger binding parameter in test methods.
                logger.LogInformation("Trigger2 Change: " + Utils.JsonSerializeObject(change));
            }
        }
    }
}
