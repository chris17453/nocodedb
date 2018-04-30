using System;
using Microsoft.Owin.Hosting;


namespace ui{
    class MainClass{
       
        public static void Main(string[] args){
            string baseUri = "http://localhost:52000";



            Console.WriteLine("Starting web Server...");
            if (baseUri != null){

                using (WebApp.Start<Startup>(baseUri ))
                {
                    Console.WriteLine("Hosted: "+baseUri );
                    Console.ReadLine();
                }
            }
        }
    }
}
