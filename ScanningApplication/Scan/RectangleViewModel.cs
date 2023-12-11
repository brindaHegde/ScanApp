using ScanApp.Common;
using System.Windows.Media;

namespace ScanningApplication
{
    public class RectangleViewModel : ViewModelBase
    {
        #region Data Members

        /// <summary>
        /// The id of the zone.
        /// </summary>
        private int id = 0;

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double x = 0.0;

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double y = 0.0;

        /// <summary>
        /// The width of the rectangle (in content coordinates).
        /// </summary>
        private double width = 0.0;

        /// <summary>
        /// The height of the rectangle (in content coordinates).
        /// </summary>
        private double height = 0.0;

        /// <summary>
        /// The color of the rectangle.
        /// </summary>
        private Color color;

        /// <summary>
        /// The opacity of the rectangle.
        /// </summary>
        private double opacity;

        /// <summary>
        /// The StrokeThickness of the rectangle.
        /// </summary>
        private double strokeThickness = 1;

        /// <summary>
        /// The StrokeDashArray of the rectangle.
        /// </summary>
        private DoubleCollection strokeDash = new DoubleCollection { 1, 1 };


        /// <summary>
        /// The name of the zone.
        /// </summary>
        private string name = "";

        #endregion Data Members

        public RectangleViewModel()
        {
        }

        public RectangleViewModel(double x, double y, double width, double height, Color color, double opacity, string file = "")
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
            this.opacity = opacity;

            this.name = name;
            SampleFile = file;
            //this.OCRs = new OCR();
            //this.OMRs = new OMR();
            //this.Barcodes = new Barcode();

        }

        public void Reset()
        {
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;
            this.color = Colors.Transparent;
            this.opacity = 0;
        }

        /// <summary>
        /// The id of the rectangle.
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                SetProperty(ref id, value);
            }
        }

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                SetProperty(ref x, value);
            }
        }

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                SetProperty(ref y, value);
            }
        }

        /// <summary>
        /// The width of the rectangle (in content coordinates).
        /// </summary>
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                SetProperty(ref width, value);
            }
        }

        /// <summary>
        /// The height of the rectangle (in content coordinates).
        /// </summary>
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                SetProperty(ref height, value);
            }
        }

        /// <summary>
        /// The name of the zone.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }

        public int job_id { get; set; }
        //public Barcode Barcodes { get; set; }
        //public OCR OCRs { get; set; }
        //public OMR OMRs { get; set; }
        public bool IsSelected { get; set; }

        /// <summary>
        /// The Dpi X of the rectangle.
        /// </summary>
        public double DpiX { get; } = 96;

        /// <summary>
        /// The Dpi Y of the rectangle.
        /// </summary>
        public double DpiY { get; } = 96;

        /// <summary>
        /// The color of the item.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                SetProperty(ref color, value);
            }
        }

        /// <summary>
        /// The Opacity of the rectangle.
        /// </summary>
        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                SetProperty(ref opacity, value);
            }
        }

        /// <summary>
        /// The StrokeThickness of the rectangle.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return strokeThickness;
            }
            set
            {
                SetProperty(ref strokeThickness, value);
            }
        }

        /// <summary>
        /// The StrokeDash of the rectangle.
        /// </summary>
        public DoubleCollection StrokeDash
        {
            get
            {
                return strokeDash;
            }
            set
            {
                SetProperty(ref strokeDash, value);
            }
        }

        /// <summary>
        /// The Zone sample image file path and name.
        /// </summary>
        public string SampleFile { get; set; }


        private double scaleX = 1;
        public double ScaleX
        {
            get
            {
                return scaleX;
            }
            set
            {
                SetProperty(ref scaleX, value);
            }
        }

        private double scaleY = 1;
        public double ScaleY
        {
            get
            {
                return scaleY;
            }
            set
            {
                SetProperty(ref scaleY, value);
            }
        }

        private double translateX = 0;
        public double TranslateX
        {
            get
            {
                return translateX;
            }
            set
            {
                SetProperty(ref translateX, value);
            }
        }

        private double translateY = 0;
        public double TranslateY
        {
            get
            {
                return translateY;
            }
            set
            {
                SetProperty(ref translateY, value);
            }
        }
    }
}
