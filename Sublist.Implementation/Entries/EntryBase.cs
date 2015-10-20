using System;
using System.Collections.ObjectModel;
using Sublist.Common.Useful;
using Sublist.Contracts.Entries;

namespace Sublist.Implementation.Entries
{
    public abstract class EntryBase : ViewStateAware, ISublistEntry
    {
        private bool _completed;
        private string _title;
        private long? _parentId;
        private long _id;
        private ObservableCollection<ISublistEntry> _subEntries = new ObservableCollection<ISublistEntry>();
        private DateTime _createdAtUtc;

        protected EntryBase(DateTime createdAtUtc)
        {
            _createdAtUtc = createdAtUtc;
        }

        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public long? ParentId
        {
            get { return _parentId; }
            set
            {
                _parentId = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public bool Completed
        {
            get { return _completed; }
            set
            {
                _completed = value;
                IsVisible = !value;
                OnPropertyChanged();
            }
        }

        public DateTime CreatedAtUtc
        {
            get { return _createdAtUtc; }
            private set
            {
                _createdAtUtc = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ISublistEntry> SubEntries
        {
            get { return _subEntries; }
            set
            {
                _subEntries = value;
                OnPropertyChanged();
            }
        }

        public void AddSubEntrySafely(ISublistEntry entry)
        {
            if (this == entry)
            {
                return;
            }
            foreach (var subEntryInNewEntry in entry.SubEntries)
            {
                AddSubEntrySafely(subEntryInNewEntry);
            }
            entry.ParentId = this.Id;
            SubEntries.Add(entry);
        }
    }
}
