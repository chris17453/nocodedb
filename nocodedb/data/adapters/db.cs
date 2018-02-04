/***********************************************
    ███╗   ██╗ ██████╗██████╗ ██████╗ 
    ████╗  ██║██╔════╝██╔══██╗██╔══██╗
    ██╔██╗ ██║██║     ██║  ██║██████╔╝
    ██║╚██╗██║██║     ██║  ██║██╔══██╗
    ██║ ╚████║╚██████╗██████╔╝██████╔╝
    ╚═╝  ╚═══╝ ╚═════╝╚═════╝ ╚═════╝ 
    author : Charles Watkins
    created: 2017-12-23
    email  : chris17453@gmail.com
    github : https://github.com/chris17453
**********************************************/
using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using nocodedb.data.@interface;
using nocodedb.data.models;


using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Reflection;

namespace nocodedb.data{

    public static class db{

        public static int execute_non_query(string connection_string, string query, parameters parameters=null) {
            IData a=get_adapter(connection_string);
            return a.execute_non_query(a._connection_string,query,parameters);
        }


        public static column_data execute_scalar(string connection_string, string query, parameters parameters=null) {
            IData a=get_adapter(connection_string);
            return a.execute_scalar(a._connection_string,query,parameters);
        }

        public static data_set fetch(string connection_string, string query, parameters parameters=null, bool meta = false) {
            IData a=get_adapter(connection_string);
            return a.fetch(a._connection_string,query,parameters,meta);
        }

        public static data_set fetch_all(string connection_string, string query, parameters parameters=null, bool meta = false) {
            IData a=get_adapter(connection_string);
            return a.fetch_all(a._connection_string,query,parameters,meta);
        }

        public static data_set sp_fetch(string connection_string, string query, parameters parameters=null, bool meta = false) {
            IData a=get_adapter(connection_string);
            return a.sp_fetch(a._connection_string,query,parameters,meta);
        }

        public static data_set sp_fetch_all(string connection_string, string query, parameters parameters=null, bool meta = false) {
            IData a=get_adapter(connection_string);
            return a.sp_fetch_all(a._connection_string,query,parameters,meta);
        }

        public static bool test_connect(string connection_string) {
            IData a=get_adapter(connection_string);
            return a.test_connect(a._connection_string);
        }
        public static string extract_query(query_params q){
            IData a=get_adapter(q.connection_string);
            return a.extract_query(q);
        }
            
        public static IData get_adapter(string connection_target){ 
            string _connection_string = String.Empty;
            string _provider_name     = String.Empty;
            IData adapter           = null;

            if(connection_target=="titan") {                                                                    //its a nocodedb connection
                _connection_string = ConfigurationManager.ConnectionStrings["titan_server"].ConnectionString;
                _provider_name     = ConfigurationManager.ConnectionStrings["titan_server"].ProviderName;
            } else if(String.IsNullOrEmpty(connection_target)) {                                                //its a data connection
                _connection_string = ConfigurationManager.ConnectionStrings["data_server"].ConnectionString;
                _provider_name     = ConfigurationManager.ConnectionStrings["data_server"].ProviderName;
            } else {                                                                                            
                if(null==ConfigurationManager.ConnectionStrings[connection_target]) {                                       //its the actual connection string...
                    string[] tokens=connection_target.Split('|');
                    if(tokens.Length>=2) {
                        _connection_string=tokens[1];
                        _provider_name    =tokens[0];
                    } else {
                        throw new Exception("Invalid number of connection components");
                    }
                } else {
                    _connection_string = ConfigurationManager.ConnectionStrings[connection_target].ConnectionString;        //use custom config connection string
                    _provider_name     = ConfigurationManager.ConnectionStrings[connection_target].ProviderName;
                }
            }

            switch(_provider_name){
                case "System.Data.SqlClient" : adapter=new adapters.mssql_adapter(); adapter._connection_string=_connection_string; break;    //MSSQL
                case "MySql.Data.MysqlClient": adapter=new adapters.mysql_adapter(); adapter._connection_string=_connection_string; break;    //MySQL
            }
            if(null!=adapter) {
                adapter.LogEvent+=HandleEventHandler;

            }
            return adapter;
        }

        static void HandleEventHandler(object sender, query_params q) {
            log.write("data",q.log_type,q.ToString());
        }



    }
}