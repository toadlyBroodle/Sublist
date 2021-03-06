﻿using System.Collections.Generic;
using Sublist.Contracts.Entries;

namespace Sublist.Providers.Entries
{
    public interface IEntryProvider
    {
        ISublistEntry StoreNewEntry(ISublistEntry entry);
        void ChangeEntry(ISublistEntry entry);
        IEnumerable<ISublistEntry> GetAllEntries();
        void DeleteEntry(ISublistEntry entry);
        ISublistEntry GetParent(ISublistEntry entry, IList<ISublistEntry> tree);
    }
}
