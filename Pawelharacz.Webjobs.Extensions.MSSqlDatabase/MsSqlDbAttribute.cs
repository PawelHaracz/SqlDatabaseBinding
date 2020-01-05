using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase
{
    /// <summary>
    /// Attribute used to bind to an Azure CosmosDB collection.
    /// </summary>
    /// <remarks>
    /// The method parameter type can be one of the following:
    /// <list type="bullet">
    /// <item><description><see cref="ICollector{T}"/></description></item>
    /// <item><description><see cref="IAsyncCollector{T}"/></description></item>
    /// <item><description>out T</description></item>
    /// <item><description>out T[]</description></item>
    /// </list>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public class MsSqlDbAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public MsSqlDbAttribute()
        {
            
        }
        
        /// <summary>
        /// Optional. A string value indicating the app setting to use as the MS sql connection string, if different
        /// than the one specified in the <see cref="MsSqlDbOptions"/>.
        /// </summary>
        [AppSetting]
        public string ConnectionStringSetting { get; set; }
        
        
        /// <summary>
        /// Optional.
        /// When specified on an input binding using an <see cref="IEnumerable{T}"/>, defines the query to run against the collection. 
        /// May include binding parameters.
        /// </summary>
        [AutoResolve(ResolutionPolicyType = typeof(DbSqlResolutionPolicy))]
        public string SqlQuery { get; set; }

        internal IEnumerable<SqlParameter> SqlQueryParameters { get; set; }
    }
}
