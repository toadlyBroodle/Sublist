using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            foreach (var sublistEntry in entries)
            {
                _entryProvider.DeleteEntry(sublistEntry);
                AllEntries.Remove(sublistEntry);
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
    }
}