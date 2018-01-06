using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace nocodedb.models {
    public class database : INotifyPropertyChanged {
        public string               name { get; set; }
        public bool                 data_set { get {   if(null!=tables.SingleOrDefault(x=>x.data_set)) return false;  return true;}  }
        public db_settings          connection {get; set; }
        public BindingList<table>   tables {get; set;} 
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = ""){
            if (PropertyChanged != null){
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
  
        public database() {
            name="";
            connection=new db_settings();
            tables=new BindingList<table>();
            
        }

        public database(db_settings settings,string name) {
            this.name=name;
            connection=settings;
            tables=new BindingList<table>();
        }

        public void load_tables() {
            /*List<Hashtable> results=db.fetchAll(connection.ToString(),@"USE "+name+@"; SELECT TABLE_NAME as 'name'
                                                                    FROM INFORMATION_SCHEMA.TABLES
                                                                    WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG=@db_name ORDER BY TABLE_NAME",new string[,] { {"db_name" , name} });
            if(null==tables) tables=new BindingList<table>();

            foreach(Hashtable table in results) {
                string table_name=(string)table["name"];
                if(null==tables.SingleOrDefault(x=>x.name==table_name)) {                    //if the table is not found in this list, load it.
                    tables.Add(new models.table(connection,name,table_name));
                }
            }*/
        }

    }
}
