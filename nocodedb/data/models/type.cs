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
using System.Reflection;

namespace nocodedb.data.models{
    public static class type {

        public static Type get_property(object item,string property){
            PropertyInfo p=item.GetType().GetProperty(property);
            if(null==p) return null;
            if(p.PropertyType.Name=="Nullable`1") {
                return Nullable.GetUnderlyingType(p.PropertyType);
            } else {
                return p.PropertyType;
            }
        }


        //SQL Data Types -> https://msdn.microsoft.com/en-us/library/system.data.sqldbtype(v=vs.110).aspx
        public static bool set_property(object item,string property_name,object value){
            try{
                if (null!=value && value.ToString()=="System.DateTime") {
//                    == "ProviderSpecificDataType") {
                    var g=12;
                }
                if(null==item) {                                                                                    //no item to get the signature from
                    return false;                                                                           
                }
                if(string.IsNullOrWhiteSpace(property_name)){                                                       //no property
                    return false;                                                                           
                }
                PropertyInfo p=item.GetType().GetProperty(property_name);                                           //This is the signature
                if(null==p) {                                                                                       //property not found. Bail.
                    return false;                                                                           
                }
                
                Type resolved_property_type=Nullable.GetUnderlyingType(p.PropertyType);
                bool is_nullable=true;
                if(null==resolved_property_type)   {                                                               //its not a nullable type
                    resolved_property_type=p.PropertyType;
                    is_nullable=false;
                }

                if(resolved_property_type==null) {                                                                  //no type.. jet.
                    return false;                                                                        
                }
            
                if (null==value) {                                                                                  //is it a null?
                    if(!is_nullable) {                                                                              //its not a nullable type, but its a null value!
                        if(null==p.GetValue(item)) return true;                                                     //the property is already null. so its set! TRUE. but not...
                        return false;                        
                    } else {                                                                                        //its a null. and nullable 
                        p.SetValue(item,null);    
                        return true;
                    }
                }

                Type data_type=value.GetType();

                if(typeof(System.Type).Equals(resolved_property_type)) {                                            //its a type... oh bother special snowflake
                    p.SetValue(item,(Type)value);                                                                   //assign and jet
                    return true;
                }

                if(data_type.Equals(resolved_property_type)) {                                                      //objects are the same type 
                    p.SetValue(item,value);                                                                         //assign and jet
                    return true;
                } else {                                                                                            //convert different types
                    IConvertible convertible = value as IConvertible;
                    if (convertible == null){                                                                       //cant convert this. WTF bro!?
                        if(resolved_property_type.FullName=="System.String"){                                           //assign type to a string.... fail on other types?
                            p.SetValue(item,value.ToString());                                                          
                            return true;
                        }
                        return false;
                    }
                }

                if(Convert.IsDBNull(value) && !is_nullable) {
                   if(null==p.GetValue(item)) return true;                                                          //the property is already null. so its set! TRUE. but not...
                   return false;                                                                                    //cant assign null to non nullable
                }
                
                object resolved_value=Convert.ChangeType(value,resolved_property_type);
                p.SetValue(item,value);                                                                             //assign converted type and jet
                return true;
            }catch (Exception e){
                log.write("column_meta",log_type.Error,e.ToString());                                               //shit went sideways. you don't get paid man
            }
            
            return false;
        }

        /*public static Type get_sql_type(object item){
            Type t=(item).GetType();
            if(t.Name=="RuntimeType") {
                string type_name=item.ToString();
                Type resolved_type=t.;
                return resolved_type;
            } else {
                return t;
            }*/
                /*switch(type_name){
                    case "System.Data.SqlTypes.SqlBinary"     : resolved_type=typeof(System.Data.SqlTypes.SqlBinary); break;
                    case "System.Data.SqlTypes.SqlBoolean"    : resolved_type=typeof(System.Data.SqlTypes.SqlBoolean); break;
                    case "System.Data.SqlTypes.SqlByte"       : resolved_type=typeof(System.Data.SqlTypes.SqlByte); break;
                    case "System.Data.SqlTypes.SqlBytes"      : resolved_type=typeof(System.Data.SqlTypes.SqlBytes); break;
                    case "System.Data.SqlTypes.SqlChars"      : resolved_type=typeof(System.Data.SqlTypes.SqlChars); break;
                    case "System.Data.SqlTypes.SqlDateTime"   : resolved_type=typeof(System.Data.SqlTypes.SqlDateTime); break;
                    case "System.Data.SqlTypes.SqlDecimal"    : resolved_type=typeof(System.Data.SqlTypes.SqlDecimal); break;
                    case "System.Data.SqlTypes.SqlDouble"     : resolved_type=typeof(System.Data.SqlTypes.SqlDouble); break;
                    case "System.Data.SqlTypes.SqlFileStream" : resolved_type=typeof(System.Data.SqlTypes.SqlFileStream); break;
                    case "System.Data.SqlTypes.SqlGuid"       : resolved_type=typeof(System.Data.SqlTypes.SqlGuid); break;
                    case "System.Data.SqlTypes.SqlInt16"      : resolved_type=typeof(System.Data.SqlTypes.SqlInt16); break;
                    case "System.Data.SqlTypes.SqlInt32"      : resolved_type=typeof(System.Data.SqlTypes.SqlInt32); break;
                    case "System.Data.SqlTypes.SqlInt64"      : resolved_type=typeof(System.Data.SqlTypes.SqlInt64); break;
                    case "System.Data.SqlTypes.SqlMoney"      : resolved_type=typeof(System.Data.SqlTypes.SqlMoney); break;
                    case "System.Data.SqlTypes.SqlString"     : resolved_type=typeof(System.Data.SqlTypes.SqlString); break;
                    case "System.Data.SqlTypes.SqlXml"        : resolved_type=typeof(System.Data.SqlTypes.SqlXml); break;
                    case "System.Data.SqlTypes.System.Guid"   : resolved_type=typeof(System.Data.SqlTypes.SqlGuid); break;
                    case "System.Type"                        : resolved_type=typeof(System.Type); break;
                    case "System.Data.SqlDbType"              : resolved_type=typeof(System.Data.SqlDbType); break;
                    case "System.Guid"                        : resolved_type=typeof(System.Guid); break;
                    case "System.Boolean"                     : resolved_type=typeof(System.Boolean ); break;
                    case "System.Byte"                        : resolved_type=typeof(System.Byte    ); break;
                    case "System.Char"                        : resolved_type=typeof(System.Char    ); break;
                    case "System.DateTime"                    : resolved_type=typeof(System.DateTime); break;
                    case "System.DBNull"                      : resolved_type=typeof(System.DBNull  ); break;
                    case "System.Decimal"                     : resolved_type=typeof(System.Decimal ); break;
                    case "System.Double"                      : resolved_type=typeof(System.Double  ); break;
                    case "System.Int16"                       : resolved_type=typeof(System.Int16   ); break;
                    case "System.Int32"                       : resolved_type=typeof(System.Int32   ); break;
                    case "System.Int64"                       : resolved_type=typeof(System.Int64   ); break;
                    case "System.Object"                      : resolved_type=typeof(System.Object  ); break;
                    case "System.SByte"                       : resolved_type=typeof(System.SByte   ); break;
                    case "System.Single"                      : resolved_type=typeof(System.Single  ); break;
                    case "System.String"                      : resolved_type=typeof(System.String  ); break;
                    case "System.UInt16"                      : resolved_type=typeof(System.UInt16  ); break;
                    case "System.UInt32"                      : resolved_type=typeof(System.UInt32  ); break;
                    case "System.UInt64"                      : resolved_type=typeof(System.UInt64  ); break;
                    default : continue; //we dont know what it is. bail bro
                }

 
/*
System.Data.SqlTypes.SqlBinary
System.Data.SqlTypes.SqlBoolean
System.Data.SqlTypes.SqlByte
System.Data.SqlTypes.SqlBytes
System.Data.SqlTypes.SqlChars
System.Data.SqlTypes.SqlDateTime
System.Data.SqlTypes.SqlDecimal
System.Data.SqlTypes.SqlDouble
System.Data.SqlTypes.SqlFileStream
System.Data.SqlTypes.SqlGuid
System.Data.SqlTypes.SqlInt16
System.Data.SqlTypes.SqlInt32
System.Data.SqlTypes.SqlInt64
System.Data.SqlTypes.SqlMoney
System.Data.SqlTypes.SqlSingle
System.Data.SqlTypes.SqlString
System.Data.SqlTypes.SqlXml

* 
 * 
 */
        //}//end function
    }//end class
}//end namespace
