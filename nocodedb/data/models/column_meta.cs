using System;
using System.Data;
using System.Reflection;

namespace nocodedb.data.models{
    
    public class column_meta{
        public bool   AllowDBNull               { get; set; }
        public string BaseCatalogName           { get; set; }
        public string BaseColumnName            { get; set; }
        public string BaseSchemaName            { get; set; }
        public string BaseServerName            { get; set; }
        public string BaseTableName             { get; set; }
        public string ColumnName                { get; set; }
        public int    ColumnOrdinal             { get; set; }
        public int    ColumnSize                { get; set; }
        public Type   DataType                  { get; set; }
        public bool   IsAliased                 { get; set; }
        public bool   IsAutoIncrement           { get; set; }
        public bool   IsColumnSet               { get; set; }
        public bool   IsExpression              { get; set; }
        public bool   IsHidden                  { get; set; }
        public bool   IsIdentity                { get; set; }
        public bool   IsKey                     { get; set; }
        public bool   IsLong                    { get; set; }
        public bool   IsReadOnly                { get; set; }
        public bool   IsRowVersion              { get; set; }
		public bool   IsUnique                  { get; set; }
        public int    NonVersionedProviderType        { get; set; }
        public int    NumericPrecision                { get; set; }
        public int    NumericScale                    { get; set; }
        public string ProviderSpecificDataType        { get; set; }
        public string ProviderType                    { get; set; }
        public string UdtAssemblyQualifiedName        { get; set; }
        public string XmlSchemaCollectionDatabase     { get; set; } 
        public string XmlSchemaCollectionName         { get; set; }
        public string XmlSchemaCollectionOwningSchema { get; set; }

        public column_meta(){
        }

        public static bool CanChangeType(object value, Type conversionType)
        {
            if (conversionType == null){
                return false;
            }

            if (value == null){              
                return false;
            }

            IConvertible convertible = value as IConvertible;

            if (convertible == null){
                return false;
            }

            return true;
        }
        public column_meta(DataColumnCollection columns,DataRow  row) {
            try{
                DataColumn c=new DataColumn();

                foreach (DataColumn col in columns){
                    PropertyInfo p=this.GetType().GetProperty(col.ColumnName);
                    if(null!=p) {
                        if(row!=null && row[col]!=DBNull.Value) {
                            if(CanChangeType(row[col],p.PropertyType)) {
                                p.SetValue(this, Convert.ChangeType(row[col], p.PropertyType),null);
                            } else {
                                p.SetValue(this, row[col],null);    

                            }
                        } else {
                            p.SetValue(this, null,null);
                        }
                    }
                }
            }catch (Exception e){
                log.write("column_meta",log_type.Error,e.ToString());
            }
        }
    }
}