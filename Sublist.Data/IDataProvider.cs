using System.Collections.Generic;
using Sublist.Contracts.App;
using Sublist.Contracts.Entries;

namespace Sublist.Data
{
    public interface IDataProvider
    {
        long AddSublistEntry(ISublistEntry entry);
        void DeleteSublistEntry(ISublistEntry entry);
        void UpdateSublistEntry(ISublistEntry entry);
        IEnumerable<ISublistEntry> GetAllSublistEntries();
        ISublistEntry GetSublistEntry(long id);
        IAppData GetAppData();
        void UpdateAppData(IAppData appData);
    }
}