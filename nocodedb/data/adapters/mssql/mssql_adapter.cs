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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

using nocodedb;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Text;
using nocodedb.data.@interface;
using nocodedb.data.models;

namespace nocodedb.data.adapters{
    public class mssql_adapter:base_adapter{
        public override char left_field_seperator  { get { return '['; } }
        public override char right_field_seperator { get { return ']'; } }

        public mssql_adapter(){

        //    SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

        }


        public override string build_connection_string(string host, string user, string password,string database) {
            SqlConnectionStringBuilder builder =  new SqlConnectionStringBuilder();  
            builder.UserID      =user;
            builder.DataSource  =host;
            builder.Password    =password;
            if(!String.IsNullOrWhiteSpace(database)) builder.InitialCatalog=database;
            //builder["integrated Security"] = true;  
            //builder["Initial Catalog"] = "AdventureWorks;NewValue=Bad";  
            return builder.ConnectionString;
        }

        public override bool test_connect(string connection_string) {
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

        SqlParameter[] parse_parameters(query_params q) {                               //just looping through the collection and setting the params up.
            List<SqlParameter> parameterCollection=new List<SqlParameter>();
            try{
                foreach (parameter  parameter in q.parameters){
                    if(((string)parameter.key)[0]!='@') {
                        parameterCollection.Add(new SqlParameter("@" + parameter.key, parameter.value));
                    } else {
                        parameterCollection.Add(new SqlParameter((string)parameter.key, (string)parameter.value));
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
                //this.log(q,log_type.Info);                                                                    //dont need this roes returned not returned and error handle this.

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
                        SqlParameter[] param_collection=parse_parameters(q);                                        //load params into array
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
                            int dbFields = schema.Rows.Count;;

                            for (int i = 0; i < dbFields; i++){
                                results.columns.Add(new column_meta(schema.Columns,schema.Rows[i]));
                            }
                        }

                        if (reader.HasRows) {
                            this.log(q,log_type.Info,"Rows Returned");
                            while (reader.Read()) {
                                row result=new row();
                                result.meta=results.columns;
                                for (int i = 0; i < reader.FieldCount; i++) {
                                    try{
                                        result.columns.Add(new column_data(reader[i]));
                                    }catch (Exception e){
                                        this.log(q,log_type.Error,e.ToString());
                                    }

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

        public override fk.fk_member get_fk_to_table(string connection_string,string database,string table,string schema){
            // Parameter: @database @table 
            // Columns    //  fk	//  table	//  schema	//  column	//  fk_table	//  fk_schema	//  fk_column	//  delete_action	//  update_ac
            string query=
                               @"SELECT	f.name									AS fk,
		                                OBJECT_NAME(f.parent_object_id)			AS [table],
		                                SCHEMA_NAME(f.schema_id)				AS [schema],
		                                COL_NAME(fc.parent_object_id, 
				                                    fc.parent_column_id)		AS [column],
		                                OBJECT_NAME (f.referenced_object_id)	AS [fk_table],
		                                SCHEMA_NAME(o.schema_id)				AS [fk_schema],
		                                COL_NAME(fc.referenced_object_id, 
				                                    fc.referenced_column_id)	AS [fk_column],
                                        delete_referential_action_desc          AS [delete_action],
                                        update_referential_action_desc          AS [update_action],
                                        DB_NAME()                               AS db
                                FROM sys.foreign_keys AS f
                                JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
                                JOIN	sys.objects				o  ON f.referenced_object_id = o.object_id
                                WHERE OBJECT_NAME (f.referenced_object_id) = @table
		                        AND     SCHEMA_NAME(o.schema_id)=@schema
                                AND     DB_NAME()=@database";
            
                parameters    p=new parameters();
                p.add("@database",database);
                p.add("@table",table);
                p.add("@schema",schema);
                query_params q = new query_params(connection_string, query, p, false, query_types.multiple);
                data_set res=sql_query(q);
                fk.fk_member obj=new fk.fk_member(res);
                return obj;
            }//end function

            public override fk.fk_member get_fk_from_table(string connection_string,string database,string table,string schema){
            // Parameter: @database @table 
            // Columns    //  fk	//  db   //  table	//  schema	//  column	//  fk_table	//  fk_schema	//  fk_column	//  delete_action	//  update_ac
                string query=
                        @"SELECT        f.name										AS fk,
                                        OBJECT_NAME(f.parent_object_id)             AS [table],
                                        SCHEMA_NAME(f.schema_id)                    AS [schema],
                                        COL_NAME(fc.parent_object_id,
                                                            fc.parent_column_id)    AS [column],
                                        OBJECT_NAME (f.referenced_object_id)		AS [fk_table],
                                        SCHEMA_NAME(o.schema_id)                    AS [fk_schema],
                                        COL_NAME(fc.referenced_object_id,
                                                fc.referenced_column_id)            AS [fk_column],
                                        delete_referential_action_desc              AS [delete_action],
                                        update_referential_action_desc              AS [update_action],
                                        DB_NAME()                                   AS db
                        FROM	sys.foreign_keys f
                        JOIN	sys.foreign_key_columns fc ON f.object_id = fc.constraint_object_id
                        JOIN    sys.objects             o  ON f.referenced_object_id = o.object_id
                        WHERE	OBJECT_NAME (f.parent_object_id) = @table
                        AND     SCHEMA_NAME(o.schema_id)=@schema
                        AND     DB_NAME()=@database";
   
                parameters    p=new parameters();
                p.add("@database",database);
                p.add("@table",table);
                p.add("@schema",schema);
                query_params q = new query_params(connection_string, query, p, true, query_types.multiple);
                data_set res=sql_query(q);
                 fk.fk_member obj=new fk.fk_member(res);
                return obj;
            }//end function      
    }
}

