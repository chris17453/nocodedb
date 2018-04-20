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
using System.Text;

namespace nocodedb.data.models{
    public class parameter {
        public string key  { get; set; }
        public object value { get; set; }
        public parameter(){
        }
        public parameter(string key,object value){
            this.key=key;
            this.value=value;
        }
        public override string ToString()
        {
            if(null==key) return "INVALID PARAMETER";
            return String.Format("{0}:{1}",key,value);
        }


    }
    public class parameters : IEnumerator,IEnumerable   {
        private List<parameter> items=new List<parameter>();
        int position = -1;


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
        public object Current
        {
            get { 
                if(null==items) return null; 
                if(position<items.Count) return items[position];
                return null;
            }
        }

        public string[] Keys { 
            get {
                if(null==items || items.Count==0) return null;
                string [] _Keys=new string[items.Count];
                for(int i=0;i<items.Count;i++) _Keys[i]=items[i].key;
                return _Keys;
            } 
        }

        public object this[string key] {
            get {
                return get_data(key);
            }
            set  {
                set_data(key, value);
            }
        }

        private object get_data(string key){
            int index=items.FindIndex((parameter obj) => obj.key==key);
            if(index>=0) return items[index];
            return null;
        }

        private void set_data(string key, object value){
            int index=items.FindIndex((parameter obj) => obj.key==key);
            if(index>=0) items[index].value=value;
            else add(key,value);
        }

        
        public bool ContainsKey(string key) { 
            int index=items.FindIndex((parameter obj) => obj.key==key);
            if(index>=0) return true;
            return false;
        }

        public void AddRange(parameters p){
            items.AddRange(p.ToArray());
        }
        public parameter[] ToArray(){
            return items.ToArray();
        }


        public parameters()
        {
        }
        public parameters add(string name,object parameter) {
            items.Add(new parameter(name ,parameter));
            return this;
        }
        public override string ToString(){
            
            if(items.Count==0) {
                return "No parameters";
            }
            List<string> parameters =new List<String>();
                parameters.Add(String.Format("Parameters: Count {0} ",items.Count));
            foreach(parameter p in items){
                parameters.Add(p.ToString());
            }
            return String.Join(",",parameters.ToArray());
        }
    }
}
