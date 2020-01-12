using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using FastMember;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static T ConvertToObject<T>(this SqlDataReader rd) where T : class, new()
        {
            var type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (var i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    var fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return t;
        }

        public static object ConvertToObject(this SqlDataReader rd)
        {
            dynamic expando = new ExpandoObject();
            var obj = expando as IDictionary<String, object>;
            for (var i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    var fieldName = rd.GetName(i);
                    
                   obj[fieldName] = rd.GetValue(i);;    
                }
            }

            return obj;
        }
    }
}