using System;
using System.Collections;
using Gtk;
using nocodedb.data;
using System.Web.Script.Serialization;
public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        textview2.Buffer.Text="START";

        mssql_adapter mssql=new mssql_adapter();
        string conn     =mssql.connection_string(@"10.0.0.229\ND","monarch","p825amwX$");
        string query    ="SELECT top 100 * FROM shipit.dbo.enr_txn_shipping";
        Hashtable param=new Hashtable(){{"id","6"}};

        data_set ds=mssql.fetch_all(conn,query,param,true);
        var serializer = new JavaScriptSerializer();
        var serializedResult = serializer.Serialize(ds);
        textview2.Buffer.Text=serializedResult;

    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}
