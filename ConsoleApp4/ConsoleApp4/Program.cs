using System;
using HtmlAgilityPack;

namespace Node
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = "http://www.w3.org/1999/xhtml";
            var web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            Console.WriteLine(doc.DocumentNode.InnerHtml);
        }

        static void NodeV1()
        {
            
        }
    }
}