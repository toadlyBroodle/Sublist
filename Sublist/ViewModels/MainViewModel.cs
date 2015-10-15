using Sublist.Providers.Container;
using Sublist.Providers.Entries;

namespace Sublist.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEntryProvider _entryProvider;

        public MainViewModel()
        {
            _entryProvider = ProC.GetInstance<IEntryProvider>();
        }
    }
}