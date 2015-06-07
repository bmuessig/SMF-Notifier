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

		public double VScroll {
			get {
				Adjustment adj = ScrollView.Vadjustment;

				return (adj.Value - adj.Lower) / (adj.Upper - adj.Lower);
				// (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min
			}

			set {
				Adjustment adj = ScrollView.Vadjustment;

				ScrollView.Vadjustment.Value = value * (adj.Upper - adj.Lower) + adj.Lower;
			}
		}

		public void ScrollUp()
		{
			VScroll = 0d;
		}

		public void ScrollDown()
		{
			VScroll = 1d;
		}

		public int Count {
			get {
				return widgets.Count;
			}
		}
	}
}

