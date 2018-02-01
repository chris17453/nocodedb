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
            } else {                                                                                            //its a specific connection
                _connection_string = ConfigurationManager.ConnectionStrings["connection_target"].ConnectionString;
                _provider_name     = ConfigurationManager.ConnectionStrings["connection_target"].ProviderName;
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


        public static string safe_name(string name){
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            string str = rgx.Replace(name, "_");
            return str;
        }

        public static string map_database(string connection_target,string database){
            StringBuilder tables=new StringBuilder();
            data_set data=fetch_all(connection_target,string.Format("SELECT TABLE_SCHEMA,TABLE_NAME FROM {0}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'",database));

            foreach(row r in  data.rows){
                tables.Append(map_table(connection_target,database,r[1].ToString(),r[0].value.ToString()));
            }

            return tables.ToString();
        }

        public static Type ToType(TypeCode code) {
            switch (code) {
                case TypeCode.Boolean: return typeof(bool);
                case TypeCode.Byte: return typeof(byte);
                case TypeCode.Char: return typeof(char);
                case TypeCode.DateTime: return typeof(DateTime);
                case TypeCode.DBNull: return typeof(DBNull);
                case TypeCode.Decimal: return typeof(decimal);
                case TypeCode.Double: return typeof(double);
                case TypeCode.Empty: return null;
                case TypeCode.Int16: return typeof(short);
                case TypeCode.Int32: return typeof(int);
                case TypeCode.Int64: return typeof(long);
                case TypeCode.Object: return typeof(object);
                case TypeCode.SByte: return typeof(sbyte);
                case TypeCode.Single: return typeof(Single);
                case TypeCode.String: return typeof(string);
                case TypeCode.UInt16: return typeof(UInt16);
                case TypeCode.UInt32: return typeof(UInt32);
                case TypeCode.UInt64: return typeof(UInt64);
            }
            return null;
        }

        public static string map_columns(List<column_meta> columns,string database,string schema,string table){
            StringBuilder o=new StringBuilder();
            o.Append(string.Format("namespace ncdb.{0}.{1}.{2} {{",database,schema,table));

            foreach(column_meta c in columns) {
                string column_name=safe_name(c.ColumnName);
                o.Append(string.Format("\t public static class {0} {{",column_name));



                Type t=typeof(column_meta);
                foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) { 
                    string property_name  = pi.Name;
                    object property_value = pi.GetValue(c, null);

                    TypeCode tc=System.Type.GetTypeCode(pi.PropertyType);
                    Type targetType= ToType(tc);
                     switch(tc) {
                        case TypeCode.Boolean  : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Byte     : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Char     : o.Append(string.Format("\t\t const {0} \t{1}='{2}';",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.DateTime : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;;
                        case TypeCode.DBNull   : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Decimal  : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Double   : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Empty    : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Int16    : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Int32    : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;;
                        case TypeCode.Int64    : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Object   : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.SByte    : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.Single   : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.String   : o.Append(string.Format("\t\t const {0} \t{1}=\"{2}\";",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.UInt16   : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.UInt32   : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        case TypeCode.UInt64   : o.Append(string.Format("\t\t const {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                        default : break;
                    }

                }





                o.Append("\t}//end column class");
            }
            o.Append("}//end namespace");

            return o.ToString();
        }


        public static string map_table(string connection_target,string database,string schema,string table){
            data_set data=fetch_all(connection_target,"SELECT TOP 1 * FROM "+table);
            StringBuilder o=new StringBuilder();
            database=safe_name(database);
            schema  =safe_name(schema);
            table   =safe_name(table);

            o.Append(map_columns(data.columns,database,schema,table));                              //the column meta classes

            o.Append(string.Format("namespace ncdb.{0}.{1} {{",database,schema));
            o.Append(string.Format("\t public class {0} :nocodedb.data.crud <{0}> {{",table));
            o.Append("\t\t//Internal variables");


            int index=0;
            foreach(column_meta c in data.columns) {
                string internal_name="_0000"+index.ToString();
                o.Append(string.Format("\t\t private {0} __{1};",
                                       c.DataTypeName,
                                       internal_name));
            }

            o.Append("\t\t//Internal variables");

            index=0;
            string column_namespace=string.Format("ncdb.{0}.{1}.{2} {{",database,schema,table);
            foreach(column_meta c in data.columns) {
                string internal_name="_0000"+index.ToString();
                string private_name=safe_name(c.ColumnName);

                o.Append(string.Format("\t\t public {0} {1} {{ get {{ return {2}.value; }} set {{  if({3}.{1}.validate(value)) {{ {2}.value=value; }}  }}",
                                       c.DataTypeName,
                                       private_name,
                                       internal_name,
                                       column_namespace));
            }

            o.Append(String.Format("\t\tpublic {0}() {{",table));
            o.Append("\t\t}");
            o.Append("\t} //end table class");
            o.Append("}//end namespace");

            return o.ToString();
        }
        public static void compile_dll (string filename,string code){
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = filename+".dll";
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);
        }

    }
}
