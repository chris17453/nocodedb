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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using nocodedb.data.@interface;
using nocodedb.data.models;

namespace nocodedb.data.adapters
{
    public class base_adapter : IData
    {
        public virtual char left_field_seperator  { get { return '`'; } }
        public virtual char right_field_seperator { get { return '`'; } }

        public base_adapter()
        {
        }

        public string _connection_string { get; set; }

        public event EventHandler<query_params> LogEvent;

        /**************/
        //This is what you really need to impliment.
        public virtual string build_connection_string(string host, string user, string password,string database){
            throw new NotImplementedException();
        }

        public virtual bool test_connect(string connection_string){
            throw new NotImplementedException();
        }

        public virtual data_set sql_query(query_params q){
            throw new NotImplementedException();
        }
        public virtual fk.fk_objects get_fk_to_table(string connection_string,string database,string table,string schema){
            throw new NotImplementedException();
        }
        
        public virtual fk.fk_objects get_fk_from_table(string connection_string,string database,string table,string schema){
            throw new NotImplementedException();
        }
        /**************/

        [MethodImpl(MethodImplOptions.NoInlining)] 
        public string GetCurrentMethod() {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }


        protected virtual void log(query_params q, log_type type = log_type.Info, string message = null) {
            EventHandler<query_params> handler = LogEvent;
            if (handler != null) {
                q.message = message;
                q.log_type = type;
                q.function = GetCurrentMethod();
                handler(this, q);                                                                //dont lockup because of logging. Toss an event and run.
            }
        }

        public int execute_non_query(string connection_string, string query, parameters parameters = null) {
            query_params q = new query_params(connection_string, query, parameters, false, query_types.non);
            data_set ds = this.sql_query(q);
            return ds.affected_rows;
        }

        public column_data execute_scalar(string connection_string, string query, parameters parameters = null) {
            query_params q = new query_params(connection_string, query, parameters, false, query_types.scalar);
            data_set ds = this.sql_query(q);
            return ds[0][0];
        }

        public data_set fetch(string connection_string, string query, parameters parameters = null, bool meta = false) {
            query_params q = new query_params(connection_string, query, parameters, meta, query_types.single);
            return this.sql_query(q);
        }

        public data_set fetch_all(string connection_string, string query, parameters parameters = null, bool meta = false) {
            query_params q = new query_params(connection_string, query, parameters, meta, query_types.multiple);
            data_set results=this.sql_query(q);
            if(meta) {
                results.fk_from=get_fk_from_table(q.connection_string,results.columns[0].BaseCatalogName,results.columns[0].BaseTableName,results.columns[0].BaseSchemaName);
               // results.fk_to  =get_fk_to_table  (q.connection_string,results.columns[0].BaseCatalogName,results.columns[0].BaseTableName,results.columns[0].BaseSchemaName);
            }
            return results;
        }

        public data_set sp_fetch(string connection_string, string query, parameters parameters = null, bool meta = false)  {
            query_params q = new query_params(connection_string, query, parameters, meta, query_types.sp_single);
            return this.sql_query(q);
        }

        public data_set sp_fetch_all(string connection_string, string query, parameters parameters = null, bool meta = false) {
            query_params q = new query_params(connection_string, query, parameters, meta, query_types.sp_multiple);
            return this.sql_query(q);
        }

        public virtual string extract_query(query_params q) {
            StringBuilder o = new StringBuilder();
            if (null != q.parameters) {
                List<string> lst = new List<string>();
                foreach (var key2 in q.parameters) {
                    lst.Add(key2.ToString());
                }
                lst.Sort(delegate (string x, string y) { return y.Length.CompareTo(x.Length); });
                foreach (string name in lst) {
                    string value = "";
                    if (null != q.parameters[name]) value = q.parameters[name].ToString().Replace("'", "''");
                    if (name[0] != '@') {
                        //    query=query.Replace("@"+name,"'"+value+"'");
                    } else {
                        //    query=query.Replace(name,"'"+value+"'");
                    }
                    int n;
                    bool isNumeric = int.TryParse(value, out n);

                    if (value == "True")                    {
                        o.Append(String.Format("DECLARE {0,-30}     bit=1;       --TRUE\r\n", name));
                    } else if (value == "False") {
                        o.Append(String.Format("DECLARE {0,-30}     bit=0;       --FALSE\r\n", name));
                    } else if (isNumeric == true) {
                        o.Append(String.Format("DECLARE {0,-30}     int={1};\r\n", name, value));
                    } else {
                        if (value.Length >= 4000) {
                            o.Append(String.Format("DECLARE {0,-30}     varchar({2})='{1}';\r\n", name, value, value.Length + 10));
                        } else  {
                            o.Append(String.Format("DECLARE {0,-30}     nchar({2})='{1}';\r\n", name, value, value.Length + 10));
                        }
                    }
                }
            }
            o.Append("\r\n--" + q.connection_string + "\r\n");
            o.Append(q.query);
            return o.ToString();
        }

        public virtual void Dispose() {

        }

        /** CRUD Integration with base class **************************************/

/*        private string get_pk()
        {
            List<string> o = new List<string>();
            foreach (string _k in _pk)
            {
                o.Add(left_field_seperator + _k + right_field_seperator + "=@" + _k);
            }
            return String.Join(" AND ", o.ToArray());
        }
*/        
        private string get_columns(object obj) {
            Type T=obj.GetType();
            List<string> o = new List<string>();
            foreach (PropertyInfo pi in T.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){
                string field_name = pi.Name;
                object property_value = pi.GetValue(obj, null);
                if (null == property_value) continue;
                else o.Add(left_field_seperator + field_name + right_field_seperator);

            }
            return String.Join(",", o.ToArray());
        }

        private string get_params(object obj) {
            Type T=obj.GetType();
            List<string> o = new List<string>();
            foreach (PropertyInfo pi in T.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                string field_name = pi.Name;
                object property_value = pi.GetValue(obj, null);

                if (null == property_value) continue;
                else o.Add("@" + field_name);
            }
            return String.Join(",", o.ToArray());
        }

        private string get_col_param(object obj) {
            Type T=obj.GetType();
            List<string> o = new List<string>();
            foreach (PropertyInfo pi in T.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                string field_name = pi.Name;
                object property_value = pi.GetValue(obj, null);
                if (null == property_value) continue;
                else o.Add(left_field_seperator + field_name + right_field_seperator+"=" + "@" + field_name);
            }
            return String.Join(",", o.ToArray());
        }

        private bool set_property(object o,string field_name, object value){
            Type T=o.GetType();
            PropertyInfo pi = T.GetProperty(field_name);
            if (null != pi) {
                /*if (null != _json) {
                    foreach (string s in _json) {
                        if (s == field_name) {
                            Type type = pi.PropertyType;
                            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) {
                                try {
                                    object o2;

                                    object instance = null;
                                    Type itemType = type.GetGenericArguments()[0]; // use this...
                                    Type constructed = typeof(List<>).MakeGenericType(itemType);
                                    instance = Activator.CreateInstance(constructed);

                                    object s2 = itemType.MakeByRefType();
                                    o2 = JsonConvert.DeserializeAnonymousType((string)value, instance, new JsonSerializerSettings {
                                        Error = delegate (object sender, ErrorEventArgs args) {
                                            Console.WriteLine(args.ErrorContext.Error.Message);
                                            args.ErrorContext.Handled = true;
                                        }
                                    });


                                    this.GetType().GetProperty(field_name).SetValue(this, ((JArray)o2).ToObject(constructed), null);

                                } catch (Exception ex) {
                                    Console.WriteLine(ex.Message);
                                    return false;
                                }
                            }
                            return false;

                        }//end if s==name
                    }//end for
                }//end if in _json*/

                //normal assignement
                T.GetProperty(field_name).SetValue(o, value, null);
                return true;
            }
            return false;
        }


        private parameters generate_crud_parameters(object o) {
            parameters param = new parameters();
            Type T=o.GetType();
            PropertyInfo[] properties = T.GetProperties();
            foreach (PropertyInfo pi in T.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                string field_name = pi.Name;
                //if (field_name == "_table" || field_name == "_pk" || field_name == "_json") continue;              //dont paramaterize the base properties
                object property_value = pi.GetValue(o, null);
                /*if (null != _json) {
                    bool found = false;
                    foreach (string s in _json) {
                        if (s == field_name) {

                            string serial = JsonConvert.SerializeObject(property_value);
                            if (pi.GetValue(this, null) != null) {
                                param.add("@" + field_name, serial);
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found) continue;
                }*/
                if (property_value is Guid) {
                    if (((Guid)property_value) == Guid.Empty) continue;
                    param.add("@" + field_name, (Guid)property_value);
                    continue;
                }
                if (property_value is DateTime) {
                    if ((DateTime)property_value == DateTime.MinValue) continue;
                    param.add("@" + field_name, (DateTime)property_value);
                    continue;
                }
                if (property_value is string) {
                    param.add("@" + field_name, (string)property_value);
                    continue;
                }
                if (property_value is int) {
                    param.add("@" + field_name, (int)property_value);
                    continue;
                }
                if (property_value is bool) {
                    param.add("@" + field_name, (bool)property_value);
                    continue;
                }
                if (pi.GetValue(this, null) != null) {
                    param.add("@" + field_name, property_value);
                    continue;
                }

            }
            return param;
        }

        public bool load() {
            /*parameters q_param = generate_crud_parameters();
            string query = String.Format("SELECT TOP 1 * FROM {0} WHERE {1} ", this._table, this.get_pk());
            data_set res = db.fetch("titan", query, q_param);


            if (null != res) {
                foreach (String field_name in res.Keys) {
                    this.set_property(field_name, res[field_name]);
                }//loop through all keys
                return true;
            }//if it exist*/
            return false;
        }//if it exist


        public void create() {
            /*parameters q_param = generate_crud_parameters();
            string dbQ = string.Format("INSERT into [{0}] ({1}) VALUES ({2})", 
                                        left_field_seperator,right_field_seperator,
                                       this._table, this.get_columns(), this.get_params());
            db.execute_non_query("titan", dbQ, q_param);*/
        }//end create function

        public void update() {
            /*parameters q_param = generate_crud_parameters();
            string dbQ = string.Format("UPDATE [{0}] SET {1} WHERE {2}", 
                                       
                                       this._table, this.get_col_param(), this.get_pk());
            db.execute_non_query("titan", dbQ, q_param);
            */
        }//end update function

        public bool is_in_db() {
            /*string query = String.Format("SELECT TOP 1 * FROM {0} WHERE {1} ", 
                                         left_field_seperator,right_field_seperator,
                                         this._table, this.get_pk());
            parameters q_param = generate_crud_parameters();
            data_set res = db.fetch("titan", query, q_param);
            if (null != res) return true;
            */
            return false;
        }
        public void save() {
            if (this.is_in_db()) this.update();
            else this.create();
        }
        public string generate_insert_query() {
            return "";
        }
        public string generate_update_query() {
            return "";
        }




    }//end class
}//end namespace


/* Thoughts on how to impliment this......
 * 
 * 
 * 
 * 
// class is un attachted. 100% unique fields. Requires helper class to process.
// Single
var t=new ncdb.titan.dbo.titanDWS();
 db.save  (t);
 db.load  (t);
 db.delete(t);
*db.update(t);
*db.insert(t);

// class is un attachted. 100% unique fields. Requires helper class to process. more strict... more work...
// Single
var t=titan.dbo.titanDWS();
 db<t>.save  (t);
 db<t>.load  (t);
 db<t>.delete(t);
*db<t>.update(t);
*db<t>.insert(t);

// uses methods which can break database field uniqueness
// Single
var t=titan.dbo.titanDWS();
 t.save  (t);
 t.load  (t);
 t.delete(t);
*t.update(t);
*t.insert(t);

// uses methods which can break database field uniqueness, but only 1 field.
// Single
var t=titan.dbo.titanDWS();
 t.db.save  (t);
 t.db.load  (t);
 t.db.delete(t);
*t.db.update(t);
*t.db.insert(t);

//////////////////////////////////////
// Single or Multiple
var t=data_set<titan.dbo.titanDWS>();                 //cannot use VAR unless you load it first.
 t.save  (single);
 t.load  (single/multiple);
 t.delete(single);
*t.update(single);
*t.insert(single);

t.curent_item.property
t[0].property
foreach(var r in rows) {

}
*/