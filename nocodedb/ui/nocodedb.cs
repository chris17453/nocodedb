using System;
namespace ui
{
    public partial class nocodedb : Gtk.Window
    {
        public nocodedb() :
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
        }

        protected void new_package(object sender, EventArgs e)
        {
            dialogs.package_base package=new dialogs.package_base();
            package.Run();
        }
    }
}
