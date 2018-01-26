using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace monarch{
    public class generic {
        public List<Hashtable> rows=new List<Hashtable>();
        public string table;
        public string base_table;
        public string database;
        private string [,] param;
        private string  select;
        private string  delete;
        private bool    identity=false;
        private string  identity_column  ="";
        public generic() {
        }

        public generic(string connection_string,string table,string query,string delete) {
            this.delete=delete;
            this.table=table;
            rows=titan.db.fetchAll(connection_string,query);
            get_identity(connection_string);
        }
        public generic(string connection_string,string table,string query,string delete,string[,] param) {
            this.delete=delete;
            this.table=table;
            this.param=param;
            rows =titan.db.fetchAll(connection_string,query,param);
            get_identity(connection_string);
        }

        public void get_identity(string connection_string) {


            string synonym_query=@"SELECT [name], base_object_name as 'base',DB_NAME(DB_ID(PARSENAME(base_object_name,3))) as db  FROM sys.synonyms  s WHERE [name]=@name
                                UNION 
                                SELECT [name],[name] as 'base',db_name() as db FROM sys.tables WHERE [name]=@name";

            Hashtable results=titan.db.fetch(connection_string,synonym_query,new string[,]{ { "name",table} });
            
            if (null!=results && results.Count>0) {
                database=(string)results["db"];
                base_table=(string)results["base"];
            }

            
            string identity_query=@"USE "+database+@"; SELECT TOP 1 COLUMN_NAME, TABLE_NAME
            FROM "+database+@".INFORMATION_SCHEMA.COLUMNS
            WHERE COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 and table_name=@name";
            results=titan.db.fetch(connection_string,identity_query,new string[,]{ { "name",base_table} });
            if(results!=null && results.Count>0) {
                identity=true;
                identity_column=(string)results["TABLE_NAME"];

            }

        }

        public string generate_identity_insert() {
            StringBuilder o=new StringBuilder();
            o.AppendLine(string.Format("--BEGIN {0}.dbo.{1}-->{2}",database,base_table,table ));
            o.AppendLine("--DISABLE CONSTRAINTS FOR THIS TABLE");
            o.AppendLine(string.Format("ALTER TABLE [{0}].[dbo].[{1}] NOCHECK CONSTRAINT ALL",database,base_table,table ));

            o.AppendLine("--DELETE Old Data");
            o.AppendLine(delete);
                
            if(identity && rows.Count>0) o.AppendLine(string.Format("SET IDENTITY_INSERT [{0}].[dbo].["+table+"] ON;",database,base_table,table ));
            foreach(Hashtable row in rows) {
                string columns="";
                string values="";
                foreach (DictionaryEntry pair in row) {
                    if(columns!="") {  columns+=","; values+=","; }
                    columns+="["+pair.Key+"]";
                    values+="'"+pair.Value.ToString()+"'";
                }
                o.AppendLine(String.Format("INSERT INTO {0} ({1}) VALUES ({2})",table,columns,values));
            }
            if(identity && rows.Count>0) o.AppendLine(string.Format("SET IDENTITY_INSERT ["+table+"] OFF;  ",database,base_table,table ));
            o.AppendLine("--ENABLE CONSTRAINTS FOR THIS TABLE");
            o.AppendLine(string.Format("ALTER TABLE ["+table+"] CHECK CONSTRAINT ALL",database,base_table,table ));
            o.AppendLine("--END "+table);
            o.AppendLine("");
            return o.ToString();
        }
    }
}
