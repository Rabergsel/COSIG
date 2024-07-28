using COSIG.Processing;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSIG.Nodes.Web
{
    public class HTMLDownloaderNode : Node
    {

        public List<APIObject> Pages = new List<APIObject>();

        public HTMLDownloaderNode(string InputFile, string OutputFile) : base(InputFile, OutputFile, typeof(string), typeof(string), "", "HtmlDownloaderNode", "Downloads HTML Content from URL")
        {

        }

        public override void Load()
        {
            Pages.Clear();
        }

        public override void Work()
        {
            HtmlWeb web = new HtmlWeb();

            foreach(var item in APIObjects)
            {
                if (!item.IsType(typeof(string))) throw new FormatException("Expected string, not " + item.type.ToString());
                Console.WriteLine(item.data.ToString());
                string url = item.data.ToString();

                var htmldoc = web.Load(url);

                Tuple<string, string> htmldata = new Tuple<string, string>(url, htmldoc.DocumentNode.OuterHtml);
                Pages.Add(new(htmldata));

                ReportProgress("Downloaded " + Pages.Count + "/" + APIObjects.Count + "\t" + url);

            }

        }

        public override void Save(string FilePath)
        {
            File.WriteAllText(FilePath, System.Text.Json.JsonSerializer.Serialize(Pages));
        }

        public override void ReportProgress(string Info)
        {
            Console.WriteLine(Info);
        }


    }
}
