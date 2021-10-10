using CSharpFunctionalExtensions;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI.Dialogs
{
    public class DialogWindowViewModel : BaseViewModel, IDialogAware
    {
        private const string MessageKey = "Message";

        public DialogWindowViewModel(IRegionManager regionManager) : base(regionManager)
        {
            this.OKCommand = new Command(OKCommandImpl);
        }

        private void OKCommandImpl() => this.RequestClose?.Invoke(new DialogResult());

        public string Message { get; private set; }
        public ICommand OKCommand { get; }

        public string Title => Translation.UI.ERROR;

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var message = GetMessage(parameters);
            if (message.HasNoValue)
                throw new ArgumentException("No message parameter");

            this.Message = message.Value;
        }

        private Maybe<string> GetMessage(IDialogParameters dialogParameters)
        {
            if (dialogParameters.TryGetValue(MessageKey, out string message))
                return message;

            return Maybe<string>.None;
        }
    }
}
