using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KattisUtilities
{

    public class  KattisBaseProblem{
        public string Title     { get; protected set; }
        public string Link      { get; protected set; }
        public string ID        { get; protected set; }
        public DateTime PubDate { get; protected set; }
}
    public class KattisFromHTMLProblem : KattisBaseProblem
    {
        public Dictionary<string, string> MetricsDictionary { get; set; }

        // <td class="name_column"><a href="/problems/visual">Visual Python++</a></td>
        public KattisFromHTMLProblem(string title,
                                     string link,
                                     string id,
                                     Dictionary<string,string> dict)
                                     
        {
            Title = title;
            Link = link;
            ID = id;
            MetricsDictionary = dict;
            
            //XElement myTable = xdoc.Descendants("table").FirstOrDefault(xelem => xelem.Attribute("class").Value == "inner");
            //IEnumerable<IEnumerable<XElement>> myRows = myTable.Elements().Select(xelem => xelem.Elements());

            //foreach (IEnumerable<XElement> tableRow in myRows)
            //{
            //    foreach (XElement rowCell in tableRow)
            //    {
            //        // tada..
            //    }
            //}

        }
    }

    public class KattisFromRSSItem : KattisBaseProblem
    {
        
        public KattisFromRSSItem()
        {


        }
        public KattisFromRSSItem(string title,
                                    string link,
                                    string id,
                                    DateTime pubDate)
        {
            Link = title;
            ID = link;
            Title = id;
            PubDate = pubDate;
        }

        /// <summary>
        /* <title>Bee House Perimeter</title>
      <link>https://open.kattis.com/problems/beehouseperimeter</link>
      <guid>https://open.kattis.com/problems/beehouseperimeter</guid>
      <pubDate>Sat, 15 Sep 2018 04:00:00 +0200</pubDate>
      */
        /// </summary>
        /// <param name="xe"></param>
        readonly char[] chSplitDelim = { '/', '\\' };
        public KattisFromRSSItem(XElement xe)
        {
            Link = xe.Element("link").Value;
            string strID = xe.Element("guid").Value;
            string[] items = strID.Split(chSplitDelim);
            ID = items[items.Length - 1];
            Title = xe.Element("title").Value;
            if (DateTime.TryParse(xe.Element("pubDate").Value, out DateTime dt))
                PubDate = dt;
        }

    }
}
