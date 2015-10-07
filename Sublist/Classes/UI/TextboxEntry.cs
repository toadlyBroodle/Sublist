using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Sublist.Classes
{
	public class TextboxEntry : TextBox
	{
		const string TAG = "TextBoxEntry";

		public TextboxEntry()
		{
			TextWrapping = TextWrapping.Wrap;
			BorderThickness = new Thickness(1);

			GotFocus += TextboxEntry_GotFocus;
		}

		private void TextboxEntry_GotFocus(object sender, RoutedEventArgs e)
		{
			// get instance of parent row control
			RowControl parRC = (RowControl)Parent;
			// get instance of parent list view
			ListView parLV = (ListView)parRC.Parent;

			try
			{ // set list view's selected index to one behind the focussed textbox
				parLV.SelectedIndex = parRC.linkedEntry.listViewIndex;
			}
			catch	(Exception ex)
			{
				Debug.WriteLine(TAG, ex.Message);
				// if doesn't work, then set list view's selected index to last entry
				parLV.SelectedIndex = parLV.Items.Count - 1;
				// and also set Mainpage's selected index to null
				//MainPage.listViewSelectedIndex = -1;
			}
		}

		public void SetCompleted(bool tf)
		{
			// format text appropriately
			IsReadOnly = tf;
			FontStyle = (tf) ? Windows.UI.Text.FontStyle.Italic : Windows.UI.Text.FontStyle.Normal;
			ToggleForegroundColor(tf);
		}

		public void ToggleForegroundColor(Boolean tf)
		{
			// TEST change active bullet color
			SolidColorBrush fgBrush = new SolidColorBrush();
			fgBrush.Color = tf ? Colors.LightGray : Colors.Black;
			Foreground = fgBrush;
		}

	}
}