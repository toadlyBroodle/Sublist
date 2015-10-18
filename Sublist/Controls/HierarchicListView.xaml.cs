using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
        public IList<ISublistEntry> FlatItems => GetFlatItems();

        public string SubListViewName => "SubItemsListView";

        private void ToggleButton_Changed(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var item = checkBox.DataContext as ISublistEntry;
            if (item != null && checkBox.IsChecked.HasValue)
            {
                item.Completed = checkBox.IsChecked.Value;
                OnSublistEntryUpdated(item);
            }
        }

        private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            var item = textBox.DataContext as ISublistEntry;
            if (item != null)
            {
                item.Title = textBox.Text;
                OnSublistEntryUpdated(item);
            }
        }

        private void SubItemsListView_OnSublistEntryUpdated(object sender, ISublistEntry e)
        {
            OnSublistEntryUpdated(e);
        }

        private IList<ISublistEntry> GetSelectedItems()
        {
            var result = this.RootListView.SelectedItems.Cast<ISublistEntry>().ToList();
            var children = new List<HierarchicListView>();

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

        private IList<ISublistEntry> GetFlatItems()
        {
            var result = this.RootListView.Items.Cast<ISublistEntry>().ToList();
            var children = new List<HierarchicListView>();

            foreach (var item in this.RootListView.Items.ToList())
            {
                var container = this.RootListView.ContainerFromItem(item);
                children.AddRange(AllChildren(container));
            }

            foreach (var child in children)
            {
                result.AddRange(child.RootListView.Items.Cast<ISublistEntry>());
            }
            
            return result;
        }

        private IEnumerable<HierarchicListView> AllChildren(DependencyObject parent)
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