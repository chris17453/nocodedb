using System;
using System.Web.Http;
using System.Net.Http;
using ui.webAPI.models;

using nocodedb.models;
namespace ui{
    [RoutePrefix("api")]    
    public class webInterfaceController:ApiController{
        public string sources_file_location="./settings/recent_sources";
        public string project_file_location="./packages/";
        public webInterfaceController(){
            
        }

        [Route("heartbeat")]
        [HttpPost]
        public string heartbeat() {
            return DateTime.UtcNow.ToString();
        }

        [Route("get_data_sources")]
        [HttpPost]
        public recent_sources get_data_sources() {
            recent_sources sources=JsonFile<recent_sources>.Load(sources_file_location);
            return sources;
        }


        [Route("add_data_source")]
        [HttpPost]
        public bool add_data_source(db_settings ds) {
            recent_sources data=JsonFile<recent_sources>.Load(sources_file_location);
            if(null!=ds && ds.validate()) {
                int t=-1;

                for(int i=0;i<data.sources.Count;i++) {
                    if(data.sources[i].name==ds.name) t=i;
                }
                if(-1!=t) {
                    data.sources.RemoveAt(t);                                   //old entry.. remove it
                } 
                data.sources.Add(ds);                                           //its anew entry
                data.Save(sources_file_location);
                return true;
            }
            return false;
        }

        [Route("remove_data_source")]
        [HttpPost]
        public recent_sources remove_data_source(db_settings ds) {
            recent_sources data=JsonFile<recent_sources>.Load(sources_file_location);
            if(null!=ds) {
                int t=-1;

                for(int i=0;i<data.sources.Count;i++) {
                    if(data.sources[i].name==ds.name) t=i;
                }
                if(-1!=t) {
                    data.sources.RemoveAt(t);
                    data.Save(sources_file_location);
                }
            }
            return data;
        }


        [Route("load_package")]
        [HttpPost]
        public project_settings load_package(string project_name) {
            project_settings data=JsonFile<project_settings>.Load(project_file_location+project_name+".package");
            if(data==null) return null;
            return data;
        }
        [Route("new_package")]
        [HttpPost]
        public bool new_package(project_settings project) {
            if(String.IsNullOrWhiteSpace(project.name)) return false;
            try{
                project.Save(project_file_location+project.name+".package");

                return true;
            } catch(Exception ex) {
                return false;
            }
        }


        [Route("get_dir")]
        [HttpPost]
        public directory get_dir(dir_path path) {
            directory d=new directory();

            try{
                d.get(path.path);
            } catch(Exception ex) {
                return d;
            }
            return d;
        }



    }
}
