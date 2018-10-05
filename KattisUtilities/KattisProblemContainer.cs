using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace KattisUtilities
{
    public class KattisHTMLScraper : SortedDictionary<string, KattisFromHTMLProblem>
    {
        public KattisHTMLScraper()
        {

        }
        
        public KattisHTMLScraper(string page)
        {
            AppendProblemsOnPage(page);
    
        }

        public void AppendProblemsOnPage(string page)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);
            List<string> columnNames = GetTableColumnNames(doc.DocumentNode.
                SelectSingleNode("//table"));
            List<List<string>> table = doc.DocumentNode.SelectSingleNode("//table")
                        .Descendants("tr")
                        .Skip(1)
                        .Where(tr => tr.Elements("td").Count() > 1)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                        .ToList();

            var myRows = doc.DocumentNode.SelectNodes("//table//td[@class='name_column']");
            int i = 0;
            var stats = table.ToArray();
            foreach (var ht in myRows)
            {
                XElement xe = XElement.Parse(ht.InnerHtml);
                var lnk = $"https://open.kattis.com{xe.Attribute("href").Value}";
                var ttl = xe.Value;
                var t = stats[i++];
                Dictionary<string, string> metricsDict = new Dictionary<string, string>();
                int j = 0;
                foreach (string str in columnNames)
                {

                    metricsDict.Add(str, t[j++]);

                }
                var key = Utility.ProblemIdFromURL(lnk);
                this.Add(key, new KattisFromHTMLProblem(ttl, lnk, key, metricsDict));
            }
        }

        private List<string> GetTableColumnNames(HtmlNode tableNode)
        {
            List<string> lst = new List<string>();

            var trNodes = tableNode.SelectNodes("//thead/tr");
            var keyPrefixes = ParsePrefixArrays(trNodes[0].SelectNodes("*[@colspan]"));
            var headingNodes = trNodes[1].ChildNodes;
            {

                // var thNodes = tr.SelectNodes("//th");
                int k = 0;
                foreach(var th in headingNodes)
                {
                    var anchor = Utility.TrimHTMLTrash(th.InnerText);
                    
                    if (!string.IsNullOrEmpty(anchor) )
                        lst.Add($"{keyPrefixes[k++]}.{anchor}");     
                }
                
            }
            return lst;
        }

        private string[] ParsePrefixArrays(HtmlNodeCollection htmlNodes)
        {  
            string[] retVal = new string[11];
            for (int i= 0; i < retVal.Length;i++)
            {
                retVal[i] = "Problem";
            }       
            int k = 0;
            string curPrefix = "Problem";
            foreach ( HtmlNode th in htmlNodes)
            {
                XElement xe = XElement.Parse(th.OuterHtml);
                var cspan = Int32.Parse(xe.Attribute("colspan").Value);
                
                var strx = Utility.TrimHTMLTrash(xe.Value);
                if (!string.IsNullOrEmpty(strx))
                    curPrefix = strx;
                else
                    curPrefix = "Problem";
                
                for(int i = 0; i < cspan; i++)
                {
                    retVal[k++] = curPrefix;
                }
                
            }
            return retVal;


        }
    }


    
   
}
