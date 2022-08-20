using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WpfScreenBack.Views;

namespace WpfScreenBack.ViewModels
{
    public class ViewBViewModel : BindableBase , INavigationAware
    {
        private readonly IRegionManager regionManager;
        public DelegateCommand ScreenTouchCommand { get; }

        void ScreenTouchCommandExecuter()
        {
            TimerReset();
        }

        public ViewBViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ScreenTouchCommand = new DelegateCommand(ScreenTouchCommandExecuter);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false; 
        
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            TimerStop();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            TimerReset();
        }

        // タイマーのセット
        // タイマーのリセット
        // タイマーの破棄
        private DispatcherTimer timer;

        void TimerReset()
        {
            // 既にタイマーがある場合にはそのタイマーは止めておく
            if(this.timer != null)
            {
                this.timer.Stop();
            }

            // 新しいタイマーの時間をセットする
            this.timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(3000)
            };

            // タイマーが満了した時の処理を追加する
            this.timer.Tick += (sender, e) =>
            {
                // 直接触っても例外が起きない
                regionManager.RequestNavigate("ContentRegion", nameof(ViewA));
            };

            // タイマーの計測を開始する
            this.timer.Start();
        }

        void TimerStop()
        {
            // 既にタイマーがある場合にはそのタイマーは止めておく
            if (this.timer != null)
            {
                this.timer.Stop();
            }
        }
    }
}
