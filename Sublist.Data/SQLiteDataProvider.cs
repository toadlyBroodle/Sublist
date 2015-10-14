using System;
using System.Collections.Generic;
using SQLitePCL;
using Sublist.Contracts.App;
using Sublist.Contracts.Entries;
using Sublist.Data.Versions;
using Sublist.Implementation.App;
using Sublist.Implementation.Entries;

namespace Sublist.Data
{
    public class SQLiteDataProvider : SQLiteDataProviderBase, IDataProvider
    {
        protected override string DatabaseFilename => "sublist.sqlite";
        protected override List<DatabaseVersion> DatabaseVersions => new List<DatabaseVersion>()
        {
            new Version0001(),
            new Version0002()
        };

        public long AddSublistEntry(ISublistEntry entry)
        {
            const string mainSql = @"INSERT INTO Entry " +
                                   "(Title, Completed, CreatedAtUtc) " +
                                   "VALUES " +
                                   "(@title, @completed, @createdAtUtc)";

            const string relationSql = @"INSERT INTO EntryRelation " +
                                       "(ParentId, ChildId) " +
                                       "VALUES " +
                                       "(@parentId, @childId)";

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                using (var statement = transaction.Prepare(mainSql))
                {
                    statement.Binding("@title", entry.Title);
                    statement.Binding("@completed", entry.Completed);
                    statement.Binding("@createdAtUtc", entry.CreatedAtUtc);
                    transaction.Execute(statement);
                }
                entry.Id = transaction.LastInsertRowId();

                if (entry.ParentId.HasValue)
                {
                    using (var statement = transaction.Prepare(relationSql))
                    {
                        statement.Binding("@parentId", entry.ParentId.Value);
                        statement.Binding("@childId", entry.Id);
                        transaction.Execute(statement);
                    }
                }

                transaction.Commit();
            }

            return entry.Id;
        }

        public void DeleteSublistEntry(ISublistEntry entry)
        {
            const string deleteMainSql = @"DELETE FROM Entry WHERE Id = @id";
            const string updateRelationsSql = @"UPDATE EntryRelation SET ParentId = @parentId WHERE ParentId = @id";
            const string deleteRelationSql = @"DELETE FROM EntryRelation WHERE ChildId = @id";

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                using (var statement = transaction.Prepare(deleteMainSql))
                {
                    statement.Binding("@id", entry.Id);
                    transaction.Execute(statement);
                }
                using (var statement = transaction.Prepare(updateRelationsSql))
                {
                    statement.Binding("@parentId", entry.ParentId);
                    statement.Binding("@id", entry.Id);
                    transaction.Execute(statement);
                }
                using (var statement = transaction.Prepare(deleteRelationSql))
                {
                    statement.Binding("@id", entry.Id);
                    transaction.Execute(statement);
                }

                transaction.Commit();
            }
        }

        public void UpdateSublistEntry(ISublistEntry entry)
        {
            const string updateMainSql = @"UPDATE Entry " +
                                          "SET Title = @title, Completed = @completed " +
                                          "WHERE Id = @id";

            const string updateRelationsSql = @"UPDATE EntryRelation " +
                                               "SET ParentId = @parentId " +
                                               "WHERE ChildId = @id";

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                using (var statement = transaction.Prepare(updateMainSql))
                {
                    statement.Binding("@title", entry.Title);
                    statement.Binding("@completed", entry.Completed);
                    statement.Binding("@id", entry.Id);
                    transaction.Execute(statement);
                }

                using (var statement = transaction.Prepare(updateRelationsSql))
                {
                    statement.Binding("@parentId", entry.ParentId);
                    statement.Binding("@id", entry.Id);
                    transaction.Execute(statement);
                }

                transaction.Commit();
            }
        }

        public IEnumerable<ISublistEntry> GetAllSublistEntries()
        {
            const string sql = @"SELECT e.Id, e.Title, e.Completed, e.CreatedAtUtc, er.ParentId " +
                                "FROM Entry AS e " +
                                "LEFT JOIN EntryRelation AS er ON (e.Id = er.ChildId)";

            List<ISublistEntry> entries = new List<ISublistEntry>();

            using (var statement = SharedConnection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    entries.Add(new SublistEntry(statement.GetValue<DateTime>("CreatedAtUtc"))
                    {
                        Id = statement.GetValue<long>("Id"),
                        ParentId = statement.GetValue<long?>("ParentId"),
                        Title = statement.GetValue<string>("Title"),
                        Completed = statement.GetValue<bool>("Completed")
                    });
                }
            }

            return entries;
        }

        public ISublistEntry GetSublistEntry(long id)
        {
            const string sql = @"SELECT e.Id, e.Title, e.Completed, e.CreatedAtUtc, er.ParentId " +
                                "FROM Entry AS e " + 
                                "LEFT JOIN EntryRelation AS er ON (e.Id = er.ChildId) " +
                                "WHERE e.Id = @id";

            using (var statement = SharedConnection.Prepare(sql))
            {
                statement.Binding("@id", id);

                if (statement.Step() == SQLiteResult.ROW)
                {
                    return new SublistEntry(statement.GetValue<DateTime>("CreatedAtUtc"))
                    {
                        Id = statement.GetValue<long>("Id"),
                        ParentId = statement.GetValue<long?>("ParentId"),
                        Title = statement.GetValue<string>("Title"),
                        Completed = statement.GetValue<bool>("Completed")
                    };
                }
            }

            throw new DatabaseException($"No entry with Id {id} found.");
        }

        public IAppData GetAppData()
        {
            const string sql = @"SELECT ShowCompleted FROM AppData LIMIT 1";

            using (var statement = SharedConnection.Prepare(sql))
            {
                if (statement.Step() == SQLiteResult.ROW)
                {
                    return new AppData()
                    {
                        ShowCompleted = statement.GetValue<bool>("ShowCompleted")
                    };
                }
            }

            throw new DatabaseException($"No entry for app data found.");
        }

        public void UpdateAppData(IAppData appData)
        {
            const string mainSql = @"INSERT OR REPLACE INTO AppData " +
                                   "(Id, ShowCompleted) " +
                                   "VALUES " +
                                   "(1, @showCompleted)";

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                using (var statement = transaction.Prepare(mainSql))
                {
                    statement.Binding("@showCompleted", appData.ShowCompleted);
                    transaction.Execute(statement);
                }

                transaction.Commit();
            }
        }
    }
}