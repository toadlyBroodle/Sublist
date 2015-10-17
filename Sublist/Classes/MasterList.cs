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

		public MasterList()
		{

		}

		public void UpdateListView(MainPage mp)
		{
			// store selected entry
			RowControl selRow = (RowControl)mp.mainListView.SelectedItem;
			Entry selEnt = (selRow == null) ? null : selRow.linkedEntry;
			int selInd = mp.mainListView.SelectedIndex;

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

			// persist previous selected entry, accounting for changes in list view due to hidden/completed items shown/not-shown
			if (selEnt != null)
			{
				// constrain selected index to bounds of main list
				if (selInd < 0)
					selInd = 0;
				if (selInd > mp.mainListView.Items.Count - 1)
					selInd = mp.mainListView.Items.Count - 1;
				if (selEnt.listViewIndex < 0)
					selEnt.listViewIndex = 0;
				if (selEnt.listViewIndex > mp.mainListView.Items.Count - 1)
					selEnt.listViewIndex = mp.mainListView.Items.Count - 1;

				// account for completed/hidden items to set selected item correctly, TODO doesn't work
				if ((bool)mp.appBarShowCompl.IsChecked)
					mp.mainListView.SelectedIndex = selInd;
				else
					mp.mainListView.SelectedIndex = selEnt.listViewIndex;
			}

			// keep selected item in view
			mp.mainListView.ScrollIntoView(mp.mainListView.SelectedItem);

			// and save user data
			MainPage.dataHandler.SaveUserData();
		}

		public void AddRow(MainPage mp)
		{
			Entry ent = new Entry();

			// if list is empty, or if no row selected then add new row to end of masterList
			if (mp.mainListView.SelectedIndex < 0)
			{
				Add(ent);
				UpdateListView(mp);
				// and make this the new selected index
				mp.mainListView.SelectedIndex = Count - 1;
			}
			// otherwise add new item below selected entry
			else
			{
				// as long as it's not completed
				if (!this[mp.mainListView.SelectedIndex].completed)
				{
					// if last item selected, then add item to end of list, otherwise insert item at selected index
					if (mp.mainListView.SelectedIndex >= mp.mainListView.Items.Count - 1)
					{
						Add(ent);
						UpdateListView(mp);
						// and make this the new selected index
						mp.mainListView.SelectedIndex++;
					}
					else
					{
						Insert(mp.mainListView.SelectedIndex + 1, ent);
						UpdateListView(mp);
						mp.mainListView.SelectedIndex++;
					}
				}
			}

			UpdateListView(mp);
		}

		public void RemoveRow(MainPage mp)
		{
			RemoveAt(mp.mainListView.SelectedIndex);
			mp.mainListView.SelectedIndex--;

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
