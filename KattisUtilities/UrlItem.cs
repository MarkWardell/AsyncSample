using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KattisUtilities
{
    public class UrlItem
    {
        public UrlItem(string url, string html)
        {
            Url = url;
            Html = html;

        }
        public string Url   { get;  set; }
        public string Html  { get; set; }

        public async Task<int> Grab(HttpClient client )
        {
            int length = -1;
           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            var resp = await request.GetResponseAsync();
            var response = (HttpWebResponse)resp;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    Stream receiveStream = response.GetResponseStream();
                    Encoding enCoding = null;


                    if (response.CharacterSet == null)
                    {
                        enCoding = Encoding.Default;
                    }
                    else
                    {
                        enCoding = Encoding.GetEncoding(response.CharacterSet);
                    }
                    //readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    using (StreamReader readStream = new StreamReader(receiveStream, enCoding))

                    {
                        string data = await readStream.ReadToEndAsync();
                        if (data.Contains("<td class=" + '"' + "name_column" + '"' + ">"))
                        {
                            Html = data;
                            length = data.Length;
                           
                        }
                        readStream.Close();
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
 
                    
                    
                }
            return length;
            }
        
           

        


    }
}
