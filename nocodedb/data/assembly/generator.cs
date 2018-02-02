using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using nocodedb.data.models;



namespace nocodedb.data.assembly
{
    public class generator
    {
        public generator(){
            
            
        }

        public static string safe_name(string name){
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            string str = rgx.Replace(name, "_");
            return str;
        }

        public static string map_database(string connection_target,string database){
            StringBuilder tables=new StringBuilder();
            data_set data=db.fetch_all(connection_target,string.Format("SELECT TABLE_SCHEMA,TABLE_NAME FROM {0}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'",database));

            foreach(row r in  data.rows){
                tables.AppendLine(map_table(connection_target,database,r[1].ToString(),r[0].value.ToString()));
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
            o.AppendLine(string.Format("namespace ncdb.schema.{0}.{1}.{2} {{",database,schema,table));

            foreach(column_meta c in columns) {
                string column_name=safe_name(c.ColumnName);
                o.AppendLine(string.Format("\t public static class {0} {{",column_name));



                Type t=typeof(column_meta);
                foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) { 
                    string property_name  = pi.Name;
                    object property_value = pi.GetValue(c, null);

                    TypeCode tc=System.Type.GetTypeCode(pi.PropertyType);
                    Type targetType= ToType(tc);

                    if(property_value==null) {
                        switch(tc) {   
                            case TypeCode.Boolean  : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Byte     : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Char     : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.DateTime : o.AppendLine(string.Format("\t\t public const {0} \t{1}=null;",pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.DBNull   : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Decimal  : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Double   : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Empty    : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Int16    : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Int32    : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Int64    : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Object   : o.AppendLine(string.Format("\t\t public const {0} \t{1}=null;",pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.SByte    : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.Single   : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.String   : o.AppendLine(string.Format("\t\t public const {0} \t{1}=null;",pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.UInt16   : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.UInt32   : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            case TypeCode.UInt64   : o.AppendLine(string.Format("\t\t public const {0} \t{1};"     ,pi.PropertyType.Name,pi.Name)); break;
                            default : break;
                        }
                    } else {
                        switch(pi.PropertyType.Name) {
                            case "Boolean"  : if(property_value.ToString()=="True") o.AppendLine(string.Format("\t\tpublic const  {0} \t{1}=true;",pi.PropertyType.Name,pi.Name)); 
                            else                                                    o.AppendLine(string.Format("\t\tpublic const  {0} \t{1}=false;",pi.PropertyType.Name,pi.Name));  break;
                            case "Byte"     : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Char"     : o.AppendLine(string.Format("\t\t public const  {0} \t{1}='{2}';",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "DateTime" : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;;
                            case "DBNull"   : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Decimal"  : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Double"   : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Empty"    : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Int16"    : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Int32"    : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;;
                            case "Int64"    : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Object"   : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "SByte"    : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Single"   : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "String"   : o.AppendLine(string.Format("\t\t public const  {0} \t{1}=\"{2}\";",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "UInt16"   : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "UInt32"   : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "UInt64"   : o.AppendLine(string.Format("\t\t public const  {0} \t{1}={2};",pi.PropertyType.Name,pi.Name,property_value.ToString())); break;
                            case "Type"     : o.AppendLine(string.Format("\t\t public static {0} \t{1} {{ get {{ return typeof({2}); }} }}",pi.PropertyType.Name,pi.Name,((Type)property_value).Name)); break;
                            default : break;
                        }
                    }

                }
                o.AppendLine("\t}//end column class");
            }
            o.AppendLine("}//end namespace");
            return o.ToString();
        }



        public static string map_table(string connection_target,string database,string schema,string table){
            data_set data=db.fetch_all(connection_target,"SELECT TOP 1 * FROM "+table,null,true);
            StringBuilder o=new StringBuilder();
            database=safe_name(database);
            schema  =safe_name(schema);
            table   =safe_name(table);
            o.AppendLine("using System;");

            o.AppendLine(map_columns(data.columns,database,schema,table));                              //the column meta classes

            o.AppendLine(string.Format("namespace ncdb.{0}.{1} {{",database,schema));
            o.AppendLine(string.Format("\t public class {0} :nocodedb.data.crud <{0}> {{",table));
            o.AppendLine("\t\t//Internal variables");


            int index=0;
            foreach(column_meta c in data.columns) {
                string internal_name="_0000"+index.ToString();
                o.AppendLine(string.Format("\t\t private {0} __{1};",
                                           c.DataType,
                                           internal_name));
                index++;
            }

            o.AppendLine("\t\t//Internal variables");

            index=0;
            string column_namespace=string.Format("ncdb.{0}.{1}.{2}",database,schema,table);
            foreach(column_meta c in data.columns) {
                string internal_name="___0000"+index.ToString();
                string private_name=safe_name(c.ColumnName);

                o.AppendLine(string.Format("\t\t public {0} {1} {{ get {{ return {2}; }} set {{  {2}=value; }}  }}", //if({3}.{1}.validate(value)) {{ 
                                           c.DataType,
                                           private_name,
                                           internal_name,
                                           column_namespace));
                index++;
            }

            o.AppendLine(String.Format("\t\tpublic {0}() {{",table));
            o.AppendLine("\t\t}");
            o.AppendLine("\t} //end table class");
            o.AppendLine("}//end namespace");

            return o.ToString();
        }
        public static void compile_dll (string filename,string code){
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = filename+".dll";
            parameters.ReferencedAssemblies.Add("data.dll");

            CompilerResults cr = codeProvider.CompileAssemblyFromSource(parameters, code);

        
            if( cr.Errors.Count > 0 ) {
                for( int i=0; i<cr.Output.Count; i++ )  Console.WriteLine( cr.Output[i] );
                for( int i=0; i<cr.Errors.Count; i++ )  Console.WriteLine( i.ToString() + ": " + cr.Errors[i].ToString() );

            } else {
                // Display information about the compiler's exit code and the generated assembly.
                Console.WriteLine( "Compiler returned with result code: " + cr.NativeCompilerReturnValue.ToString() );
                Console.WriteLine( "Generated assembly name: " + cr.CompiledAssembly.FullName );
                if( cr.PathToAssembly == null )
                    Console.WriteLine( "The assembly has been generated in memory." );
                else
                    Console.WriteLine( "Path to assembly: " + cr.PathToAssembly );

                // Display temporary files information.
                if( !cr.TempFiles.KeepFiles ) Console.WriteLine( "Temporary build files were deleted." );
                else {
                    Console.WriteLine( "Temporary build files were not deleted." );
                    // Display a list of the temporary build files
                    IEnumerator enu = cr.TempFiles.GetEnumerator();                                        
                    for( int i=0; enu.MoveNext(); i++ )                                          
                        Console.WriteLine( "TempFile " + i.ToString() + ": " + (string)enu.Current );                  
                }
            }        
        
        
        }

    }
}
