using System;
using System.ComponentModel;
using System.Linq;

namespace nocodedb.models {
    public class MySettings : JsonFile<MySettings> {
        public int last_server= -1;
        public BindingList<db_settings> db_list=new BindingList<db_settings>();
        public BindingList<recent_file> recent_projects=new BindingList<recent_file>();
        public void add_project(string name,string file) {
            var file_in_list=recent_projects.SingleOrDefault(x=>x.file==file);
            if (null==file_in_list) {
                recent_projects.Add(new recent_file(name,file));
            } else {                                                //file exists inlist. update access time and name (it may have changed)
                file_in_list.accessed   =DateTime.Now;          
                file_in_list.name       =name;
            }
            this.recent_projects = new BindingList<recent_file>(this.recent_projects.OrderBy(x => x.accessed).ToList());

        }
    }
}
