using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using COSIG.Processing;

namespace COSIG.Nodes
{
    public class HtmlDownloadNode : Node
    {
        public HtmlDownloadNode() 
        {
            InputType = typeof(string[]);
            OutputType = typeof(string[]);

            Name = "Html Downloader";
            Description = "This node downloads the HTML of all the URLs provided in the input file";

        }

        public List<string> URLs = new List<string>();

        internal List<string> Pages = new List<string>();

        public override void Load()
        {
            URLs = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(InputFile));
        }

        public override void Work()
        {
            foreach(var  url in URLs)
            {
                using (WebClient client = new WebClient())
                {
                    string htmlCode = client.DownloadString(url);
                    Pages.Add(htmlCode);
                    ReportProgress();
                }
            }
        }

        public override void ReportProgress()
        {
            Console.WriteLine("Page " + Pages.Count + "/" + URLs.Count + " downloaded");
        }

        public override void Save()
        {
            File.WriteAllText(OutputFile, JsonSerializer.Serialize(Pages));
        }


    }
}
