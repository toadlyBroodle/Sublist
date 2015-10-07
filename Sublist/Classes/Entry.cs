using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sublist.Classes
{
	public class Entry
	{
		private static int idCounter = -1;
		public int id;
		public int parentID;
		public int listViewIndex;
		private List<int> childrenIDs;

		public int indent;
		public bool bulletButtonChecked;
		public string textboxText;
		public bool completed;
		public bool hidden;

		public Entry()
		{
			// assign unique id to each entry
			idCounter++;
			id = idCounter;
			// initialize to default values --> note that this is bypassed by saving/loading persistant instances
			listViewIndex = -1;
			childrenIDs = new List<int>();
			indent = 0;
			bulletButtonChecked = false;
			textboxText = "";
			completed = false;
			hidden = false;
		}


	}
}
