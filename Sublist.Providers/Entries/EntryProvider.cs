using System.Collections.Generic;
using System.Linq;
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
            entry.Id = _dataProvider.AddSublistEntry(entry);
            return entry;
        }

        public void ChangeEntry(ISublistEntry entry)
        {
            _dataProvider.UpdateSublistEntry(entry);
        }

        public IEnumerable<ISublistEntry> GetAllEntries()
        {
            var listedEntries = _dataProvider.GetAllSublistEntries().ToList();
            var hierarchicEntries = new List<ISublistEntry>();
            foreach (var listedEntry in listedEntries)
            {
                if (!listedEntry.ParentId.HasValue || listedEntry.ParentId.Value == 0)
                {
                    hierarchicEntries.Add(listedEntry);
                    continue;
                }
                var parent = listedEntries.FirstOrDefault(e => e.Id == listedEntry.ParentId.Value);
                parent?.AddSubEntrySafely(listedEntry);
            }
            return hierarchicEntries;
        }

        public void DeleteEntry(ISublistEntry entry)
        {
            _dataProvider.DeleteSublistEntry(entry);
        }
    }
}