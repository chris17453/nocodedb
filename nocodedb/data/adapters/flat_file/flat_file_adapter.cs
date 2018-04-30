/*******************************************************************************

███╗   ██╗ ██████╗     ██████╗ ██████╗ ██████╗ ███████╗   ██████╗ ██████╗ 
████╗  ██║██╔═══██╗   ██╔════╝██╔═══██╗██╔══██╗██╔════╝   ██╔══██╗██╔══██╗
██╔██╗ ██║██║   ██║   ██║     ██║   ██║██║  ██║█████╗     ██║  ██║██████╔╝
██║╚██╗██║██║   ██║   ██║     ██║   ██║██║  ██║██╔══╝     ██║  ██║██╔══██╗
██║ ╚████║╚██████╔╝██╗╚██████╗╚██████╔╝██████╔╝███████╗██╗██████╔╝██████╔╝
╚═╝  ╚═══╝ ╚═════╝ ╚═╝ ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝╚═╝╚═════╝ ╚═════╝ 
   Created: 12-23-2017
   Author : Charles Watkins
   Email  : chris17453@gmail.com
*******************************************************************************/
using System;
using System.Collections;

namespace  nocodedb.data{
    public class flat_file_adapter:IData{
        public flat_file_adapter(){
        }

        public event EventHandler LogEvent;

        public string connection_string(string host, string user, string password)
        {
            throw new NotImplementedException();
        }

        public int execute_non_query(string connection_string, string query, Hashtable parameters)
        {
            throw new NotImplementedException();
        }

        public column_data execute_scalar(string connection_string, string query, Hashtable parameters)
        {
            throw new NotImplementedException();
        }

        public data_set fetch(string connection_string, string query, Hashtable parameters, bool meta = false)
        {
            throw new NotImplementedException();
        }

        public data_set fetch_all(string connection_string, string query, Hashtable parameters, bool meta = false)
        {
            throw new NotImplementedException();
        }

        public data_set sp_fetch(string connection_string, string query, Hashtable parameters, bool meta = false)
        {
            throw new NotImplementedException();
        }

        public data_set sp_fetch_all(string connection_string, string query, Hashtable parameters, bool meta = false)
        {
            throw new NotImplementedException();
        }

        public bool test_connect(string connection_string)
        {
            throw new NotImplementedException();
        }
    }
}
