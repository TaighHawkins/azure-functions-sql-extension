﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Description;
using static Microsoft.Azure.WebJobs.Extensions.Sql.SqlConverters;
using static Microsoft.Azure.WebJobs.Extensions.Sql.Telemetry.Telemetry;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.Sql
{
    /// <summary>
    /// Exposes SQL input and output bindings
    /// </summary>
    [Extension("sql")]
    internal class SqlBindingConfigProvider : IExtensionConfigProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBindingConfigProvider"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either parameter is null
        /// </exception>
        public SqlBindingConfigProvider(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Initializes the SQL binding rules
        /// </summary>
        /// <param name="context"> The config context </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if context is null
        /// </exception>
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            ILogger logger = this._loggerFactory.CreateLogger(LogCategories.Bindings);
            TelemetryInstance.Initialize(this._configuration, logger);
            LogDependentAssemblyVersions(logger);
#pragma warning disable CS0618 // Fine to use this for our stuff
            FluentBindingRule<SqlAttribute> inputOutputRule = context.AddBindingRule<SqlAttribute>();
            var converter = new SqlConverter(this._configuration);
            inputOutputRule.BindToInput(converter);
            inputOutputRule.BindToInput<string>(typeof(SqlGenericsConverter<string>), this._configuration, logger);
            inputOutputRule.BindToCollector<SQLObjectOpenType>(typeof(SqlAsyncCollectorBuilder<>), this._configuration, logger);
            inputOutputRule.BindToInput<OpenType>(typeof(SqlGenericsConverter<>), this._configuration, logger);
        }

        private static readonly Assembly[] _dependentAssemblies = {
            typeof(SqlConnection).Assembly, // Microsoft.Data.SqlClient
            typeof(JsonConvert).Assembly // Newtonsoft.Json
        };

        /// <summary>
        /// Log the versions of important dependent assemblies for troubleshooting support. The Azure Functions host skips checking
        /// versions for most assemblies loaded so to allow customers to bring their own versions of assemblies and not have conflicts,
        /// but this may end up causing issues in our extension so we log the version loaded of some critical dependencies to ensure
        /// the version is expected.
        /// </summary>
        private static void LogDependentAssemblyVersions(ILogger logger)
        {
            foreach (Assembly assembly in _dependentAssemblies)
            {
                try
                {
                    logger.LogInformation($"Using {assembly.GetName().Name} {FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion}");
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Error logging version for assembly {assembly.FullName}. {ex}");
                }

            }
        }
    }

    /// <summary>
    /// Wrapper around OpenType to receive data correctly from output bindings (not as byte[])
    /// This can be used for general "T --> JObject" bindings.
    /// The exact definition here comes from the WebJobs v1.0 Queue binding.
    /// refer https://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs.Host/Bindings/OpenType.cs#L390
    /// </summary>
    internal class SQLObjectOpenType : OpenType.Poco
    {
        // return true when type is an "System.Object" to enable Object binding.
        public override bool IsMatch(Type type, OpenTypeMatchContext context)
        {
            if (type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return false;
            }

            if (type.FullName == "System.Object")
            {
                return true;
            }

            return base.IsMatch(type, context);
        }
    }
}