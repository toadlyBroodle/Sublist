using System;
using System.Collections.Generic;
using System.Linq;
using SQLitePCL;
using Sublist.Data.Versions;

namespace Sublist.Data
{
    public abstract class SQLiteDataProviderBase
    {
        private ISQLiteConnection _connection;
        private bool _disposed = false;
        private readonly object _connectionLock = new object();

        protected abstract string DatabaseFilename { get; }
        protected abstract List<DatabaseVersion> DatabaseVersions { get; }

        protected SQLiteDataProviderBase()
        {
            Initialize();
            UpgradeDatabase();
        }

        protected bool HasUpgraded { get; set; }
        protected Version UninitializedDatabaseVersion => new Version(0, 0, 0, 0);
        protected Version CurrentDatabaseVersion { get; set; }

        protected ISQLiteConnection SharedConnection
        {
            get
            {
                if (_connection == null)
                {
                    lock (_connectionLock)
                    {
                        if (_connection == null)
                        {
                            _connection = new SQLiteConnection(DatabaseFilename);
                            var pragma = _connection.Prepare(@"PRAGMA foreign_keys = ON;");
                            pragma.StepWithRetry();
                        }
                    }
                }

                return _connection;
            }
            set { _connection = value; }
        }

        private void Initialize()
        {
            using (var dbExistsStatement = SharedConnection.Prepare("SELECT 1 FROM sqlite_master WHERE type='table' AND name='DatabaseInfo'"))
            {
                if (dbExistsStatement.StepWithRetry() == SQLiteResult.ROW)
                {
                    const string sql = @"SELECT ""Version"" FROM DatabaseInfo LIMIT 1;";
                    using (var statement = SharedConnection.Prepare(sql))
                    {
                        if (statement.StepWithRetry() == SQLiteResult.ROW)
                        {
                            CurrentDatabaseVersion = new Version(statement.GetText("Version"));
                        }
                    }
                }
                else
                {
                    CurrentDatabaseVersion = UninitializedDatabaseVersion;
                }
            }

            HasUpgraded = UpgradeDatabase();
            if (HasUpgraded)
            {
                CloseConnection();
            }
        }

        public void CloseConnection()
        {
            if (_connection != null)
            {
                lock (_connectionLock)
                {
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
            }
        }

        protected bool UpgradeDatabase()
        {
            var versionBeforeUpgrades = CurrentDatabaseVersion;

            foreach (var version in DatabaseVersions.Where(version => CurrentDatabaseVersion < version.DbVersion).ToList())
            {
                try
                {
                    using (SharedConnection)
                    using (var transaction = new SQLiteTransaction(SharedConnection))
                    {
                        version.Upgrade(transaction);
                        transaction.Commit();
                    }

                    CurrentDatabaseVersion = version.DbVersion;
                    System.Diagnostics.Debug.WriteLine("Database upgrade from {0} to {1} succeeded", CurrentDatabaseVersion, version.DbVersion);
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Database upgrade from {0} to {1} failed", CurrentDatabaseVersion, version.DbVersion), exception);
                }
            }

            bool hasUpgraded = versionBeforeUpgrades != CurrentDatabaseVersion;
            return hasUpgraded;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                CloseConnection();
                _disposed = true;
            }
        }
    }
}