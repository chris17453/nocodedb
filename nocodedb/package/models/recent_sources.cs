using System;
using System.ComponentModel;

namespace nocodedb.models
{
    public class recent_sources:JsonFile<recent_sources>{
        public BindingList<db_settings> sources=new BindingList<db_settings>();
        public recent_sources()
        {
        }
    }
}
