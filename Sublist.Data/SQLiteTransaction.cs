using System;
using SQLitePCL;

namespace Sublist.Data
{
    public class SQLiteTransaction : IDisposable
    {
        private readonly ISQLiteConnection _connection;
        private bool _isFinished;
        private bool _mustRollback;

        public SQLiteTransaction(ISQLiteConnection connection)
        {
            _connection = connection;
            using (var statement = connection.Prepare("BEGIN TRANSACTION"))
            {
                statement.StepWithRetry().ThrowOnFailure("Failed to initiate a transaction");
            }
        }

        public ISQLiteConnection Connection => _connection;

        public ISQLiteStatement Prepare(string sql)
        {
            return _connection.Prepare(sql);
        }

        public long LastInsertRowId()
        {
            return _connection.LastInsertRowId();
        }

        public void Execute(params string[] sqlStatements)
        {
            foreach (var sql in sqlStatements)
            {
                using (var statement = _connection.Prepare(sql))
                {
                    Execute(statement);
                }
            }
        }

        public bool Execute(string sql)
        {
            using (var statement = _connection.Prepare(sql))
            {
                return Execute(statement);
            }
        }

        public bool Execute(ISQLiteStatement statement)
        {
            if (_isFinished)
            {
                throw new Exception("Cannot execute statement on finished transaction. Start a new transaction.");
            }

            if (_mustRollback)
            {
                throw new Exception("Cannot execute statement on failed transaction. Rollback this transaction and start a new transaction.");
            }

            var result = statement.StepWithRetry();
            if (!result.IsSuccess())
            {
                System.Diagnostics.Debug.WriteLine("Failed to execute statement.");
                _mustRollback = true;
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            if (_mustRollback || !_isFinished)
            {
                Rollback();
            }
        }

        public void Commit()
        {
            if (_mustRollback)
            {
                throw new Exception("Transaction contains failed statements and must be rolled back. Rollback this transaction and start a new transaction.");
            }

            if (!_isFinished)
            {
                using (var statement = _connection.Prepare("END TRANSACTION"))
                {
                    statement.StepWithRetry().ThrowOnFailure("Failed to commit a transaction");
                }

                _isFinished = true;
            }
        }

        public void Rollback()
        {
            if (!_isFinished)
            {
                using (var statement = _connection.Prepare("ROLLBACK"))
                {
                    statement.StepWithRetry().ThrowOnFailure("Failed to rollback a transaction");
                }

                _mustRollback = false;
                _isFinished = true;
            }
        }
    }
}
