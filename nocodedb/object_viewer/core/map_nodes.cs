using monarch.controls.data_view;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monarch.models;
using System.Drawing;

namespace monarch.logic {
    public class map_nodes {
        public class map_node {
            public int line         = 0;
            public int position     = 0;
            public int column       = 0;
            public int column_count = 0;
            public models.table t { get; set; }
            public map_node(models.table t,int depth,int column,int column_count) {
                this.t=t;
                this.line=depth;
                this.column=column;
                this.column_count=column_count;
            }
        }
        public models.project_settings          project     { get; set; }
        public BindingList<map_node>            nodes       = new BindingList<map_node>();
        public BindingList<models.table>        t_list      = new BindingList<models.table>();
        public map_nodes() {

        }

        public int max_lines   { get {  int max_lines=-1; foreach(map_node n in nodes) { if(n.line>max_lines) max_lines=n.line; }  return max_lines+1;  }  } 
        public int max_columns { get { int[] line_count=new int [max_lines]; foreach(map_node n in nodes) { line_count[n.line]++; }  return line_count.Max();  }  } 
        public int update_positions() {
            int height=300+10;
            int column_width=250+10;
            int group_width=max_columns*column_width;
            int len=max_lines,index=0;

            for(int line=0;line<len;line++) {
                int pos=0;
                foreach(map_node n in nodes) {
                    if(n.line==line) {
                        n.t.Location=new Point(pos*column_width,line*height);
                        pos++;
                    }
                }

            }
            

            return 0;
        }

        

        public void get_base_objects() {
            if (null == project) return;
            foreach (database d in project.data_set) {
                foreach (models.table t in d.tables) {
                    if (t.has_fk || t.fk_children_count != 0) t_list.Add(t);         //all tables with foreign keys
                }
            }
            int index = 0;

            foreach (models.table t in t_list) {
                if (null == t.group) {
                    string group = index.ToString();
                    traverse_group(t, group);
                    build_map(t);
                    index++;
                }
            }
        }

        //This recursivly maps the entire jobject for ALL connected objects. MArks each by the group that connects them.
        public void traverse_group(models.table t, string group) {
            t.group = group;
            foreach (column c in t.columns) {
                if (c.has_fk) {
                    models.table fk_t = project.get_table_fk_parent(c);
                    if (fk_t.group == null && null != fk_t) traverse_group(fk_t, group);
                }
                if (c.fk_children_count > 0) {
                    foreach (fk child in c.fk_children) {
                        models.table c_t = project.get_table(child.db, child.table);
                        if (c_t.group == null && null != c_t) traverse_group(c_t, group);
                    }
                }
            }
        }

        //this builds a map of all  objects that are grouped, 
        public void build_map(models.table t, int depth = 0, int column= 0,int column_count=1) {
            nodes.Add(new map_node(t,depth,column,column_count));
            depth++;
            int child_column_count = t.fk_link_count;
            t.flag1 = true;
            foreach (column c in t.columns) {
                if (c.has_fk) {
                    models.table fk_t = project.get_table_fk_parent(c);
                    if (!fk_t.flag1 && null != fk_t) {
                        build_map(fk_t,depth, column++,child_column_count );
                    }

                }
                if (c.fk_children_count > 0) {
                    foreach (fk child in c.fk_children) {
                        models.table c_t = project.get_table(child.db, child.table);
                        if (!c_t.flag1 && null != c_t) {
                            build_map(c_t, depth, column++,child_column_count );
                        }
                    }
                }
            }
        }
    }
}
