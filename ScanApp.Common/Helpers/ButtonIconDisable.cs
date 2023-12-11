using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScanApp.Common
{
    public class ButtonIconDisable : Image
    {
        static ButtonIconDisable()
        {
            IsEnabledProperty.OverrideMetadata(typeof(ButtonIconDisable), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnAutoGreyScaleImageIsEnabledPropertyChanged)));
            SourceProperty.OverrideMetadata(typeof(ButtonIconDisable), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAutoGreyScaleImageSourcePropertyChanged)));
        }

        private static void OnAutoGreyScaleImageSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonIconDisable icon = (ButtonIconDisable)d;
            if (icon?.Source == null)
                return;

            ApplyGreyScaleImage(icon, icon.IsEnabled);
        }
        protected static void OnAutoGreyScaleImageIsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ButtonIconDisable icon = (ButtonIconDisable)d;
            if (icon?.Source == null)
                return;

            var isEnabled = Convert.ToBoolean(args.NewValue);
            ApplyGreyScaleImage(icon, isEnabled);
        }

        protected static void ApplyGreyScaleImage(ButtonIconDisable autoGreyScaleImg, Boolean isEnabled)
        {
            try
            {
                if (!isEnabled)
                {
                    BitmapSource bitmapImage = null;
                    if (autoGreyScaleImg.Source is FormatConvertedBitmap)
                        return;

                    else if (autoGreyScaleImg.Source is BitmapSource)
                        bitmapImage = (BitmapSource)autoGreyScaleImg.Source;
                    else
                        bitmapImage = new BitmapImage(new Uri(autoGreyScaleImg.Source.ToString()));

                    FormatConvertedBitmap conv = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                    autoGreyScaleImg.Source = conv;

                    // Create Opacity Mask for greyscale image as FormatConvertedBitmap does not keep transparency info
                    autoGreyScaleImg.OpacityMask = new ImageBrush(((FormatConvertedBitmap)autoGreyScaleImg.Source).Source); //equivalent to new ImageBrush(bitmapImage)
                }
                else
                {
                    if (autoGreyScaleImg.Source is FormatConvertedBitmap)
                    {
                        autoGreyScaleImg.Source = ((FormatConvertedBitmap)autoGreyScaleImg.Source).Source;
                    }
                    else if (autoGreyScaleImg.Source is BitmapSource)
                    {
                        return;
                    }
                    autoGreyScaleImg.OpacityMask = null;
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
