using Microsoft.Win32;
using ScanApp.Common;
using System.IO;
using System.Windows;
using System.Windows.Input;
using static ScanningApplication.MainWindowData;

namespace ScanningApplication
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Properties       

        /// <summary>
        /// load the current user control based on navigation
        /// </summary>
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { SetProperty(ref _currentView, value); }
        }

        // home view
        public HomeViewModel HomeView { get; set; }
        /// <summary>
        /// property to check/uncheck "home" radio button 
        /// </summary>
        private bool _isHomeChecked;
        public bool IsHomeChecked
        {
            get { return _isHomeChecked; }
            set { SetProperty(ref _isHomeChecked, value); }
        }

        // docment view
        public ScanViewModel Scanview { get; set; }
        /// <summary>
        /// property to check/uncheck "document" radio button 
        /// </summary>
        private bool _isScanChecked;
        public bool IsScanChecked
        {
            get { return _isScanChecked; }
            set { SetProperty(ref _isScanChecked, value); }
        }

        //destination view
        public ScannedDocViewModel ScannedDocView { get; set; }
        /// <summary>
        /// property to check/uncheck "destination" radio button 
        /// </summary>
        private bool _isScanDocChecked;
        public bool IsScanDocChecked
        {
            get { return _isScanDocChecked; }
            set { SetProperty(ref _isScanDocChecked, value); }
        }

        #endregion

        #region commands
        public ICommand HomeButton { get; set; }
        public ICommand ScanButton { get; set; }
        public ICommand ScannedDocButton { get; set; }
        /// <summary>
        /// Command for loading user control
        /// </summary>
        /// <param name="obj">the view from selected radio button</param>
        private void ExecuteLoadUCCommand(string obj)
        {
            int UcView = 0;
            if (obj != null)
                UcView = Int16.Parse(obj);

            CurrentView = CommandToCurrentPageConv((CurrentUCView)UcView);
        }

        public RelayCommand CloseButton { get; set; }
        private void ExecuteCloseCommand()
        {
            CurrentWindow.Close();
        }
        #endregion

        #region Constructor
        public MainWindowViewModel(Window window)
        {
            GenericMediator<string>.Register("LoadUC", ExecuteLoadUCCommand);

            HomeButton = new RelayCommand<string>(ExecuteLoadUCCommand);
            ScanButton = new RelayCommand<string>(ExecuteLoadUCCommand);
            ScannedDocButton = new RelayCommand<string>(ExecuteLoadUCCommand);
            CloseButton = new RelayCommand(ExecuteCloseCommand);

            CurrentWindow = window;            

            CurrentUCView UcView = CurrentUCView.Home;
            CurrentView = CommandToCurrentPageConv(UcView);

            // if info file doesnot exist, create and initialize with docID value
            string strDocFile = DocIDInfoPath;
            if (!File.Exists(strDocFile))
            {
                File.Create(strDocFile).Close();
                File.WriteAllText(strDocFile, "0");
            }            
        }
        #endregion

        #region Private helper methods
        private ViewModelBase CommandToCurrentPageConv(CurrentUCView ucView)
        {
            switch (ucView)
            {
                case CurrentUCView.Home:
                    IsHomeChecked = true;
                    if (HomeView == null)
                        return new HomeViewModel();
                    return HomeView;

                case CurrentUCView.Scan:
                    if (!CheckScannedImageLocation())
                        break;
                    if (Scanview == null)
                        return new ScanViewModel();
                    return Scanview;

                case CurrentUCView.ScannedDoc:
                    if (!CheckScannedImageLocation())
                        break;
                    if (ScannedDocView == null)
                        return new ScannedDocViewModel();
                    return ScannedDocView;

            }
            IsHomeChecked = true;
            return new HomeViewModel();
        }
        private bool CheckScannedImageLocation()
        {
            if (string.IsNullOrEmpty(ScannedImagePath))
            {
                MessageBox.Show("Provide scanned image location to proceed", "ScanApp", MessageBoxButton.OK);
                return false;
            }
            return true;
        }
        #endregion
    }
}
