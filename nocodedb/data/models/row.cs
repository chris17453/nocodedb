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
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace nocodedb.data.models{
    
    public class row : Hashtable {
        private List<column_meta>  columns { get; set ; }
        public row(){
        }

        public row(List<column_meta> meta){

                this.columns=meta;
        }

        [JsonIgnore]
        public column_data this[string key]{
            get { 
                if(null==columns) return null;

                int ordinal=-1;

                foreach(column_meta c in columns) {
                    if(c.ColumnName==key) ordinal=c.ColumnOrdinal;
                }
                if(ordinal>=0) {
                    return new column_data(this[ordinal]);
                }
                return null;
            }
        }

        public bool ContainsKey(string key){
            foreach(column_meta c in columns) {
                if(c.ColumnName==key) return true;
            }
            return false;
        }

    }
}
