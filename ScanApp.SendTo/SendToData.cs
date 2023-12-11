using Microsoft.Win32;
using ScanApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanApp.SendTo
{
    public class SendToData : ViewModelBase
    {
        private bool _issSendToChecked;
        public bool IsSendToChecked
        {
            get { return _issSendToChecked; }
            set { SetProperty(ref _issSendToChecked, value); }
        }

        private string _sendToContent;
        public string SendToContent
        {
            get { return _sendToContent; }
            set { SetProperty(ref _sendToContent, value); }
        }

        private string _buttonContent;
        public string ButtonContent
        {
            get { return _buttonContent; }
            set { SetProperty(ref _buttonContent, value); }
        }

        #region Commands
        public RelayCommand<string> SendToCommand => new RelayCommand<string>(ExecuteSendToCommand);
        private void ExecuteSendToCommand(string obj)
        {
            if (obj == "Save To Folder:")
                ExecuteBrowseCommand();
            if (obj == "Email:")
                ExecuteEmailCommand();
            if (obj == "Print:")
                ExecutePrinterCommand();
            if (obj == "SharePoint:")
                ExecuteSPCommand();
            if (obj == "DropBox:")
                ExecuteDropBoxCommand();
        }
        private void ExecuteBrowseCommand()
        {
            string outputPath = string.Empty;
            OpenFolderDialog folderDialog = new OpenFolderDialog();
            folderDialog.Title = "ScanApplication Select Output Folder Path";
            if (folderDialog.ShowDialog() == true)
            {
                outputPath = folderDialog.FolderName;
                GenericMediator<string>.Notify("SaveToFolder", outputPath);
            }
        }
        private void ExecuteEmailCommand()
        {
            throw new NotImplementedException();
        }
        private void ExecutePrinterCommand()
        {
            throw new NotImplementedException();
        }

        private void ExecuteSPCommand()
        {
            throw new NotImplementedException();
        }

        private void ExecuteDropBoxCommand()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
