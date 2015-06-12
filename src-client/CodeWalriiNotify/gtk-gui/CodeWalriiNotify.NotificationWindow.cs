
// This file has been generated by the GUI designer. Do not modify.
namespace CodeWalriiNotify
{
	public partial class NotificationWindow
	{
		private global::Gtk.EventBox mainBox;
		
		private global::Gtk.VBox vLayoutBox;
		
		private global::Gtk.EventBox headerBox;
		
		private global::Gtk.Label subjectLabel;
		
		private global::Gtk.EventBox footerBox;
		
		private global::Gtk.Label secondLabel;
		
		private global::Gtk.EventBox actionBox;
		
		private global::Gtk.HButtonBox actionButtonBox;
		
		private global::Gtk.Button dismissButton;
		
		private global::Gtk.Button viewButton;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget CodeWalriiNotify.NotificationWindow
			this.WidthRequest = 300;
			this.HeightRequest = 150;
			this.Name = "CodeWalriiNotify.NotificationWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("NotificationWindow");
			this.Resizable = false;
			this.AllowShrink = true;
			this.Decorated = false;
			this.DestroyWithParent = true;
			this.SkipTaskbarHint = true;
			// Container child CodeWalriiNotify.NotificationWindow.Gtk.Container+ContainerChild
			this.mainBox = new global::Gtk.EventBox ();
			this.mainBox.Name = "mainBox";
			// Container child mainBox.Gtk.Container+ContainerChild
			this.vLayoutBox = new global::Gtk.VBox ();
			this.vLayoutBox.Name = "vLayoutBox";
			this.vLayoutBox.Spacing = 6;
			// Container child vLayoutBox.Gtk.Box+BoxChild
			this.headerBox = new global::Gtk.EventBox ();
			this.headerBox.HeightRequest = 50;
			this.headerBox.Events = ((global::Gdk.EventMask)(32));
			this.headerBox.Name = "headerBox";
			// Container child headerBox.Gtk.Container+ContainerChild
			this.subjectLabel = new global::Gtk.Label ();
			this.subjectLabel.WidthRequest = 430;
			this.subjectLabel.Name = "subjectLabel";
			this.subjectLabel.Xpad = 5;
			this.subjectLabel.Xalign = 0F;
			this.subjectLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Subject");
			this.subjectLabel.Ellipsize = ((global::Pango.EllipsizeMode)(3));
			this.headerBox.Add (this.subjectLabel);
			this.vLayoutBox.Add (this.headerBox);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vLayoutBox [this.headerBox]));
			w2.Position = 0;
			// Container child vLayoutBox.Gtk.Box+BoxChild
			this.footerBox = new global::Gtk.EventBox ();
			this.footerBox.Name = "footerBox";
			// Container child footerBox.Gtk.Container+ContainerChild
			this.secondLabel = new global::Gtk.Label ();
			this.secondLabel.WidthRequest = 300;
			this.secondLabel.HeightRequest = 15;
			this.secondLabel.Name = "secondLabel";
			this.secondLabel.Xpad = 5;
			this.secondLabel.Xalign = 1F;
			this.secondLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Poster");
			this.footerBox.Add (this.secondLabel);
			this.vLayoutBox.Add (this.footerBox);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vLayoutBox [this.footerBox]));
			w4.Position = 1;
			// Container child vLayoutBox.Gtk.Box+BoxChild
			this.actionBox = new global::Gtk.EventBox ();
			this.actionBox.Name = "actionBox";
			// Container child actionBox.Gtk.Container+ContainerChild
			this.actionButtonBox = new global::Gtk.HButtonBox ();
			this.actionButtonBox.Name = "actionButtonBox";
			this.actionButtonBox.BorderWidth = ((uint)(6));
			// Container child actionButtonBox.Gtk.ButtonBox+ButtonBoxChild
			this.dismissButton = new global::Gtk.Button ();
			this.dismissButton.Name = "dismissButton";
			this.dismissButton.UseUnderline = true;
			this.dismissButton.FocusOnClick = false;
			this.dismissButton.Relief = ((global::Gtk.ReliefStyle)(2));
			this.dismissButton.Label = global::Mono.Unix.Catalog.GetString ("Dismiss");
			this.actionButtonBox.Add (this.dismissButton);
			global::Gtk.ButtonBox.ButtonBoxChild w5 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.actionButtonBox [this.dismissButton]));
			w5.Secondary = true;
			w5.Expand = false;
			w5.Fill = false;
			// Container child actionButtonBox.Gtk.ButtonBox+ButtonBoxChild
			this.viewButton = new global::Gtk.Button ();
			this.viewButton.Name = "viewButton";
			this.viewButton.UseUnderline = true;
			this.viewButton.FocusOnClick = false;
			this.viewButton.Relief = ((global::Gtk.ReliefStyle)(2));
			this.viewButton.Label = global::Mono.Unix.Catalog.GetString ("View");
			this.actionButtonBox.Add (this.viewButton);
			global::Gtk.ButtonBox.ButtonBoxChild w6 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.actionButtonBox [this.viewButton]));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			this.actionBox.Add (this.actionButtonBox);
			this.vLayoutBox.Add (this.actionBox);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vLayoutBox [this.actionBox]));
			w8.PackType = ((global::Gtk.PackType)(1));
			w8.Position = 2;
			w8.Fill = false;
			this.mainBox.Add (this.vLayoutBox);
			this.Add (this.mainBox);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 300;
			this.DefaultHeight = 170;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.MotionNotifyEvent += new global::Gtk.MotionNotifyEventHandler (this.OnMotionNotifyEvent);
			this.dismissButton.Clicked += new global::System.EventHandler (this.OnDismissButtonClicked);
			this.viewButton.Clicked += new global::System.EventHandler (this.OnViewButtonClicked);
		}
	}
}
