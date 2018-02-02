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
using System.Web.Script.Serialization;

namespace nocodedb.data.models{
    public class column_data{
        public object value   { get; set; }
        [ScriptIgnore]
        public Type   type    { get { if(null!=value) return value.GetType(); return null; } }
        public column_data() {
        }
        public column_data(object column_value) {
            this.value = column_value;
        }
        public override string ToString() {
            return string.Format("{0}", value);
        }
    }
}
