using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WpfScreenBack.Views;

namespace WpfScreenBack.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand ShowViewAButton { get; }
        void ShowViewAButtonExecuter() => regionManager.RequestNavigate("ContentRegion", nameof(ViewA));
        public DelegateCommand ShowViewBButton { get; }
        void ShowViewBButtonExecuter() => regionManager.RequestNavigate("ContentRegion", nameof(ViewB));

        private readonly IRegionManager regionManager;


        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ShowViewAButton = new DelegateCommand(ShowViewAButtonExecuter);
            ShowViewBButton = new DelegateCommand(ShowViewBButtonExecuter);
        }
    }
}
