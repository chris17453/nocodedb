/*******************************************************************************

███╗   ██╗ ██████╗     ██████╗ ██████╗ ██████╗ ███████╗   ██████╗ ██████╗ 
████╗  ██║██╔═══██╗   ██╔════╝██╔═══██╗██╔══██╗██╔════╝   ██╔══██╗██╔══██╗
██╔██╗ ██║██║   ██║   ██║     ██║   ██║██║  ██║█████╗     ██║  ██║██████╔╝
██║╚██╗██║██║   ██║   ██║     ██║   ██║██║  ██║██╔══╝     ██║  ██║██╔══██╗
██║ ╚████║╚██████╔╝██╗╚██████╗╚██████╔╝██████╔╝███████╗██╗██████╔╝██████╔╝
╚═╝  ╚═══╝ ╚═════╝ ╚═╝ ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝╚═╝╚═════╝ ╚═════╝ 
   Created: 12-23-2017
   Author : Charles Watkins
   Email  : chris17453@gmail.com
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

using nocodedb;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Text;

namespace nocodedb.data{
    public class mssql_adapter:IData{
        public mssql_adapter(){
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod ()
        {
            StackTrace st = new StackTrace ();
            StackFrame sf = st.GetFrame (1);
            return sf.GetMethod().Name;
        }

        public string connection_string(string host, string user, string password) {
            SqlConnectionStringBuilder builder =  new SqlConnectionStringBuilder();  
            builder.UserID      =user;
            builder.DataSource  =host;
            builder.Password    =password;
            //builder["integrated Security"] = true;  
            //builder["Initial Catalog"] = "AdventureWorks;NewValue=Bad";  
            return builder.ConnectionString;
        }

        public event EventHandler LogEvent;
        protected virtual void log(query_params q,log_type type=log_type.Info,string message=null){
            EventHandler handler = LogEvent;
            if (handler != null){
                q.message  =message;
                q.log_type =type;
                q.function =GetCurrentMethod();
				handler(this,q);                                                                //dont lockup because of logging. Toss an event and run.
            }
        }

        SqlParameter[] parse_parameters(query_params q) {                               //just looping through the collection and setting the params up.
            List<SqlParameter> parameterCollection=new List<SqlParameter>();
            try{
                foreach (DictionaryEntry parameter in q.parameters){
                    if(((string)parameter.Key)[0]!='@') {
                        parameterCollection.Add(new SqlParameter("@" + parameter.Key, parameter.Value));
                    } else {
                        parameterCollection.Add(new SqlParameter((string)parameter.Key, (string)parameter.Value));
                    }
                }
            }catch (Exception e){
                this.log(q,log_type.Error, e.ToString());
            }
            return parameterCollection.ToArray();
        }

        public int execute_non_query(string connection_string, string query, Hashtable parameters) {
            query_params q= new query_params(connection_string,query,parameters,false, query_types.non);
            data_set ds= this.sql_query(q);
            return ds.affected_rows;
        }

        public column_data execute_scalar(string connection_string, string query, Hashtable parameters) {
            query_params q= new query_params(connection_string,query,parameters,false, query_types.scalar);
            data_set ds= this.sql_query(q);
            if(null!=ds && null!=ds.row && null!= ds.row.columns && ds.row.columns.Count>0){
                return ds.row.columns[0];
            }
            return null;
        }

        public data_set fetch(string connection_string, string query, Hashtable parameters, bool meta=false) {
            query_params q= new query_params(connection_string,query,parameters,meta, query_types.single);
            return this.sql_query(q);
        }

        public data_set fetch_all(string connection_string,string query,Hashtable parameters=null,bool meta=false){
            query_params q= new query_params(connection_string,query,parameters,meta, query_types.multiple);
            return this.sql_query(q);
        }

        public data_set sp_fetch(string connection_string, string query, Hashtable parameters, bool meta=false) {
            query_params q= new query_params(connection_string,query,parameters,meta, query_types.sp_single);
            return this.sql_query(q);
        }

        public data_set sp_fetch_all(string connection_string, string query, Hashtable parameters, bool meta=false) {
            query_params q= new query_params(connection_string,query,parameters,meta, query_types.sp_multiple);
            return this.sql_query(q);
        }

        public  data_set sql_query(query_params q){
            data_set results=new data_set();
            try{
                this.log(q,log_type.Info);

                using (SqlConnection conn = new SqlConnection(q.connection_string)) {
                    conn.Open();
                    if(conn.State!=System.Data.ConnectionState.Open) return null;                               //no connection DIE
                    if(q.type==query_types.multiple || q.type== query_types.single) {                               //only a placebo to resolve paramneter sniffing for dynamic querys that suck. not for SP's
                        using (SqlCommand comm = new SqlCommand("SET ARITHABORT ON", conn)) {   
                            comm.ExecuteNonQuery();
                        }
                    }
                    SqlDataReader reader  = null;
                    SqlCommand    command = new SqlCommand(q.query, conn);

                    if(null!=q.parameters) {
                        SqlParameter[] param_collection=parse_parameters(q);                           //load params into array
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
                                    results.row=result;
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

        public string extract_query(query_params q) {
            StringBuilder o=new StringBuilder();
            if(null!=q.parameters) {
                List<string> lst = new List<string>();
                foreach (var key2 in q.parameters) {
                    lst.Add(key2.ToString());
                }
                lst.Sort( delegate (string x,string y){ return y.Length.CompareTo(x.Length); });
                    foreach (string name in lst) {
                    
                    string value="";
                    if(null!=q.parameters[name]) value=q.parameters[name].ToString().Replace("'","''");
                    if(name[0]!='@') {
                        //    query=query.Replace("@"+name,"'"+value+"'");
                    } else {
                        //    query=query.Replace(name,"'"+value+"'");
                    }
                    int n;
                    bool isNumeric = int.TryParse(value, out n);


                    if (value=="True") {
                        o.Append(String.Format("DECLARE {0,-30}     bit=1;       --TRUE\r\n",name));
                    } else
                        if(value=="False") {
                            o.Append(String.Format("DECLARE {0,-30}     bit=0;       --FALSE\r\n",name));
                        } else
                            if(isNumeric==true) {
                                o.Append(String.Format("DECLARE {0,-30}     int={1};\r\n",name,value));
                            }
                            else { 
                                if(value.Length>=4000) {
                                    o.Append(String.Format("DECLARE {0,-30}     varchar({2})='{1}';\r\n",name,value,value.Length+10));
                                } else {
                                    o.Append(String.Format("DECLARE {0,-30}     nchar({2})='{1}';\r\n",name,value,value.Length+10));
                                }   
                            }
                }
            }
            o.Append("\r\n--"+q.connection_string+"\r\n");
            o.Append(q.query);
            return o.ToString();
        }

        public bool test_connect(string connection_string)
        {
            using (SqlConnection conn = new SqlConnection(connection_string)) {
                try{
                    conn.Open();
                    if(conn.State==System.Data.ConnectionState.Open) return true;
                } catch(Exception ex) {
                    var s=ex.Message;
                }
            }
            return false;
        }
    }
}

