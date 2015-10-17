using System;

namespace Sublist.Implementation.Entries
{
    public class SublistEntry : SublistEntryBase
    {
        public SublistEntry()
            : this(DateTime.UtcNow)
        {
        }

        public SublistEntry(DateTime createdAtUtc)
            : base(createdAtUtc)
        {
        }
    }
}