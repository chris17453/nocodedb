using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using nocodedb.data;

namespace nocodedb.models {
  public class db_settings : INotifyPropertyChanged {
        public   string        name     { get; set; } 
        public   string        host     { get; set; } 
        public   string        user     { get; set; } 
        public   string        password { get; set; }
        public   database_type type     { get; set; }
        internal IData         db       { get; set; }                             //interface for all DB stuff.

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = ""){
            if (PropertyChanged != null){
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
  
        public db_settings() {
            
        }

        public db_settings(string name,string host,string user,string password,database_type type) {
            this.name=name;
            this.host=host;
            this.user=user;
            this.password=password;
            this.type=type;

        }

        public bool validate(){
            if(
                String.IsNullOrWhiteSpace(name) ||
                String.IsNullOrWhiteSpace(host) ||
                String.IsNullOrWhiteSpace(user) ||
                String.IsNullOrWhiteSpace(password)) 
            {
                return false;
            }
            switch(type) {
                case database_type.MSSQL:  mssql_adapter ms=new mssql_adapter(); if(ms.test_connect(ms.connection_string(host,user,password))) return true; break;
            }
            return false;
        }
        /*public override string ToString() {
            string connection_string=String.Format("Data source={0}; User ID={1}; Password={2};",host,user,password);;
            return connection_string;
        }*/
    }
}
