using Microsoft.Win32;
using ScanApp.Common;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using static ScanningApplication.MainWindowData;

namespace ScanningApplication
{
    public class HomeViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<string> _scannerList;
        public ObservableCollection<string> ScannerList
        {
            get { return _scannerList; }
            set { SetProperty(ref _scannerList, value); }
        }

        private string _selectedScanner;
        public string SelectedScanner
        {
            get { return _selectedScanner; }
            set { 
                SetProperty(ref _selectedScanner, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _usrName;
        public string UsrName
        {
            get { return _usrName; }
            set { 
                SetProperty(ref _usrName, value);
                IsSaveEnabled = ((UserName != UsrName) && !IsInit);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _scannedImgPath;
        public string ScannedImgPath
        {
            get { return _scannedImgPath; }
            set
            {
                SetProperty(ref _scannedImgPath, value);
                IsSaveEnabled = ((ScannedImagePath != ScannedImgPath) && !IsInit);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isSaveEnabled;
        public bool IsSaveEnabled
        {
            get { return _isSaveEnabled; }
            set { SetProperty(ref _isSaveEnabled, value); }
        }

        public bool IsInit { get; set; }
        #endregion

        #region Commands
        public RelayCommand SaveCommand { get; set; }
        private void ExecuteSaveCommand()
        {
            if (string.IsNullOrEmpty(ScannedImgPath))
                MessageBox.Show("Please provide scanned image location");
            ScannedImagePath = Path.Combine(ScannedImgPath, ScannedFolderName);
            UserName = UsrName;

            IsSaveEnabled = false;
        }

        public RelayCommand BrowseCommand { get; set; }
        private void ExecuteBrowseCommand()
        {
            ScannedImgPath = string.Empty;
            OpenFolderDialog folderDialog = new OpenFolderDialog();
            folderDialog.Title = "ScanApplication Select Scanner Folder Path";
            if (folderDialog.ShowDialog() == true)
            {
                ScannedImgPath = folderDialog.FolderName;
            }            
        }
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HomeViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSaveCommand);
            BrowseCommand = new RelayCommand(ExecuteBrowseCommand);

            // TODO: Add all drivers list
            ScannerList = new ObservableCollection<string>();
            ScannerList.Add("Import Images");

            IsInit = true;
            if (!string.IsNullOrEmpty(ScannedImagePath))
                ScannedImgPath = Directory.GetParent(ScannedImagePath).FullName;
            UsrName = UserName;
            IsInit = false;

            IsSaveEnabled = false;
        }
        
    }
}
