using System;
using System.Collections.Generic;
using Mono.Options;
//using ncdb;


namespace ncdb_map{
    class MainClass{
        public static void Main(string[] args){
            string host    =string.Empty;
            string user    =string.Empty;
            string password=string.Empty;
            string database=string.Empty;
            string type    =string.Empty;
            string conn_str=string.Empty;

            bool show_help = false;
            List<string> names = new List<string> ();

            var p = new OptionSet () {
                { "cs|connection_string="    , "the {CONNECTION STRING} Formatted \"PROVIDER_STRING|CONNECTION_STRING\"" +
                    " "  , v => { conn_str=v.ToUpper(); }},
                { "h|host="    , "the {HOST} for this connection"           , v => { host=v; }},
                { "u|user="    , "the {USER} for this connection"           , v => { user=v; }},
                { "p|password=", "the {PASSWORD} for this connection"       , v => { password=v; }},
                { "d|database=", "the {DATABASE} to map"                    , v => { database=v; }},
                { "t|type="    , "the {TYPE} of database. (MSSQL, MySQL) "  , v => { type=v.ToUpper(); }},
                { "?|help"     , "show this message and exit"               , v => show_help = v != null },
            };

            List<string> extra;
            try {
                extra = p.Parse (args);
            }
            catch (OptionException e) {
                Console.Write ("ncdb_map: ");
                Console.WriteLine (e.Message);
                Console.WriteLine ("Try `ncdb_map --help' for more information.");
                return;
            }


            if(string.IsNullOrWhiteSpace(conn_str)) {
                switch(type){
                    case "MSSQL":   using(nocodedb.data.@interface.IData adapter=new nocodedb.data.adapters.mssql_adapter()){ 
                                        conn_str="System.Data.SqlClient|"+adapter.build_connection_string(host,user,password); 
                                    }
                                    break;
                    case "MYSQL":   using(nocodedb.data.@interface.IData adapter=new nocodedb.data.adapters.mysql_adapter()){ 
                                        conn_str="MySql.Data.MysqlClient|"+adapter.build_connection_string(host,user,password); 
                                    }
                                    break;
                    default:    Console.WriteLine("Invalid Database Type."); 
                                return;

                }
            }

            

            Console.WriteLine("Writing DB Code");
            string o=nocodedb.data.assembly.generator.map_database(conn_str,database);
            Console.WriteLine("Compiling Code");
            nocodedb.data.assembly.generator.compile_dll("ncdb."+database,o);
            Console.WriteLine("Complete, your file is ---> ncdb."+database+".dll");
        }
    }
}
