using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace Sublist.Classes
{

	public class RowControl : Grid
	{
		const string TAG = "RowControl: ";
		const int INDENT_FACTOR = 15;

		public Entry linkedEntry;
		public ToggleButton buttonBullet;
		public TextboxEntry textbox;

		private int indent;
		public int indentProp
		{
			get { return indent; }
			// set the indent property as well as it's linked entry's indent
			set
			{
				indent = value;
				// also tell textbox to change accordingly 
				linkedEntry.indent = value;
				// and update row control's margin
				Thickness margin = this.Margin;
				margin.Left = value * INDENT_FACTOR;
			}
		}
		public bool completed;
		public bool completedProp
		{
			get { return completedProp; }
			set
			{
				completed = value;
				textbox.SetCompleted(value);
			}
		}

		public RowControl(Entry entry)
		{
			linkedEntry = entry;

			// define column widths and make textbox adjust to fill window
			ColumnDefinition colDef1 = new ColumnDefinition();
			ColumnDefinition colDef2 = new ColumnDefinition();
			colDef1.Width = new GridLength(32);
			ColumnDefinitions.Add(colDef1);
			ColumnDefinitions.Add(colDef2);
			Thickness margin = this.Margin;
			// set appropriate indent
			margin.Left = linkedEntry.indent * INDENT_FACTOR;
			margin.Top = 0;
			margin.Right = 20;
			margin.Bottom = 0;
			this.Margin = margin;

			// initialize bullet and textbox
			buttonBullet = new ButtonBullet();
			textbox = new TextboxEntry();
			buttonBullet.IsChecked = linkedEntry.bulletButtonChecked;
			SetColumn(buttonBullet, 0);
			SetColumn(textbox, 1);
			Children.Add(buttonBullet);
			Children.Add(textbox);

			// set properties
			completedProp = linkedEntry.completed;
			indentProp = linkedEntry.indent;
			textbox.Text = linkedEntry.textboxText;

			// designate event handlers
			buttonBullet.Click += ButtonBullet_Click;
			textbox.TextChanged += Textbox_TextChanged;

		}

		private void ButtonBullet_Click(object sender, RoutedEventArgs e)
		{
			// save toggle state
			linkedEntry.bulletButtonChecked = (bool)buttonBullet.IsChecked;
			MainPage.dataHandler.SaveUserData();

			// TODO expand/collapse on toggle
		}

		private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			linkedEntry.textboxText = textbox.Text;
			MainPage.dataHandler.SaveUserData();
			//Debug.WriteLine(TAG, "Textbox's text changed");
		}

	}
}
