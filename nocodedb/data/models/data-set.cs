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

namespace nocodedb.data.models{
    public class data_set: IEnumerator,IEnumerable   {
        public  List<column_meta> columns        { get; set; }              //column data  (name, type etc)
        public  List<row>         rows           { get; set; }              //for multiples
        private fk.members        _fk_from       { get; set; }              //foreign keys form this table
        public  fk.members        fk_from        { get{ return this._fk_from; } set { this._fk_from=value;  update_column_fk(); } }              //foreign keys form this table
        public  fk.members        fk_to          { get; set; }              //foreign keys TO this table
        public  int               affected_rows  { get; set; }              //for nonquery
        public  column_data       scalar_results { get; set; }              //for nonquery
        int     position = -1;

        public void update_column_fk(){
            if(null==fk_from ) return;
            foreach(fk.member fk in fk_from) {
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

        public row this[int key]        {
            get {
                return get_data(key);
            }
            set  {
                set_data(key, value);
            }
        }

        public string[] Keys { 
            get {
                if(null==rows || rows.Count==0) return null;
                string [] _Keys=new string[columns.Count];
                for(int i=0;i<columns.Count;i++) _Keys[i]=columns[i].ColumnName;
                return _Keys;
            } 
        }

        private row get_data(int index){
            if(0>=index && columns.Count<=index) {
                return rows[index];
            }
            return null;
        }


        private void set_data(int index, object value){
            if(0>=index && columns.Count<=index) {
                rows[index]=(row)value;
            }
        }


        public column_data this[string key]        {
            get {
                return get_data(0)[key];
            }
            set  {
                rows[0][key]=(column_data)value;
            }
        }


        public int  Count          { 
            get {
                if(null==rows) return 0;
                if(null!=rows) return rows.Count;
                return 0;
            }
        }

        public data_set(){
            rows=new List<row>();
            columns=new List<column_meta>();
        }

        //IEnumerator and IEnumerable require these methods.
        public IEnumerator GetEnumerator() {
            return (IEnumerator)this;
        }

        //IEnumerator
        public bool MoveNext(){
            position++;
            return (position < rows.Count);
        }

        //IEnumerable
        public void Reset() {
            position = 0;
        }

        //IEnumerable
        public object Current
        {
            get { 
                if(null==rows || position<0) return null;
                return rows[position];
                }
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