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

        public static implicit operator System.Type(column_data instance) {
            if(null==instance) return null;
            return (System.Type)instance.value;
        }
        public static implicit operator System.Guid(column_data instance) {
            if(null==instance) return new System.Guid();
            return (System.Guid)instance.value;
        }
        public static implicit operator System.Boolean(column_data instance) {
            if(null==instance) return false;
            return (System.Boolean)instance.value;
        }
        public static implicit operator System.Byte(column_data instance) {
            if(null==instance) return 0;
            return (System.Byte)instance.value;
        }
        public static implicit operator System.Char(column_data instance) {
            if(null==instance) return '0';
            return (System.Char)instance.value;
        }
        public static implicit operator System.DateTime(column_data instance) {
            if(null==instance) return new DateTime();
            return (System.DateTime)instance.value;
        }
        public static implicit operator System.DBNull(column_data instance) {
            if(null==instance) return null;
            return (System.DBNull)instance.value;
        }
        public static implicit operator System.Decimal(column_data instance) {
            if(null==instance) return 0;
            return (System.Decimal)instance.value;
        }
        public static implicit operator System.Double(column_data instance) {
            if(null==instance) return 0;
            return (System.Double)instance.value;
        }
        public static implicit operator System.Int16(column_data instance) {
            if(null==instance) return 0;
            return (System.Int16)instance.value;
        }
        public static implicit operator System.Int32(column_data instance) {
            if(null==instance) return 0;
            return (System.Int32)instance.value;
        }
        public static implicit operator System.Int64(column_data instance) {
            if(null==instance) return 0;
            return (System.Int64)instance.value;
        }
        public static implicit operator System.SByte(column_data instance) {
            if(null==instance) return 0;
            return (System.SByte)instance.value;
        }
        public static implicit operator System.Single(column_data instance) {
            if(null==instance) return 0;
            return (System.Single)instance.value;
        } 
        public static implicit operator System.String(column_data instance) {
            if(null==instance) return null;
            return (System.String)instance.value;
        }
        public static implicit operator System.UInt16(column_data instance) {
            if(null==instance) return 0;
            return (System.UInt16)instance.value;
        }
        public static implicit operator System.UInt32(column_data instance) {
            if(null==instance) return 0;
            return (System.UInt32)instance.value;
        }
        public static implicit operator System.UInt64  (column_data instance) {
            if(null==instance) return 0;
            return (System.UInt64)instance.value;
        }

        public override string ToString() {
            return string.Format("{0}", value);
        }
        //public explicit operator // override object.Equals
         
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            // TODO: write your implementation of Equals() here
            throw new NotImplementedException();
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }
    }
}
