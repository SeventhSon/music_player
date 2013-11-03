using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Music_Player.ViewModel;
using System.Data;

namespace Music_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();      
            //Console.WriteLine(dt.DataSet.ToString());
            ThreadStart tester = TestMVVM;
            new Thread(tester).Start();
        }
        private void TestMVVM()
        {
            MainViewModel mvm = (new ViewModelLocator()).Main;
            DBManager dbm = DBManager.Instance;
            int rowsAffected = dbm.executeNonQuery("Insert or ignore into songs (title, album, path,id_directory) values ('GhostWriter','RJD2','Path1',1),('GhostWriter','RJD2','Path2',1),('GhostWriter','RJD2','Path3',2),('GhostWriter','RJD2','Path4',3)");
            DataTable dt = dbm.executeQuery("SELECT * FROM songs");
            mvm.Results = dt.AsDataView();
            mvm.Volume = mvm.Volume-1;
        }
    }
}
