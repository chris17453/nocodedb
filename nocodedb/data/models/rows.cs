using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace nocodedb.data.models{
   /* public class rows :IEnumerator,IEnumerable   {
        public  List<row> items =new List<row>();

        [JsonIgnore]
        int     position = -1;

        public rows(){
        }


        public row this[int key]        {
            get {
                return get_data(key);
            }
            set  {
                set_data(key, value);
            }
        }

        private row get_data(int index){
            if(0>=index && items.Count<=index) {
                return items[index];
            }
            return null;
        }

        private void set_data(int index, object value){
            if(0>=index && items.Count<=index) {
                items[index]=(row)value;
            }
        }

       
        public int  Count { 
            get {
                if(null==items) return 0;
                if(null!=items) return items.Count;
                return 0;
            }
        }

        public void Add(row item){
            items.Add(item);
            position++;
        }
        //IEnumerator and IEnumerable require these methods.
        public IEnumerator GetEnumerator() {
            return (IEnumerator)this;
        }

        //IEnumerator
        public bool MoveNext(){
            position++;
            return (position < items.Count);
        }

        //IEnumerable
        public void Reset() {
            position = 0;
        }

        //IEnumerable
        public object Current {
            get { 
                if(null==items || position<0) return null;
                return items[position];
            }
        }


    }*/
}
