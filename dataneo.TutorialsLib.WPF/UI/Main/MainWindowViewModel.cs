using Ardalis.GuardClauses;
using dataneo.TutorialLibs.WPF.Events;
using Prism.Events;
using Prism.Mvvm;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public ICommand WindowClosing { get; }

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = Guard.Against.Null(eventAggregator, nameof(eventAggregator));
            this.WindowClosing = new Command(WindowClosingImpl);
        }

        private void WindowClosingImpl()
            => this._eventAggregator.GetEvent<CloseAppEvent>().Publish();
    }
}
