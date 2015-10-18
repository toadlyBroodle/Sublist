using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sublist.Contracts.Entries;
using Sublist.ViewModels;

namespace Sublist.Views
{
    public sealed partial class MainView : Page
    {
        private readonly MainViewModel _viewModel;

        public MainView()
        {
            this.InitializeComponent();
            _viewModel = this.DataContext as MainViewModel;
        }

        private void AddEntry_OnClick(object sender, RoutedEventArgs e)
        {
            var parent = this.HierarchicListView.SelectedItems.FirstOrDefault();
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

        private void Unindent_OnClick(object sender, RoutedEventArgs e)
        {
            var entry = this.HierarchicListView.SelectedItems.FirstOrDefault();
            _viewModel?.UnindentItem(entry);
        }

        private void Indent_OnClick(object sender, RoutedEventArgs e)
        {
            var entry = this.HierarchicListView.SelectedItems.FirstOrDefault();
            var entryAbove =
                this.HierarchicListView.FlatItems.FirstOrDefault(
                    i => i.ParentId.HasValue && entry.ParentId.HasValue && i != entry && i.ParentId.Value == entry.ParentId.Value);
            if (entryAbove == null)
            {
                int entryIndex = this.HierarchicListView.Items.IndexOf(entry);
                entryAbove = this.HierarchicListView.Items.ElementAtOrDefault(entryIndex - 1);
            }
            _viewModel?.IndentItem(entry, entryAbove);
        }
    }
}