using System;
using Gtk;
using Microsoft.Owin.Hosting;


namespace ui{
    class MainClass{
       
        public static void Main(string[] args){
            string baseUri = "http://localhost:52000";



            Console.WriteLine("Starting web Server...");
            if (baseUri != null){
                using( WebApp.Start<Startup>(baseUri)){
                    Application.Init ();

                    Window window = new Window ("helloworld");
                    window.Show();
                    Application.Run ();
                    MessageDialog msdSame = new MessageDialog(null, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Package must have a name");
                    msdSame.Title="Error";
                    msdSame.Run();
                }
            }
        }
    }
}
