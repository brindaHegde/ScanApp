using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using static ScanningApplication.RectangleViewModel;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32.SafeHandles;

namespace ScanningApplication
{
    /// <summary>
    /// Interaction logic for ScanView.xaml
    /// </summary>
    public partial class ScanView : UserControl
    {
        public ScanViewModel ViewModel { get; set; } = null;

        private Thumb RectDragThumb;
        private Thumb RectThumb;
        private bool isMagnifyMouseDown = false;
        private double dpiScaleX = 1;
        private double dpiScaleY = 1;
        public ScanView()
        {
            InitializeComponent();
            view.LayoutUpdated += (o, e) => UCLoaded();
        }

        private void UCLoaded()
        {
            if (ViewModel != null)
            {
                if (ViewModel.IsDataLoaded && (view.ActualHeight > 0 || view.ActualWidth > 0))
                {
                    // You can also unsubscribe event here.
                    //ViewModel.LoadRectanglesonUI(true);

                    PreviewBorderCtrl.FitToWindow(view.ActualWidth, view.ActualHeight);
                    //DI_JOBCFG_AUTOINDEX_VARIABLE_TYPE SelectedItem = ViewModel.FilterZonesForDisplayBasedOnSides(true, true);
                    //if (SelectedItem == null)
                    //    ViewModel.SelectBarOCROMRZone(true, null);
                    //else
                    //    ViewModel.SelectBarOCROMRZone(false, SelectedItem);

                    ViewModel.IsDataLoaded = false;
                    view.LayoutUpdated -= (o, e) => UCLoaded();
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                ViewModel = (ScanViewModel)DataContext;
            PreviewBorderCtrl.ViewModel = ViewModel;
            PreviewBorderCtrl.WindowWidth = view.ActualWidth;
            PreviewBorderCtrl.WindowHeight = view.ActualHeight;
        }

        private void FitToWindowButtonClick(object sender, RoutedEventArgs e)
        {
            PreviewBorderCtrl.FitToWindow(view.ActualWidth, view.ActualHeight);
        }
        private void FitOriginalToWindowButtonClick(object sender, RoutedEventArgs e)
        {
            PreviewBorderCtrl.FitOriginalToWindow(PreviewImage.ActualWidth, PreviewImage.ActualHeight);
        }

        private void ZoomInButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageOperationType = enImageOperationType.Zoom;
            PreviewBorderCtrl.DoZoom(ZonePreviewGraphics.ZoomStep);
        }

        private void ZoomOutButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageOperationType = enImageOperationType.Zoom;
            PreviewBorderCtrl.DoZoom(-ZonePreviewGraphics.ZoomStep);
        }

        /// <summary>
        /// DrawRegion Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawRegionClickEvent(object sender, RoutedEventArgs e)
        {
            if (RectThumb != null)
                RectThumb.IsEnabled = true;
            if (RectDragThumb != null)
                RectDragThumb.IsEnabled = true;
        }

        /// <summary>
        /// Handle the user Moving entering the rectangle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Thumb_MouseEnter(object sender, MouseEventArgs e)
        {
            if (RectThumb != null)
                RectThumb.IsEnabled = true;

            if (ViewModel.DrawRegionChecked == false)
            {
                if (e.OriginalSource is Thumb SizeThumb)
                {
                    RectThumb = SizeThumb;
                    RectThumb.IsEnabled = false;
                    SizeThumb.IsEnabled = false;
                }
            }
            else
            {
                if (e.OriginalSource is Thumb SizeThumb)
                    SizeThumb.IsEnabled = true;
            }
        }

        /// <summary>
        ///// Handle the ThumbSizeAll_MouseEnter entering the rectangle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThumbSizeAll_MouseEnter(object sender, MouseEventArgs e)
        {
            if (RectDragThumb != null)
                RectDragThumb.IsEnabled = true;

            if (ViewModel.DrawRegionChecked == false)
            {
                if (e.OriginalSource is Thumb SizeThumb)
                {
                    RectDragThumb = SizeThumb;
                    RectDragThumb.IsEnabled = false;
                    SizeThumb.IsEnabled = false;
                }
            }
            else
            {
                if (e.OriginalSource is Thumb SizeThumb)
                    SizeThumb.IsEnabled = true;
            }
        }

        ///// <summary>
        ///// Handle the user dragging the rectangle.
        ///// </summary>
        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thumb = (Thumb)sender;
            RectangleViewModel myRectangle = (RectangleViewModel)thumb.DataContext;

            this.Cursor = Cursors.SizeAll;
            //if (myRectangle != null)
            //{
            //    ViewModel.SelectZone(myRectangle);

            //    //
            //    // Update the the position of the rectangle in the view-model.
            //    //
            //    myRectangle.X += e.HorizontalChange;
            //    myRectangle.Y += e.VerticalChange;

            //    ViewModel.UpdateZones(myRectangle);
            //}
        }

        ///// <summary>
        ///// Handle the rectangle resize.
        ///// </summary>
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = (Thumb)sender;
            RectangleViewModel myRectangle = (RectangleViewModel)thumb.DataContext;

            if (myRectangle != null)
            {
                //ViewModel.SelectZone(myRectangle);

                double deltaVertical, deltaHorizontal;
                //
                // Resizing vertically.
                //
                switch (thumb.VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, myRectangle.Height);
                        myRectangle.Height -= deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, myRectangle.Height);
                        myRectangle.Y += e.VerticalChange;
                        myRectangle.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }
                //
                // Resizing horizontally.
                //
                switch (thumb.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, myRectangle.Width);
                        myRectangle.X += e.HorizontalChange;
                        myRectangle.Width -= deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, myRectangle.Width);
                        myRectangle.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }

                e.Handled = true;

                //ViewModel.UpdateZones(myRectangle);

            }
        }

        private Point StartPoint = new Point(0, 0);
        private Point EndPoint = new Point(0, 0);
        private bool MouseDrag = false;


        /// <summary>
        /// The threshold distance the mouse-cursor must move before drag-selection begins.
        /// </summary>
        private readonly double DragThreshold = 5;

        private Rect GetNearestRectangle(Point mousePoint)
        {
            List<Rect> rectangles = new List<Rect>(); // Collection of rectangles

            foreach (var rectVM in ViewModel.Rectangles)
            {
                // Add rectangles to the collection
                rectangles.Add(new Rect(rectVM.X, rectVM.Y, rectVM.Width, rectVM.Height));
            }

            Rect nearestRectangle = rectangles.FirstOrDefault(rectangle => rectangle.Contains(mousePoint));

            if (nearestRectangle != default(Rect))
            {
                double minDistance = double.MaxValue; // Variable to store the minimum distance

                foreach (Rect rectangle in rectangles)
                {
                    if (!rectangle.Contains(mousePoint))
                        continue;

                    double distanceToTop = Math.Abs(rectangle.Y - mousePoint.Y);
                    double distanceToBottom = Math.Abs(rectangle.Y + rectangle.Height - mousePoint.Y);
                    double distanceToLeft = Math.Abs(rectangle.X - mousePoint.X);
                    double distanceToRight = Math.Abs(rectangle.X + rectangle.Width - mousePoint.X);

                    double distance = Math.Min(distanceToTop, Math.Min(distanceToBottom, Math.Min(distanceToLeft, distanceToRight)));


                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestRectangle = rectangle;
                    }
                }

                return nearestRectangle;
            }
            else
                return nearestRectangle;


        }
        private void PreviewImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                switch (ViewModel.OperationType)
                {
                    case enZoneType.DrawZone:
                        if (ViewModel.DrawRegionChecked)
                        {
                            Point mousePosition = e.GetPosition(sender as IInputElement);

                            double x = mousePosition.X; // X-coordinate of the mouse
                            double y = mousePosition.Y; // Y-coordinate of the mouse

                            Rect PointInsideRectangle = GetNearestRectangle(mousePosition);

                            if (PointInsideRectangle != default(Rect))
                            {
                                // Handle the image mouse left button down event for selecting a Rectangle

                                RectangleViewModel myRectangle = ViewModel.Rectangles.Where(o => o.X == PointInsideRectangle.X &&
                                                                                                 o.Y == PointInsideRectangle.Y &&
                                                                                                 o.Width == PointInsideRectangle.Width &&
                                                                                                 o.Height == PointInsideRectangle.Height).FirstOrDefault();
                            }
                            else
                            {

                                foreach (var rectVM in ViewModel.Rectangles)
                                {
                                    if (rectVM.Name == "")
                                    {
                                        ViewModel.Rectangles.Remove(rectVM);

                                        break;
                                    }
                                }
                                foreach (var rectVM in ViewModel.Rectangles)
                                {
                                    rectVM.Opacity = 0.7;
                                    rectVM.StrokeThickness = 1;
                                    rectVM.StrokeDash = new DoubleCollection { 1, 1 };

                                }
                            }

                            //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Cross;

                            ViewModel.DragRectangle.Reset();
                            MouseDrag = true;
                            StartPoint = e.GetPosition(PreviewImage);
                            e.Handled = true;
                        }
                        break;

                    case enZoneType.Pan:
                        //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                        break;

                    case enZoneType.Magnify:
                        Org_MouseDown(sender, e);
                        break;

                    default:
                        //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
                        break;
                }

            }
            catch (Exception ex)
            {
                return;
            }

        }

        private void PreviewImage_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {

                if ((e.LeftButton == MouseButtonState.Pressed) && ViewModel.DrawRegionChecked && (ViewModel.OperationType == enZoneType.DrawZone))
                {
                    if (MouseDrag == true)
                    {
                        EndPoint = e.GetPosition(PreviewImage);

                        var dragDelta = EndPoint - StartPoint;
                        double dragDistance = Math.Abs(dragDelta.Length);
                        if (dragDistance > DragThreshold)
                        {
                            //
                            // Determine x,y,width and height of the rect inverting the points if necessary.
                            // 

                            if (EndPoint.X < StartPoint.X)
                            {
                                ViewModel.DragRectangle.X = EndPoint.X;
                                ViewModel.DragRectangle.Width = StartPoint.X - EndPoint.X;
                            }
                            else
                            {
                                ViewModel.DragRectangle.X = StartPoint.X;
                                ViewModel.DragRectangle.Width = EndPoint.X - StartPoint.X;
                            }

                            if (EndPoint.Y < StartPoint.Y)
                            {
                                ViewModel.DragRectangle.Y = EndPoint.Y;
                                ViewModel.DragRectangle.Height = StartPoint.Y - EndPoint.Y;
                            }
                            else
                            {
                                ViewModel.DragRectangle.Y = StartPoint.Y;
                                ViewModel.DragRectangle.Height = EndPoint.Y - StartPoint.Y;
                            }

                            switch (ViewModel.OperationType)
                            {
                                case enZoneType.DrawZone:
                                    ViewModel.DragRectangle.Color = Colors.Blue;
                                    break;
                                default:
                                    ViewModel.DragRectangle.Color = Colors.Red;
                                    break;
                            }
                            ViewModel.DragRectangle.Opacity = 0.7;
                            ViewModel.DragRectangle.StrokeThickness = 1;
                            ViewModel.DragRectangle.StrokeDash = new DoubleCollection { 1, 1 };

                            DragSelectionCanvas.Visibility = Visibility.Visible;

                            e.Handled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }
        private void UserControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DragSelectionCanvas.Visibility = Visibility.Collapsed;
                //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;


                ////if Zones from List is not selected
                if (ViewModel != null && (ViewModel.DragRectangle.Height > 0 || ViewModel.DragRectangle.Width > 0))
                {
                    if (ViewModel.DrawRegionChecked && (ViewModel.OperationType == enZoneType.DrawZone))
                    {
                        ViewModel.SelectedRectangle = new RectangleViewModel(
                        ViewModel.DragRectangle.X, ViewModel.DragRectangle.Y,
                        ViewModel.DragRectangle.Width, ViewModel.DragRectangle.Height,
                        ViewModel.DragRectangle.Color, ViewModel.DragRectangle.Opacity, ViewModel.Image);

                        ViewModel.DragRectangle.Reset();

                        if ((MouseDrag == true) && (ViewModel.SelectedRectangle != null))
                        {
                            ViewModel.SelectedRectangle = ViewModel.SelectedRectangle;

                        }

                        e.Handled = true;
                    }


                    MouseDrag = false;
                }
                else
                    Org_MouseUp(sender, e);
                //else if ((ViewModel.DragRectangle.Height == 0 || ViewModel.DragRectangle.Width == 0) && MouseDrag == true)
                //{
                //    ViewModel.DragRectangle.Reset();
                //    MouseDrag = false;
                //    if (ViewModel.SelectedRectangle != null)
                //        ViewModel.SelectedRectangle = ViewModel.SelectedRectangle;
                //    e.Handled = true;

                //}

            }
            catch (Exception ex)
            {
                return;
            }

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                e.Handled = true;

                //Rectangle rectangle = (Rectangle)sender;
                //RectangleViewModel myRectangle = (RectangleViewModel)rectangle.DataContext;
                //ViewModel.SelectZone(myRectangle);
                //foreach (var rectVM in ViewModel.Rectangles)
                //{
                //    if (rectVM.Name == ""  /* && ViewModel.SelectedRectangle1.Name != ""*/)
                //    {
                //        ViewModel.Rectangles.Remove(rectVM);
                //        break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void PreviewImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                double newWidth = 0;
                double newHeight = 0;
                if (ViewModel != null && PreviewImage.Source != null)
                {
                    ViewModel.ZoomFactor = PreviewImage.ActualWidth / PreviewImage.Source.Width;
                    ViewModel.ZoomFactorUI = ViewModel.ZoomFactor;
                    ViewModel.MouseZoomFactor = PreviewImage.Source.Width / PreviewImage.ActualWidth;
                    if (e.PreviousSize.Width != 0 && e.PreviousSize.Height != 0)
                    {
                        newWidth = (e.NewSize.Width / e.PreviousSize.Width);
                        newHeight = (e.NewSize.Height / e.PreviousSize.Height);
                    }
                    else
                    {
                        newWidth = e.NewSize.Width;
                        newHeight = e.NewSize.Height;
                    }
                    if (ViewModel.Rectangles != null)
                    {
                        if (ViewModel.Rectangles.Count > 0)
                        {
                            foreach (var rectangle in ViewModel.Rectangles)
                            {
                                rectangle.X *= newWidth;
                                rectangle.Y *= newHeight;
                                rectangle.Width *= newWidth;
                                rectangle.Height *= newHeight;
                            }
                        }

                    }


                    ViewModel.ZoomFactor = PreviewImage.ActualWidth / PreviewImage.Source.Width;
                    ViewModel.ZoomFactorUI = ViewModel.ZoomFactor;
                    ViewModel.MouseZoomFactor = PreviewImage.Source.Width / PreviewImage.ActualWidth;
                }
                if (ViewModel == null)
                {
                    ViewModel = (ScanViewModel)DataContext;
                    ViewModel.ZoomFactor = PreviewImage.ActualWidth / PreviewImage.Source.Width;
                    ViewModel.ZoomFactorUI = ViewModel.ZoomFactor;
                    ViewModel.MouseZoomFactor = PreviewImage.Source.Width / PreviewImage.ActualWidth;
                }
            }
            catch (Exception ex)
            {

                return;
            }
        }

        //It disables the mouse scroll wheeel
        private void ImageMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void Org_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.OriginalSource is System.Windows.Controls.Image image)
                {
                    isMagnifyMouseDown = true;
                    magnifierPopup.IsOpen = true;
                    Point clickPosition = Mouse.GetPosition(PreviewImage.Parent as UIElement);

                    Point prevImage = e.GetPosition(PreviewImage);
                    Point imagePoint = PreviewImage.PointToScreen(prevImage);
                    CalculateScreenLayout();
                    if (dpiScaleX > 1.0)
                    {
                        magnifierPopup.HorizontalOffset = imagePoint.X - main_Rectangle.ActualWidth;
                        magnifierPopup.VerticalOffset = imagePoint.Y - main_Rectangle.ActualHeight + main_Rectangle.ActualHeight / 5;
                    }
                    else
                    {
                        magnifierPopup.HorizontalOffset = imagePoint.X - main_Rectangle.ActualWidth / 2;
                        magnifierPopup.VerticalOffset = imagePoint.Y - main_Rectangle.ActualHeight / 2;
                    }
                    double length = Math.Min(view.ActualWidth, view.ActualHeight) * 0.1;
                    double radius = length / 2;

                    double viewBoxX = clickPosition.X;
                    double viewBoxY = clickPosition.Y;
                    // This fixes right side portion where empty area will not be seen when clicked on mouse down on image area
                    if (prevImage.X + radius > PreviewImage.ActualWidth)
                        viewBoxX = viewBoxX - radius;

                    // This fixes bottom side portion where empty area will not be seen when clicked on mouse down on image area
                    if (prevImage.Y + radius > PreviewImage.ActualHeight)
                        viewBoxY = viewBoxY - radius;

                    // This fixes left side portion where empty area will not be seen when clicked on mouse down on image area
                    if (prevImage.X < radius)
                        viewBoxX = viewBoxX + radius;

                    // This fixes top side portion where empty area will not be seen when clicked on mouse down on image area
                    if (prevImage.Y < radius)
                        viewBoxY = viewBoxY + radius;

                    Rect viewboxRect = new Rect(viewBoxX - radius, viewBoxY - radius, length, length);

                    MagnifierBrush.Viewbox = viewboxRect;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void Org_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (ViewModel.OperationType == enZoneType.Magnify)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (isMagnifyMouseDown)
                        {
                            Point clickPosition = Mouse.GetPosition(PreviewImage.Parent as UIElement);

                            double length = Math.Min(PreviewImage.ActualWidth, PreviewImage.ActualHeight) * 0.08;
                            double radius = length / 2;
                            Point prevImage = e.GetPosition(PreviewImage);
                            Point imagePoint = PreviewImage.PointToScreen(prevImage);
                            double viewBoxX = clickPosition.X;
                            double viewBoxY = clickPosition.Y;
                            // This fixes right side portion where empty area will not be seen when clicked on mouse down on image area
                            if (prevImage.X + radius > PreviewImage.ActualWidth)
                                viewBoxX = viewBoxX - radius;

                            // This fixes bottom side portion where empty area will not be seen when clicked on mouse down on image area
                            if (prevImage.Y + radius > PreviewImage.ActualHeight)
                                viewBoxY = viewBoxY - radius;

                            // This fixes left side portion where empty area will not be seen when clicked on mouse down on image area
                            if (prevImage.X < radius)
                                viewBoxX = viewBoxX + radius;

                            // This fixes top side portion where empty area will not be seen when clicked on mouse down on image area
                            if (prevImage.Y < radius)
                                viewBoxY = viewBoxY + radius;

                            double gridEndX = Mouse.GetPosition(view).X;
                            double gridEndY = Mouse.GetPosition(view).Y;
                            if (clickPosition.X > 0 && clickPosition.Y > 0 && clickPosition.X < PreviewImage.ActualWidth && clickPosition.Y < PreviewImage.ActualHeight)
                            {
                                if (gridEndX > 0 && gridEndY > 0)
                                {
                                    if (!(clickPosition.X < 0) && !(clickPosition.Y < 0) && (gridEndX < view.ActualWidth) && (gridEndY < view.ActualHeight))
                                    {
                                        Rect viewboxRect = new Rect(viewBoxX - radius, viewBoxY - radius, length, length);

                                        MagnifierBrush.Viewbox = viewboxRect;

                                        CalculateScreenLayout();
                                        if (dpiScaleX > 1.0)
                                        {
                                            magnifierPopup.HorizontalOffset = imagePoint.X - main_Rectangle.ActualWidth;
                                            magnifierPopup.VerticalOffset = imagePoint.Y - main_Rectangle.ActualHeight + main_Rectangle.ActualHeight / 5;
                                        }
                                        else
                                        {
                                            magnifierPopup.HorizontalOffset = imagePoint.X - main_Rectangle.ActualWidth / 2;
                                            magnifierPopup.VerticalOffset = imagePoint.Y - main_Rectangle.ActualHeight / 2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void Org_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                isMagnifyMouseDown = false;
                magnifierPopup.IsOpen = false;
            }
            catch (Exception ex)
            {
                isMagnifyMouseDown = false;
                magnifierPopup.IsOpen = false;
                return;
            }
        }

        private void CalculateScreenLayout()
        {
            try
            {
                PresentationSource source = PresentationSource.FromVisual(this); // 'this' refers to the WPF window or visual element
                if (source?.CompositionTarget != null)
                {
                    Matrix matrix = source.CompositionTarget.TransformToDevice;

                    // M11 and M22 represent the scale factors for X and Y axes
                    dpiScaleX = matrix.M11;
                    dpiScaleY = matrix.M22;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }

    //public partial class ScanView : UserControl
    //{
    //    public ScanView()
    //    {
    //        InitializeComponent();
    //    }

    //    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    //    {

    //    }

    //    private void DrawRegionClickEvent(object sender, RoutedEventArgs e)
    //    {

    //    }

    //    private void ZoomInButtonClick(object sender, RoutedEventArgs e)
    //    {

    //    }

    //    private void ZoomOutButtonClick(object sender, RoutedEventArgs e)
    //    {

    //    }

    //    private void FitToWindowButtonClick(object sender, RoutedEventArgs e)
    //    {

    //    }

    //    private void FitOriginalToWindowButtonClick(object sender, RoutedEventArgs e)
    //    {

    //    }

    //    private void PreviewImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    //    {

    //    }

    //    private void PreviewImage_MouseMove(object sender, MouseEventArgs e)
    //    {

    //    }

    //    private void PreviewImage_SizeChanged(object sender, SizeChangedEventArgs e)
    //    {

    //    }

    //    private void ImageMouseWheel(object sender, MouseWheelEventArgs e)
    //    {

    //    }
    //}
}
