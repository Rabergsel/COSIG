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

        string[] prefixeSI = { "y", "z", "a", "f", "p", "n", "µ", "m", "", "k", "M", "G", "T", "P", "E", "Z", "Y" };
        string numStr(double num)
        {
            int log10 = (int)Math.Log10(Math.Abs(num));
            if (log10 < -27)
                return "0.000";
            if (log10 % -3 < 0)
                log10 -= 3;
            int log1000 = Math.Max(-8, Math.Min(log10 / 3, 8));

            return ((double)num / Math.Pow(10, log1000 * 3)).ToString("###.###" + prefixeSI[log1000 + 8]);
        }

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
            int fails = 0;
            long bytes = 0;
            foreach(var item in APIObjects)
            {
                if (!item.IsType(typeof(string))) throw new FormatException("Expected string, not " + item.type.ToString());
                string url = item.data.ToString();
                try
                {
                    var htmldoc = web.Load(url);

                    Tuple<string, string> htmldata = new Tuple<string, string>(url, htmldoc.DocumentNode.OuterHtml);
                    Pages.Add(new(htmldata));
                    bytes += htmldata.Item2.Length;
                }
                catch
                {
                    fails++;
                }
                ReportProgress("Downloaded " + Pages.Count + "/" + APIObjects.Count + " (Fails: " + fails + ") (Download: " + numStr(bytes) + "B)\t" + url);

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
