using System.Windows.Controls;


namespace KochmarLab5
{
    /// <summary>
    /// Логика взаимодействия для InfoView.xaml
    /// </summary>
    public partial class InfoView : UserControl
    {
        internal InfoView(System.Diagnostics.Process process)
        {
            InitializeComponent();
            DataContext = new InfoViewModel(process.Modules, process.Threads);
        }
    }
}
