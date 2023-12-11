using ScanApp.Common;
using ScanApp.SendTo;
using ScanningApplication.Data;
using System.Collections.ObjectModel;
using System.IO;

namespace ScanningApplication
{
    public class ScannedDocViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<ScannedImageData> _scannedDocuments;
        public ObservableCollection<ScannedImageData> ScannedDocuments
        {
            get { return _scannedDocuments; }
            set { SetProperty(ref _scannedDocuments, value); }
        }

        private ScannedImageData _selctedScannedDoc;
        public ScannedImageData SelctedScannedDoc
        {
            get { return _selctedScannedDoc; }
            set { 
                SetProperty(ref _selctedScannedDoc, value);
                DeleteCommand.RaiseCanExecuteChanged();
                SendToCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand SendToCommand { get; set; }
        private void ExecuteSendToCommand()
        {
            SendToView sendToView = new SendToView();
            sendToView.Owner = MainWindowData.CurrentWindow;
            sendToView.ShowDialog();
        }
        private bool CanExecuteSendToCommand()
        {
            if (SelctedScannedDoc == null)
                return false;
            return true;
        }

        public RelayCommand DeleteCommand { get; set; }
        private void ExecuteDeleteCommand()
        {
            if (SelctedScannedDoc == null)
                return;            

            if (!string.IsNullOrEmpty(SelctedScannedDoc.ScanDocPath))
            {
                foreach(var fileItem in Directory.GetFiles(SelctedScannedDoc.ScanDocPath))
                {
                    File.Delete(fileItem);
                }
                Directory.Delete(SelctedScannedDoc.ScanDocPath);
            }
            ScannedDocuments.Remove(SelctedScannedDoc);

            DeleteCommand.RaiseCanExecuteChanged();
        }
        private bool CanExecuteDeleteCommand()
        {
            if (SelctedScannedDoc == null)
                return false;
            return true;
        }
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public ScannedDocViewModel()
        {
            GenericMediator<string>.Register("SaveToFolder", ExecuteSaveToFolder);

            ScannedDocuments = new ObservableCollection<ScannedImageData>();

            SendToCommand = new RelayCommand(ExecuteSendToCommand, CanExecuteSendToCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            PopulateDataGrid();
        }        

        #region private methods
        /// <summary>
        /// Copy to the target folder
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteSaveToFolder(string obj)
        {
            string sourceFolder = SelctedScannedDoc.ScanDocPath;

            string[] allFiles = Directory.GetFiles(sourceFolder);
            foreach(var item in allFiles)
            {
                string targetFile = Path.Combine(obj, item);
                //File.Copy(item, targetFile, true);
            }
        }

        /// <summary>
        /// read all documents and display them on the data grid
        /// </summary>
        private void PopulateDataGrid()
        {
            if (string.IsNullOrEmpty(MainWindowData.ScannedImagePath))
                return;
            
            string[] strGetAllPaths = Directory.GetDirectories(MainWindowData.ScannedImagePath);
            foreach(var item in strGetAllPaths)
            {
                if(string.IsNullOrEmpty(item)) continue;
                ScannedImageData scannedImageData = new ScannedImageData();
                string DocName = item.Trim('\\').Split('\\').LastOrDefault();
                
                scannedImageData.DocumentName = DocName.Split('_').FirstOrDefault();
                scannedImageData.CreatedTime = Directory.GetCreationTime(item).ToString();
                scannedImageData.UserName = DocName.Split('_').LastOrDefault();
                scannedImageData.ScanDocPath = item;

                ScannedDocuments.Add(scannedImageData);
            }
        }
        #endregion
    }
}
