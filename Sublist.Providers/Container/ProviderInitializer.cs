using Sublist.Data;
using Sublist.Providers.Entries;
using Sublist.Providers.Settings;

namespace Sublist.Providers.Container
{
    public class ProviderInitializer
    {
        public static void Initialize()
        {
            var dataProvider = new SQLiteDataProvider();
            var entryProvider = new EntryProvider(dataProvider);
            var settingsProvider = new SettingsProvider(dataProvider);

            ProC.Register<IEntryProvider>(entryProvider);
            ProC.Register<ISettingsProvider>(settingsProvider);
        }
    }
}