using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.Model;
using System.Collections.Generic;

namespace Music_Player.ViewModel
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
        private List<DirectoryModel> _directories;
        public SettingsViewModel()
        {
            Messenger.Default.Register<List<DirectoryModel>>
            (
                 this,
                 (action) => ReceiveMessage(action)
            );
            MusicPlayer.Instance.BroadcastDirectories();
        }
        /// <summary>
        /// Opens directory browser dialog and scans recursively selected directory
        /// </summary>
        private void AddFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MusicPlayer.Instance.ScanDirectory(dialog.SelectedPath);
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