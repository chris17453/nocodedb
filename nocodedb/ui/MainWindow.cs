using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void new_package(object sender, EventArgs e){
        
    }

    protected void open_package(object sender, EventArgs e)
    {
    }

    protected void save_package(object sender, EventArgs e)
    {
    }

    protected void save_as_package(object sender, EventArgs e)
    {
    }

    protected void close_package(object sender, EventArgs e)
    {
    }
}
