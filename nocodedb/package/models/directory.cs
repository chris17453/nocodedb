using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace nocodedb.models
{
    public class dir_path{
        public string path {get; set; }
        public dir_path(){
        }
    }
    public class file_system_item{
        public string type {get; set;}
        public string name {get; set;}
        public string path {get; set;}
        public string size {get; set;}
        public string extension { get; set; }
        public string modified {get; set;}
        public bool   hidden {get; set; }
        public file_system_item(){
        }
    }
    public class directory{
        public string                 path          {get; set; }
        public List<file_system_item> directories   { get; set; }
        public List<file_system_item> files         { get; set; }

        public directory()
        {
        }
        public string get_os(){
            OperatingSystem os = Environment.OSVersion;
            PlatformID     pid = os.Platform;
            switch(pid){
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return "Windows";
                case PlatformID.Unix: return "Linux";
                default             : return "OSX";
            }
        }

        public string get_home_dir(){
            string home_dir=null;
            string os=get_os();

            switch(os){
                case "Linux"    :   home_dir = System.Environment.GetEnvironmentVariable("HOME");
                    break;

                case "Windows"  :   string home_drive;
                    home_drive=System.Environment.GetEnvironmentVariable("HOMEDRIVE");
                    home_dir  = System.Environment.GetEnvironmentVariable("HOMEPATH");
                    home_dir  =home_drive+@"\"+home_dir;
                    break;
            }
            return home_dir;
        }

        public static string relative_time(DateTime t) {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - t.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)return ts.Seconds == 1 ? "1 sec" : ts.Seconds + " sec";

            if (delta < 2 * MINUTE)return "1 minute";

            if (delta < 45 * MINUTE)return ts.Minutes + " minutes";

            if (delta < 90 * MINUTE)return "1 hour";

            if (delta < 24 * HOUR)return ts.Hours + " hours";

            if (delta < 48 * HOUR)return "yesterday";

            if (delta < 30 * DAY) return ts.Days + " days";

            if (delta < 12 * MONTH){
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1 month" : months + " months";
            }else{
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "1 year" : years + " years";
            }
        }

        private static List<file_system_item> get_directories(string path){
            List<file_system_item> t_item=new List<file_system_item>();
            try{
                
                List<string> t_dir=Directory.GetDirectories(path).OrderBy(f => f).ToList();
                foreach(string t in t_dir) {
                    file_system_item tf=new file_system_item();
                    DirectoryInfo dir = new DirectoryInfo(t);
                    if ((dir.Attributes & FileAttributes.Hidden) == (FileAttributes.Hidden)) {  
                        tf.hidden=true;
                    }
                    DateTime dt = Directory.GetLastWriteTime(t);
                    tf.type="folder";
                    tf.path=t;
                    tf.modified=relative_time(dt);
                    string[] tokens=t.Split(new char[] {'/','\\'});
                    tf.name=tokens.Last();
                    t_item.Add(tf);
                }

            }catch (UnauthorizedAccessException){
                
            }
            return t_item;
        }
        public static string get_bytes_readable(long i){
            // Get absolute value
            long absolute_i = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (i >> 50);
            }else if (absolute_i >= 0x4000000000000) // Petabyte
            {suffix = "PB";
                readable = (i >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (i >> 30);
            }else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (i >> 20);
            }else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (i >> 10);
            }else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }else{
                return i.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.# ") + suffix;
        }

        public static string get_file_size(string file){
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
            if (fileInfo.Exists){ 
               return  get_bytes_readable(fileInfo.Length);
            }
            return "-";
        }
        private static List<file_system_item> get_files(string path){
            List<file_system_item> t_item=new List<file_system_item>();
            try{
                var sorted = Directory.GetFiles(".").OrderBy(f => f);
                List<string> t_files=Directory.GetFiles(path,"*",SearchOption.TopDirectoryOnly).OrderBy(f => f).ToList();
                foreach(string t in t_files) {
                    file_system_item tf=new file_system_item();
                    var fInfo = new FileInfo(t);
                    if (fInfo.Attributes.HasFlag(FileAttributes.Hidden)){
                        tf.hidden=true;
                    }
                    DateTime dt = File.GetLastWriteTime(t);

                    tf.type="file";
                    tf.path=t;
                    tf.size=get_file_size(t);
                    tf.modified=relative_time(dt);
                    tf.name=Path.GetFileName(t);
                    tf.extension = Path.GetExtension(t);
                    t_item.Add(tf);
                }

            }catch (UnauthorizedAccessException){

            }
            return t_item;
        }

        public string get_parent_directory(String path){
            System.IO.DirectoryInfo directoryInfo =System.IO.Directory.GetParent(path);
            return directoryInfo.FullName;
        }
        public bool get(string path){
            try{
                if(path==null) {
                    path=get_home_dir();
                    this.path=path;
                }
                if(path.Length>=3 && path.Substring(path.Length-2)=="..") {
                    path=get_parent_directory(path.Substring(0,path.Length-3));
                }
                this.path=path;
                files=get_files(path); 
                directories=get_directories(path);
            }catch(Exception ex) {
                return false;
            }
            return true;
        }
    }
}
