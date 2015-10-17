using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sublist.Common.Extensions;
using Sublist.Contracts.Entries;

namespace Sublist.Controls
{
    public sealed partial class HierarchicListView : UserControl
    {
        public DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<ISublistEntry>),
            typeof(HierarchicListView), null);

        public event EventHandler<ISublistEntry> SublistEntryUpdated; 
        
        public HierarchicListView()
        {
            this.InitializeComponent();
            this.RootListView.DataContext = this;
        }
        
        public ObservableCollection<ISublistEntry> Items
        {
            get { return (ObservableCollection<ISublistEntry>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        
        public IEnumerable<ISublistEntry> SelectedItems => this.RootListView.SelectedItems.Cast<ISublistEntry>().ToList();

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var item = ((CheckBox) sender).DataContext as ISublistEntry;
            if (item != null)
            {
                item.Completed = true;
                OnSublistEntryUpdated(item);
            }
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var item = ((CheckBox)sender).DataContext as ISublistEntry;
            if (item != null)
            {
                item.Completed = false;
                OnSublistEntryUpdated(item);
            }
        }

        private void OnSublistEntryUpdated(ISublistEntry e)
        {
            SublistEntryUpdated?.Invoke(this, e);
        }
    }
}