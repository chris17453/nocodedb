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
            if(string.IsNullOrWhiteSpace(name)) return "FAILED";
            Regex rgx = new Regex("[^a-zA-Z0-9-]+");
            string str = rgx.Replace(name, "_");
            return str;
        }

        public static string map_database(string connection_target,string database){
            StringBuilder tables=new StringBuilder();
            data_set data=db.fetch_all(connection_target,string.Format("SELECT top 10 TABLE_SCHEMA,TABLE_NAME FROM {0}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA='dbo' ORDER BY TABLE_NAME,TABLE_SCHEMA",database));
            tables.AppendLine("using System;");
            tables.AppendLine("using System.Data;");

            foreach(row r in  data.rows){
                string source_file= map_table(connection_target,database,r[0].ToString(),r[1].ToString());
                tables.AppendLine(source_file);
     //           Console.ReadKey();
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

            string table_name=safe_name(table);
                
            o.AppendLine(string.Format("namespace ncdb.schema.{0}.{1}.{2} {{",database,schema,table_name));

            o.AppendLine(string.Format("\t public static class col {{"));
            foreach(column_meta c in columns) {
                string column_name=safe_name(c.ColumnName);
                o.AppendLine(string.Format("\t public static class {0} {{",column_name));
                o.AppendLine(string.Format("\t\t public static class prop {{"));



                Type t=typeof(column_meta);
                foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) { 
                    string property_name  = pi.Name;
                    object property_value = pi.GetValue(c, null);

                    TypeCode tc=System.Type.GetTypeCode(pi.PropertyType);
                    //Type targetType= ToType(tc);
                    string resolved_type_name=pi.PropertyType.Name;
                    if(pi.PropertyType.Name=="Nullable`1") {
                            Type answer = Nullable.GetUnderlyingType(pi.PropertyType);
                            resolved_type_name=answer.Name;
                            //tc=System.Type.GetTypeCode(pi.PropertyType);
                     }

                    object resolved_value;
                    if(null==property_value)    resolved_value="System.null";
                    else                        resolved_value=property_value.ToString();

                    switch(resolved_type_name) {
                        case "Boolean"   : if(property_value.ToString()=="True") o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}=true;" ,resolved_type_name,pi.Name)); 
                        else                                                     o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}=false;",resolved_type_name,pi.Name));  break;
                        case "Byte"      : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Char"      : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}='{2}';"                                      ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "DateTime"  : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;;
                        case "DBNull"    : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Decimal"   : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Double"    : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Empty"     : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Int16"     : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Int32"     : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;;
                        case "Int64"     : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Object"    : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "SByte"     : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Single"    : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "String"    : if(null==property_value) {
                                            o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}=String.Empty;"                              ,resolved_type_name,pi.Name,resolved_value)); break;
                                           } else {
                                            o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}=\"{2}\";"                                   ,resolved_type_name,pi.Name,resolved_value)); break;
                                           }
                        case "UInt16"    : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "UInt32"    : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "UInt64"    : o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                        ,resolved_type_name,pi.Name,resolved_value)); break;
                        case "Type"      : if(null==property_value){
                                            o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1}={2};"                                       ,resolved_type_name,pi.Name,resolved_value)); break;
                                           } else {
                                            o.AppendLine(string.Format("\t\t\tpublic static  {0} \t{1} {{ get {{ return typeof({2}); }} }}"        ,resolved_type_name,pi.Name,resolved_value));
                                           }
                                           break;
                        default : o.AppendLine(string.Format("//\t\t\tpublic static  {0} \t{1};"        ,resolved_type_name,pi.Name,"System.Data.SqlTypes."+property_value)); 

                            break;     
                    }

                }
                o.AppendLine("\t\t}//end properties class");
                o.AppendLine("\t}//end column class");
            }
            o.AppendLine("\t}//end colums wrapper ");
            o.AppendLine("}//end namespace");
            return o.ToString();
        }



        public static string map_table(string connection_target,string database,string schema,string table){
            data_set data=db.fetch_all(connection_target,string.Format("SELECT TOP 1 * FROM [{0}].[{1}].[{2}]",database,schema,table),null,true);
            StringBuilder o=new StringBuilder();
            database=safe_name(database);
            schema  =safe_name(schema);
            table   =safe_name(table);
           
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

            o.AppendLine("\t\t//Public Variables");

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

        public static bool compile_dll (string filename,string code,bool save_source){
            Console.WriteLine("Compiling");
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = filename+".dll";
            parameters.ReferencedAssemblies.Add("data.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
          //  parameters.ReferencedAssemblies.Add("Microsoft.SqlServer.Types.dll");


            if(String.IsNullOrWhiteSpace(code)) {
                Console.WriteLine("No code.");
                return false;
            }
            if(String.IsNullOrWhiteSpace(filename)) {
                Console.WriteLine("No DLL name.");
                return false;
            }

            if(save_source) {
                write_source(filename,code);
            }

            CompilerResults cr = codeProvider.CompileAssemblyFromSource(parameters, code);

        
            if( cr.Errors.Count > 0 ) {
                for( int i=0; i<cr.Output.Count; i++ )  Console.WriteLine( cr.Output[i] );
                for( int i=0; i<cr.Errors.Count; i++ )  Console.WriteLine( i.ToString() + ": " + cr.Errors[i].ToString() );
                return false;
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
            Console.WriteLine("Compiling Finished");
            return true;
        }

        public static void write_source(string file_path,string source){ 
            string filename=file_path+".cs";
            Console.WriteLine("Source file Written : "+filename);
            try{
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename)) {
                    file.Write(source);
                }
            }catch(Exception ex) {
                Console.WriteLine("Error Writing source file :"+ex.Message);
             
            }
        }//end write_source

    }
}
