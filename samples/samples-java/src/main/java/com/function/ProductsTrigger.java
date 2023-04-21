/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for
 * license information.
 */

package com.function;

import com.microsoft.azure.functions.ExecutionContext;
import com.microsoft.azure.functions.annotation.FunctionName;
import com.microsoft.azure.functions.sql.annotation.SQLTrigger;
import com.function.Common.SqlChangeProduct;
import com.google.gson.Gson;

import java.util.logging.*;

public class ProductsTrigger {
    @FunctionName("ProductsTrigger")
    public void run(
            @SQLTrigger(
                name = "changes",
                tableName = "[dbo].[Products]",
                connectionStringSetting = "SqlConnectionString")
                SqlChangeProduct[] changes,
            ExecutionContext context) {
        Gson gson = new Gson();
        Logger logger = context.getLogger();
        logger.log(Level.INFO, "SQL Changes: " + changes.length);
        for (SqlChangeProduct change : changes) {
            // The output is used to inspect the trigger binding parameter in test methods.
            logger.log(Level.INFO, "SQL Change: " + gson.toJson(change));
        }
    }
}