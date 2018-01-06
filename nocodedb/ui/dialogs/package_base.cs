using System;
using Gtk;

namespace ui.dialogs
{
    public partial class package_base : Gtk.Dialog
    {
        public package_base()
        {
            this.Build();
        }

        protected void target_removed(object sender, EventArgs e)
        {
            int x=0;
        }

        protected void close_dialog(object sender, EventArgs e)
        {
            this.Destroy();
        }

        protected void save_package(object sender, EventArgs e)
        {
            //(string.IsNullOrWhiteSpace((package_name.Text))) {
                //Gtk.MessageDialog d =new Gtk.MessageDialog(
                    MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Package must have a name");
                    msdSame.Title="Error";
                    msdSame.Run();
                    return;
            //}
        }

    }
}
