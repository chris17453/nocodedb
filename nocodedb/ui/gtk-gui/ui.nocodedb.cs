
// This file has been generated by the GUI designer. Do not modify.
namespace ui
{
	public partial class nocodedb
	{
		private global::Gtk.UIManager UIManager;

		private global::Gtk.Action PackagesAction;

		private global::Gtk.Action NewAction;

		private global::Gtk.Action OpenAction;

		private global::Gtk.Action SaveAction;

		private global::Gtk.Action SaveAsAction;

		private global::Gtk.Action CloseAction;

		private global::Gtk.VBox vbox2;

		private global::Gtk.MenuBar menubar1;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget ui.nocodedb
			this.UIManager = new global::Gtk.UIManager();
			global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup("Default");
			this.PackagesAction = new global::Gtk.Action("PackagesAction", global::Mono.Unix.Catalog.GetString("Packages"), null, null);
			this.PackagesAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Packages");
			w1.Add(this.PackagesAction, null);
			this.NewAction = new global::Gtk.Action("NewAction", global::Mono.Unix.Catalog.GetString("New"), null, null);
			this.NewAction.ShortLabel = global::Mono.Unix.Catalog.GetString("New");
			w1.Add(this.NewAction, null);
			this.OpenAction = new global::Gtk.Action("OpenAction", global::Mono.Unix.Catalog.GetString("Open"), null, null);
			this.OpenAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Open");
			w1.Add(this.OpenAction, null);
			this.SaveAction = new global::Gtk.Action("SaveAction", global::Mono.Unix.Catalog.GetString("Save"), null, null);
			this.SaveAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Save");
			w1.Add(this.SaveAction, null);
			this.SaveAsAction = new global::Gtk.Action("SaveAsAction", global::Mono.Unix.Catalog.GetString("Save As"), null, null);
			this.SaveAsAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Save As");
			w1.Add(this.SaveAsAction, null);
			this.CloseAction = new global::Gtk.Action("CloseAction", global::Mono.Unix.Catalog.GetString("Close"), null, null);
			this.CloseAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Close");
			w1.Add(this.CloseAction, null);
			this.UIManager.InsertActionGroup(w1, 0);
			this.AddAccelGroup(this.UIManager.AccelGroup);
			this.Name = "ui.nocodedb";
			this.Title = global::Mono.Unix.Catalog.GetString("nocodedb");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child ui.nocodedb.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString(@"<ui><menubar name='menubar1'><menu name='PackagesAction' action='PackagesAction'><menuitem name='NewAction' action='NewAction'/><separator/><menuitem name='OpenAction' action='OpenAction'/><separator/><menuitem name='SaveAction' action='SaveAction'/><menuitem name='SaveAsAction' action='SaveAsAction'/><separator/><menuitem name='CloseAction' action='CloseAction'/></menu></menubar></ui>");
			this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget("/menubar1")));
			this.menubar1.Name = "menubar1";
			this.vbox2.Add(this.menubar1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.menubar1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			this.Add(this.vbox2);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.Show();
			this.NewAction.Activated += new global::System.EventHandler(this.new_package);
		}
	}
}
