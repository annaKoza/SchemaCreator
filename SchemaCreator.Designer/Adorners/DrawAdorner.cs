using SchemaCreator.Designer.Adorners;
using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.AttachedProperties
{
    internal class DrawAdorner : BaseAdorner
    {
        private Point? _endPoint;
        
        internal DrawAdorner(Canvas itemsPanel, Point? selectionStartPoint, IDesignerViewModel designerPanel, IDrawableItem drawableItem) : base(itemsPanel, new SizeChrome() { DataContext=drawableItem })
        {
            ItemsPanel = itemsPanel;
            DrawableItem = drawableItem;
            DesignerPanel = designerPanel;
            SelectionStartPoint = selectionStartPoint;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var rectangle = new Rect(SelectionStartPoint.Value,
                                     _endPoint ??
                SelectionStartPoint.Value);

            Chrome.Arrange(new Rect(rectangle.TopLeft, rectangle.Size));
            Chrome.Width = rectangle.Width * XScaleVectorLength;
            Chrome.Height = rectangle.Height * YScaleVectorLength;

            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => VisualsToRender[index];

        protected override int VisualChildrenCount => VisualsToRender.Count;
        public Point? SelectionStartPoint { get; set; }
        public IDesignerViewModel DesignerPanel { get; set; }
        public IDrawableItem DrawableItem { get; set; }
        public Canvas ItemsPanel { get; set; }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if(!IsMouseCaptured)
                    CaptureMouse();
                var currentPosition = e.GetPosition(this);

                if(Keyboard.IsKeyDown(Key.LeftShift))
                {
                    if(Math.Abs(currentPosition.X - SelectionStartPoint.Value.X) <
                        Math.Abs(currentPosition.Y -
                            SelectionStartPoint.Value.Y))
                        currentPosition.X = SelectionStartPoint.Value.X; else
                        currentPosition.Y = SelectionStartPoint.Value.Y;
                }

                _endPoint = currentPosition;
                InvalidateVisual();
            }
            else if(IsMouseCaptured) ReleaseMouseCapture();

            e.Handled = true;
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if(IsMouseCaptured) ReleaseMouseCapture();
            var adornerLayer = AdornerLayer.GetAdornerLayer(ItemsPanel);
            adornerLayer?.Remove(this);

            if(DrawableItem == null) return;

            DrawableItem.Y2 = _endPoint.Value.Y;
            DrawableItem.X2 = _endPoint.Value.X;

            DrawableItem.Left = Math.Min(DrawableItem.X1,
                                              DrawableItem.X2);
            DrawableItem.Top = Math.Min(DrawableItem.Y1,
                                             DrawableItem.Y2);

            var width = Math.Abs(DrawableItem.X1 - DrawableItem.X2);
            var height = Math.Abs(DrawableItem.Y1 - DrawableItem.Y2);

            CreateDrawableInstance(width, height);
            DesignerPanel.AddItem(DrawableItem);
            DesignerPanel.SelectionService.SelectItem(DrawableItem);

            e.Handled = true;
        }

        private void CreateDrawableInstance(double width, double height)
        {
            DrawableItem.Parent = DesignerPanel;
            DrawableItem.Width = width;
            DrawableItem.Height = height;
            DrawableItem.MinWidth = 10;
            DrawableItem.MinHeight = 10;
            DrawableItem.ZIndex = DesignerPanel.Items.Count;
            DrawableItem.TransformOrigin = new Point(0.5, 0.5);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.PushOpacity(1);
            drawingContext.DrawRectangle(Brushes.Transparent,
                                         null,
                                         new Rect(RenderSize));

            if(SelectionStartPoint.HasValue && _endPoint.HasValue)
            {
                DrawableItem.DrawAdorner(drawingContext,
                                              SelectionStartPoint.Value,
                                              _endPoint.Value);
                drawingContext.DrawRectangle(Brushes.Transparent,
                                             new Pen(new SolidColorBrush(Colors.White),
                                                     0.1),
                                             new Rect(SelectionStartPoint.Value,
                                                      _endPoint.Value));
            }
        }
    }
}