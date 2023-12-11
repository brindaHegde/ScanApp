using Microsoft.Win32;
using ScanApp.Common;
using System.Collections.ObjectModel;
using System.Windows;

namespace ScanApp.SendTo
{
    public class SendToViewModel : ViewModelBase
    {
        #region properties
        public Window CurrentWindow { get; set; }

        private ObservableCollection<SendToData> _sendToCollection;
        public ObservableCollection<SendToData> SendToCollection
        {
            get { return _sendToCollection; }
            set { SetProperty(ref _sendToCollection, value); }
        }

        #endregion

        #region commands
        public RelayCommand CloseButton { get; set; }
        private void ExecuteCloseCommand()
        {
            CurrentWindow.Close();
        }        
        #endregion

        #region Constructor
        public SendToViewModel(Window window)
        {
            CloseButton = new RelayCommand(ExecuteCloseCommand);

            CurrentWindow = window;

            PopulateSendToCollection();
            
        }
        #endregion

        #region Private methods
        private void PopulateSendToCollection()
        {
            SendToCollection = new ObservableCollection<SendToData>();
            SendToCollection.Add(new SendToData() { IsSendToChecked = false, SendToContent = "Save To Folder:", ButtonContent = "Browse" });
            SendToCollection.Add(new SendToData() { IsSendToChecked = false, SendToContent = "Email:", ButtonContent = "Setup" });
            SendToCollection.Add(new SendToData() { IsSendToChecked = false, SendToContent = "Print:", ButtonContent = "Setup" });
            SendToCollection.Add(new SendToData() { IsSendToChecked = false, SendToContent = "SharePoint:", ButtonContent = "Setup" });
            SendToCollection.Add(new SendToData() { IsSendToChecked = false, SendToContent = "DropBox", ButtonContent = "Setup" });
        }
        #endregion
    }
}
