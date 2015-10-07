using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sublist.Classes
{
	public class MasterList<T> : List<Entry>
	{
		const string TAG = "MasterList";

		private int activeIndex;
		private Entry activeEntry;
		public Entry activeEntryProp {
			get { return activeEntry; } 
			// keep activeIndex up to date when active entry is changed
			set { activeEntry = value;  activeIndex = QueryIndexByID(value.id); } }

		public MasterList() //: base()
		{

		}

		public void UpdateListView(MainPage mp)
		{
			// store selected index value
			//int selInd = mp.mainListView.SelectedIndex;
			// store selected entry
			RowControl selRow = (RowControl)mp.mainListView.SelectedItem;
			Entry selEnt = (selRow == null) ? null : selRow.linkedEntry;

			// clear list
			mp.mainListView.Items.Clear();
			// populate list
			for (int i = 0; i < this.Count; i++)
			{
				// clear any previously set listViewIndexes
				this[i].listViewIndex = -1;

				// only add currently visible entries to list view
				if (this[i].completed)
				{
					if ((bool)mp.appBarShowCompl.IsChecked)
					{
						// set listViewIndex for later use and add to list
						this[i].listViewIndex = i;
						mp.mainListView.Items.Add(new RowControl(this[i]));
					}
				}
				else
				{
					this[i].listViewIndex = i;
					mp.mainListView.Items.Add(new RowControl(this[i]));
				}
			}
			// persist previous selected index
			//int newInd = selInd;
			//if (selInd >= mp.mainListView.Items.Count) newInd = mp.mainListView.Items.Count - 1;
			//if (selInd < 0) newInd = 0;
			//try {
			//	mp.mainListView.SelectedIndex = newInd;
			//} catch (Exception ex)
			//{
			//	Debug.WriteLine(TAG, ex.Message);
			//}

			// persist previous selected entry, accounting for changes in list view due to hidden/completed items shown/not-shown
			if (selEnt != null)
				mp.mainListView.SelectedIndex = selEnt.listViewIndex;

			// keep selected item in view
			mp.mainListView.ScrollIntoView(mp.mainListView.SelectedItem);

			// and save user data
			MainPage.dataHandler.SaveUserData();
		}

		public void AddRow(MainPage mp)
		{
			Entry ent = new Entry();

			// if no row selected then add new row to end of masterList
			if (MainPage.listViewSelectedIndex < 0)
			{
				Add(ent);
				// and make this the new selected index
				MainPage.listViewSelectedIndex = Count - 1;
			}
			// otherwise add new item below selected entry
			else
			{
				// as long as it's not completed
				if (!this[MainPage.listViewSelectedIndex].completed)
				{
					// and make this the new selected index
					MainPage.listViewSelectedIndex++;
					Insert(MainPage.listViewSelectedIndex, ent);
				}
			}

			UpdateListView(mp);
		}

		public void RemoveRow(MainPage mp)
		{
			RemoveAt(MainPage.listViewSelectedIndex);
			MainPage.listViewSelectedIndex--;

			UpdateListView(mp);
		}

		public int QueryIndexByID(int id)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].id == id)
					// return index of item with specific id
					return i;
			}
			// else didn't find this id in master list
			return -1;
		}

		public Entry QueryEntryByID(int id)
		{
			IEnumerable<Entry> queryEntryByID =
				from entry in this
				where entry.id == id
				select entry;

			return queryEntryByID.First();
		}
	}
}
