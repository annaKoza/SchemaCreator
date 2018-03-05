using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class ZoomBox : Control
    {
        public static readonly DependencyProperty DesignerGridProperty =
            DependencyProperty.Register("DesignerGrid",
                                        typeof(Grid),
                                        typeof(ZoomBox),
                                        new PropertyMetadata(OnDesignerGridChanged));

        public static readonly DependencyProperty ParentPanelProperty =
            DependencyProperty.Register("ParentPanel",
                                        typeof(Panel),
                                        typeof(ZoomBox),
                                        new PropertyMetadata(OnParentPanelChanged));

        public static readonly DependencyProperty ScrollViewerProperty =
            DependencyProperty.Register("ScrollViewer",
                                        typeof(ScrollViewer),
                                        typeof(ZoomBox),
                                        new PropertyMetadata(OnScrollViewerChanged));

        public static readonly DependencyProperty SliderValueProperty =
            DependencyProperty.Register("SliderValue",
                                        typeof(double),
                                        typeof(ZoomBox),
                                        new PropertyMetadata(100.0));

        private ScaleTransform _scaleTransform;

        private Canvas _zoomCanvas;
        private Slider _zoomSlider;
        private Thumb _zoomThumb;

        private Point _centerOfViewPort = new Point();

        static ZoomBox() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomBox),
                                                                     new FrameworkPropertyMetadata(typeof(ZoomBox)));

        private void _zoomSlider_ValueChanged(object sender,
                                              RoutedPropertyChangedEventArgs<double> e)
        {
            double scale = e.NewValue / e.OldValue;

            _scaleTransform.ScaleX *= scale;
            _scaleTransform.ScaleY *= scale;

            _centerOfViewPort.X = (ScrollViewer.ViewportWidth / 2);
            _centerOfViewPort.Y = (ScrollViewer.ViewportHeight / 2);
            LastCenterPositionOnTarget = ScrollViewer.TranslatePoint(_centerOfViewPort,
                                                                     DesignerGrid);
            SliderValue = e.NewValue;
            e.Handled = true;
        }

        private void _zoomThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            InvalidateScale(out double scale,
                            out double xOffset,
                            out double yOffset);
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset +
                e.HorizontalChange /
                scale);
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset +
                e.VerticalChange /
                scale);
        }

        private void DesignerCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            InvalidateScale(out double scale,
                            out double xOffset,
                            out double yOffset);
            if(_zoomThumb.Width != ScrollViewer.ViewportWidth * scale)
                _zoomThumb.Width = ScrollViewer.ViewportWidth * scale;
            if(_zoomThumb.Height != ScrollViewer.ViewportHeight * scale)
                _zoomThumb.Height = ScrollViewer.ViewportHeight * scale;
            if(Canvas.GetLeft(_zoomThumb) !=
                xOffset +
                ScrollViewer.HorizontalOffset *
                scale)
                Canvas.SetLeft(_zoomThumb,
                               xOffset +
                    ScrollViewer.HorizontalOffset *
                    scale);
            if(Canvas.GetTop(_zoomThumb) !=
                yOffset +
                ScrollViewer.VerticalOffset *
                scale)
                Canvas.SetTop(_zoomThumb,
                              yOffset +
                    ScrollViewer.VerticalOffset *
                    scale);
        }

        private void InvalidateScale(out double scale,
                                     out double xOffset,
                                     out double yOffset)
        {
            double width = DesignerGrid.ActualWidth * _scaleTransform.ScaleX;
            double height = DesignerGrid.ActualHeight * _scaleTransform.ScaleY;

            // zoom canvas size
            double x = _zoomCanvas.ActualWidth;
            double y = _zoomCanvas.ActualHeight;
            double scaleX = x / width;
            double scaleY = y / height;
            scale = (scaleX < scaleY) ? scaleX : scaleY;
            xOffset = (x - scale * width) / 2;
            yOffset = (y - scale * height) / 2;
        }

        private void OnDesignerGridChanged(Grid oldGrid, Grid newGrid)
        {
            if(oldGrid != null)
            {
                oldGrid.LayoutUpdated -= DesignerCanvas_LayoutUpdated;
            }
            if(newGrid != null)
            {
                newGrid.LayoutUpdated += DesignerCanvas_LayoutUpdated;

                _scaleTransform = new ScaleTransform();
                DesignerGrid.LayoutTransform = _scaleTransform;
            }
        }

        private static void OnDesignerGridChanged(DependencyObject d,
                                                  DependencyPropertyChangedEventArgs e)
        {
            var target = (ZoomBox)d;
            var oldGrid = e.OldValue as Grid;
            var newGrid = e.NewValue as Grid;
            target.OnDesignerGridChanged(oldGrid, newGrid);
        }

        private static void OnParentPanelChanged(DependencyObject d,
                                                 DependencyPropertyChangedEventArgs e)
        {
            var target = (ZoomBox)d;
            var oldControl = e.OldValue as ItemsControl;
            var newControl = e.NewValue as ItemsControl;
            target.OnParentPanelChanged(oldControl, newControl);
        }

        private void OnParentPanelChanged(ItemsControl oldPanel,
                                          ItemsControl newPanel)
        {
            if(oldPanel != null)
            {
                oldPanel.PreviewMouseWheel -= ParentPanel_MouseWheel;
            }
            if(newPanel != null)
            {
                newPanel.PreviewMouseWheel += ParentPanel_MouseWheel;
            }
        }

        private static void OnScrollViewerChanged(DependencyObject d,
                                                  DependencyPropertyChangedEventArgs e)
        {
            var target = (ZoomBox)d;
            var oldScrollViewer = e.OldValue as ScrollViewer;
            var newScrollViewer = e.NewValue as ScrollViewer;
            target.OnScrollViewerScrollChanged(oldScrollViewer, newScrollViewer);
        }

        private void ParentPanel_MouseWheel(object sender,
                                            MouseWheelEventArgs e)
        {
            if(_canResize)
            {
                base.OnPreviewMouseWheel(e);
                LastMousePositionOnTarget = Mouse.GetPosition(DesignerGrid);

                _zoomSlider.Value += e.Delta / 10;

                e.Handled = true;
            } else
            {
                e.Handled = false;
            }
        }

        private Point? targetBefore;
        private Point? targetNow;

        private void ScrollViewer_ScrollChanged(object sender,
                                                ScrollChangedEventArgs e)
        {
            if(e.ExtentHeightChange == 0 || e.ExtentWidthChange == 0) return;

            targetBefore = null;
            targetNow = null;

            if(LastMousePositionOnTarget.HasValue)
            {
                targetBefore = LastMousePositionOnTarget;
                targetNow = Mouse.GetPosition(DesignerGrid);
                LastMousePositionOnTarget = null;
            } else
            {
                if(LastCenterPositionOnTarget.HasValue)
                {
                    var centerOfViewport = new Point(ScrollViewer.ViewportWidth /
                        2,
                                                        ScrollViewer.ViewportHeight /
                        2);
                    Point centerOfTargetNow =
                            ScrollViewer.TranslatePoint(centerOfViewport,
                                                        DesignerGrid);

                    targetBefore = LastCenterPositionOnTarget;
                    targetNow = centerOfTargetNow;
                }
            }

            if(targetBefore.HasValue)
            {
                double dXInTargetPixels = targetNow.Value.X -
                    targetBefore.Value.X;
                double dYInTargetPixels = targetNow.Value.Y -
                    targetBefore.Value.Y;

                double multiplicatorX = e.ExtentWidth / DesignerGrid.ActualWidth;
                double multiplicatorY = e.ExtentHeight /
                    DesignerGrid.ActualHeight;

                double newOffsetX = ScrollViewer.HorizontalOffset -
                    (dXInTargetPixels * multiplicatorX);
                double newOffsetY = ScrollViewer.VerticalOffset -
                    (dYInTargetPixels * multiplicatorY);

                if(double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY)) return;

                ScrollViewer.ScrollToHorizontalOffset(newOffsetX);
                ScrollViewer.ScrollToVerticalOffset(newOffsetY);
            }
            e.Handled = true;
        }

        private bool _canResize => Keyboard.IsKeyDown(Key.LeftCtrl);

        protected Point? LastCenterPositionOnTarget
        {
            get; set;
        }

        protected Point? LastDragPoint
        {
            get; set;
        }

        protected Point? LastMousePositionOnTarget
        {
            get; set;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if(ScrollViewer == null) return;

            _zoomThumb = Template.FindName("PART_ZoomThumb", this) as Thumb;
            _zoomCanvas = Template.FindName("PART_ZoomCanvas", this) as Canvas;
            _zoomSlider = Template.FindName("PART_ZoomSlider", this) as Slider;

            _zoomThumb.DragDelta += _zoomThumb_DragDelta;
            _zoomSlider.ValueChanged += _zoomSlider_ValueChanged;

            _zoomSlider.Value = SliderValue;
        }

        public void OnScrollViewerScrollChanged(ScrollViewer oldScrollViewer,
                                                ScrollViewer newScrollViewer)
        {
            if(oldScrollViewer != null)
            {
                oldScrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
            if(newScrollViewer != null)
            {
                newScrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
        }

        public Grid DesignerGrid
        {
            get => (Grid)GetValue(DesignerGridProperty);
            set => SetValue(DesignerGridProperty, value);
        }

        public Panel ParentPanel
        {
            get => (Panel)GetValue(ParentPanelProperty);
            set => SetValue(ParentPanelProperty, value);
        }

        public ScrollViewer ScrollViewer
        {
            get => (ScrollViewer)GetValue(ScrollViewerProperty);
            set => SetValue(ScrollViewerProperty, value);
        }

        public double SliderValue
        {
            get => (double)GetValue(SliderValueProperty);
            set => SetValue(SliderValueProperty, value);
        }
    }
}