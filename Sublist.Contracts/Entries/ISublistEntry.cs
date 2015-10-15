using System;

namespace Sublist.Contracts.Entries
{
    public interface ISublistEntry
    {
        long Id { get; set; }
        long? ParentId { get; set; }
        string Title { get; set; }
        bool Completed { get; set; }
        DateTime CreatedAtUtc { get; }
    }
}