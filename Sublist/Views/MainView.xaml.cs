using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sublist.Contracts.Entries;
using Sublist.ViewModels;

namespace Sublist.Views
{
    public sealed partial class MainView : Page
    {
        private MainViewModel _viewModel;

        public MainView()
        {
            this.InitializeComponent();
            _viewModel = this.DataContext as MainViewModel;
        }

        private void AddEntry_OnClick(object sender, RoutedEventArgs e)
        {
            ISublistEntry parent = this.HierarchicListView.SelectedItems.FirstOrDefault();
            _viewModel?.CreateEntry(parent);
        }

        private void RemoveSelectedEntries_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel?.DeleteSelectedEntries(this.HierarchicListView.SelectedItems);
        }

        private void HierarchicListView_OnSublistEntryUpdated(object sender, ISublistEntry e)
        {
            _viewModel?.UpdateEntry(e);
        }
    }
}