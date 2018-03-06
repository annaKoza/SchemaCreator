using SchemaCreator.Designer.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public abstract class BaseAdorner : Adorner
    {
        protected VisualCollection VisualsToRender { get; }
        protected FrameworkElement Chrome { get; }
        protected double XScaleVectorLength { get; private set; }
        protected double YScaleVectorLength { get; private set; }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var matrix = transform as MatrixTransform;
            if (VisualsToRender == null || matrix == null) return base.GetDesiredTransform(transform);
            XScaleVectorLength = new Vector(matrix.Matrix.M11, matrix.Matrix.M12).Length;
            YScaleVectorLength = new Vector(matrix.Matrix.M21, matrix.Matrix.M22).Length;
            Chrome.LayoutTransform = new ScaleTransform(1 / XScaleVectorLength, 1 / YScaleVectorLength);
            return base.GetDesiredTransform(transform);
        }

        internal BaseAdorner(UIElement adornedElement) : this(adornedElement,null)
        {
        }
        internal BaseAdorner(UIElement adornedElement, FrameworkElement chromeElement) : base(adornedElement)
        {
            Chrome = chromeElement;
            VisualsToRender = new VisualCollection(this)
            {
                Chrome
            };
        }

        protected override Visual GetVisualChild(int index) => VisualsToRender[index];
        protected override int VisualChildrenCount => VisualsToRender.Count;
    }
}
