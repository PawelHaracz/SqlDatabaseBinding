using System;
using System.Data.SqlClient;
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

            for (int i = 0; i < rd.FieldCount; i++)
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
    }
}