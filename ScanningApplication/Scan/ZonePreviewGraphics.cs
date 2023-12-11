using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ScanningApplication
{
    public enum enZoneType
    {
        None,
        DrawZone,
        Pan,
        Magnify
    }

    public enum enImageOperationType
    {
        None,
        Zoom,
        FitToSize,
        Original
    }
    public class ZonePreviewGraphics : Border
    {
        public static readonly double ZoomStep = 0.05;
        public double WindowWidth = 0;
        public double WindowHeight = 0;
        public bool IsFittoWindowSet = false;
        public double FitToWinScaleFact = 1;

        public ScanViewModel ViewModel { get; set; } = null;

        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != this.Child)
                    this.Initialize(value);
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            this.child = element;
            if (child != null)
            {
                TransformGroup group = new TransformGroup();

                ScaleTransform st = new ScaleTransform();
                group.Children.Add(st);

                TranslateTransform tt = new TranslateTransform();
                group.Children.Add(tt);

                child.RenderTransform = group;

                child.RenderTransformOrigin = new Point(0.0, 0.0);
                this.MouseWheel += child_MouseWheel;
                this.MouseLeftButtonDown += child_MouseLeftButtonDown;
                this.MouseLeftButtonUp += child_MouseLeftButtonUp;
                this.MouseMove += child_MouseMove;
                //this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(child_PreviewMouseRightButtonDown);
            }
        }

        public void Reset()
        {
            if (child != null)
            {
                // reset zoom
                var st = GetScaleTransform(child);
                st.ScaleX = 1.0;
                st.ScaleY = 1.0;

                // reset pan
                var tt = GetTranslateTransform(child);
                tt.X = 0.0;
                tt.Y = 0.0;
            }
        }

        public void DoZoom(double zoom)
        {
            // zoom image
            var st = GetScaleTransform(child);

            bool IsPortraitImage = true;
            //Find whether the image is Portrait or Landscape
            if (ActualHeight < ActualWidth)
                IsPortraitImage = false;

            if (zoom < 0) //Zoom out
            {
                if (!IsFittoWindowSet)
                {
                    if (((st.ScaleX * ActualWidth) >= WindowWidth) || ((st.ScaleY * ActualHeight) >= WindowHeight))
                    {
                        for (double i = -0.01; i >= zoom; i = i - 0.01)
                        {
                            double tmpScaleFact = -0.01;
                            var Scalex = st.ScaleX + tmpScaleFact;
                            var Scaley = st.ScaleY + tmpScaleFact;

                            if (IsPortraitImage)
                            {
                                if (Scaley < FitToWinScaleFact)
                                {
                                    st.ScaleY = FitToWinScaleFact;
                                    st.ScaleX = FitToWinScaleFact;
                                    break;
                                }
                                else
                                    st.ScaleY += tmpScaleFact;

                                if (Scalex < FitToWinScaleFact)
                                {
                                    st.ScaleX = FitToWinScaleFact;
                                    st.ScaleY = FitToWinScaleFact;
                                    break;
                                }
                                else
                                    st.ScaleX += tmpScaleFact;
                            }
                            else
                            {
                                if (Scalex < FitToWinScaleFact)
                                {
                                    st.ScaleX = FitToWinScaleFact;
                                    st.ScaleY = FitToWinScaleFact;
                                    break;
                                }
                                else
                                    st.ScaleX += tmpScaleFact;

                                if (Scaley < FitToWinScaleFact)
                                {
                                    st.ScaleY = FitToWinScaleFact;
                                    st.ScaleX = FitToWinScaleFact;
                                    break;
                                }
                                else
                                    st.ScaleY += tmpScaleFact;
                            }

                            // update zoom percent
                            ViewModel.ZoomFactor += tmpScaleFact;
                            ViewModel.ZoomFactorUI += tmpScaleFact;
                        }
                    }
                }
            }
            else //Zoom in
            {
                st.ScaleX += zoom;
                st.ScaleY += zoom;

                // update zoom percent
                ViewModel.ZoomFactor += zoom;
                ViewModel.ZoomFactorUI += zoom;

                //image is zoomed
                IsFittoWindowSet = false;

            }
        }

        public void FitToWindow(double windowWidth, double windowHeight)
        {
            IsFittoWindowSet = true;
            double scaleFactorWidth = windowWidth / ActualWidth;
            double scaleFactorHeight = windowHeight / ActualHeight;
            double scaleFactor = scaleFactorHeight;
            if (scaleFactorWidth < scaleFactorHeight)
                scaleFactor = scaleFactorWidth;

            FitToWinScaleFact = scaleFactor;

            var st = GetScaleTransform(child);
            st.ScaleX = scaleFactor;
            st.ScaleY = scaleFactor;

            var tt = GetTranslateTransform(child);
            tt.X = 0;
            tt.Y = 0;
        }

        public void FitOriginalToWindow(double ImageWidth, double ImageHeight)
        {
            IsFittoWindowSet = false;
            double scaleFactorWidth = ImageWidth / ActualWidth;
            double scaleFactorHeight = ImageHeight / ActualHeight;
            double scaleFactor = scaleFactorHeight;
            if (scaleFactorWidth < scaleFactorHeight)
                scaleFactor = scaleFactorWidth;

            var st = GetScaleTransform(child);
            st.ScaleX = scaleFactor;
            st.ScaleY = scaleFactor;

            var tt = GetTranslateTransform(child);
            tt.X = 0;
            tt.Y = 0;
        }

        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is TranslateTransform);
        }

        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is ScaleTransform);
        }

        private UIElement child = null;
        private Point origin;
        private Point start;

        #region Child Events

        private void child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((ViewModel != null) && (child != null))
            {
                if (ViewModel.ImageOperationType == enImageOperationType.Zoom)
                {
                    Image image = child as Image;
                    if (image != null)
                    {
                        var position = e.GetPosition(image);
                        image.RenderTransformOrigin = new Point(position.X / image.ActualWidth, position.Y / image.ActualHeight);
                    }

                    double zoom = e.Delta > 0 ? ZoomStep : -ZoomStep;

                    DoZoom(zoom);
                }
            }
        }

        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((ViewModel != null) && (child != null))
            {
                var tt = GetTranslateTransform(child);
                start = e.GetPosition(this);
                origin = new Point(tt.X, tt.Y);

                //if (ViewModel.OperationType == enZoneType.Pan)
                //    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;

                child.CaptureMouse();
            }
        }

        private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((ViewModel != null) && (child != null))
            {
                child.ReleaseMouseCapture();

                // System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
            }
        }

        void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // this.Reset();
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (!child.IsMouseCaptured) return;

            if ((ViewModel != null) && (child != null))
            {
                if (ViewModel.OperationType == enZoneType.Pan)
                {
                    // pan image
                    var tt = GetTranslateTransform(child);
                    var st = GetScaleTransform(child);
                    Vector v = start - e.GetPosition(this);

                    var tx = origin.X - v.X;
                    var ty = origin.Y - v.Y;

                    if (tx < 0 && tx > (WindowWidth - (st.ScaleX * ActualWidth)))
                        tt.X = tx;
                    if (ty < 0 && ty > (WindowHeight - (st.ScaleY * ActualHeight)))
                        tt.Y = ty;
                }
            }
        }


        #endregion
    }
}
