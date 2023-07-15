using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SnapPreviewPrototype.Controls
{
    public sealed partial class PreviewWindowControl : UserControl
    {
        public PreviewWindowControl()
        {
            InitializeComponent();
        }

        // Window1
        public void PreviewWindow1Default()
        {
            RemoveAllPreviewSnaps();
            imgWindow1Default.Visibility = Visibility.Visible;
        }

        public void PreviewWindow1SnapLeft()
        {
            RemoveAllPreviewSnaps();
            imgWindow1SnapLeft.Visibility = Visibility.Visible;
        }

        public void PreviewWindow1SnapRight()
        {
            RemoveAllPreviewSnaps();
            imgWindow1SnapRight.Visibility = Visibility.Visible;
        }

        public void PreviewWindow1SnapMaximize()
        {
            RemoveAllPreviewSnaps();
            imgWindow1SnapMaximize.Visibility = Visibility.Visible;
        }

        // Window2
        public void PreviewWindow2Default()
        {
            RemoveAllPreviewSnaps();
            imgWindow2Default.Visibility = Visibility.Visible;
        }

        public void PreviewWindow2SnapLeft()
        {
            RemoveAllPreviewSnaps();
            imgWindow2SnapLeft.Visibility = Visibility.Visible;
        }

        public void PreviewWindow2SnapRight()
        {
            RemoveAllPreviewSnaps();
            imgWindow2SnapRight.Visibility = Visibility.Visible;
        }

        public void PreviewWindow2SnapMaximize()
        {
            RemoveAllPreviewSnaps();
            imgWindow2SnapMaximize.Visibility = Visibility.Visible;
        }

        // Remove all preview snaps
        public void RemoveAllPreviewSnaps()
        {
            imgWindow1Default.Visibility = Visibility.Collapsed;
            imgWindow1SnapLeft.Visibility = Visibility.Collapsed;
            imgWindow1SnapRight.Visibility = Visibility.Collapsed;
            imgWindow1SnapMaximize.Visibility = Visibility.Collapsed;
            imgWindow2Default.Visibility = Visibility.Collapsed;
            imgWindow2SnapLeft.Visibility = Visibility.Collapsed;
            imgWindow2SnapRight.Visibility = Visibility.Collapsed;
            imgWindow2SnapMaximize.Visibility = Visibility.Collapsed;
        }
    }
}
