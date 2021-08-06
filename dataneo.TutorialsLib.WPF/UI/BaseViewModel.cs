using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dataneo.TutorialsLibs.WPF.UI
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        private string caption;
        public string Caption
        {
            get { return caption; }
            set { caption = value; Notify(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName] string propertyName = "")
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
