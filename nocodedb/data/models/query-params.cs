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
        public override string ToString()
        {
            return string.Format("[query_params: connection_string={0}, query={1}, parameters={2}, meta={3}, type={4}, message={5}, log_type={6}, function={7}]", connection_string, query, parameters, meta, type, message, log_type, function);
        }
    }
}