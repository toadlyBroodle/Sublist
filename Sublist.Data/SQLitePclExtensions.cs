using System;
using System.Threading;
using SQLitePCL;

namespace Sublist.Data
{
    internal static class SQLitePclExtensions
    {
        /// <summary>
        /// Executes a single statement on a connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SQLiteResult Execute(this ISQLiteConnection connection, string sql)
        {
            using (var statement = connection.Prepare(sql))
            {
                return statement.Step();
            }
        }

        /// <summary>
        /// Executes multiple statements on a connnection.
        /// Return values are ignored.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sqlStatements"></param>
        public static void Execute(this ISQLiteConnection connection, params string[] sqlStatements)
        {
            foreach (var sql in sqlStatements)
            {
                using (var statement = connection.Prepare(sql))
                {
                    statement.Step();
                }
            }
        }

        public static void Binding(this ISQLiteStatement statement, string paramName, object value)
        {
            if (value is DateTime)
            {
                statement.Bind(paramName, ((DateTime)value).ToUniversalTime().Ticks);
                return;
            }
            if (value is DateTime?)
            {
                if (value != null)
                {
                    statement.Bind(paramName, ((DateTime)value).ToUniversalTime().Ticks);
                    return;
                }
            }
            else if (value is bool)
            {
                statement.Bind(paramName, Convert.ToInt64((bool)value));
                return;
            }
            else if (value is Enum)
            {
                statement.Bind(paramName, (int)value);
                return;
            }
            statement.Bind(paramName, value);
            return;
        }

        public static T GetValue<T>(this ISQLiteStatement statement, int index)
        {
            return GetValue<T>(statement[index]);
        }

        public static T GetValue<T>(this ISQLiteStatement statement, string key)
        {
            return GetValue<T>(statement[key]);
        }

        private static T GetValue<T>(object value)
        {
            var type = typeof(T);
            if (type == typeof(DateTime))
            {
                if (value != null)
                {
                    var ticks = (long)value;
                    if (ticks != 0)
                    {
                        return (T)(object)(new DateTime(ticks, DateTimeKind.Utc));
                    }
                }
                return (T)(object)(new DateTime());
            }
            else if (type == typeof(DateTime?))
            {
                if (value != null)
                {
                    var ticks = (long)value;
                    if (ticks != 0)
                    {
                        return (T)(object)(new DateTime(ticks, DateTimeKind.Utc));
                    }
                }
            }
            else if (type == typeof(bool))
            {
                if (value != null)
                {
                    var result = (long)value;
                    if (result != 0)
                    {
                        return (T)(object)(true);
                    }
                }
                return (T)(object)(false);
            }
            else if (type == typeof(bool?))
            {
                if (value != null)
                {
                    var result = (long)value;
                    if (result != 0)
                    {
                        return (T)(object)(true);
                    }
                }
            }
            else if (type == typeof(decimal))
            {
                if (value != null)
                {
                    return (T)Convert.ChangeType(value, type);
                }
            }

            return (T)value;
        }

        public static bool IsSuccess(this SQLiteResult result)
        {
            return result == SQLiteResult.OK || result == SQLiteResult.DONE;
        }

        public static void ThrowOnFailure(this SQLiteResult result, string exceptionMessage)
        {
            if (result != SQLiteResult.OK && result != SQLiteResult.DONE)
            {
                throw new Exception(string.Format("{0} | Result: {1}", exceptionMessage, result));
            }
        }
    }
}
