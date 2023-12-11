using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScanningApplication
{
    public static class MainWindowData
    {
        /// <summary>
        /// views for the setup
        /// </summary>
        public enum CurrentUCView
        {
            Home = 1,
            Scan,
            ScannedDoc,
        }

        public static Window? CurrentWindow = null;

        //cursor paths
        public const string DrawRegionCur = "/ScanApp.Common;component/Themes/Cursors/cur_tool_octopod_init-1.cur";
        public const string PanCur = "/ScanApp.Common;component/Themes/Cursors/cur_tool_pan.cur";
        public const string MagnifyCur = "/ScanApp.Common;component/Themes/Cursors/cur_tool_magnifier.cur";
        public const string FlagCur = "/ScanApp.Common;component/Themes/Cursors/cur_flag.cur";
        public const string SplitDocumentEditCur = "/ScanApp.Common;component/Themes/Cursors/cur_tool_splitdoc.cur";
        public const string DragAndDropOCRCur = "/ScanApp.Common;component/Themes/Cursors/cur_tool_octopod_init.cur";
        public const string ZoomZonesCur = "/ScanApp.Common;component/Themes/Cursors/cur_zoom_zones.cur";
        public const string OneClickOCRCur = "/ScanApp.Common;component/Themes/Cursors/cur_tool_octopod_init-3.cur";

        public const string DocIDInfoPath = "DocID.txt";
        public const string ScannedFolderName = "SampleAppScan";

        public static string ScannedImagePath = string.Empty;
        public static string UserName = string.Empty;
    }
}
