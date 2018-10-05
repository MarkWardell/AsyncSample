using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KattisUtilities
{
    /// <summary>
    /// Class for reading the KattisRSS XML
    /// </summary>
    public class KattisRSS : SortedDictionary<string, KattisFromRSSItem>
    {
        public KattisRSS(XElement xe)
        {
            loadFromElelment(xe);

        }

        public KattisRSS(System.IO.Stream txtStr)
        {
            XElement xe = XDocument.Load(txtStr).Root;
            loadFromElelment(xe);
        }



        private void loadFromElelment(XElement xe)
        {
            var itemSegs =
                from item in xe.Elements("channel").Elements("item")
                select (XElement)item;
            foreach (var item in itemSegs)
            {
                this.Add(item.Element("title").Value, new KattisFromRSSItem(item));
            }

        }
    }
}
