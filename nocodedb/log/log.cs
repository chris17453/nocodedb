using System;
using System.Collections;
namespace nocodedb {
    public enum log_type{
        Debug,Error,Info,Warning,System
        
    }
    public static class log {
        public static void write(string source,log_type type,string data) {
            //do something
        }
        public static void query(string source,string connection_string,string query,Hashtable parameters){
        }
    }
}
