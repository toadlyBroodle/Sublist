using System;

namespace Sublist.Data.Versions
{
    public abstract class DatabaseVersion
    {
        internal abstract Version DbVersion { get; }
        internal abstract void Upgrade(SQLiteTransaction transaction);

        protected void UpdateDatabaseVersion(SQLiteTransaction transaction)
        {
            const string sql = @"INSERT OR REPLACE INTO ""DatabaseInfo"" (""Id"", ""Version"") VALUES (1, @version);";
            using (var statement = transaction.Prepare(sql))
            {
                statement.Bind("@version", DbVersion.ToString());

                bool success = transaction.Execute(statement);
                if (!success)
                {
                    throw new Exception(string.Format("Failed to update database version to {0}", DbVersion));
                }
            }
        }
    }
}