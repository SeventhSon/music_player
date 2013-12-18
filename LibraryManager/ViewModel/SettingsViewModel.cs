using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LibraryManager.Model;
using System.Collections.Generic;

namespace LibraryManager.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    /// 
    public class SettingsViewModel : ViewModelBase 
    {
        private RelayCommand _addCommand;
        private RelayCommand _saveChangesCommand;
        private List<DirectoryModel> _directories;
        public SettingsViewModel()
        {
            Messenger.Default.Register<List<DirectoryModel>>
            (
                 this,
                 (action) => ReceiveMessage(action)
            );
            //Request directories
        }
        /// <summary>
        /// Opens directory browser dialog and scans recursively selected directory
        /// </summary>
        private void AddFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Request directory scan
            }
        }
        public RelayCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(AddFolder);
                }

                return _addCommand;
            }
        }
        public RelayCommand SaveChangesCommand
        {
            get
            {
                if (_saveChangesCommand == null)
                {
                    _saveChangesCommand = new RelayCommand(SaveDirectories);
                }

                return _saveChangesCommand;
            }
        }

        private void SaveDirectories()
        {
            DBManager dbm = DBManager.Instance;
            foreach(DirectoryModel dm in Directories)
            {
                if(dm.NoRemove==false)
                {
                    dbm.ExecuteNonQuery("Delete from songs where id_directory=" + dm.Id);
                    dbm.ExecuteNonQuery("Delete from directories where id="+dm.Id);
                }
            }
            //TODO request directories
        }
        public List<DirectoryModel> Directories
        {
            get
            {
                return _directories;
            }
            set
            {
                _directories = value;
                RaisePropertyChanged("Directories");
            }
        }
        private void ReceiveMessage(List<DirectoryModel> packet)
        {
            Directories = packet;
        }
    }
}