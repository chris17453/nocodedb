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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using nocodedb.data.models;

namespace nocodedb.data {
    public class crud<T> {
        protected string _table      =null;
        protected string [] _pk      =null;
        protected string [] _json    =null;

        private string get_pk() {
            List<string> o=new List<string>();
            foreach(string _k in _pk) {
                o.Add("["+_k+"]=@"+_k);
            }
            return String.Join(" AND ",o.ToArray());
        }
        private string get_columns() {
            List<string> o=new List<string>();
            Type t=this.GetType();

            foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) { 
                string field_name     = pi.Name;
                object property_value = pi.GetValue(this, null);
                if(null==property_value ) continue;
                else o.Add("["+field_name+"]");

            }
            return String.Join(",",o.ToArray());
        }
        private string get_params() {
            List<string> o=new List<string>();
            Type t=this.GetType();

            foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) { 
                string field_name     = pi.Name;
                object property_value = pi.GetValue(this, null);

                if(null==property_value ) continue;
                else o.Add("@"+field_name);
            }
            return String.Join(",",o.ToArray());
        }

        private string get_col_param() {
            List<string> o=new List<string>();
            Type t=this.GetType();

            foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) { 
                string field_name     = pi.Name;
                object property_value = pi.GetValue(this, null);
                if(null==property_value ) continue;
                else o.Add("["+field_name+"]="+"@"+field_name);
            }
            return String.Join(",",o.ToArray());
        }

        public bool set_list_property(string field_name,Type property_type,object value){
            try {
                object o2;

                object instance=null;
                Type itemType = property_type.GetGenericArguments()[0]; // use this...
                Type constructed = typeof(List<>).MakeGenericType(itemType);
                instance = Activator.CreateInstance(constructed);

                object s2=itemType.MakeByRefType();
                o2=JsonConvert.DeserializeAnonymousType((string)value,instance,new JsonSerializerSettings{
                    Error = delegate(object sender, ErrorEventArgs args){
                        Console.WriteLine(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    }
                });

                this.GetType().GetProperty(field_name).SetValue(this,((JArray)o2).ToObject(constructed), null);
                return true;
            } catch (Exception ex) {
                Console.WriteLine(String.Format("{0} {1}",field_name,ex.Message));
            }
            return false;
        }

        public bool set_property(string field_name,object value) {
            try{

                Type  vT=value.GetType();

                //unravel nested type
                if(vT==typeof(column_data)) {
                    value=((column_data)value).value;
                }

                PropertyInfo pi=this.GetType().GetProperty(field_name);
                Type pT = pi.PropertyType;
                if (null!=pi) {
                    if(null!=_json) {
                        foreach(string s in _json) {
                            if (s==field_name) {
                                if(pT.IsGenericType && pT.GetGenericTypeDefinition()== typeof(List<>)) {
                                    return set_list_property(field_name,pT,value);
                                }
                            }//end if s==name
                        }//end for
                    }//end if in _json

                    //This is json data
                    if(pT.IsGenericType && pT.GetGenericTypeDefinition()== typeof(List<>)) {
                        return set_list_property(field_name,pT,value);
                    }
                        
                    //This is NOT JSON data
                    //normal assignement... we are being specific here
                    if(value is Boolean ) { this.GetType().GetProperty(field_name).SetValue(this, (Boolean )value, null);  return true; }
                    if(value is Byte    ) { this.GetType().GetProperty(field_name).SetValue(this, (Byte    )value, null);  return true; }
                    if(value is Char    ) { this.GetType().GetProperty(field_name).SetValue(this, (Char    )value, null);  return true; }
                    if(value is DBNull  ) { this.GetType().GetProperty(field_name).SetValue(this, (DBNull  )value, null);  return true; }
                    if(value is Decimal ) { this.GetType().GetProperty(field_name).SetValue(this, (Decimal )value, null);  return true; }
                    if(value is Double  ) { this.GetType().GetProperty(field_name).SetValue(this, (Double  )value, null);  return true; } 
                    if(value is Int16   ) { this.GetType().GetProperty(field_name).SetValue(this, (Int16   )value, null);  return true; }
                    if(value is Int32   ) { this.GetType().GetProperty(field_name).SetValue(this, (Int32   )value, null);  return true; }
                    if(value is Int64   ) { this.GetType().GetProperty(field_name).SetValue(this, (Int64   )value, null);  return true; }
                    if(value is Object  ) { this.GetType().GetProperty(field_name).SetValue(this, (Object  )value, null);  return true; }
                    if(value is SByte   ) { this.GetType().GetProperty(field_name).SetValue(this, (SByte   )value, null);  return true; }
                    if(value is Single  ) { this.GetType().GetProperty(field_name).SetValue(this, (Single  )value, null);  return true; }
                    if(value is String  ) { this.GetType().GetProperty(field_name).SetValue(this, (String  )value, null);  return true; }
                    if(value is UInt16  ) { this.GetType().GetProperty(field_name).SetValue(this, (UInt16  )value, null);  return true; }
                    if(value is UInt32  ) { this.GetType().GetProperty(field_name).SetValue(this, (UInt32  )value, null);  return true; }
                    if(value is UInt64  ) { this.GetType().GetProperty(field_name).SetValue(this, (UInt64  )value, null);  return true; }
                    if(value is Guid    ) { this.GetType().GetProperty(field_name).SetValue(this, (Guid    )value, null);  return true; }
                    if(value is DateTime) { this.GetType().GetProperty(field_name).SetValue(this, (DateTime)value, null);  return true; }

                }
            } catch(Exception ex) {
                Console.WriteLine(String.Format("{0} {1}",field_name,ex.Message));
            }
            return false;
        }


        public parameters generate_crud_parameters() {
            parameters param=new parameters();
            PropertyInfo[] properties = typeof(T).GetProperties();
            Type t=this.GetType();
            foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) { 
                string field_name     = pi.Name;
                if(field_name=="_table" || field_name=="_pk" || field_name=="_json") continue;              //dont paramaterize the base properties
                object property_value = pi.GetValue(this, null);
                if(null!=_json) {
                    bool found=false;
                    foreach(string s in _json) { 
                        if (s==field_name) {

                            string serial=JsonConvert.SerializeObject(property_value);
                            if(pi.GetValue(this, null)!=null) {
                                param.add("@"+field_name,serial);
                                found=true;
                                break;
                            }
                        }
                    }
                    if(found) continue;
                }
                if(property_value is Guid) {
                    if(((Guid)property_value) == Guid.Empty) continue;
                    param.add("@"+field_name,(Guid)property_value);
                    continue;
                }
                if (property_value is DateTime) {
                    if((DateTime)property_value==DateTime.MinValue) continue;
                    param.add("@"+field_name,(DateTime)property_value);
                    continue;
                }
                if(property_value is string) {
                    param.add("@"+field_name,(string)property_value);
                    continue;
                }
                if(property_value is int) {
                    param.add("@"+field_name,(int)property_value);
                    continue;
                }
                if(property_value is bool) {
                    param.add("@"+field_name,(bool)property_value);
                    continue;
                }

                if (pi.GetValue(this, null)!=null) {
                    param.add("@"+field_name,property_value);
                    continue;
                }

            }
            return param;
        }

        public bool load() {
            parameters q_param=generate_crud_parameters();
            string query=String.Format("SELECT TOP 1 * FROM {0} WHERE {1} ",this._table,this.get_pk());
            data_set res=db.fetch("titan",query,q_param);


            if (null!=res) {
                foreach (String field_name in res.Keys) {
                    this.set_property(field_name,res[0,field_name]);
                    //this.set_property(field_name,res.columns[field_name]);
                }//loop through all keys
                return true;
            }//if it exist
            return false;
        }//if it exist


        public void create() {
            parameters q_param=generate_crud_parameters();
            string dbQ=string.Format("INSERT into [{0}] ({1}) VALUES ({2})",this._table,this.get_columns(),this.get_params());
            db.execute_non_query("titan",dbQ,q_param);
        }//end create function

        public void update() {
            parameters q_param=generate_crud_parameters();
            string dbQ=string.Format("UPDATE [{0}] SET {1} WHERE {2}",this._table,this.get_col_param(),this.get_pk());
            db.execute_non_query("titan",dbQ,q_param);
        }//end update function

        public bool is_in_db() {
            string query=String.Format("SELECT TOP 1 * FROM {0} WHERE {1} ",this._table,this.get_pk());
            parameters q_param=generate_crud_parameters();
            data_set res=db.fetch("titan",query,q_param);
            if (null!=res) return true;
            return false;
        }
        public void save() {
            if(this.is_in_db())    this.update();
            else                   this.create();
        }
        public string generate_insert_query() {
            return "";
        }
        public string generate_update_query() {
            return "";
        }

    }//end partial class
}//end namespace