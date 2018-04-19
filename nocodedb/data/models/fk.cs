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

namespace nocodedb.data.models{
   public class fk{

        public class fk_object{
            public string fk	        { get; set; }
            public string table	        { get; set; }
            public string schema	    { get; set; }
            public string column	    { get; set; }
            public string fk_table	    { get; set; }
            public string fk_schema	    { get; set; }
            public string fk_column	    { get; set; }
            public string update_action { get; set; }
            public string delete_action { get; set; }
            public fk_object (row data){
                fk	         =data["fk"];
                table	     =data["table"];
                schema	     =data["schema"];
                column	     =data["column"];
                fk_table     =data["fk_table"];
                fk_schema    =data["fk_schema"];
                fk_column    =data["fk_column"];
                update_action=data["update_action"];
                delete_action=data["delete_action"];
            }

        }
        public class fk_objects{
            public List<fk_object> items=new List<fk_object>();
            public fk_objects(data_set data){
                foreach(row item in data.rows) {
                    items.Add(new fk_object(item));
                }
            }
        }

    }//end class
}//end namespace
