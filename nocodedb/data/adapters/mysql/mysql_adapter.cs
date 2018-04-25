/***********************************************
    ███╗   ██╗ ██████╗██████╗ ██████╗ 
    ████╗  ██║██╔════╝██╔══██╗██╔══██╗
    ██╔██╗ ██║██║     ██║  ██║██████╔╝
    ██║╚██╗██║██║     ██║  ██║██╔══██╗
    ██║ ╚████║╚██████╗██████╔╝██████╔╝
    ╚═╝  ╚═══╝ ╚═════╝╚═════╝ ╚═════╝ 
    Author : Charles Watkins
    Created: 2017-12-23
    email  : chris17453@gmail.com
    github : https://github.com/chris17453
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

using nocodedb;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Text;
using MySql.Data.MySqlClient;
using nocodedb.data.@interface;
using nocodedb.data.models;

namespace nocodedb.data.adapters{
    public class mysql_adapter:base_adapter,IData{
        public override char left_field_seperator  { get { return '`'; } }
        public override char right_field_seperator { get { return '`'; } }

        public mysql_adapter(){
        }


        public override string build_connection_string(string host, string user, string password,string database) {
            MySqlConnectionStringBuilder builder =  new MySqlConnectionStringBuilder();  
            builder.UserID      =user;
            builder.Server      =host;
            builder.Password    =password;
            if(!string.IsNullOrWhiteSpace(database)) builder.Database    =database;
            return builder.ConnectionString;
        }

        public override  bool test_connect(string connection_string) {

            using (MySqlConnection conn = new MySqlConnection(connection_string)) {
                try{
                    conn.Open();
                    if(conn.State==System.Data.ConnectionState.Open) return true;
                } catch(Exception ex) {
                    var s=ex.Message;
                }
            }
            return false;
        }

        MySqlParameter[] parse_parameters(query_params q) {                               //just looping through the collection and setting the params up.
            List<MySqlParameter> parameterCollection=new List<MySqlParameter>();
            try{
                foreach (parameter parameter in q.parameters){
                    if(((string)parameter.key)[0]!='@') {
                        parameterCollection.Add(new MySqlParameter("@" + parameter.key, parameter.value));
                    } else {
                        parameterCollection.Add(new MySqlParameter((string)parameter.key, (string)parameter.value));
                    }
                }
            }catch (Exception e){
                this.log(q,log_type.Error, e.ToString());
            }
            return parameterCollection.ToArray();
        }

        public override data_set sql_query(query_params q){
            data_set results=new data_set();
            try{
                this.log(q,log_type.Info);

                using (MySqlConnection conn = new MySqlConnection(q.connection_string)) {
                    conn.Open();
                    if(conn.State!=System.Data.ConnectionState.Open) return null;                               //no connection DIE
                    if(q.type==query_types.multiple || q.type== query_types.single) {                               //only a placebo to resolve paramneter sniffing for dynamic querys that suck. not for SP's
                        using (MySqlCommand comm = new MySqlCommand("SET ARITHABORT ON", conn)) {   
                            comm.ExecuteNonQuery();
                        }
                    }
                    MySqlDataReader reader  = null;
                    MySqlCommand    command = new MySqlCommand(q.query, conn);

                    if(null!=q.parameters) {
                        MySqlParameter[] param_collection=parse_parameters(q);                           //load params into array
                        if(param_collection.Length>0) {
                            command.Parameters.AddRange(param_collection);                                          //add params to sql command
                        }
                    }
                    CommandBehavior command_behavior=CommandBehavior.CloseConnection;
                    if(q.meta) {
                        command_behavior|=CommandBehavior.KeyInfo;
                    }


                    if(q.type==query_types.non) {
                        results.affected_rows = command.ExecuteNonQuery();

                    }
                    if(q.type==query_types.scalar){
                        var firstColumn = command.ExecuteScalar();
                        if (firstColumn != null) {
                            results.scalar_results = new column_data(firstColumn);
                        }

                    }

                    if(q.type==query_types.multiple || q.type== query_types.single || q.type==query_types.sp_single || q.type==query_types.sp_multiple) {
                        reader = command.ExecuteReader(command_behavior);
                        if(q.meta) {
                            DataTable schema=reader.GetSchemaTable();
                            int dbFields = schema.Columns.Count;;

                            for (int i = 0; i < dbFields; i++){
                                results.columns.Add(new column_meta(schema.Columns,schema.Rows[i]));
                            }
                        }

                        if (reader.HasRows) {
                            this.log(q,log_type.Info,"Rows Returned");
                            while (reader.Read()) {
                                row result=new row();
                                for (int i = 0; i < reader.FieldCount; i++) {
                                    result.columns.Add(new column_data(reader[i]));
                                }//end field loop
                                if(q.type==query_types.single || q.type==query_types.sp_single) {                                                                        //only 1 row
                                    results.rows.Add(result);
                                    break;
                                } else {
                                    results.rows.Add(result);
                                }
                            }//end while
                        }//end if reader
                        else {
                            this.log(q,log_type.Info,"NO Rows Returned");
                        }
                    }//end data collection for query types multiple or single

                    reader.Close();                                                                             //close this out as well..
                    conn.Close();                                                                               //close it out
                    conn.Dispose();                                                                             //clear it (using does this...)
                }//end using
            }catch (Exception e){
                this.log(q,log_type.Error,e.ToString());
            }
            return results;
        }
        public override void Dispose(){
            base.Dispose();
        }
    }
}

