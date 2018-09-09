using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DispatcherTest
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static Dispatcher BackgroundDispatcher => CreateBackgroundDispatcherAsync().Result;
        static Task<Dispatcher> CreateBackgroundDispatcherAsync()
        {
            var tcs = new TaskCompletionSource<Dispatcher>();

            var th = new Thread(() => {
                var d = Dispatcher.CurrentDispatcher;
                tcs.SetResult(d);
                Current.Dispatcher.InvokeAsync(() => {
                    Current.Exit += (sender, e) =>
                    {
                        d.InvokeShutdown();
                    };
                });
                Dispatcher.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();

            return tcs.Task;
        }
    }
}
