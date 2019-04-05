using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fomin05;
using FontAwesome.WPF;

namespace KochmarLab5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
            private ImageAwesome _loader;
            private ProcessesListView _processesListView;

            public MainWindow()
            {
                InitializeComponent();
                ShowProcessesListView();
            }

            private void ShowProcessesListView()
            {
                MainGrid.Children.Clear();
                if (_processesListView == null)
                    _processesListView = new ProcessesListView(ShowLoader);
                MainGrid.Children.Add(_processesListView);
            }

            protected override void OnClosing(CancelEventArgs e)
            {
                _processesListView?.Close();
                Process2.Close();
                base.OnClosing(e);
            }

            private void ShowLoader(bool isShow)
            {
                LoaderHelper.OnRequestLoader(MainGrid, ref _loader, isShow);
            }
        }
    }

