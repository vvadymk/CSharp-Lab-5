
using System.Windows;

namespace KochmarLab5
{
    /// <summary>
    /// Логика взаимодействия для InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        private InfoView _infoView;

        internal InfoWindow(System.Diagnostics.Process process)
        {
            InitializeComponent();
            Title = $"{process.ProcessName} Info";
            ShowInfoView(process);
        }

        private void ShowInfoView(System.Diagnostics.Process process)
        {
            MainGrid.Children.Clear();
            if (_infoView == null)
                _infoView = new InfoView(process);
            MainGrid.Children.Add(_infoView);
        }
    }
}
