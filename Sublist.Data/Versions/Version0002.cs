using System;

namespace Sublist.Data.Versions
{
    internal class Version0002 : DatabaseVersion
    {
        internal override Version DbVersion => new Version(0, 0, 0, 2);

        internal override void Upgrade(SQLiteTransaction transaction)
        {
            // Create table to save userdata
            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""UserData"" (
                                 ""Id"" INTEGER PRIMARY KEY AUTOINCREMENT, 
                                 ""ShowCompleted"" NUMBER DEFAULT 0)");

            // Create table for entries
            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""Entry"" (
                                 ""Id"" INTEGER PRIMARY KEY AUTOINCREMENT, 
                                 ""Title"" TEXT NULL,
                                 ""Completed"" NUMBER DEFAULT 0,
                                 ""CreatedAtUtc"" NUMBER NOT NULL)");
            transaction.Execute(@"CREATE UNIQUE INDEX IxEntry ON Entry (Id)");


            // Create table to store the entries relationship
            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""EntryRelation"" (
                                 ""ParentId"" NUMBER NOT NULL,
                                 ""ChildId"" NUMBER UNIQUE NOT NULL,
                                 PRIMARY KEY (""ParentId"", ""ChildId""),
                                 FOREIGN KEY(""ParentId"") REFERENCES ""Entry""(""Id""),
                                 FOREIGN KEY(""ChildId"") REFERENCES ""Entry""(""Id""))");
            transaction.Execute(@"CREATE UNIQUE INDEX IxEntryRelation ON EntryRelation (ParentId, ChildId)");

            UpdateDatabaseVersion(transaction);
        }
    }
}
