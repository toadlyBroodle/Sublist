using System;
using System.Collections.ObjectModel;

namespace Sublist.Contracts.Entries
{
    public interface ISublistEntry
    {
        long Id { get; set; }
        long? ParentId { get; set; }
        string Title { get; set; }
        bool Completed { get; set; }
        DateTime CreatedAtUtc { get; }
        ObservableCollection<ISublistEntry> SubEntries { get; set; }
        bool IsVisible { get; set; }

        /// <summary>
        /// Adds an entry safely, so that a parent cannot contain itself. Use this instead of Subentries.Add(...). The ParentId of the child gets updated.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        void AddSubEntrySafely(ISublistEntry entry);
    }
}