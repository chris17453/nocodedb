using System;
using nocodedb.data;
using nocodedb.data.@interface;

namespace create_db_object{
    class MainClass{
        public static void Main(string[] args){
            Console.WriteLine("Hello World!");


            Console.Write(nocodedb.data.db.map_table("titan","titanDWS","dbo","titanDWS"));
            Console.ReadKey();

            //IData db=nocodedb.data.db.map_table("")
        }
    }
}
