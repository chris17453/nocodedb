using System;
using System.Collections;
using System.Collections.Generic;

namespace nocodedb.data.models{
    public class data_set: IEnumerator,IEnumerable   {
        public List<column_meta> columns        { get; set; }              //column data  (name, type etc)
        public List<row>         rows           { get; set; }              //for multiples
        public int               affected_rows  { get; set; }              //for nonquery
        public column_data       scalar_results { get; set; }              //for nonquery
        int position = -1;


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


        public int  Count          { get {
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
            get { return rows[position];}
        }

    }
}
