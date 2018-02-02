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
using System.Runtime.CompilerServices;
using System.Text;
using nocodedb.data.@interface;
using nocodedb.data.models;

namespace nocodedb.data.adapters {
    public class base_adapter :IData
    {
        public base_adapter()
        {
        }

        public string _connection_string { get; set; }

        public event EventHandler<query_params> LogEvent;

        /**************/
        //This is what you really need to impliment.
        public virtual string connection_string(string host, string user, string password)
        {
            throw new NotImplementedException();
        }

        public virtual bool test_connect(string connection_string)
        {
            throw new NotImplementedException();
        }

        public virtual data_set sql_query(query_params q){
            throw new NotImplementedException();
        }
        /**************/

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod ()
        {
            StackTrace st = new StackTrace ();
            StackFrame sf = st.GetFrame (1);
            return sf.GetMethod().Name;
        }


        protected virtual void log(query_params q,log_type type=log_type.Info,string message=null){
            EventHandler<query_params> handler = LogEvent;
            if (handler != null){
                q.message  =message;
                q.log_type =type;
                q.function =GetCurrentMethod();
                handler(this,q);                                                                //dont lockup because of logging. Toss an event and run.
            }
        }

        public int execute_non_query(string connection_string, string query, parameters parameters=null) {
            query_params q= new query_params(connection_string,query,parameters,false, query_types.non);
            data_set ds= this.sql_query(q);
            return ds.affected_rows;
        }

        public column_data execute_scalar(string connection_string, string query, parameters parameters=null) {
            query_params q= new query_params(connection_string,query,parameters,false, query_types.scalar);
            data_set ds= this.sql_query(q);
            return ds[0][0];
        }

        public data_set fetch(string connection_string, string query, parameters parameters=null, bool meta=false) {
            query_params q= new query_params(connection_string,query,parameters,meta, query_types.single);
            return this.sql_query(q);
        }

        public data_set fetch_all(string connection_string,string query,parameters parameters=null,bool meta=false){
            query_params q= new query_params(connection_string,query,parameters,meta, query_types.multiple);
            return this.sql_query(q);
        }

        public data_set sp_fetch(string connection_string, string query, parameters parameters=null, bool meta=false) {
            query_params q= new query_params(connection_string,query,parameters,meta, query_types.sp_single);
            return this.sql_query(q);
        }

        public data_set sp_fetch_all(string connection_string, string query, parameters parameters=null, bool meta=false) {
            query_params q= new query_params(connection_string,query,parameters,meta, query_types.sp_multiple);
            return this.sql_query(q);
        }

        public virtual string extract_query(query_params q) {
            StringBuilder o=new StringBuilder();
            if(null!=q.parameters) {
                List<string> lst = new List<string>();
                foreach (var key2 in q.parameters) {
                    lst.Add(key2.ToString());
                }
                lst.Sort( delegate (string x,string y){ return y.Length.CompareTo(x.Length); });
                foreach (string name in lst) {

                    string value="";
                    if(null!=q.parameters[name]) value=q.parameters[name].ToString().Replace("'","''");
                    if(name[0]!='@') {
                        //    query=query.Replace("@"+name,"'"+value+"'");
                    } else {
                        //    query=query.Replace(name,"'"+value+"'");
                    }
                    int n;
                    bool isNumeric = int.TryParse(value, out n);


                    if (value=="True") {
                        o.Append(String.Format("DECLARE {0,-30}     bit=1;       --TRUE\r\n",name));
                    } else
                        if(value=="False") {
                            o.Append(String.Format("DECLARE {0,-30}     bit=0;       --FALSE\r\n",name));
                        } else
                            if(isNumeric==true) {
                                o.Append(String.Format("DECLARE {0,-30}     int={1};\r\n",name,value));
                            }
                            else { 
                                if(value.Length>=4000) {
                                    o.Append(String.Format("DECLARE {0,-30}     varchar({2})='{1}';\r\n",name,value,value.Length+10));
                                } else {
                                    o.Append(String.Format("DECLARE {0,-30}     nchar({2})='{1}';\r\n",name,value,value.Length+10));
                                }   
                            }
                }
            }
            o.Append("\r\n--"+q.connection_string+"\r\n");
            o.Append(q.query);
            return o.ToString();
        }
    }
}
