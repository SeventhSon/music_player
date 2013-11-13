using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.Model
{

    public class DirectoryModel
    {
        private string _path;
        private long _lastWrite;
        public int Id { get; set; }
        public bool NoRemove { get; set; }

        /// <summary>
        /// Gets and sets directory path
        /// </summary>
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

        /// <summary>
        /// Gets and sets directory name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets directory last write date
        /// </summary>
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
        
        /// <summary>
        /// Gets and sets directory last write string
        /// </summary>
        public string LastWriteString { get; set; }
    }

}
