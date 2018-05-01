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
using System.Data;
using System.Reflection;

namespace nocodedb.data.models{
    
    public class column_meta{
        public bool        AllowDBNull                     { get; set; }
        public string      BaseCatalogName                 { get; set; }
        public string      BaseColumnName                  { get; set; }
        public string      BaseSchemaName                  { get; set; }
        public string      BaseServerName                  { get; set; }
        public string      BaseTableName                   { get; set; }
        public string      ColumnName                      { get; set; }
        public int         ColumnOrdinal                   { get; set; }
        public int         ColumnSize                      { get; set; }
        public Type        DataType                        { get; set; }
        public string      DataTypeName                    { get; set; }
        public bool        IsAliased                       { get; set; }
        public bool        IsAutoIncrement                 { get; set; }
        public bool        IsColumnSet                     { get; set; }
        public bool        IsExpression                    { get; set; }
        public bool        IsHidden                        { get; set; }
        public bool        IsIdentity                      { get; set; }
        public bool        IsKey                           { get; set; }
        public bool        IsLong                          { get; set; }
        public bool        IsReadOnly                      { get; set; }
        public bool        IsRowVersion                    { get; set; }
		public bool        IsUnique                        { get; set; }
        public int         NonVersionedProviderType        { get; set; }
        public bool        IsPrimaryKey                    { get; set; }
        public bool        IsUniqueKey                     { get; set; }
        public bool        IsForeignKey                    { get; set; }
        public string      ForeignKey_name                 { get; set; }
        public string      ForeignKey_table                { get; set; }
        public string      ForeignKey_schema               { get; set; }
        public string      ForeignKey_column               { get; set; }
        public int         NumericPrecision                { get; set; }
        public int         NumericScale                    { get; set; }
        public string      ProviderSpecificDataType        { get; set; }       //did we ever get here with other versions of sql server?
        public int         ProviderType                    { get; set; }
        public string      UdtAssemblyQualifiedName        { get; set; }
        public string      XmlSchemaCollectionDatabase     { get; set; } 
        public string      XmlSchemaCollectionName         { get; set; }
        public string      XmlSchemaCollectionOwningSchema { get; set; } 


        public column_meta(){
        }

    
        public column_meta(int ordnal,string name,Type data_type) {
            this.ColumnName=name;
            this.ColumnOrdinal=ordnal;
            this.DataType=data_type;
        }

        public column_meta(DataColumnCollection columns,DataRow  row) {
            try{
                DataColumn c=new DataColumn();

                foreach (DataColumn col in columns){
                    if (!type.set_property(this, col.ColumnName, row[col])) {
                        log.write("column_meta",log_type.Info,"Failed to set property :"+col.ColumnName);
                    }
                }//end foreach
            }catch (Exception e){
                log.write("column_meta",log_type.Error,e.ToString());
            }
        }
    }
}