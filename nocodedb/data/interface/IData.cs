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
using System.Data;
using System.Reflection;
using System.Web.Script.Serialization;

namespace nocodedb.data{
    [Serializable]
    public enum  database_type{
        MSSQL=1,MySQL=2
    }
    public class column_meta{
        public bool   AllowDBNull           { get; set; }
        public string BaseCatalogName       { get; set; }
        public string BaseColumnName        { get; set; }
        public string BaseSchemaName        { get; set; }
        public string BaseServerName        { get; set; }
        public string BaseTableName         { get; set; }
		public string ColumnName            { get; set; }
        public int    ColumnOrdinal         { get; set; }
        public int    ColumnSize            { get; set; }
        public string DataTypeName          { get; set; }
        public bool   IsAliased             { get; set; }
        public bool   IsAutoIncrement       { get; set; }
        public bool   IsColumnSet           { get; set; }
        public bool   IsExpression          { get; set; }
        public bool   IsHidden              { get; set; }
        public bool   IsIdentity            { get; set; }
        public bool   IsKey                 { get; set; }
        public bool   IsLong                { get; set; }
        public bool   IsReadOnly            { get; set; }
        public bool   IsRowVersion          { get; set; }
        public bool   IsUnique              { get; set; }
        public int    NonVersionedProviderType        { get; set; }
        public int    NumericPrecision                { get; set; }
        public int    NumericScale                    { get; set; }
        public string ProviderSpecificDataType        { get; set; }
        public string ProviderType                    { get; set; }
        public string UdtAssemblyQualifiedName        { get; set; }
        public string XmlSchemaCollectionDatabase     { get; set; } 
        public string XmlSchemaCollectionName         { get; set; }
        public string XmlSchemaCollectionOwningSchema { get; set; }

        public column_meta(){
        }
        public column_meta(DataColumnCollection columns,DataRow  row) {
            try{
                foreach (DataColumn col in columns){
                    PropertyInfo p=this.GetType().GetProperty(col.ColumnName);
                    if(null!=p) {
                        if(row!=null && row[col]!=DBNull.Value) {
                            p.SetValue(this, Convert.ChangeType(row[col], p.PropertyType),null);
                        } else {
                            p.SetValue(this, null,null);
                        }
                    }
                }
            }catch (Exception e){
                log.write("column_meta",log_type.Error,e.ToString());
            }
        }
    }
    public class column_data{
        public object value   { get; set; }
        [ScriptIgnore]
        public Type   type    { get { if(null!=value) return value.GetType(); return null; } }
        public column_data() {
        }
        public column_data(object column_value) {
                this.value = column_value;
        }
        public override string ToString() {
            return string.Format("{0}", value);
        }
    }
    public class column{
        public column_meta meta{ get; set; }
        public column_data data{ get; set; }
        public column(){
            data=new column_data();
            meta=new column_meta();
        }
    }

    public class row{
        public List<column_data>  columns { get; set; }
        public row(){
            columns=new List<column_data>();
        }
    }

    public class data_set{
        public List<column_meta> columns        { get; set; }              //column data  (name, type etc)
        public List<row>         rows           { get; set; }              //for multiples
        public row               row            { get; set; }              //for singles
        public int               affected_rows  { get; set; }              //for nonquery
        public column_data       scalar_results { get; set; }              //for nonquery
        public data_set(){
            row=new row();
            rows=new List<row>();
            columns=new List<column_meta>();
        }
    }

    public enum query_types {
        single,multiple,non,scalar,sp_single,sp_multiple
    };

    public class query_params:EventArgs{
        public string      connection_string { get; set; }
        public string      query             { get; set; }
        public Hashtable   parameters        { get; set; }
        public bool        meta              { get; set; }
        public query_types type              { get; set; }
        public string      message           { get; set; }
        public log_type    log_type          { get; set; }
        public string      function          { get; set; }

        public query_params(){
        }
        public query_params(string connection_string,string query,Hashtable parameters,bool meta,query_types type){
                this.connection_string =connection_string;
                this.query             =query;
                this.parameters        =parameters;
                this.meta              =meta;
                this.type              =type;
        }
    }

    public interface IData{
        event EventHandler LogEvent;  
        string        connection_string  (string host, string user,string password);
        bool          test_connect       (string connection_string);                                                                  //tests a connection
        int           execute_non_query  (string connection_string, string query, Hashtable parameters);                              //returns number of rows affected
        column_data   execute_scalar     (string connection_string, string query, Hashtable parameters);                              //returns first column of result
        data_set      fetch              (string connection_string, string query, Hashtable parameters,bool meta=false);              //returns a single row of data froma query.
        data_set      fetch_all          (string connection_string, string query, Hashtable parameters,bool meta=false);              //returns a list of rows from a query
        data_set      sp_fetch           (string connection_string, string query, Hashtable parameters,bool meta=false);              //returns a rows from a stored procedure 
        data_set      sp_fetch_all       (string connection_string, string query, Hashtable parameters,bool meta=false);              //returns a list of rows from a stored procedure 
	}
    public static class db{
        public static IData @new(database_type type){
            IData adapter =null;
                
            switch(type){
                case database_type.MSSQL    : adapter=new mssql_adapter();     break;
                case database_type.MySQL    : adapter=new mysql_adapter();     break;
            }
            return adapter;
        }
    }
}
