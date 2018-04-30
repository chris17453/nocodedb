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
    [JsonObject(MemberSerialization.OptIn)]
    public class row : IList,IEnumerator,IEnumerable,ISerializable{
        private  List<column_data>  columns { get; set; }
        private  List<column_meta>  meta    { get; set; }
        public Hashtable bob;
        private int position =-1;

        /*public column_data[] this        {
        }*/

            
        /*public object this[object key] { 
            get  {
                return 
            }
            set  {
            }
        }*/
        public column_data this[string key]        {
            get {
                return get_data(key);
            }
            set  {
                set_data(key, value);
            }
        }
    

        [JsonProperty]
        public column_data this[int key]        {
            get {
                return get_data(key);
            }
            set  {
                set_data(key, value);
            }
        }


        private column_data get_data(string key){
            if(null!=meta && !String.IsNullOrWhiteSpace(key)) {
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
            if(null!=meta && !String.IsNullOrWhiteSpace(key)) {
                foreach(column_meta c  in meta) {
                    if(c.ColumnName==key) {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool set_data(string key, column_data value){
            if(null!=meta && !String.IsNullOrWhiteSpace(key) && null!=value) {
                foreach(column_meta c  in meta) {
                    if(c.ColumnName==key) {
                        if(0<=c.ColumnOrdinal && columns.Count>c.ColumnOrdinal) {
                            columns[c.ColumnOrdinal]=value;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void set_data(int index, column_data value){
            if(0<=index && null!=columns && columns.Count>index && null!=value) {
                columns[index]=value;
            }
        }

        public void Add(column_data data) {
            columns.Add(data);
        }
        public void AddMeta(List<column_meta> data) {
            meta=data;
        }


        public row(){
            columns=new List<column_data>();
        }


        //IEnumerator and IEnumerable require these methods.
        public IEnumerator GetEnumerator() {
            return (IEnumerator)this;
        }

        //IEnumerator
        public bool MoveNext(){
            position++;
            return (position < columns.Count && position>=0);
        }

        //IEnumerable
        public void Reset() {
            position = 0;
        }

        //IEnumerable
        public object Current
        {
            get
            {
                if (null == columns || position < 0 || position >= columns.Count) return null;
                return columns[position];
            }
        }

        public bool IsFixedSize => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        object IList.this[int index] { 
            get  { 
                if(null!= columns && columns.Count>index) {
                return columns[index];
                } 
                return null;
            }
            set {
                
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)        {
            int index=0;
            foreach(column_data c in columns) {
                info.AddValue(index.ToString(),(string)c);
                index++;
            }
        }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
