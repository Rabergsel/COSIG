using COSIG.Processing;
using HtmlAgilityPack;
using System.Text.Json;

namespace COSIG.Nodes.Web
{
    public class HTMLLinkExtractorNode : Node
    {
        private List<APIObject> Links = new List<APIObject>();

        public HTMLLinkExtractorNode(string InputFile, string OutputFile) : base(InputFile, OutputFile, typeof(string), typeof(string), "", "HtmlLinkExtractor", "Returns a list of all links in a HTML File")
        {

        }


        public override void Load()
        {
            Links.Clear();
        }

        public override void Work()
        {
            foreach (var o in APIObjects)
            {
                if (!o.IsType(typeof(string)) & !o.IsType(typeof(Tuple<string, string>)))
                {
                    throw new FormatException("Expected string or Tuple<string, string>, but got " + o.type + " instead");
                }

                if (o.IsType(typeof(string)))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(o.data.ToString());
                    var linkedPages = doc.DocumentNode.Descendants("a")
                                  .Select(a => a.GetAttributeValue("href", null))
                                  .Where(u => !String.IsNullOrEmpty(u));

                    foreach (string link in linkedPages)
                    {
                        Links.Add(new(link));
                    }
                }
                if (o.IsType(typeof(Tuple<string, string>)))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(JsonSerializer.Deserialize<Tuple<string, string>>(o.data.ToString()).Item2);
                    var linkedPages = doc.DocumentNode.Descendants("a")
                                  .Select(a => a.GetAttributeValue("href", null))
                                  .Where(u => !String.IsNullOrEmpty(u));

                    foreach (string link in linkedPages)
                    {
                        Links.Add(new(link));
                    }
                }
            }
        }

        public override void Save(string FilePath)
        {
            File.WriteAllText(FilePath, System.Text.Json.JsonSerializer.Serialize(Links));
        }

    }
}
