using System;
using Sublist.Contracts.Entries;
using Sublist.Implementation.Base;

namespace Sublist.Implementation.Entries
{
    public class SublistEntry : PropertyChangedBase, ISublistEntry
    {
        private bool _completed;
        private string _title;
        private long? _parentId;
        private long _id;

        public SublistEntry()
            : this(DateTime.UtcNow)
        {
        }

        public SublistEntry(DateTime createdAtUtc)
        {
            CreatedAtUtc = createdAtUtc;
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
                OnPropertyChanged();
            }
        }

        public DateTime CreatedAtUtc { get; }
    }
}