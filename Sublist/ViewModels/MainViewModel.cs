﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sublist.Common.Extensions;
using Sublist.Contracts.Entries;
using Sublist.Implementation.Entries;
using Sublist.Providers.Container;
using Sublist.Providers.Entries;
using Sublist.Providers.Settings;

namespace Sublist.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEntryProvider _entryProvider;
        private readonly ISettingsProvider _settingsProvider;

        private ObservableCollection<ISublistEntry> _allEntries;
        private bool _showCompleted;

        public MainViewModel()
        {
            _entryProvider = ProC.GetInstance<IEntryProvider>();
            _settingsProvider = ProC.GetInstance<ISettingsProvider>();
            
            Initialize();
        }

        protected void Initialize()
        {
            AllEntries = _entryProvider.GetAllEntries().ToObservableCollection();
            ShowCompleted = _settingsProvider.GetShowCompleted();
        }

		public void RefreshAllEntries()
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

        public bool ShowCompleted
        {
            get { return _showCompleted; }
            set
            {
                _showCompleted = value;
                ChangeShowCompleted(value, AllEntries);
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
			var newEntry = new SublistEntry();

            if (parent != null)
            {
                parent.AddSubEntrySafely(newEntry);
            }
            else
            {
                AllEntries.Add(newEntry);
            }

            _entryProvider.StoreNewEntry(newEntry);

			RefreshAllEntries();
		}

        public void UpdateEntry(ISublistEntry entry)
        {
            entry.IsVisible = _showCompleted || !entry.Completed;
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

				RefreshAllEntries();
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

				RefreshAllEntries();

			}
		}

        private void ChangeShowCompleted(bool show, IEnumerable<ISublistEntry> tree)
        {
            _settingsProvider.SetShowCompleted(show);
            ChangeShowCompletedInternal(show, tree);
        }

        private void ChangeShowCompletedInternal(bool show, IEnumerable<ISublistEntry> tree)
        {
            foreach (var sublistEntry in tree)
            {
                if (!show && sublistEntry.Completed)
                {
                    sublistEntry.IsVisible = false;
                }
                else
                {
                    sublistEntry.IsVisible = true;
                }
                ChangeShowCompleted(show, sublistEntry.SubEntries);
            }
        }
    }
}