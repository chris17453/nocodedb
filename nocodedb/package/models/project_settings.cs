using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace nocodedb.models {
    public class project_settings: nocodedb.models.JsonFile<project_settings>,INotifyPropertyChanged{
        public string name { get; set; }
        public db_settings src {get; set; }
        //public db_settings dst {get; set; }
        
        public BindingList<database> data_set { get; set; }
        internal string  current_database { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = ""){
            if (PropertyChanged != null){
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public project_settings() {
            name="";
            src=new db_settings();
            //dst=new db_settings();
            data_set=new BindingList<database>();
            current_database="";
        }

        
        public column get_column_fk_parent(column c) {
            if(null==c.fk) return null;
            database FK_D=data_set.SingleOrDefault(x=>x.name==c.fk.db);
            if(FK_D==null) return null;                                                     //no DB?
            table FK_T=FK_D.tables.SingleOrDefault(x=>x.name==c.fk.table);
            if(FK_T==null) return null;                                                     //no Table
            column FK_C=FK_T.columns.SingleOrDefault(x=>x.name==c.fk.column);
            if(FK_C==null) return null;                                                     //no column?
            return FK_C;                                                                    //return the destination Column
        }
        public table get_table_fk_parent(column c) {
            if(null==c.fk) return null;
            database FK_D=data_set.SingleOrDefault(x=>x.name==c.fk.db);
            if(FK_D==null) return null;                                                     //no DB?
            table FK_T=FK_D.tables.SingleOrDefault(x=>x.name==c.fk.table);
            if(FK_T==null) return null;                                                     //no Table
            return FK_T;                                                                    //return the destination Column
        }
        public database get_database_fk_parent(column c) {
            if(null==c.fk) return null;
            database FK_D=data_set.SingleOrDefault(x=>x.name==c.fk.db);
            if(FK_D==null) return null;                                                     //no DB?
            return FK_D;                                                                    //return the destination Column
        }
        public database get_db(string db) {
            database FK_D=data_set.SingleOrDefault(x=>x.name==db);
            if(FK_D==null) return null;                                                     //no DB?
            return FK_D;
        }
        public table get_table(string db,string table) {
            database t_db=get_db(db);
            if(t_db==null) return null;
            table t_table=t_db.tables.SingleOrDefault(x=>x.name==table);
            if(null==t_table) return null;
            return t_table;
        }

        /*public List<table> get_table_fk_children(c) {
            database FK_D=data_set.SingleOrDefault(x=>x.name==c.fk.db);
            if(FK_D==null) return null;                                                     //no DB?
            table FK_T=FK_D.tables.SingleOrDefault(x=>x.name==c.fk.table);
            if(FK_T==null) return null;                                                     //no Table
            return FK_T;                                                                    //return the destination Column
        }*/

    }
}
