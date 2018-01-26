using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace monarch {
    public class data_set {
        public string       table;
        public string       folder;
        public string       file;
        public string       select;
        public string       delete;
        public monarch.generic rows;
        public string[,]    param;
        public string       connection_string;
        public data_set(string file,string folder,string connection_string,string[,] param) {
            this.file               =file;
            this.folder             =folder;
            this.connection_string  =connection_string;
            this.param              =param;
            string data=File.ReadAllText(file);
            string[] tokens=data.Split(';');
            this.delete=tokens[0];
            this.select=tokens[1];
            string select_tolower=select.ToLower();
            int from_start=select_tolower.IndexOf("from")+5;
            string temp1=select_tolower.Substring(from_start);
            temp1=temp1.Trim();
            int end_of_word=temp1.IndexOfAny(new char[] {' ',')','(','\n','\t','\r'});
            if(end_of_word<0) end_of_word=temp1.Length;
            table=temp1.Substring(0,end_of_word);
        }

        public void load() {
            if(String.IsNullOrWhiteSpace(connection_string)) {
                connection_string="";
                rows=new monarch.generic(connection_string,table,select,delete,param);
            }
        }

    }

    public class data_migrator {
        public List<data_set> data=new List<data_set>();

        public data_migrator(string dirToSearch,string connection_string,string[,] param) {
            string[] files = Directory.GetFiles(dirToSearch, "*.sql", SearchOption.AllDirectories);
            var sorted_data_sets= files.OrderBy(r => Path.GetDirectoryName(r)).
            ThenBy(p => p.Count(c => c == Path.PathSeparator)).
            ThenBy(s => Path.GetFileNameWithoutExtension(s));
            //Select(t => t.Replace(dirToSearch, ""));

            foreach(var file in sorted_data_sets) {
                string dir=Path.GetDirectoryName(file);
                dir=dir.Replace(dirToSearch,"");
                if(dir[0]=='\\') dir=dir.Substring(1);
                data.Add(new data_set(file,dir,connection_string,param));
            }
        }
        public void load () {
            foreach(data_set d in data) {
                d.load();
            }
        }
        public string generate_scripts() {
            StringBuilder o=new StringBuilder();
            foreach (data_set d in data) {
                o.Append(d.rows.generate_identity_insert());
            }
            return o.ToString();
        }
    }
}
