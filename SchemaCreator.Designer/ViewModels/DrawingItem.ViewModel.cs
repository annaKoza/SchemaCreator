using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.UserControls;

namespace SchemaCreator.Designer.DrawingPart
{
    public class DrawingItemViewModel : BaseDesignerItemViewModel, IDrawableItem
    {
        private double _x1;
        public double X1
        {
            get => _x1;
            set
            {
                _x1 = value; RaisePropertyChanged(nameof(X1));
            }
        }
        private double _x2;
        public double X2
        {
            get => _x2;
            set
            {
                _x2 = value; RaisePropertyChanged(nameof(X2));
            }
        }
        private double _y1;
        public double Y1
        {
            get => _y1;
            set
            {
                _y1 = value; RaisePropertyChanged(nameof(Y1));
            }
        }
        private double _y2;
        public double Y2
        {
            get => _y2;
            set
            {
                _y2 = value; RaisePropertyChanged(nameof(Y2));
            }
        }
    }
}