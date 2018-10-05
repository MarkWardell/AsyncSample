using System;
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
using KattisUtilities;
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

      
        private async void startButtonClickAsync(object sender, RoutedEventArgs e)
        {
            resultsTextBox.Clear();

           
            int n = await GrabAllKattisAsync();
            
           
        }

        private async Task <int> GrabAllKattisAsync()
        {
            // Make a list of web addresses.  
            List<string> urlList = SetUpURLList();

            // Declare an HttpClient object and increase the buffer size. The  
            // default buffer size is 65,536.  
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };

            // Create a query.  
            IEnumerable<Task<int>> downloadTasksQuery =
                from url in urlList select ProcesKattissURL(url, client);

            // Use ToArray to execute the query and start the download tasks.  
            Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

            // You can do other work here before awaiting.  

            // Await the completion of all the running tasks.  
            int[] lengths = await Task.WhenAll(downloadTasks);

            //// The previous line is equivalent to the following two statements.  
            //Task<int[]> whenAllTask = Task.WhenAll(downloadTasks);  
            //int[] lengths = await whenAllTask;  

            
             return lengths.Sum();

        }

        private List<string> SetUpURLList()
        {
            List<string> urls = new List<string>
            {
                "https://open.kattis.com/problems",
                "https://open.kattis.com/problems?page=1",
                "https://open.kattis.com/problems?page=2",
                "https://open.kattis.com/problems?page=3",
                "https://open.kattis.com/problems?page=4",
                "https://open.kattis.com/problems?page=5",
                "https://open.kattis.com/problems?page=6",
                "https://open.kattis.com/problems?page=7",
                "https://open.kattis.com/problems?page=8"

            };
            return urls;
        }

     
        // The actions from the foreach loop are moved to this async method.  
        async Task<int> ProcesKattissURL(string url, HttpClient client)
        {
            UrlItem urlItem = new UrlItem(url, "");
            int n = await urlItem.Grab(client);
            resultsTextBox.Text += urlItem.Html;
            int retVal = urlItem.Html.Length;
            

            return retVal;
        }


        private void resultsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lblTotal != null)
            lblTotal.Text  = resultsTextBox.Text.Length.ToString();
        }
    }
}
