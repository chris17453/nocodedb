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
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace nocodedb.data.models{
    public class column_data:ISerializable{
        public object value   { get; set; }

        public column_data() {
        }
        public column_data(object column_value) {
            this.value = column_value;
        }
        public Type   type()    { 
            if(null!=value) 
                return value.GetType(); 
            return null; 
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
            return true;
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
            return System.DBNull.Value;
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
            if(null==instance || instance.value.Equals(System.DBNull.Value)) return null;
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("value",value);
        }
        //public explicit operator // override object.Equals
          
         public override bool Equals(object obj)
         {
             if (obj == null || GetType() != obj.GetType()) {
                 return false;
             }

             if(this.value.ToString()==obj.ToString()) {
                return true;
             }
                return false;

         }

         // override object.GetHashCode
         public override int GetHashCode() {
            return value.GetHashCode();
         }
    }





}
