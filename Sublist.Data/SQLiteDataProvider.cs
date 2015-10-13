using System.Collections.Generic;
using Sublist.Data.Versions;

namespace Sublist.Data
{
    public class SQLiteDataProvider : SQLiteDataProviderBase, IDataProvider
    {
        protected override string DatabaseFilename => "userdata.sqlite";
        protected override List<DatabaseVersion> DatabaseVersions => new List<DatabaseVersion>()
        {
            new Version0001()
        };
    }
}