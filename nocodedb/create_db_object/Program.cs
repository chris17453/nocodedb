﻿using System;
using nocodedb.data;
using nocodedb.data.@interface;

namespace create_db_object{
    class MainClass{
        public static void Main(string[] args){
            Console.WriteLine("Hello World!");


            string o=nocodedb.data.assembly.generator.map_table("titan","titanDWS","dbo","titanDWS");


            nocodedb.data.assembly.generator.compile_dll("test",o);
            Console.Write(o);
            Console.ReadKey();

            //IData db=nocodedb.data.db.map_table("")
        }
    }
}