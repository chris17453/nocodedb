using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace nocodedb.data.models{
[JsonObject(MemberSerialization.OptIn)]
    public class rows :JsonConverter,IEnumerator,IEnumerable   {
        private List<row> items =new List<row>();
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

        public column_data this[string key]        {
            get {
                if(position>0 &&  items.Count>position && String.IsNullOrWhiteSpace(key)){
                return get_data(position)[key];
                }
                return null;
            }
            set  {
                if(position>0 &&  items.Count>position && String.IsNullOrWhiteSpace(key)){
                    items[position][key]=(column_data)value;
                } else {
                    throw new ArgumentException();
                }
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


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                //IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                //o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

    }
}
