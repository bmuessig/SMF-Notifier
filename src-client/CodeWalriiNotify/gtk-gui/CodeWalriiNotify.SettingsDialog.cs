
// This file has been generated by the GUI designer. Do not modify.
namespace CodeWalriiNotify
{
	public partial class SettingsDialog
	{
		private global::Gtk.Notebook tabControl;
		
		private global::Gtk.Table notifierTable;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.ScrolledWindow styleScroll;
		
		private global::Gtk.Table styleTable;
		
		private global::Gtk.ColorButton authorFgColorBtn;
		
		private global::Gtk.CheckButton bodyAntiAliasCb;
		
		private global::Gtk.ColorButton bodyBgColorBtn;
		
		private global::Gtk.ScrolledWindow bodyFormatScroll;
		
		private global::Gtk.TextView bodyFormatTxt;
		
		private global::Gtk.FontButton detailFontBtn;
		
		private global::Gtk.ColorButton footerBgColorBtn;
		
		private global::Gtk.ColorButton headerBgColorBtn;
		
		private global::Gtk.Label detailFnLbl;
		
		private global::Gtk.Label bodyFormatTxLbl;
		
		private global::Gtk.Label bodyAntiAliasCbLbl;
		
		private global::Gtk.Label headerBgClLbl;
		
		private global::Gtk.Label timeFgClLbl;
		
		private global::Gtk.Label titleFgClLbl;
		
		private global::Gtk.Label bodyBgClLbl;
		
		private global::Gtk.Label footerBgClLbl;
		
		private global::Gtk.Label authorFgClLbl;
		
		private global::Gtk.Label titleFnLbl;
		
		private global::Gtk.ColorButton timeFgColorBtn;
		
		private global::Gtk.ColorButton titleFgColorBtn;
		
		private global::Gtk.FontButton titleFontBtn;
		
		private global::Gtk.Label label2;
		
		private global::Gtk.Button cancelButton;
		
		private global::Gtk.Button okButton;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget CodeWalriiNotify.SettingsDialog
			this.Name = "CodeWalriiNotify.SettingsDialog";
			this.Title = global::Mono.Unix.Catalog.GetString ("Settings");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child CodeWalriiNotify.SettingsDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.tabControl = new global::Gtk.Notebook ();
			this.tabControl.CanFocus = true;
			this.tabControl.Name = "notebook1";
			this.tabControl.CurrentPage = 1;
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.notifierTable = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
			this.notifierTable.Name = "table1";
			this.notifierTable.RowSpacing = ((uint)(6));
			this.notifierTable.ColumnSpacing = ((uint)(6));
			this.tabControl.Add (this.notifierTable);
			// Notebook tab
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Notifier Settings");
			this.tabControl.SetTabLabel (this.notifierTable, this.label1);
			this.label1.ShowAll ();
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.styleScroll = new global::Gtk.ScrolledWindow ();
			this.styleScroll.CanFocus = true;
			this.styleScroll.Name = "scrolledwindow1";
			this.styleScroll.VscrollbarPolicy = ((global::Gtk.PolicyType)(0));
			this.styleScroll.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
			this.styleScroll.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow1.Gtk.Container+ContainerChild
			global::Gtk.Viewport w3 = new global::Gtk.Viewport ();
			w3.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.styleTable = new global::Gtk.Table (((uint)(11)), ((uint)(2)), false);
			this.styleTable.Name = "table2";
			this.styleTable.RowSpacing = ((uint)(6));
			this.styleTable.ColumnSpacing = ((uint)(6));
			// Container child table2.Gtk.Table+TableChild
			this.authorFgColorBtn = new global::Gtk.ColorButton ();
			this.authorFgColorBtn.CanFocus = true;
			this.authorFgColorBtn.Events = ((global::Gdk.EventMask)(784));
			this.authorFgColorBtn.Name = "authorFgColorBtn";
			this.styleTable.Add (this.authorFgColorBtn);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.styleTable [this.authorFgColorBtn]));
			w4.TopAttach = ((uint)(5));
			w4.BottomAttach = ((uint)(6));
			w4.LeftAttach = ((uint)(1));
			w4.RightAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(1));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.bodyAntiAliasCb = new global::Gtk.CheckButton ();
			this.bodyAntiAliasCb.CanFocus = true;
			this.bodyAntiAliasCb.Name = "bodyAntiAliasCb";
			this.bodyAntiAliasCb.Label = "";
			this.bodyAntiAliasCb.DrawIndicator = true;
			this.styleTable.Add (this.bodyAntiAliasCb);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.styleTable [this.bodyAntiAliasCb]));
			w5.TopAttach = ((uint)(9));
			w5.BottomAttach = ((uint)(10));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(1));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.bodyBgColorBtn = new global::Gtk.ColorButton ();
			this.bodyBgColorBtn.CanFocus = true;
			this.bodyBgColorBtn.Events = ((global::Gdk.EventMask)(784));
			this.bodyBgColorBtn.Name = "bodyBgColorBtn";
			this.styleTable.Add (this.bodyBgColorBtn);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.styleTable [this.bodyBgColorBtn]));
			w6.TopAttach = ((uint)(3));
			w6.BottomAttach = ((uint)(4));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.XOptions = ((global::Gtk.AttachOptions)(1));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.bodyFormatScroll = new global::Gtk.ScrolledWindow ();
			this.bodyFormatScroll.Name = "bodyFormatScroll";
			this.bodyFormatScroll.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child bodyFormatScroll.Gtk.Container+ContainerChild
			this.bodyFormatTxt = new global::Gtk.TextView ();
			this.bodyFormatTxt.CanFocus = true;
			this.bodyFormatTxt.Name = "bodyFormatTxt";
			this.bodyFormatScroll.Add (this.bodyFormatTxt);
			this.styleTable.Add (this.bodyFormatScroll);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.styleTable [this.bodyFormatScroll]));
			w8.TopAttach = ((uint)(8));
			w8.BottomAttach = ((uint)(9));
			w8.LeftAttach = ((uint)(1));
			w8.RightAttach = ((uint)(2));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.detailFontBtn = new global::Gtk.FontButton ();
			this.detailFontBtn.CanFocus = true;
			this.detailFontBtn.Name = "detailFontBtn";
			this.styleTable.Add (this.detailFontBtn);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.styleTable [this.detailFontBtn]));
			w9.TopAttach = ((uint)(7));
			w9.BottomAttach = ((uint)(8));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.footerBgColorBtn = new global::Gtk.ColorButton ();
			this.footerBgColorBtn.CanFocus = true;
			this.footerBgColorBtn.Events = ((global::Gdk.EventMask)(784));
			this.footerBgColorBtn.Name = "footerBgColorBtn";
			this.styleTable.Add (this.footerBgColorBtn);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.styleTable [this.footerBgColorBtn]));
			w10.TopAttach = ((uint)(4));
			w10.BottomAttach = ((uint)(5));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.XOptions = ((global::Gtk.AttachOptions)(1));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.headerBgColorBtn = new global::Gtk.ColorButton ();
			this.headerBgColorBtn.CanFocus = true;
			this.headerBgColorBtn.Events = ((global::Gdk.EventMask)(784));
			this.headerBgColorBtn.Name = "headerBgColorBtn";
			this.styleTable.Add (this.headerBgColorBtn);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.styleTable [this.headerBgColorBtn]));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.XOptions = ((global::Gtk.AttachOptions)(1));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.detailFnLbl = new global::Gtk.Label ();
			this.detailFnLbl.Name = "label10";
			this.detailFnLbl.Xalign = 0F;
			this.detailFnLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Detail Font");
			this.styleTable.Add (this.detailFnLbl);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.styleTable [this.detailFnLbl]));
			w12.TopAttach = ((uint)(7));
			w12.BottomAttach = ((uint)(8));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.bodyFormatTxLbl = new global::Gtk.Label ();
			this.bodyFormatTxLbl.Name = "label11";
			this.bodyFormatTxLbl.Xalign = 0F;
			this.bodyFormatTxLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Body Format");
			this.styleTable.Add (this.bodyFormatTxLbl);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.styleTable [this.bodyFormatTxLbl]));
			w13.TopAttach = ((uint)(8));
			w13.BottomAttach = ((uint)(9));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.bodyAntiAliasCbLbl = new global::Gtk.Label ();
			this.bodyAntiAliasCbLbl.Name = "label12";
			this.bodyAntiAliasCbLbl.Xalign = 0F;
			this.bodyAntiAliasCbLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Body Use Anti Alias");
			this.styleTable.Add (this.bodyAntiAliasCbLbl);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.styleTable [this.bodyAntiAliasCbLbl]));
			w14.TopAttach = ((uint)(9));
			w14.BottomAttach = ((uint)(10));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.headerBgClLbl = new global::Gtk.Label ();
			this.headerBgClLbl.Name = "label3";
			this.headerBgClLbl.Xalign = 0F;
			this.headerBgClLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Header Backcolor");
			this.styleTable.Add (this.headerBgClLbl);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.styleTable [this.headerBgClLbl]));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.timeFgClLbl = new global::Gtk.Label ();
			this.timeFgClLbl.Name = "label4";
			this.timeFgClLbl.Xalign = 0F;
			this.timeFgClLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Timestamp Forecolor");
			this.styleTable.Add (this.timeFgClLbl);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.styleTable [this.timeFgClLbl]));
			w16.TopAttach = ((uint)(1));
			w16.BottomAttach = ((uint)(2));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.titleFgClLbl = new global::Gtk.Label ();
			this.titleFgClLbl.Name = "label5";
			this.titleFgClLbl.Xalign = 0F;
			this.titleFgClLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Title Forecolor");
			this.styleTable.Add (this.titleFgClLbl);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.styleTable [this.titleFgClLbl]));
			w17.TopAttach = ((uint)(2));
			w17.BottomAttach = ((uint)(3));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.bodyBgClLbl = new global::Gtk.Label ();
			this.bodyBgClLbl.Name = "label6";
			this.bodyBgClLbl.Xalign = 0F;
			this.bodyBgClLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Body Backcolor");
			this.styleTable.Add (this.bodyBgClLbl);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.styleTable [this.bodyBgClLbl]));
			w18.TopAttach = ((uint)(3));
			w18.BottomAttach = ((uint)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.footerBgClLbl = new global::Gtk.Label ();
			this.footerBgClLbl.Name = "label7";
			this.footerBgClLbl.Xalign = 0F;
			this.footerBgClLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Footer Backcolor");
			this.styleTable.Add (this.footerBgClLbl);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.styleTable [this.footerBgClLbl]));
			w19.TopAttach = ((uint)(4));
			w19.BottomAttach = ((uint)(5));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.authorFgClLbl = new global::Gtk.Label ();
			this.authorFgClLbl.Name = "label8";
			this.authorFgClLbl.Xalign = 0F;
			this.authorFgClLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Author Forecolor");
			this.styleTable.Add (this.authorFgClLbl);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.styleTable [this.authorFgClLbl]));
			w20.TopAttach = ((uint)(5));
			w20.BottomAttach = ((uint)(6));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.titleFnLbl = new global::Gtk.Label ();
			this.titleFnLbl.Name = "label9";
			this.titleFnLbl.Xalign = 0F;
			this.titleFnLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Title Font");
			this.styleTable.Add (this.titleFnLbl);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.styleTable [this.titleFnLbl]));
			w21.TopAttach = ((uint)(6));
			w21.BottomAttach = ((uint)(7));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.timeFgColorBtn = new global::Gtk.ColorButton ();
			this.timeFgColorBtn.CanFocus = true;
			this.timeFgColorBtn.Events = ((global::Gdk.EventMask)(784));
			this.timeFgColorBtn.Name = "timeFgColorBtn";
			this.styleTable.Add (this.timeFgColorBtn);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.styleTable [this.timeFgColorBtn]));
			w22.TopAttach = ((uint)(1));
			w22.BottomAttach = ((uint)(2));
			w22.LeftAttach = ((uint)(1));
			w22.RightAttach = ((uint)(2));
			w22.XOptions = ((global::Gtk.AttachOptions)(0));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.titleFgColorBtn = new global::Gtk.ColorButton ();
			this.titleFgColorBtn.CanFocus = true;
			this.titleFgColorBtn.Events = ((global::Gdk.EventMask)(784));
			this.titleFgColorBtn.Name = "titleFgColorBtn";
			this.styleTable.Add (this.titleFgColorBtn);
			global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.styleTable [this.titleFgColorBtn]));
			w23.TopAttach = ((uint)(2));
			w23.BottomAttach = ((uint)(3));
			w23.LeftAttach = ((uint)(1));
			w23.RightAttach = ((uint)(2));
			w23.XOptions = ((global::Gtk.AttachOptions)(1));
			w23.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.titleFontBtn = new global::Gtk.FontButton ();
			this.titleFontBtn.CanFocus = true;
			this.titleFontBtn.Name = "titleFontBtn";
			this.styleTable.Add (this.titleFontBtn);
			global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.styleTable [this.titleFontBtn]));
			w24.TopAttach = ((uint)(6));
			w24.BottomAttach = ((uint)(7));
			w24.LeftAttach = ((uint)(1));
			w24.RightAttach = ((uint)(2));
			w24.YOptions = ((global::Gtk.AttachOptions)(4));
			w3.Add (this.styleTable);
			this.styleScroll.Add (w3);
			this.tabControl.Add (this.styleScroll);
			global::Gtk.Notebook.NotebookChild w27 = ((global::Gtk.Notebook.NotebookChild)(this.tabControl [this.styleScroll]));
			w27.Position = 1;
			// Notebook tab
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Style Settings");
			this.tabControl.SetTabLabel (this.styleScroll, this.label2);
			this.label2.ShowAll ();
			w1.Add (this.tabControl);
			global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(w1 [this.tabControl]));
			w28.Position = 0;
			// Internal child CodeWalriiNotify.SettingsDialog.ActionArea
			global::Gtk.HButtonBox w29 = this.ActionArea;
			w29.Name = "dialog1_ActionArea";
			w29.Spacing = 10;
			w29.BorderWidth = ((uint)(5));
			w29.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.cancelButton = new global::Gtk.Button ();
			this.cancelButton.CanDefault = true;
			this.cancelButton.CanFocus = true;
			this.cancelButton.Name = "buttonCancel";
			this.cancelButton.UseStock = true;
			this.cancelButton.UseUnderline = true;
			this.cancelButton.Label = "gtk-cancel";
			this.AddActionWidget (this.cancelButton, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w30 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w29 [this.cancelButton]));
			w30.Expand = false;
			w30.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.okButton = new global::Gtk.Button ();
			this.okButton.CanDefault = true;
			this.okButton.CanFocus = true;
			this.okButton.Name = "buttonOk";
			this.okButton.UseStock = true;
			this.okButton.UseUnderline = true;
			this.okButton.Label = "gtk-ok";
			this.AddActionWidget (this.okButton, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w31 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w29 [this.okButton]));
			w31.Position = 1;
			w31.Expand = false;
			w31.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 503;
			this.DefaultHeight = 314;
			this.Show ();
		}
	}
}
