using System.Windows;
using System.Windows.Media;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IDesignerPanelSettings
    {
        Transform Transform
        {
            get; set;
        }

        bool SnapItemToGrid
        {
            get; set;
        }

        bool IsGridSnapVisible
        {
            get; set;
        }

        Point SnapGridOffset
        {
            get; set;
        }
    }
}