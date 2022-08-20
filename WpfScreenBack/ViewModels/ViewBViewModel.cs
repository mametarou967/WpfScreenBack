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
using WpfScreenBack.Models;
using WpfScreenBack.Views;

namespace WpfScreenBack.ViewModels
{
    public class ViewBViewModel : BindableBase , INavigationAware
    {
        private readonly IRegionManager regionManager;


        public ViewBViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ScreenTouchCommand = new DelegateCommand(ScreenTouchCommandExecuter); // これも忘れずに
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        // -------------------------------------------------------------------------------- // 
        ScreenTimer screenTimer;
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // この画面に移ってきた際に時間と、時間が満了したときの処理を付与して、スクリーンタイマーを生成しておく
            // 生成すると同時にタイマーは自動的に開始する
            // 今回は例として画面Bで3秒経ったら画面Aに遷移にするものとする
            screenTimer = new ScreenTimer(3000,() => regionManager.RequestNavigate("ContentRegion", nameof(ViewA)));
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // 何らかの条件でこの画面から遷移する際には、スクリーンタイマーは止めておく必要があるので
            // 忘れずに破棄しておく
            screenTimer?.Dispose();
        }

        // View側で一定領域を画面タッチすると以下のコマンドが発行されるように定義してある
        public DelegateCommand ScreenTouchCommand { get; }
        void ScreenTouchCommandExecuter()
        {
            // UserControl領域のどこかをタッチされたら、スクリーンタイマーはUpdate(=時間を初期化)
            // しておく、これにより、一定時間内に画面を触られている限りは処理は発動しない
            // このサンプルでは画面B(緑)を3秒以内にポチポチしていれば画面Aには遷移しない
            // 画面Bの文字の外側はUserControlの外側なので、この部分をポチポチしていても画面Aに遷移してしまう
            // これを防ごうとすると画面Bの画面領域を広げないといけないと思われる(x,yを指定するという意味)
            // これを頑張るかはこだわり次第と思う（もっといい方法があるかも...)
            screenTimer?.Update(); 
        }
    }
}
