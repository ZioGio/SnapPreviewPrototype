using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SnapPreviewPrototype.Controls
{
    public sealed partial class Window2Control : UserControl
    {
        public Window2Control()
        {
            InitializeComponent();
        }

        public bool IsSnapped
        {
            get { return (bool)GetValue(isSnappedProperty); }
            set { SetValue(isSnappedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSnapped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isSnappedProperty =
            DependencyProperty.Register("IsSnapped", typeof(bool), typeof(Window2Control), new PropertyMetadata(0));

        public bool IsMaximized
        {
            get { return (bool)GetValue(isMaximizedProperty); }
            set { SetValue(isMaximizedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMaximized.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isMaximizedProperty =
            DependencyProperty.Register("IsMaximized", typeof(bool), typeof(Window1Control), new PropertyMetadata(0));

        public void AddPointerHandlers(PointerEventHandler pressedHandlerLeft, PointerEventHandler releasedHandlerLeft, PointerEventHandler pressedHandlerRight, PointerEventHandler releasedHandlerRight, PointerEventHandler pressedHandlerMaximize, PointerEventHandler releasedHandlerMaximize)
        {
            btn_SnapLeft2.AddHandler(PointerPressedEvent, pressedHandlerLeft, true);
            btn_SnapLeft2.AddHandler(PointerReleasedEvent, releasedHandlerLeft, true);
            btn_SnapRight2.AddHandler(PointerPressedEvent, pressedHandlerRight, true);
            btn_SnapRight2.AddHandler(PointerReleasedEvent, releasedHandlerRight, true);
            btn_Maximize2.AddHandler(PointerPressedEvent, pressedHandlerMaximize, true);
            btn_Maximize2.AddHandler(PointerReleasedEvent, releasedHandlerMaximize, true);
        }

        public void Maximized()
        {
            btn_Maximize2.Visibility = Visibility.Collapsed;
            btn_Restore2.Visibility = Visibility.Visible;
        }

        public void Restored()
        {
            btn_Maximize2.Visibility = Visibility.Visible;
            btn_Restore2.Visibility = Visibility.Collapsed;
        }

        public void HighlightLeftEdge()
        {
            LeftSnapHighlight.Visibility = Visibility.Visible;
            RightSnapHighlight.Visibility = Visibility.Collapsed;
        }

        public void HighlightRightEdge()
        {
            LeftSnapHighlight.Visibility = Visibility.Collapsed;
            RightSnapHighlight.Visibility = Visibility.Visible;
        }

        public void RemoveHighlight()
        {
            LeftSnapHighlight.Visibility = Visibility.Collapsed;
            RightSnapHighlight.Visibility = Visibility.Collapsed;
        }

        public event PointerEventHandler ButtonSnapLeftPressed;
        public event PointerEventHandler ButtonSnapLeftReleased;
        public event PointerEventHandler ButtonSnapRightPressed;
        public event PointerEventHandler ButtonSnapRightReleased;
        public event PointerEventHandler ButtonMaximizePressed;
        public event PointerEventHandler ButtonMaximizeReleased;
        public event RoutedEventHandler ButtonRestoreClick;

        private void Btn_SnapLeft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ButtonSnapLeftPressed?.Invoke(this, e);
        }

        private void Btn_SnapLeft_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ButtonSnapLeftReleased?.Invoke(this, e);
        }

        private void Btn_SnapRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ButtonSnapRightPressed?.Invoke(this, e);
        }

        private void Btn_SnapRight_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ButtonSnapRightReleased?.Invoke(this, e);
        }

        private void Btn_Maximize2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ButtonMaximizePressed?.Invoke(this, e);
        }

        private void Btn_Maximize2_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ButtonMaximizeReleased?.Invoke(this, e);
        }

        private void Btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            ButtonRestoreClick?.Invoke(this, e);
        }
    }
}