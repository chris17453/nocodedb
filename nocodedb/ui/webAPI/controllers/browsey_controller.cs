using System;
using System.Web.Http;
using browsey.models;       //browsey
using nocodedb.models;

namespace ui.webAPI.controllers
{
    [RoutePrefix("api")]    
    public class browsey_controller:ApiController{
        [Route("get_dir")]
        [HttpPost]
        public directory get_dir(dir_path path) {
            directory d=new directory();

            try{
                d.get(path);
            } catch {
                return d;
            }
            return d;
        }

    }
}
