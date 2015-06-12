
// This file has been generated by the GUI designer. Do not modify.
namespace CodeWalriiNotify
{
	public partial class VersionDialog
	{
		private global::Gtk.VBox mainSplitVbox;
		
		private global::Gtk.HBox headerHbox;
		
		private global::Gtk.Image logoImg;
		
		private global::Gtk.Label appTitleLbl;
		
		private global::Gtk.Table infoTable;
		
		private global::Gtk.Label copyrightLbl;
		
		private global::Gtk.Label copyrightLblLbl;
		
		private global::Gtk.Label dateLbl;
		
		private global::Gtk.Label dateLblLbl;
		
		private global::Gtk.Label versionLbl;
		
		private global::Gtk.Label versionLblLbl;
		
		private global::Gtk.Button okButton;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget CodeWalriiNotify.VersionDialog
			this.Name = "CodeWalriiNotify.VersionDialog";
			this.Title = global::Mono.Unix.Catalog.GetString ("Version");
			this.Icon = global::Stetic.IconLoader.LoadIcon (this, "gtk-dialog-info", global::Gtk.IconSize.Dnd);
			this.TypeHint = ((global::Gdk.WindowTypeHint)(1));
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child CodeWalriiNotify.VersionDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialogVbox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialogVbox.Gtk.Box+BoxChild
			this.mainSplitVbox = new global::Gtk.VBox ();
			this.mainSplitVbox.Name = "mainSplitVbox";
			this.mainSplitVbox.Spacing = 6;
			// Container child mainSplitVbox.Gtk.Box+BoxChild
			this.headerHbox = new global::Gtk.HBox ();
			this.headerHbox.Name = "headerHbox";
			this.headerHbox.Spacing = 6;
			// Container child headerHbox.Gtk.Box+BoxChild
			this.logoImg = new global::Gtk.Image ();
			this.logoImg.Name = "logoImg";
			this.logoImg.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Bell.png");
			this.headerHbox.Add (this.logoImg);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.headerHbox [this.logoImg]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child headerHbox.Gtk.Box+BoxChild
			this.appTitleLbl = new global::Gtk.Label ();
			this.appTitleLbl.Name = "appTitleLbl";
			this.appTitleLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Application Title");
			this.headerHbox.Add (this.appTitleLbl);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.headerHbox [this.appTitleLbl]));
			w3.Position = 1;
			this.mainSplitVbox.Add (this.headerHbox);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.mainSplitVbox [this.headerHbox]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child mainSplitVbox.Gtk.Box+BoxChild
			this.infoTable = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
			this.infoTable.Name = "infoTable";
			this.infoTable.RowSpacing = ((uint)(6));
			this.infoTable.ColumnSpacing = ((uint)(6));
			this.infoTable.BorderWidth = ((uint)(9));
			// Container child infoTable.Gtk.Table+TableChild
			this.copyrightLbl = new global::Gtk.Label ();
			this.copyrightLbl.Name = "copyrightLbl";
			this.copyrightLbl.Xalign = 0F;
			this.copyrightLbl.Yalign = 0F;
			this.copyrightLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Copyright");
			this.infoTable.Add (this.copyrightLbl);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.infoTable [this.copyrightLbl]));
			w5.TopAttach = ((uint)(2));
			w5.BottomAttach = ((uint)(3));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child infoTable.Gtk.Table+TableChild
			this.copyrightLblLbl = new global::Gtk.Label ();
			this.copyrightLblLbl.Name = "copyrightLblLbl";
			this.copyrightLblLbl.Xalign = 1F;
			this.copyrightLblLbl.Yalign = 0F;
			this.copyrightLblLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Copyright:");
			this.infoTable.Add (this.copyrightLblLbl);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.infoTable [this.copyrightLblLbl]));
			w6.TopAttach = ((uint)(2));
			w6.BottomAttach = ((uint)(3));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child infoTable.Gtk.Table+TableChild
			this.dateLbl = new global::Gtk.Label ();
			this.dateLbl.Name = "dateLbl";
			this.dateLbl.Xalign = 0F;
			this.dateLbl.Yalign = 0F;
			this.dateLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Date");
			this.infoTable.Add (this.dateLbl);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.infoTable [this.dateLbl]));
			w7.TopAttach = ((uint)(1));
			w7.BottomAttach = ((uint)(2));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child infoTable.Gtk.Table+TableChild
			this.dateLblLbl = new global::Gtk.Label ();
			this.dateLblLbl.Name = "dateLblLbl";
			this.dateLblLbl.Xalign = 1F;
			this.dateLblLbl.Yalign = 0F;
			this.dateLblLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Build Date:");
			this.infoTable.Add (this.dateLblLbl);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.infoTable [this.dateLblLbl]));
			w8.TopAttach = ((uint)(1));
			w8.BottomAttach = ((uint)(2));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child infoTable.Gtk.Table+TableChild
			this.versionLbl = new global::Gtk.Label ();
			this.versionLbl.Name = "versionLbl";
			this.versionLbl.Xalign = 0F;
			this.versionLbl.Yalign = 0F;
			this.versionLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Version");
			this.infoTable.Add (this.versionLbl);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.infoTable [this.versionLbl]));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child infoTable.Gtk.Table+TableChild
			this.versionLblLbl = new global::Gtk.Label ();
			this.versionLblLbl.Name = "versionLblLbl";
			this.versionLblLbl.Xalign = 1F;
			this.versionLblLbl.Yalign = 0F;
			this.versionLblLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Version:");
			this.infoTable.Add (this.versionLblLbl);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.infoTable [this.versionLblLbl]));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			this.mainSplitVbox.Add (this.infoTable);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.mainSplitVbox [this.infoTable]));
			w11.Position = 1;
			w11.Expand = false;
			w11.Fill = false;
			w1.Add (this.mainSplitVbox);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(w1 [this.mainSplitVbox]));
			w12.Position = 0;
			w12.Expand = false;
			w12.Fill = false;
			// Internal child CodeWalriiNotify.VersionDialog.ActionArea
			global::Gtk.HButtonBox w13 = this.ActionArea;
			w13.Name = "dialogActionArea";
			w13.Spacing = 10;
			w13.BorderWidth = ((uint)(5));
			w13.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialogActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.okButton = new global::Gtk.Button ();
			this.okButton.CanDefault = true;
			this.okButton.CanFocus = true;
			this.okButton.Name = "okButton";
			this.okButton.UseStock = true;
			this.okButton.UseUnderline = true;
			this.okButton.Label = "gtk-ok";
			this.AddActionWidget (this.okButton, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w14 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w13 [this.okButton]));
			w14.Expand = false;
			w14.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 240;
			this.Show ();
			this.okButton.Clicked += new global::System.EventHandler (this.OnOkButtonClicked);
		}
	}
}
