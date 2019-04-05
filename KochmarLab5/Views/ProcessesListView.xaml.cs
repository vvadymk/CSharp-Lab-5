using System;
using System.Windows.Controls;


namespace KochmarLab5
{
    /// <summary>
    /// Логика взаимодействия для ProcessesListView.xaml
    /// </summary>
    public partial class ProcessesListView : UserControl
    {
        internal ProcessesListView(Action<bool> showLoaderAction)
        {
            InitializeComponent();
            DataContext = new ProcessesListViewModel(showLoaderAction);
        }

        internal void Close()
        {
            ((ProcessesListViewModel)DataContext).Close();
        }
    }
}
