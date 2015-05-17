using System;
using System.Collections.Generic;
using Gtk;

namespace CodeWalriiNotify
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RecyclerView : Gtk.Bin
	{
		private Stack<Widget> widgets;

		public RecyclerView()
		{
			widgets = new Stack<Widget>();
			this.Build();
		}

		public void InsertTop(Widget Widget)
		{
			ContainerView.Add(Widget);
			widgets.Push(Widget);
			ContainerView.ShowAll();
		}

		public void Clear()
		{
			foreach (Widget wg in widgets) {
				ContainerView.Remove(wg);
			}
			widgets.Clear();
		}
	}
}

