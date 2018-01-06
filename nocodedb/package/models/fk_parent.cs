using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nocodedb.models {
    public class fk_parent {
        public string db        { get; set; }
        public string table     { get; set; }
        public string column    { get; set; }
        public fk_parent() {
        }
        public fk_parent(string db,string table,string column) {
            this.db     =db;
            this.table  =table;
            this.column =column;
        }
        public override String ToString() {
            if(string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(column)) return "";
            return db+"."+table+"."+column;
        }
    }
}
