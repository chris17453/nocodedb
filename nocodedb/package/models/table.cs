using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace nocodedb.models {
    public class table : INotifyPropertyChanged{
        private bool                data_set_value      {get; set; }
        public bool                 data_set    { get { return data_set_value; } set {  if(value!=data_set_value) data_set_value=value; NotifyPropertyChanged(); } }
        public string               name        { get; set; }
        public string               database    { get; set; }
        public int                  fk_children_count { get { int x=0; foreach(column c in columns) x+=c.fk_children_count; return x; } }
        public bool                 has_fk      { get { foreach(column c in columns) if(c.has_fk) return true; return false; } }
        public int                  fk_link_count { get { if(has_fk) return fk_children_count+1;  return  fk_children_count; } }
        public string               group       { get; set; }
        public Point                Location    { get; set; }
        public BindingList<column>  columns     { get; set; }
        public bool                 flag1       { get; set; }
        public bool                 flag2       { get; set; }
        public bool                 flag3       { get; set; }
       // public column               selected_column { get; set; }
        public db_settings          connection { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = ""){
            if (PropertyChanged != null){
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public table() {
            flag1=false;
            flag2=false;
            flag3=false;
        }

        public table(db_settings db_settings,string db_name,string table_name) {
            data_set_value=false;
            this.data_set=false;
            this.name    =table_name;
            this.database=db_name;
            this.connection=db_settings;
            this.columns=new BindingList<column>();
            Location=new Point(0,0);
            flag1=false;
            flag2=false;
            flag3=false;
        }

        public void load_columns() {
           /* List<Hashtable> results=titan.db.fetchAll(
            connection.ToString(),@"USE "+database+@"; SELECT		COLUMN_NAME as 'name',cast(COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') as bit) as 'identity'
            FROM		INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME =@table_name ORDER BY ORDINAL_POSITION",new string[,] { {"table_name" , name} });
            if(null==columns) columns=new BindingList<column>();
            foreach (Hashtable item in results) {
                columns.Add(new column(database,name,(string)item["name"],(bool)item["identity"]));
            }*/

        }
    }
}
