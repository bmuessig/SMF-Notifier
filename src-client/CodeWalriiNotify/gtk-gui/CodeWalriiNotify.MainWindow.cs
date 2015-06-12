
// This file has been generated by the GUI designer. Do not modify.
namespace CodeWalriiNotify
{
	public partial class MainWindow
	{
		private global::Gtk.UIManager UIManager;
		
		private global::Gtk.Action MenuAction;
		
		private global::Gtk.Action refreshAction;
		
		private global::Gtk.Action preferencesAction;
		
		private global::Gtk.Action quitAction;
		
		private global::Gtk.ToggleAction autoRefreshAction;
		
		private global::Gtk.Action versionAction;
		
		private global::Gtk.VBox horizontalSplit;
		
		private global::Gtk.MenuBar menuBar;
		
		private global::CodeWalriiNotify.RecyclerView mainRecyclerview;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget CodeWalriiNotify.MainWindow
			this.UIManager = new global::Gtk.UIManager ();
			global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
			this.MenuAction = new global::Gtk.Action ("MenuAction", global::Mono.Unix.Catalog.GetString ("Menu"), null, null);
			this.MenuAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Menu");
			w1.Add (this.MenuAction, null);
			this.refreshAction = new global::Gtk.Action ("refreshAction", global::Mono.Unix.Catalog.GetString ("_Refresh Posts"), null, "gtk-refresh");
			this.refreshAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Refresh");
			w1.Add (this.refreshAction, "F5");
			this.preferencesAction = new global::Gtk.Action ("preferencesAction", global::Mono.Unix.Catalog.GetString ("_Settings"), null, "gtk-preferences");
			this.preferencesAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Settings");
			w1.Add (this.preferencesAction, null);
			this.quitAction = new global::Gtk.Action ("quitAction", global::Mono.Unix.Catalog.GetString ("Quit"), null, "gtk-quit");
			this.quitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Quit");
			w1.Add (this.quitAction, null);
			this.autoRefreshAction = new global::Gtk.ToggleAction ("autoRefreshAction", global::Mono.Unix.Catalog.GetString ("_Auto Refresh"), null, null);
			this.autoRefreshAction.Active = true;
			this.autoRefreshAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Auto Refresh");
			w1.Add (this.autoRefreshAction, null);
			this.versionAction = new global::Gtk.Action ("versionAction", global::Mono.Unix.Catalog.GetString ("Version"), null, "gtk-dialog-info");
			this.versionAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Version");
			w1.Add (this.versionAction, null);
			this.UIManager.InsertActionGroup (w1, 0);
			this.AddAccelGroup (this.UIManager.AccelGroup);
			this.WidthRequest = 540;
			this.HeightRequest = 580;
			this.Name = "CodeWalriiNotify.MainWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("Notifier");
			this.TypeHint = ((global::Gdk.WindowTypeHint)(1));
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.DefaultWidth = 540;
			this.DefaultHeight = 580;
			// Container child CodeWalriiNotify.MainWindow.Gtk.Container+ContainerChild
			this.horizontalSplit = new global::Gtk.VBox ();
			this.horizontalSplit.Name = "horizontalSplit";
			this.horizontalSplit.Spacing = 6;
			// Container child horizontalSplit.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString (@"<ui><menubar name='menuBar'><menu name='MenuAction' action='MenuAction'><menuitem name='refreshAction' action='refreshAction'/><menuitem name='autoRefreshAction' action='autoRefreshAction'/><separator/><menuitem name='preferencesAction' action='preferencesAction'/><menuitem name='versionAction' action='versionAction'/><separator/><menuitem name='quitAction' action='quitAction'/></menu></menubar></ui>");
			this.menuBar = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menuBar")));
			this.menuBar.Name = "menuBar";
			this.horizontalSplit.Add (this.menuBar);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.horizontalSplit [this.menuBar]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child horizontalSplit.Gtk.Box+BoxChild
			this.mainRecyclerview = new global::CodeWalriiNotify.RecyclerView ();
			this.mainRecyclerview.HeightRequest = 500;
			this.mainRecyclerview.Events = ((global::Gdk.EventMask)(256));
			this.mainRecyclerview.Name = "mainRecyclerview";
			this.mainRecyclerview.VScroll = 0D;
			this.horizontalSplit.Add (this.mainRecyclerview);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.horizontalSplit [this.mainRecyclerview]));
			w3.Position = 1;
			this.Add (this.horizontalSplit);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.FocusInEvent += new global::Gtk.FocusInEventHandler (this.OnFocusInEvent);
			this.refreshAction.Activated += new global::System.EventHandler (this.OnRefreshActionActivated);
			this.preferencesAction.Activated += new global::System.EventHandler (this.OnPreferencesActionActivated);
			this.quitAction.Activated += new global::System.EventHandler (this.OnQuitActionActivated);
			this.autoRefreshAction.Activated += new global::System.EventHandler (this.OnAutoRefreshActionActivated);
			this.versionAction.Activated += new global::System.EventHandler (this.OnVersionActionActivated);
		}
	}
}
