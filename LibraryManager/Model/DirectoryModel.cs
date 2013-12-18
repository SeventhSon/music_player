using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager.Model
{
    public class DirectoryModel
    {
        private string _path;
        private long _lastWrite;
        public int Id { get; set; }
        public bool NoRemove { get; set; }
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                Name = Path.Split('\\').Last();
            }
        }
        public string Name { get; set; }
        public long LastWrite
        {
            get
            {
                return _lastWrite;
            }
            set
            {
                _lastWrite = value;
                LastWriteString = new DateTime(LastWrite).ToString();
            }
        }
        public string LastWriteString { get; set; }
    }

}
