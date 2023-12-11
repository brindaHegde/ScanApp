using ScanApp.Common;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using Microsoft.Win32;
using ScanningApplication.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Microsoft.VisualBasic;
using static ScanningApplication.RectangleViewModel;

namespace ScanningApplication
{
    public class ScanViewModel : ViewModelBase
    {
        #region Properties
        private String _imgName;
        public String ImgName
        {
            get { return _imgName; }
            set { SetProperty(ref _imgName, value); }
        }

        private String _imgSize;
        public String ImgSize
        {
            get { return _imgSize; }
            set { SetProperty(ref _imgSize, value); }
        }

        private String _imgWidth;
        public String ImgWidth
        {
            get { return _imgWidth; }
            set { SetProperty(ref _imgWidth, value); }
        }

        private String _imgHieght;
        public String ImgHieght
        {
            get { return _imgHieght; }
            set { SetProperty(ref _imgHieght, value); }
        }

        private String _imgResolution;
        public String ImgResolution
        {
            get { return _imgResolution; }
            set { SetProperty(ref _imgResolution, value); }
        }

        private String _imgLoc;
        public String ImgLoc
        {
            get { return _imgLoc; }
            set { SetProperty(ref _imgLoc, value); }
        }

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }

        private int _rotateAngle;
        public int RotateAngle
        {
            get { return _rotateAngle; }
            set { SetProperty(ref _rotateAngle, value); }
        }

        private bool _rotate90Checked;
        public bool Rotate90Checked
        {
            get { return _rotate90Checked; }
            set { 
                SetProperty(ref _rotate90Checked, value);
                if (value)
                {
                    RotateAngle = 90;
                }
                IsSaved = false;
            }
        }

        private bool _rotate180Checked;
        public bool Rotate180Checked
        {
            get { return _rotate180Checked; }
            set { 
                SetProperty(ref _rotate180Checked, value);
                if (value)
                {
                    RotateAngle = 180;
                }
                IsSaved = false;
            }
        }

        private bool _rotate270Checked;
        public bool Rotate270Checked
        {
            get { return _rotate270Checked; }
            set { 
                SetProperty(ref _rotate270Checked, value);
                if (value)
                {
                    RotateAngle = 270;
                }
                IsSaved = false;
            }
        }

        private bool _rotate0Checked;
        public bool Rotate0Checked
        {
            get { return _rotate0Checked; }
            set { 
                SetProperty(ref _rotate0Checked, value);
                if (value)
                {
                    RotateAngle = 0;
                }
            }
        }

        public List<string> ImagesList { get; set; }
        public int CurrentImgIndex { get; set; }
        public bool IsSaved { get; set; }

        #region draw zones
        private string _PreviewImageAreaCursor;
        public string PreviewImageAreaCursor
        {
            get { return _PreviewImageAreaCursor; }
            set { SetProperty(ref _PreviewImageAreaCursor, value); }
        }
        private bool _ZoomInSet;
        public bool ZoomInSet
        {
            get { return _ZoomInSet; }
            set { SetProperty(ref _ZoomInSet, value); }
        }

        private bool _ZoomOutSet;
        public bool ZoomOutSet
        {
            get { return _ZoomOutSet; }
            set { SetProperty(ref _ZoomOutSet, value); }
        }

        private bool _FittoWindowSet;
        public bool FittoWindowSet
        {
            get { return _FittoWindowSet; }
            set { SetProperty(ref _FittoWindowSet, value); }
        }
        private bool _FitImageFullSet;
        public bool FitImageFullSet
        {
            get { return _FitImageFullSet; }
            set { SetProperty(ref _FitImageFullSet, value); }
        }

        private bool _drawRegionChecked;
        public bool DrawRegionChecked
        {
            get { return _drawRegionChecked; }
            set { SetProperty(ref _drawRegionChecked, value); }
        }
        public double MouseZoomFactor { get; set; } = 0;
        private double zoomFactor = 0;
        public double ZoomFactor
        {
            get { return zoomFactor; }
            set
            {
                zoomFactor = value;
                if (zoomFactor != 0)
                    ZoomFactorText = (int)(zoomFactor * 100) + "%";
            }
        }
        private double zoomFactorUI = 0;
        public double ZoomFactorUI
        {
            get { return zoomFactorUI; }
            set
            {
                zoomFactorUI = value;
                if (zoomFactorUI != 0)
                    ZoomFactorText = (int)(zoomFactorUI * 100) + "%";
            }
        }
        private string zoomFactorText = string.Empty;
        public string ZoomFactorText
        {
            get { return zoomFactorText; }
            set { SetProperty(ref zoomFactorText, value); }
        }

        private RectangleViewModel dragRectangle = new RectangleViewModel(0, 0, 0, 0, Colors.Transparent, 0);
        public RectangleViewModel DragRectangle
        {
            get { return dragRectangle; }
            set { SetProperty(ref dragRectangle, value); }
        }

        private RectangleViewModel selectedRectangle = null;
        public RectangleViewModel SelectedRectangle
        {
            get { return selectedRectangle; }
            set { SetProperty(ref selectedRectangle, value); }
        }

        private RectangleViewModel _prevselectedRectangle = null;
        public RectangleViewModel PrevSelectedRectangle
        {
            get { return _prevselectedRectangle; }
            set { SetProperty(ref _prevselectedRectangle, value); }
        }

        private Visibility _dragSelectionCanvasVisibility;

        public Visibility DragSelectionCanvasVisibility
        {
            get { return _dragSelectionCanvasVisibility; }
            set { SetProperty(ref _dragSelectionCanvasVisibility, value); }
        }

        private Cursor _previewImageCursor;
        public Cursor PreviewImageCursor
        {
            get { return _previewImageCursor; }
            set { SetProperty(ref _previewImageCursor, value); }
        }



        public static IDictionary<string, string> JobImagesList = new Dictionary<string, string>();

        private ObservableCollection<RectangleViewModel> _rectangles = new ObservableCollection<RectangleViewModel>();
        public ObservableCollection<RectangleViewModel> Rectangles
        {
            get { return _rectangles; }
            set
            {
                SetProperty(ref _rectangles, value);
            }
        }

        public enZoneType OperationType { get; set; } = enZoneType.None;
        public enImageOperationType ImageOperationType { get; set; } = enImageOperationType.None;

        // this is updated when ImageSrc is updated
        // this holds the current image path
        public string Image { get; set; }
        #endregion
        #endregion

        #region Commands
        /// <summary>
        /// triggered when back button is clicked
        /// </summary>
        public RelayCommand BackImgCommand { get; set; }
        private bool CanExecuteBackImgCommand()
        {
            if (CurrentImgIndex == 0)
                return false;
            return true;
        }
        private void ExecuteBackImgCommand()
        {
            CurrentImgIndex--;
            if (CurrentImgIndex >= 0)
                Image = ImagesList[CurrentImgIndex];
            NextImgCommand.RaiseCanExecuteChanged();
            BackImgCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand NextImgCommand { get; set; }
        public bool IsDataLoaded { get; internal set; }

        private bool CanExecuteNextImgCommand()
        {
            if (CurrentImgIndex >= (ImagesList.Count - 1))
                return false;
            return true;
        }

        private void ExecuteNextImgCommand()
        {
            CurrentImgIndex++;
            if (CurrentImgIndex <= (ImagesList.Count - 1))
                Image = ImagesList[CurrentImgIndex];
            NextImgCommand.RaiseCanExecuteChanged();
            BackImgCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand MagnifierButtonClick => new RelayCommand(OnMagnifierButtonClick);
        private void OnMagnifierButtonClick()
        {
            DrawRegionChecked = false;
            OperationType = enZoneType.Magnify;
            //set the cursor
            PreviewImageAreaCursor = MainWindowData.MagnifyCur;

            //Uncheck other options
            ZoomInSet = false;
            ZoomOutSet = false;
            FittoWindowSet = false;
            FitImageFullSet = false;
        }

        public RelayCommand PanButtonClick => new RelayCommand(OnPanButtonClick);
        private void OnPanButtonClick()
        {
            DrawRegionChecked = false;
            OperationType = enZoneType.Pan;
            //set the cursor
            PreviewImageAreaCursor = MainWindowData.PanCur;

            //Uncheck other options
            ZoomInSet = false;
            ZoomOutSet = false;
            FittoWindowSet = false;
            FitImageFullSet = false;
        }

        public RelayCommand DrawRegionClickCommand => new RelayCommand(OnDrawRegionCommand);
        private void OnDrawRegionCommand()
        {
            if (DrawRegionChecked)
            {
                //set the cursor
                PreviewImageAreaCursor = MainWindowData.DragAndDropOCRCur;

                //Uncheck other options
                ZoomInSet = false;
                ZoomOutSet = false;
                FittoWindowSet = false;
                FitImageFullSet = false;
            }
        }
        #endregion

        public ScanViewModel()
        {            
            ImagesList = new List<string>();

            BackImgCommand = new RelayCommand(ExecuteBackImgCommand, CanExecuteBackImgCommand);
            NextImgCommand = new RelayCommand(ExecuteNextImgCommand, CanExecuteNextImgCommand);

            GetImageList();
        }

        #region Private methods
        private void GetImageList()
        {
            if (string.IsNullOrEmpty(MainWindowData.ScannedImagePath))
                return;

            string strDocFile = MainWindowData.DocIDInfoPath;
            string strdocID = string.Empty;
            int docID = 0;
            if (!File.Exists(strDocFile))
                File.Create(strDocFile).Close();
            else
            {
                strdocID = File.ReadAllText(strDocFile);
                docID = Int32.Parse(strdocID);                
            }
            docID += 1;

            string targetFileName = docID + "_" + MainWindowData.UserName;
            string targetFolderName = Path.Combine(MainWindowData.ScannedImagePath, targetFileName);
            if (!Directory.Exists(targetFolderName))
                Directory.CreateDirectory(targetFolderName);

            // load image from sample path
            try
            {
                ImagesList.Clear();
                OpenFileDialog folderDialog = new OpenFileDialog();
                folderDialog.Title = "ScanApplication Select Files to Scan";
                folderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                folderDialog.Multiselect = true;
                if (folderDialog.ShowDialog() != true)
                    return;

                foreach (var item in folderDialog.FileNames)
                {
                    string strFileName = item.Trim('\\').Split('\\').LastOrDefault();
                    string targetPath = Path.Combine(targetFolderName, strFileName);
                    File.Copy(item, targetPath, true);

                    ImagesList.Add(targetPath);
                }
            }
            catch (Exception e)
            {

            }
            File.Delete(strDocFile);
            File.Create(strDocFile).Close();
            File.WriteAllText(strDocFile, docID.ToString());

            CurrentImgIndex = 0;

            if (ImagesList.Count <= 0)
                return;

            // load first image            
            LoadBitmap(CurrentImgIndex);

            NextImgCommand.RaiseCanExecuteChanged();
            BackImgCommand.RaiseCanExecuteChanged();
        }
        private void LoadBitmap(int index)
        {
            using (Stream stream = File.OpenRead(ImagesList[CurrentImgIndex]))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                ImageSource = bitmap;
                Image = ImagesList[CurrentImgIndex];
                GetImageProperties();
            }

            DrawRegionChecked = true;
            //set the cursor
            PreviewImageAreaCursor = MainWindowData.DragAndDropOCRCur;
            OperationType = enZoneType.DrawZone;
        }
        private void GetImageProperties()
        {
            System.Drawing.Image tempImg = new Bitmap(ImagesList[CurrentImgIndex]);
            ImgName = ImagesList[CurrentImgIndex].Trim('\\').Split('\\').LastOrDefault();
            ImgWidth = tempImg.Width.ToString();
            ImgHieght = tempImg.Height.ToString();
            ImgResolution = "X:" + tempImg.HorizontalResolution + " Y:" + tempImg.VerticalResolution;
            long imgSize = (tempImg.Width * tempImg.Height);
            ImgSize = imgSize.ToString();
            ImgLoc = Path.GetDirectoryName(ImagesList[CurrentImgIndex]);
        }
        #endregion
    }
}
