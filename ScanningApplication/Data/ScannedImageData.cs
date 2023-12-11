using ScanApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanningApplication.Data
{
    public class ScannedImageData : ViewModelBase
    {
        #region Properties
        private string _documentName;
        public string DocumentName
        {
            get { return _documentName; }
            set { SetProperty(ref _documentName, value); }
        }

        private string _createdTime;
        public string CreatedTime
        {
            get { return _createdTime; }
            set { SetProperty(ref _createdTime, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _scanDocPath;
        public string ScanDocPath
        {
            get { return _scanDocPath; }
            set { SetProperty(ref _scanDocPath, value); }
        }

        #endregion

    }
}
