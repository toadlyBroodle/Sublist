using System.Collections.Generic;
using Sublist.Contracts.Entries;

namespace Sublist.Data
{
    public interface IDataProvider
    {
        long AddSublistEntry(ISublistEntry entry);
        void DeleteSublistEntry(ISublistEntry entry);
        IEnumerable<ISublistEntry> GetAllSublistEntries();
        ISublistEntry GetSublistEntry(long id);
    }
}