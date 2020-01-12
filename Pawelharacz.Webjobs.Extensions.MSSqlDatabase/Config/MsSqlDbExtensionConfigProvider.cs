using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Bindings;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Service;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config
{
    /// <summary>
    /// Defines the configuration options for the Ms SQL database binding.
    /// </summary>
    [Extension("MsSqlDb")]
    internal class MsSqlDbExtensionConfigProvider : IExtensionConfigProvider
    {
        internal const string MsSqlDbConnectionStringName = "MsSqlDbConnectionString";
        private readonly MsSqlDbOptions _options;
        private readonly IMsSqlDbServiceFactory _msSqlDbServiceFactory;
        private readonly IConfiguration _configuration;
        private readonly INameResolver _nameResolver;
        private readonly ILoggerFactory _loggerFactory;
        private readonly string _defaultConnectionString;
        
        public MsSqlDbExtensionConfigProvider(IOptions<MsSqlDbOptions> options, 
            IMsSqlDbServiceFactory msSqlDbServiceFactory, IConfiguration configuration,
            INameResolver nameResolver, ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _msSqlDbServiceFactory = msSqlDbServiceFactory;
            _configuration = configuration;
            _nameResolver = nameResolver;
            _loggerFactory = loggerFactory;
            _defaultConnectionString = _nameResolver.Resolve(MsSqlDbConnectionStringName);
        }

        private ConcurrentDictionary<string, IMsSqlDbService> ClientCache { get; } = new ConcurrentDictionary<string, IMsSqlDbService>();
        
        /// <inheritdoc />
        public void Initialize(ExtensionConfigContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var rule = context.AddBindingRule<MsSqlDbAttribute>();
            rule.AddValidator(ValidateConnection);
            rule.WhenIsNotNull(nameof(MsSqlDbAttribute.SqlQuery));
           
            rule.BindToInput<IEnumerable<dynamic>>(typeof(MsSqlDbDynamicAsyncConverter), this);
            rule.BindToInput<IEnumerable<OpenType>>(typeof(MsSqlDbEnumerableAsyncConverter<>), this);
            rule.BindToInput<OpenType>(typeof(MsSqlDbAsyncConverter<>), this);
        }

        public MsSqlDbContext CreateContext(MsSqlDbAttribute input)
        {
            var resolvedConnectionString = ResolveConnectionString(input.ConnectionStringSetting);
            var service = GetService(resolvedConnectionString);
            
            return new MsSqlDbContext()
            {
                Attribute = input,
                MsSqlDbService = service
            };
        }
        
        private void ValidateConnection(MsSqlDbAttribute attribute, Type paramType)
        {
            if (string.IsNullOrEmpty(_options.ConnectionString) &&
                string.IsNullOrEmpty(attribute.ConnectionStringSetting) &&
                string.IsNullOrWhiteSpace(_defaultConnectionString))
            {
                var attributeProperty = $"{nameof(MsSqlDbAttribute)}.{nameof(MsSqlDbAttribute.ConnectionStringSetting)}";
                var optionsProperty = $"{nameof(MsSqlDbOptions)}.{nameof(MsSqlDbOptions.ConnectionString)}";
                throw new InvalidOperationException(
                    $"The CosmosDB connection string must be set either via the '{Constants.DefaultConnectionStringName}' IConfiguration connection string, via the {attributeProperty} property or via {optionsProperty}.");
            }
            
        }

        private string ResolveConnectionString(string attributeConnectionString)
        {
            // First, try the Attribute's string.
            if (!string.IsNullOrEmpty(attributeConnectionString))
            {
                return attributeConnectionString;
            }

            // Second, try the config's ConnectionString
            if (!string.IsNullOrEmpty(_options.ConnectionString))
            {
                return _options.ConnectionString;
            }

            // Finally, fall back to the default.
            return _defaultConnectionString;
        }
            
        
        private IMsSqlDbService GetService(string connectionString)
        {
            return ClientCache.GetOrAdd(connectionString,
                key => _msSqlDbServiceFactory.CreateService(CreateConnection(key)));
        }

        private SqlConnectionStringBuilder CreateConnection(string key)
        {
            return new SqlConnectionStringBuilder(key)
            {
                ConnectRetryCount = 3
            };
        }
        
        
    }
}