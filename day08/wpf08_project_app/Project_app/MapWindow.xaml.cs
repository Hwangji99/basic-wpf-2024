using System.Windows;

namespace Project_app
{
    /// <summary>
    /// MapWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow()
        {
            InitializeComponent();
        }

        public MapWindow(double posy, double posx) : this()
        {
            BrsLoc.Address = $"https://google.com/maps/place/{posy},{posx}";
        }
    }
}
