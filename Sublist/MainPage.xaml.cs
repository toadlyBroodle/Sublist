using Sublist.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sublist
{
	// TODO handle key strokes: Text Typed -> begin entering it in selected row's textbox; Enter -> save entry, unfocus textbox

	public sealed partial class MainPage : Page
	{
		const string TAG = "MainPage: ";

		// for loading and saving user data and settings
		public static DataHandler dataHandler;

		public static MasterList<Entry> masterList;
		//public static int listViewSelectedIndex = -1;

		public MainPage()
        {
			this.InitializeComponent();

			dataHandler = new DataHandler(this);
			masterList = new MasterList<Entry>();

			// load user data
			if (dataHandler.userDataList != null)
				masterList = dataHandler.userDataList;

			masterList.UpdateListView(this);
		}

		//private void MainListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		//{
		//	try
		//	{
		//		// change masterList's active index to reflect the currently selected item
		//		RowControl rc = (RowControl)mainListView.Items[mainListView.SelectedIndex];
		//		listViewSelectedIndex = masterList.QueryIndexByID(rc.linkedEntry.id);
		//		masterList.activeEntryProp = rc.linkedEntry;
		//	}
		//	catch (Exception ex)
		//	{
		//		Debug.WriteLine(TAG, ex.Message);
		//		// if doesn't work then set selected to 0
		//		//listViewSelectedIndex = 0;
		//	}
		//}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			dataHandler.LoadUserSettings();
		}


		private void AppBarAdd_Click(object sender, RoutedEventArgs e)
		{
			masterList.AddRow(this);
		}

		private void AppBarRemove_Click(object sender, RoutedEventArgs e)
		{
			if (!(mainListView.SelectedIndex < 0))
			{
				masterList.RemoveRow(this);
			}
		}

		private void AppBarMoveDown_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AppBarMoveUp_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AppBarIndent_Click(object sender, RoutedEventArgs e)
		{
			// indent the row control if currently selected index is a list view item
			if (-1 < mainListView.SelectedIndex && mainListView.SelectedIndex < mainListView.Items.Count)
			{
				// but don't allow more than one indent past above row's indent level
				RowControl rc = (RowControl)mainListView.Items[mainListView.SelectedIndex];
				int indexMinus1 = mainListView.SelectedIndex - 1;
				if (-1 < indexMinus1 && rc.indentProp <= masterList[indexMinus1].indent)
				{
					rc.indentProp++;
				}
			}
			// then update list view
			masterList.UpdateListView(this);
		}

		private void AppBarUnindent_Click(object sender, RoutedEventArgs e)
		{
			// unindent the row control if currently selected index is a list view item
			if (-1 < mainListView.SelectedIndex && mainListView.SelectedIndex < mainListView.Items.Count)
			{
				// but don't allow unindenting off left side of page
				RowControl rc = (RowControl)mainListView.Items[mainListView.SelectedIndex];
				if (rc.indentProp > 0)
				{
					rc.indentProp--;
				}
			}
			// then update list view
			masterList.UpdateListView(this);
		}

		public void AppBarShowCompl_Click(object sender, RoutedEventArgs e)
		{
			dataHandler.SaveUserSettings();

			masterList.UpdateListView(this);
		}

		public void AppBarMarkAsCompleted_Click(object sender, RoutedEventArgs e)
		{
			// toggle hidden state of active entry
			if (-1 < mainListView.SelectedIndex && mainListView.SelectedIndex < masterList.Count)
			{
				masterList[mainListView.SelectedIndex].completed = (masterList[mainListView.SelectedIndex].completed) ? false : true;

				masterList.UpdateListView(this);
			}
		}

	}
}
