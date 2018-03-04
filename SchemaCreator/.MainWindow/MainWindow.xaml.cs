using CustomControls.Controls;

namespace SchemaCreator.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CustomWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            zoomBox.DesignerGrid = gr;
            zoomBox.ScrollViewer = sc;
            zoomBox.ParentPanel = par;
        }
    }
}