using System;
using System.Collections.Generic;
using Gtk;

namespace CodeWalriiNotify
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RecyclerView : Bin
	{
		List<Widget> widgets;

		public RecyclerView()
		{
			widgets = new List<Widget>();
			this.Build();
		}

		public void InsertFirst(Widget Widget)
		{
			ContainerView.Add(Widget);
			widgets.Add(Widget);
			ContainerView.ShowAll();
		}

		public void RemoveLast()
		{
			if (widgets.Count > 0) {
				Widget wg = widgets[widgets.Count - 1];
				ContainerView.Remove(wg);
				widgets.Remove(wg);
			}
		}

		public void Clear()
		{
			foreach (Widget wg in widgets) {
				ContainerView.Remove(wg);
			}
			widgets.Clear();
		}

		public int Count {
			get {
				return widgets.Count;
			}
		}
	}
}

