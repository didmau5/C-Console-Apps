using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ConsoleWebScraper
{
    class BingWebScraper
    {
        
        public static string BING_URL = "http://www.bing.com/news/search?q=brexit";
        public static string LINK_URL_FILTER = "reuters";
        public static string LINK_CONTENT_FILTER = "Hayley Platt";
		
        //Given a list of strings, prints each
        static void printStringList(List<string> strList)
        {
            Console.WriteLine(String.Join("\n ", strList.ToArray()));
        }

        //Given a URL, extracts html as string
        static string getHtml(string url)
        {
            string html = "";
            //Console.WriteLine("Requesting ... " + url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader responseReader = new StreamReader(responseStream);

                html = responseReader.ReadToEnd();
            }
            catch(WebException e)
            {
                WebResponse response = e.Response;
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                Console.WriteLine("HTTP ERROR CODE:\n {0}\n", httpResponse.StatusCode);
            }
            
            return html;

        }

        //Given html string, returns links as List object
        static List<string> getLinks(string html)
        {
            List<string> links = new List<string>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            foreach(HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string linkString = link.GetAttributeValue("href", string.Empty);
                if(linkString.StartsWith("http://www.") || linkString.StartsWith("https://www."))
                {
                    links.Add(linkString);
                }
            }
            return links;

        }
        
        //Given List of strings and keyword, returns filtered list based on url
        static List<string> filterLinksByUrl(List<string> sourceLinks, string keyword)
        {
            List<string> targetLinks = new List<string>();
            foreach (var link in sourceLinks)
            {
                if (link.Contains(keyword))
                {
                    targetLinks.Add(link);
                }
            }
            return targetLinks;

        }

        //Given List strings and keyword, returns filtered list based on html content at links
        static List<string> filterLinksByContent(List<string> sourceLinks, string keyword)
        {
            List<string> targetLinks = new List<string>();
            foreach (var link in sourceLinks)
            {
                string html = getHtml(link);
                if(html.Contains(keyword))
                {
                    targetLinks.Add(link);
                }
            }
            return targetLinks;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Begining Web Scraper...\n");
         
            //Get Bing News Search Result Links for "Brexit"
            string bingHtml = getHtml(BING_URL);
            List<string> bingLinks  = getLinks(bingHtml);

            //Get Links to Reuters
            List<string> reutersLinks = filterLinksByUrl(bingLinks, LINK_URL_FILTER);

            //Search Reuters Pages for Hayley Platt
            List<string> haleyPlattLinks = filterLinksByContent(reutersLinks, LINK_CONTENT_FILTER);

            //Print list of links mentioning Hayley Platt
            Console.WriteLine("Program Output:\n");
            printStringList(haleyPlattLinks);
        }
    }
}
