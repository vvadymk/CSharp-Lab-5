using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;


namespace KochmarLab5
{
    internal class InfoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ProcessModule> _modules;
        private ObservableCollection<ProcessThread> _threads;

        public ObservableCollection<ProcessModule> Modules
        {
            get => _modules;
            private set
            {
                _modules = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProcessThread> Threads
        {
            get => _threads;
            private set
            {
                _threads = value;
                OnPropertyChanged();
            }
        }

        internal InfoViewModel(ProcessModuleCollection modules, ProcessThreadCollection threads)
        {
            Modules = new ObservableCollection<ProcessModule>(modules.Cast<ProcessModule>());
            Threads = new ObservableCollection<ProcessThread>(threads.Cast<ProcessThread>());
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
