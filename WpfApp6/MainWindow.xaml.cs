using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace WpfApp6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebClient client = new();
        string tmp;
        public MainWindow()
        {
            InitializeComponent();

            client.DownloadProgressChanged += Client_DownloadProgressChanged;
        }

        private async void DonwloadBtnClick(object sender, RoutedEventArgs e)
        {
            tmp = Categories.Children.OfType<RadioButton>().Where(x => x.IsChecked == true).Select(x => x.Content.ToString()).First();
            if (client.IsBusy)
            {
                MessageBox.Show("Try again later!");
                return;
            }


            await DownloadFileAsync($"https://source.unsplash.com/random/{Convert.ToInt32(WidthParameter.Text)}x{Convert.ToInt32(HeightParameter.Text)}/?{tmp}&1");

            // add new item to history
            AddHistoryItem($"https://source.unsplash.com/random/{Convert.ToInt32(WidthParameter.Text)}x{Convert.ToInt32(HeightParameter.Text)}/?{tmp}&1");
        }

        private async Task DownloadFileAsync(string url)
        {
            tmp = Categories.Children.OfType<RadioButton>().Where(x => x.IsChecked == true).Select(x => x.Content.ToString()).First();
            // get desktop path
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // get file name
            string name = $"{tmp}{new Random().Next(1, 10000)}.jpg";

            // create destination file path: "desktop/name.ext"
            string dest = System.IO.Path.Combine(desktopPath, name);

            await client.DownloadFileTaskAsync(url, dest);


        }

        private void AddHistoryItem(string fileName)
        {
            historyList.Items.Add($"{DateTime.Now.ToShortTimeString()}: {fileName}");
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
    }
}
