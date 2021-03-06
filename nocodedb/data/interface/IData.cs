﻿/***********************************************
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
using nocodedb.data.models;

namespace nocodedb.data.@interface{

    public interface IData:IDisposable{
        event EventHandler<query_params> LogEvent;  
        string        _connection_string     { get; set; }
        string        build_connection_string(string host, string user,string password,string database);
        bool          test_connect           (string connection_string);                                                                  //tests a connection
        int           execute_non_query      (string connection_string, string query, parameters parameters=null);                        //returns number of rows affected
        column_data   execute_scalar         (string connection_string, string query, parameters parameters=null);                        //returns first column of result
        data_set      fetch                  (string connection_string, string query, parameters parameters=null,bool meta=false);        //returns a single row of data froma query.
        data_set      fetch_all              (string connection_string, string query, parameters parameters=null,bool meta=false);        //returns a list of rows from a query
        data_set      sp_fetch               (string connection_string, string query, parameters parameters=null,bool meta=false);        //returns a rows from a stored procedure 
        data_set      sp_fetch_all           (string connection_string, string query, parameters parameters=null,bool meta=false);        //returns a list of rows from a stored procedure 
        string        extract_query          (query_params q);                                                                            //returns a computed SQL String value of the preformed query
        new void      Dispose                ();

        fk.fk_members get_fk_to_table        (string connection_string,string database,string table,string schema);
        fk.fk_members get_fk_from_table      (string connection_string,string database,string table,string schema);
     
	}
  }


