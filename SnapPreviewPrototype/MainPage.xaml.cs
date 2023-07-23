using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using SnapPreviewPrototype.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SnapPreviewPrototype
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Delegates
        private Border border;
        private CompositeTransform window1SavedTransform;
        private CompositeTransform window1Transform;
        private CompositeTransform window2SavedTransform;
        private CompositeTransform window2Transform;
        private double bottomEdge;
        private double canvasHeight;
        private double canvasWidth;
        private double canvasX;
        private double canvasY;
        private double leftEdge;
        private double rightEdge;
        private double screenHeight;
        private double screenWidth;
        private double topEdge;
        private double window1BottomEdge;
        private double window1LeftEdge;
        private double window1PointerX;
        private double window1PointerY;
        private double window1PosX;
        private double window1PosY;
        private double window1RightEdge;
        private double window1TopEdge;
        private double window2BottomEdge;
        private double window2LeftEdge;
        private double window2PointerX;
        private double window2PointerY;
        private double window2PosX;
        private double window2PosY;
        private double window2RightEdge;
        private double window2TopEdge;
        private int topZIndex = 10;
        private readonly int windowDefaultHeight = 888;
        private readonly int windowDefaultWidth = 912;
        private PointerPoint window1PointerPosition;
        private PointerPoint window2PointerPosition;
        private readonly SolidColorBrush snapBrush = Application.Current.Resources["SnapBrush"] as SolidColorBrush;
        private Window1Control window1;
        private Window2Control window2;
        private PreviewWindowControl previewWindow;
        private readonly double previewWindowWidth = 227;
        private readonly double previewWindowHeight = 131;
        private PointerPoint previewWindowPointer;
        private double previewWindowX;
        private double previewWindowY;
        #endregion

        public MainPage()
        {
            InitializeComponent();
            CalculateScreenEdges();
            LoadWindow2();
            LoadWindow1();

            window1.AddPointerHandlers(SnapLeftButton_Pressed, SnapLeftButton_Released, SnapRightButton_Pressed, SnapRightButton_Released, SnapMaximizeButton_Pressed, SnapMaximizeButton_Released);
            window2.AddPointerHandlers(SnapLeftButton_Pressed, SnapLeftButton_Released, SnapRightButton_Pressed, SnapRightButton_Released, SnapMaximizeButton_Pressed, SnapMaximizeButton_Released);
        }

        #region Load Methods
        private void LoadWindow1()
        {
            window1 = new Window1Control();
            window1Transform = new CompositeTransform();
            window1.Width = windowDefaultWidth;
            window1.Height = windowDefaultHeight;
            window1.ManipulationMode = ManipulationModes.All; // allows for inertia, otherwise change to ManipulationModes.TranslateX | ManipulationModes.TranslateY
            window1.ManipulationStarted += Window_ManipulationStarted;
            window1.ManipulationDelta += Window_ManipulationDelta;
            window1.ManipulationCompleted += Window_ManipulationCompleted;
            window1.RenderTransformOrigin = new Point(0.5, 0.5);
            window1.RenderTransform = window1Transform;
            window1.IsSnapped = false;
            window1.IsMaximized = false;
            window1.ButtonSnapLeftPressed += new PointerEventHandler(SnapLeftButton_Pressed);
            window1.ButtonSnapLeftReleased += new PointerEventHandler(SnapLeftButton_Released);
            window1.ButtonSnapRightPressed += new PointerEventHandler(SnapRightButton_Pressed);
            window1.ButtonSnapRightReleased += new PointerEventHandler(SnapRightButton_Released);
            window1.ButtonMaximizePressed += new PointerEventHandler(SnapMaximizeButton_Pressed);
            window1.ButtonMaximizeReleased += new PointerEventHandler(SnapMaximizeButton_Released);
            window1.ButtonRestoreClick += new RoutedEventHandler(RestoreButton_Click);
            window1.PointerPressed += Window_PointerPressed;
            window1.PointerReleased += Window_PointerReleased;
            window1.DoubleTapped += Window_Window1DoubleTapped;
            window1.PointerMoved += Window_PointerMoved;

            window1PosX = (rightEdge - windowDefaultWidth) / 2;
            window1PosY = (bottomEdge - windowDefaultHeight) / 2;

            window1LeftEdge = window1PosX;
            window1TopEdge = window1PosY;
            window1RightEdge = window1LeftEdge + windowDefaultWidth;
            window1BottomEdge = window1TopEdge + windowDefaultHeight;

            MainCanvas.Children.Add(window1);
            Canvas.SetLeft(window1, window1PosX);
            Canvas.SetTop(window1, window1PosY);
        }

        private void LoadWindow2()
        {
            window2 = new Window2Control();
            window2Transform = new CompositeTransform();
            window2.Width = windowDefaultWidth;
            window2.Height = windowDefaultHeight;
            window2.ManipulationMode = ManipulationModes.All;
            window2.ManipulationStarted += Window_ManipulationStarted;
            window2.ManipulationDelta += Window_ManipulationDelta;
            window2.ManipulationCompleted += Window_ManipulationCompleted;
            window2.RenderTransformOrigin = new Point(0.5, 0.5);
            window2.RenderTransform = window2Transform;
            window2.IsSnapped = false;
            window2.IsMaximized = false;
            window2.ButtonSnapLeftPressed += new PointerEventHandler(SnapLeftButton_Pressed);
            window2.ButtonSnapLeftReleased += new PointerEventHandler(SnapLeftButton_Released);
            window2.ButtonSnapRightPressed += new PointerEventHandler(SnapRightButton_Pressed);
            window2.ButtonSnapRightReleased += new PointerEventHandler(SnapRightButton_Released);
            window2.ButtonMaximizePressed += new PointerEventHandler(SnapMaximizeButton_Pressed);
            window2.ButtonMaximizeReleased += new PointerEventHandler(SnapMaximizeButton_Released);
            window2.ButtonRestoreClick += new RoutedEventHandler(RestoreButton_Click);
            window2.PointerPressed += Window_PointerPressed;
            window2.PointerReleased += Window_PointerReleased;
            window2.DoubleTapped += Window_Window1DoubleTapped;
            window2.PointerMoved += Window_PointerMoved;

            window2PosX = ((rightEdge - windowDefaultWidth) / 2) + 50;
            window2PosY = ((bottomEdge - windowDefaultHeight) / 2) - 50;

            window2LeftEdge = window2PosX;
            window2TopEdge = window2PosY;
            window2RightEdge = window2LeftEdge + windowDefaultWidth;
            window2BottomEdge = window2TopEdge + windowDefaultHeight;

            MainCanvas.Children.Add(window2);
            Canvas.SetLeft(window2, window2PosX);
            Canvas.SetTop(window2, window2PosY);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalculateScreenEdges();
        }

        private void CalculateScreenEdges()
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            var size = new Size(bounds.Width, bounds.Height);

            screenWidth = size.Width;
            screenHeight = size.Height;
            leftEdge = 0;
            topEdge = 0;
            rightEdge = screenWidth;
            bottomEdge = screenHeight;
        }
        #endregion

        #region Collision
        private bool IntersectElements(FrameworkElement window, FrameworkElement canvas, bool contains)
        {
            // Sets up a boundary so the windows don't slide completely off the screen

            GeneralTransform generalTransform = window.TransformToVisual(canvas);

            canvasX = 0 - window.ActualWidth + 64;
            canvasY = 0;
            canvasWidth = canvas.ActualWidth + (window.ActualWidth * 2) - 128;
            canvasHeight = canvas.ActualHeight + window.ActualHeight;

            Rect boundsWindow = new Rect(0, 0, window.ActualWidth, window.ActualHeight);
            Rect boundsCanvas = new Rect(canvasX, canvasY, canvasWidth, canvasHeight);
            Rect boundsInner = generalTransform.TransformBounds(boundsWindow);

            if (contains)
            {
                return boundsInner.X > boundsCanvas.X &&
                       boundsInner.Y > boundsCanvas.Y &&
                       boundsInner.Right < boundsCanvas.Right &&
                       boundsInner.Bottom < boundsCanvas.Bottom;
            }
            else
            {
                boundsCanvas.Intersect(boundsInner);
                return !boundsCanvas.IsEmpty;
            }
        }

        private void CopyTransform(CompositeTransform orig, CompositeTransform copy)
        {
            copy.TranslateX = orig.TranslateX;
            copy.TranslateY = orig.TranslateY;
        }
        #endregion

        #region Manipulation RoutedEventArgs
        void Window_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            RemoveSnapBorders();

            if (sender.GetType().ToString().Contains("Window1"))
            {
                window1 = sender as Window1Control;

                if (window1 != null)
                {
                    if (window1.IsSnapped == true || window1.IsMaximized == true)
                    {
                        window1.Width = windowDefaultWidth;
                        window1.Height = windowDefaultHeight;
                        window1.IsSnapped = false;
                        window1.IsMaximized = false;
                        window1.Restored();
                    }
                }
            }
            else
            {
                window2 = sender as Window2Control;

                if (window2 != null)
                {
                    if (window2.IsSnapped == true || window2.IsMaximized == true)
                    {
                        window2.Width = windowDefaultWidth;
                        window2.Height = windowDefaultHeight;
                        window2.IsSnapped = false;
                        window2.IsMaximized = false;
                        window2.Restored();
                    }
                }
            }
        }

        void Window_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (sender.GetType().ToString().Contains("Window1"))
            {
                window1 = sender as Window1Control;
                window1SavedTransform = new CompositeTransform();
                CopyTransform(window1Transform, window1SavedTransform);

                window1Transform.TranslateX += e.Delta.Translation.X;
                window1Transform.TranslateY += e.Delta.Translation.Y;

                if (window1 != null && !IntersectElements(window1, MainCanvas, true))
                {
                    // We're outside the boundary so undo the Transforms
                    CopyTransform(window1SavedTransform, window1Transform);
                }
            }
            else
            {
                window2 = sender as Window2Control;
                window2SavedTransform = new CompositeTransform();
                CopyTransform(window2Transform, window2SavedTransform);

                window2Transform.TranslateX += e.Delta.Translation.X;
                window2Transform.TranslateY += e.Delta.Translation.Y;

                if (window2 != null && !IntersectElements(window2, MainCanvas, true))
                {
                    // We're outside the boundary so undo the Transforms
                    CopyTransform(window2SavedTransform, window2Transform);
                }
            }
        }

        void Window_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (sender.GetType().ToString().Contains("Window1"))
            {
                var transformToVisual = MainCanvas.TransformToVisual(window1);
                var window1Position = transformToVisual.TransformPoint(new Point(0, 0));
                window1PosX = window1Position.X;
                window1PosY = window1Position.Y;
                window1LeftEdge = window1PosX * -1;
                window1TopEdge = window1PosY * -1;
                window1RightEdge = window1LeftEdge + windowDefaultWidth;
                window1BottomEdge = window1TopEdge + windowDefaultHeight;
            }
            else
            {
                var transformToVisual = MainCanvas.TransformToVisual(window2);
                var window2Position = transformToVisual.TransformPoint(new Point(0, 0));
                window2PosX = window2Position.X;
                window2PosY = window2Position.Y;
                window2LeftEdge = window2PosX * -1; ;
                window2TopEdge = window2PosY * -1; ;
                window2RightEdge = window2LeftEdge + windowDefaultWidth;
                window2BottomEdge = window2TopEdge + windowDefaultHeight;
            }
        }
        #endregion

        #region PointerRoutedEventArgs
        private void SnapLeftButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            ShowLeftSnapBorder();
        }

        private void SnapRightButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            ShowRightSnapBorder();
        }

        private void SnapMaximizeButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            ShowMaximumSnapBorder();
        }

        private void SnapLeftButton_Released(object sender, PointerRoutedEventArgs e)
        {
            RemoveSnapBorders();
            Button btn = sender as Button;
            if (btn != null)
            {
                if (btn.Name.ToString().Contains("1"))
                {
                    SnapLeft(window1);
                }
                else
                {
                    SnapLeft(window2);
                }
            }
        }

        private void SnapRightButton_Released(object sender, PointerRoutedEventArgs e)
        {
            RemoveSnapBorders();
            Button btn = sender as Button;
            if (btn != null)
            {
                if (btn.Name.ToString().Contains("1"))
                {
                    SnapRight(window1);
                }
                else
                {
                    SnapRight(window2);
                }
            }
        }

        private void SnapMaximizeButton_Released(object sender, PointerRoutedEventArgs e)
        {
            RemoveSnapBorders();
            Button btn = sender as Button;
            if (btn != null)
            {
                if (btn.Name.ToString().Contains("1"))
                {
                    SnapMaximize(window1);
                }
                else
                {
                    SnapMaximize(window2);
                }
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveSnapBorders();

            if (sender.GetType().ToString().Contains("Window1"))
            {
                window1.Width = windowDefaultWidth;
                window1.Height = windowDefaultHeight;
                window1PosX = (rightEdge - windowDefaultWidth) / 2;
                window1PosY = (bottomEdge - windowDefaultHeight) / 2;
                Canvas.SetLeft(window1, window1PosX);
                Canvas.SetTop(window1, window1PosY);
                window1.IsMaximized = false;
                window1.Restored();
            }
            else
            {
                window2.Width = windowDefaultWidth;
                window2.Height = windowDefaultHeight;
                window2PosX = ((rightEdge - windowDefaultWidth) / 2) + 50;
                window2PosY = ((bottomEdge - windowDefaultHeight) / 2) - 50;
                Canvas.SetLeft(window2, window2PosX);
                Canvas.SetTop(window2, window2PosY);
                window2.IsMaximized = false;
                window2.Restored();
            }
        }

        private void Window_Window1DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            RemoveSnapBorders();

            if (MainCanvas.Children.Contains(previewWindow))
            {
                RemovePreviewWindow();
            }

            if (sender.GetType().ToString().Contains("Window1"))
            {
                if (window1.IsMaximized != true)
                {
                    SnapMaximize(window1);
                }
                else
                {
                    RestoreButton_Click(window1, e);
                }
            }
            else
            {
                if (window2.IsMaximized != true)
                {
                    SnapMaximize(window2);
                }
                else
                {
                    RestoreButton_Click(window2, e);
                }
            }
        }

        private void Window_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            RemoveSnapBorders();

            if (sender.GetType().ToString().Contains("Window1"))
            {
                window1 = sender as Window1Control;
                window1PointerPosition = e.GetCurrentPoint(MainCanvas);
                window1PointerX = window1PointerPosition.Position.X;
                window1PointerY = window1PointerPosition.Position.Y;
                Canvas.SetZIndex(window1, ++topZIndex);

                if (window1.IsSnapped || window1.IsMaximized)
                {
                    Canvas.SetLeft(window1, window1PointerX - (windowDefaultWidth / 2));
                }

                // To keep previewWindow on top, needs to come after window1
                if (!MainCanvas.Children.Contains(previewWindow))
                {
                    LoadPreviewWindow(window1, e);
                }
            }
            else
            {
                window2 = sender as Window2Control;
                window2PointerPosition = e.GetCurrentPoint(MainCanvas);
                window2PointerX = window2PointerPosition.Position.X;
                window2PointerY = window2PointerPosition.Position.Y;
                Canvas.SetZIndex(window2, ++topZIndex);

                if (window2.IsSnapped || window2.IsMaximized)
                {
                    Canvas.SetLeft(window2, window2PointerX - (windowDefaultWidth / 2));
                }

                // To keep previewWindow on top, needs to come after window2
                if (!MainCanvas.Children.Contains(previewWindow))
                {
                    LoadPreviewWindow(window2, e);
                }
            }
        }

        private void Window_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (sender.GetType().ToString().Contains("Window1"))
            {
                window1PointerPosition = e.GetCurrentPoint(MainCanvas);
                window1PointerX = window1PointerPosition.Position.X;
                window1PointerY = window1PointerPosition.Position.Y;

                // Preview window
                if (MainCanvas.Children.Contains(previewWindow))
                {
                    if (window1PointerX <= (previewWindowX + (previewWindowWidth / 2) + 38) || window1PointerX >= (previewWindowX + (previewWindowWidth / 2) - 38))
                    {
                        RemoveSnapBorders();
                        previewWindow.PreviewWindow1Default();
                    }

                    if (window1PointerY <= (previewWindowY + (previewWindowHeight / 2) - 15) || window1PointerY >= (previewWindowY + (previewWindowHeight / 2) + 15))
                    {
                        RemoveSnapBorders();
                        previewWindow.PreviewWindow1Default();
                    }

                    if (window1PointerX < previewWindowX + (previewWindowWidth / 3) && window1PointerX > previewWindowX)
                    {
                        RemoveSnapBorders();
                        ShowLeftSnapBorder();
                        previewWindow.PreviewWindow1SnapLeft();
                    }

                    if (window1PointerX < previewWindowX + previewWindowWidth && window1PointerX > previewWindowX + ((previewWindowWidth / 3) * 2))
                    {
                        RemoveSnapBorders();
                        ShowRightSnapBorder();
                        previewWindow.PreviewWindow1SnapRight();
                    }

                    if (window1PointerY < previewWindowY + (previewWindowHeight / 3) && window1PointerY > previewWindowY)
                    {
                        RemoveSnapBorders();
                        ShowMaximumSnapBorder();
                        previewWindow.PreviewWindow1SnapMaximize();
                    }

                    if (window1PointerX < previewWindowX || window1PointerX > previewWindowX + previewWindowWidth || window1PointerY < previewWindowY || window1PointerY > previewWindowY + previewWindowHeight)
                    {
                        RemovePreviewWindow();
                    }
                }

                // Highlight window edge on drag and hover
                if (window1PointerY >= window2TopEdge && window1PointerY <= window2BottomEdge)
                {
                    // Gives a 20px leeway on either side of the window edge
                    if (window1PointerX >= (window2LeftEdge - 20) && window1PointerX <= window2LeftEdge + 20)
                    {
                        window2.HighlightLeftEdge();
                    }
                    else if (window1PointerX >= (window2RightEdge - 20) && window1PointerX <= window2RightEdge + 20)
                    {
                        window2.HighlightRightEdge();
                    }
                    else
                    {
                        window2.RemoveHighlight();
                    }
                }
            }
            else
            {
                window2PointerPosition = e.GetCurrentPoint(MainCanvas);
                window2PointerX = window2PointerPosition.Position.X;
                window2PointerY = window2PointerPosition.Position.Y;

                // Preview window
                if (MainCanvas.Children.Contains(previewWindow))
                {
                    if (window2PointerX <= (previewWindowX + (previewWindowWidth / 2) + 38) || window2PointerX >= (previewWindowX + (previewWindowWidth / 2) - 38))
                    {
                        RemoveSnapBorders();
                        previewWindow.PreviewWindow2Default();
                    }

                    if (window2PointerY <= (previewWindowY + (previewWindowHeight / 2) - 15) || window2PointerY >= (previewWindowY + (previewWindowHeight / 2) + 15))
                    {
                        RemoveSnapBorders();
                        previewWindow.PreviewWindow2Default();
                    }

                    if (window2PointerX < previewWindowX + (previewWindowWidth / 3) && window2PointerX > previewWindowX)
                    {
                        RemoveSnapBorders();
                        ShowLeftSnapBorder();
                        previewWindow.PreviewWindow2SnapLeft();
                    }

                    if (window2PointerX < previewWindowX + previewWindowWidth && window2PointerX > previewWindowX + ((previewWindowWidth / 3) * 2))
                    {
                        RemoveSnapBorders();
                        ShowRightSnapBorder();
                        previewWindow.PreviewWindow2SnapRight();
                    }

                    if (window2PointerY < previewWindowY + (previewWindowHeight / 3) && window2PointerY > previewWindowY)
                    {
                        RemoveSnapBorders();
                        ShowMaximumSnapBorder();
                        previewWindow.PreviewWindow2SnapMaximize();
                    }

                    if (window2PointerX < previewWindowX || window2PointerX > previewWindowX + previewWindowWidth || window2PointerY < previewWindowY || window2PointerY > previewWindowY + previewWindowHeight)
                    {
                        RemovePreviewWindow();
                    }
                }

                // Highlight window edge on drag and hover
                if (window2PointerY >= window1TopEdge && window2PointerY <= window1BottomEdge)
                {
                    // Gives a 20px leeway on either side of the window edge
                    if (window2PointerX >= (window1LeftEdge - 20) && window2PointerX <= window1LeftEdge + 20)
                    {
                        window1.HighlightLeftEdge();
                    }
                    else if (window2PointerX >= (window1RightEdge - 20) && window2PointerX <= window1RightEdge + 20)
                    {
                        window1.HighlightRightEdge();
                    }
                    else
                    {
                        window1.RemoveHighlight();
                    }
                }
            }
        }

        private void Window_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (sender.GetType().ToString().Contains("Window1"))
            {
                window1PointerPosition = e.GetCurrentPoint(MainCanvas);
                window1PointerX = window1PointerPosition.Position.X;
                window1PointerY = window1PointerPosition.Position.Y;

                // Preview window
                if (MainCanvas.Children.Contains(previewWindow))
                {
                    if (window1PointerX > previewWindowX && window1PointerX < previewWindowX + (previewWindowWidth / 3))
                    {
                        RemoveSnapBorders();
                        SnapLeft(window1);
                    }

                    if (window1PointerX > previewWindowX + ((previewWindowWidth / 3) * 2) && window1PointerX < previewWindowX + previewWindowWidth)
                    {
                        RemoveSnapBorders();
                        SnapRight(window1);
                    }

                    if (window1PointerY > previewWindowY && window1PointerY < previewWindowY + (previewWindowHeight / 3))
                    {
                        RemoveSnapBorders();
                        SnapMaximize(window1);
                    }
                }

                // Snap the two windows
                if (window1PointerY >= window2TopEdge && window1PointerY <= window2BottomEdge)
                {
                    // Gives a 10px leeway on either side of the window edge
                    if (window1PointerX >= (window2LeftEdge - 10) && window1PointerX <= window2LeftEdge + 10)
                    {
                        SnapLeft(window1);
                        SnapRight(window2);
                    }
                    else if (window1PointerX >= (window2RightEdge - 10) && window1PointerX <= window2RightEdge + 10)
                    {
                        SnapLeft(window2);
                        SnapRight(window1);
                    }
                }
            }
            else
            {
                window2PointerPosition = e.GetCurrentPoint(MainCanvas);
                window2PointerX = window2PointerPosition.Position.X;
                window2PointerY = window2PointerPosition.Position.Y;

                // Preview window
                if (MainCanvas.Children.Contains(previewWindow))
                {
                    if (window2PointerX > previewWindowX && window2PointerX < previewWindowX + (previewWindowWidth / 3))
                    {
                        RemoveSnapBorders();
                        SnapLeft(window2);
                    }

                    if (window2PointerX > previewWindowX + ((previewWindowWidth / 3) * 2) && window2PointerX < previewWindowX + previewWindowWidth)
                    {
                        RemoveSnapBorders();
                        SnapRight(window2);
                    }

                    if (window2PointerY > previewWindowY && window2PointerY < previewWindowY + (previewWindowHeight / 3))
                    {
                        RemoveSnapBorders();
                        SnapMaximize(window2);
                    }
                }

                // Snap the two windows
                if (window2PointerY >= window1TopEdge && window2PointerY <= window1BottomEdge)
                {
                    // Gives a 10px leeway on either side of the window edge
                    if (window2PointerX >= (window1LeftEdge - 10) && window2PointerX <= window1LeftEdge + 10)
                    {
                        SnapLeft(window2);
                        SnapRight(window1);
                    }
                    else if (window2PointerX >= (window1RightEdge - 10) && window2PointerX <= window1RightEdge + 10)
                    {
                        SnapLeft(window1);
                        SnapRight(window2);
                    }
                }
            }

            RemovePreviewWindow();
        }
        #endregion

        #region Snapping
        private void RemoveSnapBorders()
        {
            foreach (UIElement item in MainCanvas.Children)
            {
                if (item == border)
                {
                    MainCanvas.Children.Remove(item);
                }
            }

            return;
        }

        private void ShowLeftSnapBorder()
        {
            border = new Border();
            border.Width = rightEdge / 2;
            border.Height = bottomEdge - 50;
            border.BorderBrush = snapBrush;
            border.BorderThickness = new Thickness(4);
            MainCanvas.Children.Add(border);
            Canvas.SetLeft(border, 0);
            Canvas.SetTop(border, 0);
        }

        private void ShowRightSnapBorder()
        {
            border = new Border();
            border.Width = rightEdge / 2;
            border.Height = bottomEdge - 50;
            border.BorderBrush = snapBrush;
            border.BorderThickness = new Thickness(4);
            MainCanvas.Children.Add(border);
            Canvas.SetLeft(border, (rightEdge - border.Width));
            Canvas.SetTop(border, 0);
        }

        private void ShowMaximumSnapBorder()
        {
            border = new Border();
            border.Width = rightEdge;
            border.Height = bottomEdge - 50;
            border.BorderBrush = snapBrush;
            border.BorderThickness = new Thickness(4);
            MainCanvas.Children.Add(border);
            Canvas.SetLeft(border, 0);
            Canvas.SetTop(border, 0);
        }

        private void SnapLeft(FrameworkElement fe)
        {
            if (fe.GetType().ToString().Contains("Window1"))
            {
                // Window1 snap left
                window1Transform.TranslateX = 0;
                window1Transform.TranslateY = 0;
                window1.Width = rightEdge / 2;
                window1.Height = bottomEdge - 50;
                window1PosX = leftEdge - 2;
                window1PosY = topEdge;
                Canvas.SetLeft(window1, window1PosX);
                Canvas.SetTop(window1, window1PosY);
                window1.IsSnapped = true;

                // Window2 snap right
                //window2Transform.TranslateX = 0;
                //window2Transform.TranslateY = 0;
                //window2.Width = rightEdge / 2;
                //window2.Height = bottomEdge - 50;
                //window2PosX = (rightEdge - window1.Width) + 2;
                //window2PosY = topEdge;
                //Canvas.SetLeft(window2, window2PosX);
                //Canvas.SetTop(window2, window2PosY);
                //window2.IsSnapped = true;
            }
            else
            {
                // Window2 snap left
                window2Transform.TranslateX = 0;
                window2Transform.TranslateY = 0;
                window2.Width = rightEdge / 2;
                window2.Height = bottomEdge - 50;
                window2PosX = leftEdge - 2;
                window2PosY = topEdge;
                Canvas.SetLeft(window2, window2PosX);
                Canvas.SetTop(window2, window2PosY);
                window2.IsSnapped = true;

                // Window1 snap right
                //window1Transform.TranslateX = 0;
                //window1Transform.TranslateY = 0;
                //window1.Width = rightEdge / 2;
                //window1.Height = bottomEdge - 50;
                //window1PosX = (rightEdge - window1.Width) + 2;
                //window1PosY = topEdge;
                //Canvas.SetLeft(window1, window1PosX);
                //Canvas.SetTop(window1, window1PosY);
                //window1.IsSnapped = true;
            }

            window1.RemoveHighlight();
            window2.RemoveHighlight();
        }

        private void SnapRight(FrameworkElement fe)
        {
            if (fe.GetType().ToString().Contains("Window1"))
            {
                // Window1 snap right
                window1Transform.TranslateX = 0;
                window1Transform.TranslateY = 0;
                window1.Width = rightEdge / 2;
                window1.Height = bottomEdge - 50;
                window1PosX = (rightEdge - window1.Width) + 2;
                window1PosY = topEdge;
                Canvas.SetLeft(window1, window1PosX);
                Canvas.SetTop(window1, window1PosY);
                window1.IsSnapped = true;

                // Window2 snap left
                //window2Transform.TranslateX = 0;
                //window2Transform.TranslateY = 0;
                //window2.Width = rightEdge / 2;
                //window2.Height = bottomEdge - 50;
                //window2PosX = leftEdge - 2;
                //window2PosY = topEdge;
                //Canvas.SetLeft(window2, window2PosX);
                //Canvas.SetTop(window2, window2PosY);
                //window2.IsSnapped = true;
            }
            else
            {
                // Window2 snap right
                window2Transform.TranslateX = 0;
                window2Transform.TranslateY = 0;
                window2.Width = rightEdge / 2;
                window2.Height = bottomEdge - 50;
                window2PosX = (rightEdge - window1.Width) + 2;
                window2PosY = topEdge;
                Canvas.SetLeft(window2, window2PosX);
                Canvas.SetTop(window2, window2PosY);
                window2.IsSnapped = true;

                // Window1 snap left
                //window1Transform.TranslateX = 0;
                //window1Transform.TranslateY = 0;
                //window1.Width = rightEdge / 2;
                //window1.Height = bottomEdge - 50;
                //window1PosX = leftEdge - 2;
                //window1PosY = topEdge;
                //Canvas.SetLeft(window1, window1PosX);
                //Canvas.SetTop(window1, window1PosY);
                //window1.IsSnapped = true;
            }

            window1.RemoveHighlight();
            window2.RemoveHighlight();
        }

        private void SnapMaximize(FrameworkElement fe)
        {
            if (fe.GetType().ToString().Contains("Window1"))
            {
                // Window1 maximize
                window1Transform.TranslateX = 0;
                window1Transform.TranslateY = 0;
                window1.Width = rightEdge;
                window1.Height = bottomEdge - 50;
                window1PosX = leftEdge;
                window1PosY = topEdge;
                Canvas.SetLeft(window1, window1PosX);
                Canvas.SetTop(window1, window1PosY);
                window1.IsMaximized = true;
                window1.Maximized();
            }
            else
            {
                // Window2 maximize
                window2Transform.TranslateX = 0;
                window2Transform.TranslateY = 0;
                window2.Width = rightEdge;
                window2.Height = bottomEdge - 50;
                window2PosX = leftEdge;
                window2PosY = topEdge;
                Canvas.SetLeft(window2, window2PosX);
                Canvas.SetTop(window2, window2PosY);
                window2.IsMaximized = true;
                window2.Maximized();
            }

            window1.RemoveHighlight();
            window2.RemoveHighlight();
        }
        #endregion

        #region PreviewWindow
        private void LoadPreviewWindow(object sender, PointerRoutedEventArgs e)
        {
            previewWindow = new PreviewWindowControl();
            previewWindowPointer = e.GetCurrentPoint(MainCanvas);
            previewWindowX = previewWindowPointer.Position.X - (previewWindowWidth / 2);
            previewWindowY = previewWindowPointer.Position.Y - (previewWindowHeight / 2);
            MainCanvas.Children.Add(previewWindow);
            Canvas.SetZIndex(previewWindow, topZIndex + 1);
            Canvas.SetLeft(previewWindow, previewWindowX);
            Canvas.SetTop(previewWindow, previewWindowY);

            if (sender.GetType().ToString().Contains("Window1"))
            {
                previewWindow.PreviewWindow1Default();
            }
            else
            {
                previewWindow.PreviewWindow2Default();
            }
        }

        private void RemovePreviewWindow()
        {
            MainCanvas.Children.Remove(previewWindow);
            RemoveSnapBorders();
        }
        #endregion
    }
}
