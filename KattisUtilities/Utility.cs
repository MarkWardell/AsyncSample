using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KattisUtilities
{
    public  class Utility
    {
        readonly static char[] chSplitDelim = { '/', '\\' };
        public static string ProblemIdFromURL(string url)
        {
            string[] items = url.Split(Utility.chSplitDelim);
            return items[items.Length - 1];

        }

        public static string TrimHTMLTrash(string html)
        {
            var str = html
                       .Replace(System.Environment.NewLine, string.Empty).Trim();
            str = str.Replace("&nbsp", string.Empty);
            return str;
        }
    }
}


