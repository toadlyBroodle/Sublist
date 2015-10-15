using System.Collections.Generic;
using Sublist.Contracts.Entries;
using Sublist.Data;

namespace Sublist.Providers.Entries
{
    public class EntryProvider : IEntryProvider
    {
        private readonly IDataProvider _dataProvider;

        public EntryProvider(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public ISublistEntry StoreNewEntry(ISublistEntry entry)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeEntry(ISublistEntry entry)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ISublistEntry> GetAllEntries()
        {
            throw new System.NotImplementedException();
        }
    }
}