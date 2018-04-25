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
using System.Collections.Generic;

namespace nocodedb.data.models{
    public class row{
        public List<column_data>  columns { get; set; }
        public List<column_meta>  meta    { get; set; }

        public column_data this[string key]        {
            get {
                return get_data(key);
            }
            set  {
                set_data(key, value);
            }
        }
        public column_data this[int key]        {
            get {
                return get_data(key);
            }
            set  {
                set_data(key, value);
            }
        }

        private column_data get_data(string key){
            if(null!=meta) {
                foreach(column_meta c  in meta) {
                    if(c.ColumnName==key) {
                        if(0<=c.ColumnOrdinal && columns.Count>c.ColumnOrdinal) {
                            return columns[c.ColumnOrdinal];
                        }
                    }
                }
            }
            return null;
        }

        private column_data get_data(int index){
            if(0<=index && columns.Count>index) {
                return columns[index];
            }
            return null;
        }


        public bool ContainsKey(string key) { 
            if(null!=meta) {
                foreach(column_meta c  in meta) {
                    if(c.ColumnName==key) {
                        return true;
                    }
                }
            }
            return false;
        }

        private void set_data(string key, column_data value){
            if(null!=meta) {
                foreach(column_meta c  in meta) {
                    if(c.ColumnName==key) {
                        if(0<=c.ColumnOrdinal && columns.Count>c.ColumnOrdinal) {
                            columns[c.ColumnOrdinal]=value;
                        }
                    }
                }
            }
        }

        private void set_data(int index, column_data value){
            if(0<=index && columns.Count>index) {
                columns[index]=value;
            }
        }

    

        public row(){
            columns=new List<column_data>();
        }
    }
}
