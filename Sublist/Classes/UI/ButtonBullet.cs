using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Sublist.Classes
{
	public class ButtonBullet : ToggleButton
	{
		public ButtonBullet()
		{
			Width = 32;
			Height = 32;
			Content = "o";

		}

	}
}
