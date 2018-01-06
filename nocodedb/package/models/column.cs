using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace nocodedb.models {

    public class column : INotifyPropertyChanged {
        public bool     identity    { get; set; }
        public string   name        { get; set; }
        public string   table       { get; set; }
        public string   db          { get; set; }

        public bool     has_fk      { get { if(null==fk) return false; return true; }  }
        public fk        fk         { get; set; }
        public BindingList<fk> fk_children  { get; set; }
        public int      fk_children_count { get { if(null==fk_children) return 0; return fk_children.Count;}  } 

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = ""){
            if (PropertyChanged != null){
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
  
        public column() {
            if(fk==null) fk_children=new BindingList<fk>();
        }
        public column(string database,string table,string name,bool identity) {

            this.name=name;
            this.identity   =identity;
            this.db         =database;
            this.table      =table;
            this.fk_children=new BindingList<fk>();
        }
    }
}
