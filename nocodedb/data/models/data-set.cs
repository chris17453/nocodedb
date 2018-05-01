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
using Newtonsoft.Json;

namespace nocodedb.data.models{

    //json.net igores everything on a class that extends ienumerable... a custom class is required for conversion
    public class data_set:List<row>{
        [JsonProperty]
        public  List<column_meta>   columns        { get; set; }              //column data  (name, type etc)

        private fk.fk_members       _fk_from       { get; set; }              //foreign keys form this table
        [JsonIgnore]
        public  fk.fk_members       fk_from        { get{ return this._fk_from; } set { this._fk_from=value;  update_column_fk(); } }              //foreign keys form this table
        [JsonIgnore]
        public  fk.fk_members       fk_to          { get; set; }              //foreign keys TO this table
        [JsonIgnore]
        public  int                 affected_rows  { get; set; }              //for nonquery
        [JsonIgnore]
        public  column_data         scalar_results { get; set; }              //for nonquery
  //      int     position = -1;

        public void update_column_fk(){
            if(null==fk_from ) return;
            foreach(fk.fk_member fk in fk_from) {
                column_meta results=columns.Find(x=>
                x.BaseCatalogName==fk.db    && 
                x.BaseTableName==fk.table   && 
                x.BaseSchemaName==fk.schema && 
                x.BaseColumnName==fk.column);
                if(null!=results){
                    results.IsForeignKey     =true;
                    results.ForeignKey_name  =fk.fk;
                    results.ForeignKey_table =fk.fk_table;
                    results.ForeignKey_schema=fk.fk_schema;
                    results.ForeignKey_column=fk.fk_column;
                }
            }
        }


        [JsonIgnore]
        public string[] Keys { 
            get {
                if(null==columns|| columns.Count==0) return null;
                string [] _Keys=new string[columns.Count];
                for(int i=0;i<columns.Count;i++) _Keys[i]=columns[i].ColumnName;
                return _Keys;
            } 
        }

        public bool ContainsKey(string key){
            foreach(column_meta c in columns) {
                if(c.ColumnName==key) return true;
            }
            return false;
        }

        [JsonIgnore]
        public column_data this[int index,string key]{
            get { 
                if(null==columns) return null;

                if(index<=this.Count && index>=0 ) {
                    return this[index][key];
                }
                return null;
            }
        }

        [JsonIgnore]
        public column_data this[int index,int index2]{
            get { 
                if(index<=this.Count && index>=0 ) {
                    if(index2<=this[index].Count && index2>=0 ) {
                        return new column_data(this[index][index2]);
                    }
                }
                return null;
            }
        }

        public data_set(){
            columns=new List<column_meta>();
        }
    }
}
/*
D  = DEFAULT (constraint or stand-alone)
F = FOREIGN KEY constraint
PK = PRIMARY KEY constraint
UQ = UNIQUE constraint
*/
/*
AF = Aggregate function (CLR)
C = CHECK constraint
D = DEFAULT (constraint or stand-alone)
F = FOREIGN KEY constraint
FN = SQL scalar function
FS = Assembly (CLR) scalar-function
FT = Assembly (CLR) table-valued function
IF = SQL inline table-valued function
IT = Internal table
P = SQL Stored Procedure
PC = Assembly (CLR) stored-procedure
PG = Plan guide
PK = PRIMARY KEY constraint
R = Rule (old-style, stand-alone)
RF = Replication-filter-procedure
S = System base table
SN = Synonym
SO = Sequence object
U = Table (user-defined)
V = View
SQ = Service queue
TA = Assembly (CLR) DML trigger
TF = SQL table-valued-function
TR = SQL DML trigger
TT = Table type
UQ = UNIQUE constraint
X = Extended stored procedure
ET = External Table 
     
     */