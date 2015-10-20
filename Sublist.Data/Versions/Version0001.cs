using System;

namespace Sublist.Data.Versions
{
    internal class Version0001 : DatabaseVersion
    {
        internal override Version DbVersion => new Version(0, 0, 0, 1);

        internal override void Upgrade(SQLiteTransaction transaction)
        {
            // Create database helper table
            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""DatabaseInfo"" (
                                 ""Id"" INTEGER PRIMARY KEY AUTOINCREMENT, 
                                 ""Version"" TEXT NOT NULL)");

            UpdateDatabaseVersion(transaction);
        }
    }
}