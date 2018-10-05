
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KattisUtilities
{
    public class KattisProblemPageGrabber
    {
        
        const string FMT_STRING = @"https://open.kattis.com/problems?page={0}";
        public event EventHandler<PageGrabbedEventArgs> GrabbedPage;
        protected virtual void OnGrabbed(PageGrabbedEventArgs e)
        {

            GrabbedPage?.Invoke(this, e);
        }
      
        int page = 0;
        public int Page
        {
            get { return page; }
            protected set { page = value; }
        }
        public List<string> HTMLPages { get; protected set; }

        public KattisProblemPageGrabber(string fmt = FMT_STRING)
        {
            //int sizePage = 0;
            //int cumulativeSize = 0;
            //double time = 0;
            //double cumulativeTime = 0;
            //HTMLPages = new List<string>();
            //List<string> lstHTML = new List<string>();
            //Boolean bReadPages = true;
            //string pageUrl = string.Empty;
            //while (bReadPages)
            
            //{
            //    Stopwatch sw = new Stopwatch();
            //    sw.Start();
            //    pageUrl = string.Format(fmt, Page++);
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pageUrl);
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        try
            //        {
            //            Stream receiveStream = response.GetResponseStream();
            //            StreamReader readStream = null;
            //            if (response.CharacterSet == null)
            //            {
            //                readStream = new StreamReader(receiveStream);
            //            }
            //            else
            //            {
            //                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                           
            //            }
            //            try
            //            {
            //                string data = readStream.ReadToEnd();
                            
            //                if (data.Contains("<td class=" + '"' + "name_column" + '"' + ">"))
            //                {
            //                    HTMLPages.Add(data);
            //                    lstHTML.Add(data);
            //                    sizePage = data.Length;
            //                    cumulativeSize += sizePage;
            //                    sw.Stop();
            //                    time = sw.Elapsed.TotalSeconds;
            //                    cumulativeTime += time;

            //                    OnGrabbed(new PageGrabbedEventArgs()
            //                    {
            //                        Size = sizePage,
            //                        TotalDownloadSize = cumulativeSize,
            //                        Url = pageUrl,
            //                        Time = time,
            //                        CumulativeTime = cumulativeTime
            //                    });


            //                }
            //                else
            //                    bReadPages = false;

            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex.Message);
            //            }
            //            response.Close();
            //            readStream.Close();
            //        }catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
                    
            //    }
            //    else
            //        bReadPages = false;             

            //}





        }

        public async Task GrabPagesAsync()
        {
            int sizePage = 0;
            int cumulativeSize = 0;
            double time = 0;
            double cumulativeTime = 0;
            HTMLPages = new List<string>();
            List<string> lstHTML = new List<string>();
            Boolean bReadPages = true;
            string pageUrl = string.Empty;
            while (bReadPages)

            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                pageUrl = string.Format(FMT_STRING, Page++);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pageUrl);
                var resp = await request.GetResponseAsync();
                var response = (HttpWebResponse)resp;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;
                        if (response.CharacterSet == null)
                        {
                            readStream = new StreamReader(receiveStream);
                        }
                        else
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                        }
                        try
                        {
                            string data = readStream.ReadToEnd();

                            if (data.Contains("<td class=" + '"' + "name_column" + '"' + ">"))
                            {
                                HTMLPages.Add(data);
                                lstHTML.Add(data);
                                sizePage = data.Length;
                                cumulativeSize += sizePage;
                                sw.Stop();
                                time = sw.Elapsed.TotalSeconds;
                                cumulativeTime += time;

                                OnGrabbed(new PageGrabbedEventArgs()
                                {
                                    Size = sizePage,
                                    TotalDownloadSize = cumulativeSize,
                                    Url = pageUrl,
                                    Time = time,
                                    CumulativeTime = cumulativeTime
                                });


                            }
                            else
                                bReadPages = false;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        response.Close();
                        readStream.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                else
                    bReadPages = false;

            }

        }

    }

    public class PageGrabbedEventArgs : EventArgs
    {
        public int Size { get; set; }
        public string Url { get; set; }
        public double Time { get; set; }
        public double CumulativeTime { get; set; }
        public int TotalDownloadSize { get; set; }

    }
}
