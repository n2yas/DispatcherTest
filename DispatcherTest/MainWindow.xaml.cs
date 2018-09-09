using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DispatcherTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        async void btnCounter_Click(object sender, RoutedEventArgs e)
        {
            var cnt = await App.BackgroundDispatcher.InvokeAsync(() => new Counter());

            await Task.WhenAll(Enumerable.Range(0, 10).Select(i => Task.Run(async () =>
            {
                foreach (var j in Enumerable.Range(0, 1000))
                {
                    await cnt.Dispatcher.InvokeAsync(() => cnt.Add(1));
                }
            })));

            MessageBox.Show($"{ await cnt.Dispatcher.InvokeAsync(() => cnt.Count) }");
        }



        async void btnDownloadImage_Click(object sender, RoutedEventArgs e)
        {
            imgMain.Source = await DownloadImageAsync(new Uri(txtUri.Text));
        }

        Task<BitmapImage> DownloadImageAsync(Uri uri)
        {
            var tcs = new TaskCompletionSource<BitmapImage>();

            App.BackgroundDispatcher.InvokeAsync(() => {
                var bmp = new BitmapImage(uri);
                void handler(object sender, EventArgs e)
                {
                    bmp.DownloadCompleted -= handler;
                    bmp.Freeze();
                    tcs.SetResult(bmp);
                }
                bmp.DownloadCompleted += handler;
            });

            return tcs.Task;
        }
    }
}
