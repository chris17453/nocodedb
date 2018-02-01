using System;
namespace nocodedb.data.models{
    public class column{
        public column_meta meta{ get; set; }
        public column_data data{ get; set; }
        public column(){
            data=new column_data();
            meta=new column_meta();
        }
    }
}
