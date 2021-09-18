using Prism.Mvvm;
using Prism.Regions;

namespace dataneo.TutorialLibs.WPF.UI
{
    public abstract class BaseViewModel : BindableBase, INavigationAware
    {
        private string caption;
        public string Caption
        {
            get { return caption; }
            set { caption = value; RaisePropertyChanged(); }
        }

        protected IRegionManager RegionManager { get; }

        public BaseViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
    }
}
