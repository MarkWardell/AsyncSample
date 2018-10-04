﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
/// <summary>
/// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/walkthrough-accessing-the-web-by-using-async-and-await
/// </summary>
namespace AsyncExampleWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void startButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            resultsTextBox.Clear();
            await SumPageSizesAsync();
            resultsTextBox.Text += "\r\nControl returned to startButton_Click.";
            startButton.IsEnabled = true;
        }

        private async Task SumPageSizesAsync()
        {
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
            // Make a list of web addresses.
            List<string> urlList = SetUpURLList();

            var total = 0;
            foreach (var url in urlList)
            {
                // GetURLContents returns the contents of url as a byte array.
                //byte[] urlContents = await GetURLContentsAsync(url);
                byte[] urlContents = await client.GetByteArrayAsync(url);

                DisplayResults(url, urlContents);

                // Update the total.
                total += urlContents.Length;
            }

            // Display the total count for all of the web addresses.
            resultsTextBox.Text +=
                string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", total);
        }
        private async Task<byte[]> GetURLContentsAsync(string url)
        {
            // The downloaded resource ends up in the variable named content.
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL.
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for
            // the response.
            // Note: you can't use HttpWebRequest.GetResponse in a Windows Store app.
            using (WebResponse response = await webReq.GetResponseAsync())
            {
                // Get the data stream that is associated with the specified URL.
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Read the bytes in responseStream and copy them to content.
                    responseStream.CopyTo(content);
                }
            }

            // Return the result as a byte array.
            return content.ToArray();
        }

        private List<string> SetUpURLList()
        {
            var urls = new List<string>
    {
        "http://msdn.microsoft.com/library/windows/apps/br211380.aspx",
        "http://msdn.microsoft.com",
        "http://msdn.microsoft.com/library/hh290136.aspx",
        "http://msdn.microsoft.com/library/ee256749.aspx",
        "http://msdn.microsoft.com/library/hh290138.aspx",
        "http://msdn.microsoft.com/library/hh290140.aspx",
        "http://msdn.microsoft.com/library/dd470362.aspx",
        "http://msdn.microsoft.com/library/aa578028.aspx",
        "http://msdn.microsoft.com/library/ms404677.aspx",
        "http://msdn.microsoft.com/library/ff730837.aspx"
    };
            return urls;
        }

        //private async Task<int> ProcessURLAsync(string url)
        //{
        //    var byteArray = await GetURLContentsAsync(url);
        //    DisplayResults(url, byteArray);
        //    return byteArray.Length;
        //}

        private void DisplayResults(string url, byte[] content)
        {
            // Display the length of each website. The string format
            // is designed to be used with a monospaced font, such as
            // Lucida Console or Global Monospace.
            var bytes = content.Length;
            // Strip off the "http://".
            var displayURL = url.Replace("http://", "");
            resultsTextBox.Text += string.Format("\n{0,-58} {1,8}", displayURL, bytes);
        }

        
    }
}
