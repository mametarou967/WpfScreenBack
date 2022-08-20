using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfScreenBack.Models
{
    /// <summary>
    /// 主に画面表示時に、時間(ミリ秒)と処理内容（画面遷移）などを与えておくことで、
    /// 指定した時間経過後に該当の処理を発動させるクラスです
    /// 時間を延長させたい場合はUpdateメソッドを呼んでください(例えば、画面タッチ時など)
    /// また、画面を遷移する際にはかならずScreenTimer.dipose()を呼ぶようにしてください。
    /// 理由としてはタイマーが残ったままになってしまう可能性があるので、画面遷移後も時間満了後に
    /// この処理が走ってしまう可能性があります
    /// </summary>
    public class ScreenTimer : IDisposable
    {
        private DispatcherTimer timer;
        int triggerMillisecond;
        Action executionContentsOnTrigger;

        private bool disposedValue;

        public ScreenTimer(int triggerMillisecond,Action executionContentsOnTrigger)
        {
            this.triggerMillisecond = triggerMillisecond;
            this.executionContentsOnTrigger = executionContentsOnTrigger;
            TimerReset();
        }

        public void Update()
        {
            TimerReset();
        }

        private void TimerReset()
        {
            // 既にタイマーがある場合にはそのタイマーは止めておく
            if (timer != null)
            {
                timer.Stop();
            }

            // 新しいタイマーの時間をセットする
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(triggerMillisecond)
            };

            // タイマーが満了した時の処理を追加する
            timer.Tick += (sender, e) =>
            {
                // 直接触っても例外が起きない
                this.executionContentsOnTrigger();
            };

            // タイマーの計測を開始する
            this.timer.Start();
        }

        private void TimerStop()
        {
            // 既にタイマーがある場合にはそのタイマーは止めておく
            if (this.timer != null)
            {
                this.timer.Stop();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    TimerStop();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~ScreenTimer()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
