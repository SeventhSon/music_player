using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
namespace Music_Player.ViewModel
{
    public class NavigationItemModel
    {
        public ViewModelBase ViewModel { get; set; }
        public string Title { get; set; }

        public NavigationItemModel(ViewModelBase vm,string title)
        {
            ViewModel = vm;
            Title = title;
        }
    }
}
