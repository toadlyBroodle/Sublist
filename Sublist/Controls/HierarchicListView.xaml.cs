using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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

        public IList<ISublistEntry> SelectedItems => GetSelectedItems();

        public string SubListViewName => "SubItemsListView";

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

        private IList<ISublistEntry> GetSelectedItems()
        {
            var result = this.RootListView.SelectedItems.Cast<ISublistEntry>().ToList();
            List<HierarchicListView> children = new List<HierarchicListView>();

            foreach (var item in this.RootListView.Items.ToList())
            {
                var container = this.RootListView.ContainerFromItem(item);
                children.AddRange(AllChildren(container));
            }

            foreach (var child in children)
            {
                result.AddRange(child.RootListView.SelectedItems.Cast<ISublistEntry>());
            }
            
            return result;
        }

        private List<HierarchicListView> AllChildren(DependencyObject parent)
        {
            var children = new List<HierarchicListView>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is HierarchicListView && ((HierarchicListView)child).Name == SubListViewName)
                {
                    children.Add((HierarchicListView)child);
                }
                children.AddRange(AllChildren(child));
            }
            return children;
        } 

        private void OnSublistEntryUpdated(ISublistEntry e)
        {
            SublistEntryUpdated?.Invoke(this, e);
        }
    }
}