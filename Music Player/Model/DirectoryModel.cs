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
        public long LastWrite { get; set; }
    }

}
