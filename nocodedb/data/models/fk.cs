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
            public class fk_member{
            public string fk	        { get; set; }
            public string db	        { get; set; }
            public string table	        { get; set; }
            public string schema	    { get; set; }
            public string column	    { get; set; }
            public string fk_table	    { get; set; }
            public string fk_schema	    { get; set; }
            public string fk_column	    { get; set; }
            public string update_action { get; set; }
            public string delete_action { get; set; }
            public fk_member (row data){
              /*  fk	         =data["fk"];
                db	         =data["db"];
                table	     =data["table"];
                schema	     =data["schema"];
                column	     =data["column"];
                fk_table     =data["fk_table"];
                fk_schema    =data["fk_schema"];
                fk_column    =data["fk_column"];
                update_action=data["update_action"];
                delete_action=data["delete_action"];*/
            }

        }
        public class fk_members: IEnumerator,IEnumerable   {
            public List<fk_member> items=new List<fk_member>();
            public int position=-1;
            public fk_members(data_set data){
                foreach(row item in data) {
                    items.Add(new fk_member(item));
                }
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
        }

    }//end class
}//end namespace
