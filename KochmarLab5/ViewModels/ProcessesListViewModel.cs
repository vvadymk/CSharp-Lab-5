using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using KMA.ProgrammingInCSharp2019.Practice6.Serialization.Tools;

namespace KochmarLab5
{
    internal class ProcessesListViewModel : INotifyPropertyChanged
    {
        private readonly Action<bool> _showLoaderAction;
        private ObservableCollection<Process> _processes;
        private readonly Thread _updateThread;
        private Process _selectedProcess;
        private RelayCommand<object> _endTaskCommand;
        private RelayCommand<object> _getInfoCommand;
        private RelayCommand<object> _openFileLocationCommand;
        private InfoWindow _infoWindow;

        public bool IsItemSelected => SelectedProcess != null;

        public Process SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
              
            }
        }

        public ObservableCollection<Process> Processes
        {
            get => _processes;
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        internal ProcessesListViewModel(Action<bool> showLoaderAction)
        {
            _showLoaderAction = showLoaderAction;
            _updateThread = new Thread(UpdateUsers);
            Thread initializationThread = new Thread(InitializeProcesses);
            initializationThread.Start();
        }

        public RelayCommand<object> EndTaskCommand => _endTaskCommand ?? (_endTaskCommand = new RelayCommand<object>(EndTaskImpl));
        public RelayCommand<object> GetInfoCommand => _getInfoCommand ?? (_getInfoCommand = new RelayCommand<object>(GetInfoImpl));
        public RelayCommand<object> OpenFileLocationCommand => _openFileLocationCommand ?? (_openFileLocationCommand = new RelayCommand<object>(OpenFileLocationImpl));

        private void EndTaskImpl(object o)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(SelectedProcess.Id);
                try
                {
                    process.Kill();
                }
                catch (Win32Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }

        private async void GetInfoImpl(object o)
        {
            try
            {
                await Task.Run(() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(SelectedProcess.Id);
                        _infoWindow?.Close();
                        try
                        {
                            _infoWindow = new InfoWindow(process);
                            _infoWindow.Show();
                        }
                        catch (Win32Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OpenFileLocationImpl(object o)
        {
            await Task.Run(() =>
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(SelectedProcess.Id);
                try
                {
                    string fullPath = process.MainModule.FileName;
                    System.Diagnostics.Process.Start("", fullPath.Remove(fullPath.LastIndexOf('\\')));
                }
                catch (Win32Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }

        private async void UpdateUsers()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        try
                        {
                            lock (Processes)
                            {
                                List<Process> toRemove =
                                    new List<Process>(
                                        Processes.Where(proc => !Process2.Processes.ContainsKey(proc.Id)));
                                foreach (Process proc in toRemove)
                                {
                                    Processes.Remove(proc);
                                }

                                List<Process> toAdd =
                                    new List<Process>(
                                        Process2.Processes.Values.Where(proc => !Processes.Contains(proc)));
                                foreach (Process proc in toAdd)
                                {
                                    Processes.Add(proc);
                                }
                            }
                        }
                        catch (NullReferenceException e)
                        {
                            MessageBox.Show(e.Message);
                        }
                        catch (ArgumentNullException e)
                        {
                            MessageBox.Show(e.Message);
                        }
                        catch (InvalidOperationException e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    });
                });
                Thread.Sleep(4000);
            }
        }

        private async void InitializeProcesses()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate { _showLoaderAction.Invoke(true); });
            await Task.Run(() =>
            {
                Processes = new ObservableCollection<Process>(Process2.Processes.Values);
            });
            _updateThread.Start();
            while (Process2.Processes.Count == 0)
                Thread.Sleep(3000);
            System.Windows.Application.Current.Dispatcher.Invoke(delegate { _showLoaderAction.Invoke(false); });
        }

        internal void Close()
        {
            _updateThread.Join(3000);
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged( string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
