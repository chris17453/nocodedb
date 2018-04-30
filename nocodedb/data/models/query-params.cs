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
using System.Text;

namespace nocodedb.data.models{
    public class query_params:EventArgs{
        public string      connection_string { get; set; }
        public string      query             { get; set; }
        public parameters  parameters        { get; set; }
        public bool        meta              { get; set; }
        public query_types type              { get; set; }
        public string      message           { get; set; }
        public log_type    log_type          { get; set; }
        public string      function          { get; set; }

        public query_params(){
        }
        public query_params(string connection_string,string query,parameters parameters,bool meta,query_types type){
                this.connection_string =connection_string;
                this.query             =query;
                this.parameters        =parameters;
                this.meta              =meta;
                this.type              =type;
        }
        public override string ToString(){
            //This is temporary.... part of the conversion from nocodedb....

            StringBuilder o=new StringBuilder();
            if(!String.IsNullOrWhiteSpace(connection_string)) {
                o.Append(string.Format("connection_string={0}",connection_string));
            } else { 
                o.Append(string.Format("connection_string={0}","INVALID"));
            }
            if(!String.IsNullOrWhiteSpace(query)){
                o.Append(string.Format("query={0}",query));

            } else { 
                o.Append(string.Format("query={0}","INVALID"));
            }
            if(null!=meta){
                o.Append(string.Format("meta={0}",meta.ToString()));
            } else { 
                o.Append(string.Format("meta={0}","INVALID"));
            }
            if(null!=type){
                o.Append(string.Format("type={0}",type.ToString()));
            } else { 
                o.Append(string.Format("type={0}","INVALID"));
            }
            if(!String.IsNullOrWhiteSpace(message )){
                o.Append(string.Format("message={0}",message));
            } else { 
                o.Append(string.Format("message={0}","INVALID"));
            }
            if(null!=log_type){
                o.Append(string.Format("log_type={0}",log_type.ToString()));
            } else { 
                o.Append(string.Format("log_type={0}","INVALID"));
            }
            if(!String.IsNullOrWhiteSpace(function)){
                o.Append(string.Format("function={0}",function));
            } else { 
                o.Append(string.Format("function={0}","INVALID"));
            }

            return o.ToString();
        }
    }
}