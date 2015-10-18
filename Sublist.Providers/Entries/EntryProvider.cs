using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Sublist.Contracts.Entries;
using Sublist.Data;

namespace Sublist.Providers.Entries
{
    public class EntryProvider : IEntryProvider
    {
        private readonly IDataProvider _dataProvider;

        private IList<ISublistEntry> _allHierarchicalEntries;

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
            var allListedEntries = _dataProvider.GetAllSublistEntries().ToList();
            _allHierarchicalEntries = new List<ISublistEntry>();
            foreach (var listedEntry in allListedEntries)
            {
                if (!listedEntry.ParentId.HasValue || listedEntry.ParentId.Value == 0)
                {
                    _allHierarchicalEntries.Add(listedEntry);
                    continue;
                }
                var parent = allListedEntries.FirstOrDefault(e => e.Id == listedEntry.ParentId.Value);
                parent?.AddSubEntrySafely(listedEntry);
            }
            return _allHierarchicalEntries;
        }

        public void DeleteEntry(ISublistEntry entry)
        {
            _dataProvider.DeleteSublistEntry(entry);
        }

        public ISublistEntry GetParent(ISublistEntry entry, IList<ISublistEntry> tree)
        {
            if (!entry.ParentId.HasValue)
            {
                return null;
            }

            foreach (var item in tree)
            {
                if (item.Id == entry.ParentId.Value)
                {
                    return item;
                }

                var parent = GetParent(entry, item.SubEntries);
                if (parent != null)
                {
                    return parent;
                }
            }

            return null;
        }
    }
}