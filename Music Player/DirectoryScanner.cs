using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player
{
    class DirectoryScanner
    {
        private static volatile DirectoryScanner instance;
        private static object monitor = new Object();
        private DirectoryScanner()
        {

        }
        public static DirectoryScanner Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (monitor)
                    {
                        if (instance == null)
                            instance = new DirectoryScanner();
                    }
                }
                return instance;
            }
        }
    }
}
