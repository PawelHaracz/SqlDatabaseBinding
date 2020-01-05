using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase
{
#pragma warning disable 618
    public class DbSqlResolutionPolicy : IResolutionPolicy
#pragma warning restore 618
    {
        public string TemplateBind(PropertyInfo propInfo, Attribute resolvedAttribute, BindingTemplate bindingTemplate,
            IReadOnlyDictionary<string, object> bindingData)
        {
            if (bindingTemplate == null)
            {
                throw new ArgumentNullException(nameof(bindingTemplate));
            }

            if (bindingData == null)
            {
                throw new ArgumentNullException(nameof(bindingData));
            }

            var @attribute = resolvedAttribute as MsSqlDbAttribute;

            if (attribute is null)
            {
                throw new NotSupportedException($"This policy is only supported for {nameof(MsSqlDbAttribute)}.");
            }
            
            var paramCollection = new List<SqlParameter>();
            
            var replacements = new Dictionary<string, object>();
            foreach (var token in bindingTemplate.ParameterNames.Distinct())
            {
                var sqlToken = $"@{token}";
                paramCollection.Add(new SqlParameter(sqlToken, bindingData[token]));
                replacements.Add(token, sqlToken);
            }

            attribute.SqlQueryParameters = paramCollection.AsEnumerable();
            
            var replacement = bindingTemplate.Bind(new ReadOnlyDictionary<string, object>(replacements));
            return replacement;
        }
    }
}