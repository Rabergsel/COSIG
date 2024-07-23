using COSIG.Processing;
using System.Net;
using System.Text.Json;

namespace COSIG.Nodes
{
    public class HtmlDownloadNode : Node
    {
        public HtmlDownloadNode(string InputFile, string OutputFile) : base(InputFile, OutputFile, typeof(string[]), typeof(string[]), "", "HtmlDownloader", "Download HTML from URL")
        {
        }

        public List<string> URLs = new List<string>();

        internal List<string> Pages = new List<string>();

        public override void Load()
        {
            URLs.Clear();
            Pages.Clear();
            foreach (var file in ExistingInputFiles)
            {
                URLs.AddRange(JsonSerializer.Deserialize<List<string>>(File.ReadAllText(file)));
            }

        }

        public override void Work()
        {
            foreach (var url in URLs)
            {
                if (url == null)
                {
                    continue;
                }

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        string htmlCode = client.DownloadString(url);
                        Pages.Add(htmlCode);
                    }
                    catch
                    {
                        Pages.Add("");
                    }
                    ReportProgress();

                }
            }
        }

        public override void ReportProgress()
        {
            Console.WriteLine("Page " + Pages.Count + "/" + URLs.Count + " downloaded");
        }

        public override void Save(string FilePath)
        {

            File.WriteAllText(FilePath, JsonSerializer.Serialize(Pages, new JsonSerializerOptions() { WriteIndented = true }));


        }


    }
}
