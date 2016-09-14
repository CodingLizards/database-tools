namespace Coding.Lizards.Database.Creation {
    using Attributes;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// This class generates create statements
    /// </summary>
    public static class SqliteGenerator {

        /// <summary>
        /// Generates create statements for all types that are decorated with the <see cref="Attributes.TableAttribute"/>
        /// </summary>
        /// <param name="asm">The assembly that should get scanned</param>
        /// <returns>The create statement</returns>
        public static string GetCreateScript(this Assembly asm) {
            var script = string.Empty;
            foreach (var item in asm.ExportedTypes.Where(c => c.GetTypeInfo().GetCustomAttribute<TableAttribute>() != null)) {
                script += item.GetCreateScript();
            }
            return script;
        }

        /// <summary>
        /// Generates a create statement for the given type
        /// </summary>
        /// <param name="type">The type that should be used</param>
        /// <returns>The create statement</returns>
        public static string GetCreateScript(this Type type) {
            var simpleProperties = type.GetProperties().Where(t => isSimpleType(t.PropertyType) && t.GetCustomAttribute<ColumnAttribute>() != null);
            var res = $"CREATE TABLE {type.Name} (";
            foreach (var item in simpleProperties) {
                var notnull = "NOT NULL";
                var column = item.GetCustomAttribute<ColumnAttribute>();
                if (column.NotNull) {
                    notnull = string.Empty;
                }

                var sqltype = "";
                if (column.Type != null) {
                    sqltype = column.Type.ToString().ToUpper();
                    if (column.Type == SqlDbType.Binary || column.Type == SqlDbType.Char || column.Type == SqlDbType.NChar || column.Type == SqlDbType.NVarChar || column.Type == SqlDbType.VarBinary || column.Type == SqlDbType.VarChar) {
                        sqltype += "(255)";
                    }
                }
                if (string.IsNullOrEmpty(sqltype)) {
                    sqltype = mapSqlType(item.PropertyType);
                }

                res = $"{res}{item.Name} {sqltype} {notnull}";
                if (simpleProperties.Last() != item) {
                    res = $"{res},";
                }
            }
            res = $"{res});{Environment.NewLine}";
            return res;
        }

        private static string mapSqlType(Type type) {
            if (type == typeof(int) || type == typeof(short) || type == typeof(byte))
                return "INT";
            else if (type == typeof(long))
                return "BIGINT";
            else if (type == typeof(double) || type == typeof(float))
                return "FLOAT";
            else if (type == typeof(string))
                return "NVARCHAR(255)";
            else if (type == typeof(Guid))
                return "UNIQUEIDENTIFIER";
            else if (type == typeof(DateTime))
                return "DATETIME";
            else
                return "NVARCHAR(255)";
        }

        private static bool isSimpleType(Type type) {
            var result = type.GetTypeInfo().IsPrimitive || typeof(string) == type || typeof(DateTime) == type || typeof(Guid) == type || typeof(Nullable<>).IsAssignableFrom(type);
            return result;
        }
    }
}