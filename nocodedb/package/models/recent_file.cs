using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace nocodedb.models {
    public class recent_file : INotifyPropertyChanged {
        public string name { get; set; }
        public string file { get; set; }
        public string file_name { get  { if(string.IsNullOrWhiteSpace(file)) return ""; return Path.GetFileName(file);  } }
        public string file_path { get  { if(string.IsNullOrWhiteSpace(file)) return ""; return Path.GetDirectoryName(file);  } }
        public DateTime accessed { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = ""){
            if (PropertyChanged != null){
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public recent_file() {
            this.name="";
            this.file="";
            this.accessed=DateTime.Now;
        }

        public recent_file(string name,string file) {
            this.name=name;
            this.file=file;
            this.accessed=DateTime.Now;
        }
    }
}
