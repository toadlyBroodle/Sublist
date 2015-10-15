using Sublist.Data;
using Sublist.Providers.Entries;

namespace Sublist.Providers.Container
{
    public class ProviderInitializer
    {
        public static void Initialize()
        {
            var dataProvider = new SQLiteDataProvider();
            var entryProvider = new EntryProvider(dataProvider);

            ProC.Register<IEntryProvider>(entryProvider);
        }
    }
}