using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sublist.Common.Extensions;
using Sublist.Contracts.Entries;
using Sublist.Implementation.Entries;
using Sublist.Providers.Container;
using Sublist.Providers.Entries;

namespace Sublist.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEntryProvider _entryProvider;
        private ObservableCollection<ISublistEntry> _allEntries;
        private string _newEntryText;

        public MainViewModel()
        {
            _entryProvider = ProC.GetInstance<IEntryProvider>();
            
            Initialize();
        }

        protected void Initialize()
        {
            AllEntries = _entryProvider.GetAllEntries().ToObservableCollection();
        }

        public ObservableCollection<ISublistEntry> AllEntries
        {
            get { return _allEntries; }
            set
            {
                _allEntries = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ISublistEntry> SelectedItems
        {
            get { return _allEntries; }
            set
            {
                _allEntries = value;
                OnPropertyChanged();
            }
        }

        public string NewEntryText
        {
            get { return _newEntryText; }
            set
            {
                _newEntryText = value;
                OnPropertyChanged();
            }
        }

        public void DeleteSelectedEntries(IEnumerable<ISublistEntry> entries)
        {
            foreach (var sublistEntry in entries.ToList())
            {
                _entryProvider.DeleteEntry(sublistEntry);

                var parent = _entryProvider.GetParent(sublistEntry, AllEntries);
                if (parent == null)
                {
                    AllEntries.Remove(sublistEntry);
                }
                else
                {
                    parent.SubEntries.Remove(sublistEntry);
                }
            }
        }

        public void CreateEntry(ISublistEntry parent)
        {
            var newEntry = new SublistEntry()
            {
                Title = NewEntryText
            };

            if (parent != null)
            {
                parent.AddSubEntrySafely(newEntry);
            }
            else
            {
                AllEntries.Add(newEntry);
            }

            _entryProvider.StoreNewEntry(newEntry);
        }

        public void UpdateEntry(ISublistEntry entry)
        {
            _entryProvider.ChangeEntry(entry);
        }

        public void UnindentItem(ISublistEntry entry)
        {
            var currentParent = _entryProvider.GetParent(entry, AllEntries);
            if (currentParent != null)
            {
                var parentOfParent = _entryProvider.GetParent(currentParent, AllEntries);
                if (parentOfParent != null)
                {
                    entry.ParentId = parentOfParent.Id;
                    parentOfParent.AddSubEntrySafely(entry);
                    currentParent.SubEntries.Remove(entry);
                    _entryProvider.ChangeEntry(entry);
                }
                else
                {
                    entry.ParentId = 0;
                    _entryProvider.ChangeEntry(entry);
                    currentParent.SubEntries.Remove(entry);
                    AllEntries.Add(entry);
                }
            }
        }

        public void IndentItem(ISublistEntry entry, ISublistEntry newParent)
        {
            if (newParent != null)
            {
                var oldParent = _entryProvider.GetParent(entry, AllEntries);
                if (oldParent != null && oldParent.SubEntries.First() == entry)
                {
                    return;
                }

                entry.ParentId = newParent.Id;
                newParent.AddSubEntrySafely(entry);
                oldParent?.SubEntries.Remove(entry);
                AllEntries.Remove(entry);
                _entryProvider.ChangeEntry(entry);
            }
        }
    }
}