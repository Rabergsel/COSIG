﻿using HtmlAgilityPack;
using System.Text.Json;

namespace COSIG.Nodes.WWW
{
    public class HtmlLinkExtractorNode : Processing.Node
    {
        private List<string> Htmls = new List<string>();
        private List<string> Links = new List<string>();

        public HtmlLinkExtractorNode(string InputFile, string OutputFile) : base(InputFile, OutputFile, typeof(string[]), typeof(string[]), "", "HtmlLinkExtractor", "Extracts links from a HTML string") { }

        public override void Load()
        {
            Htmls.Clear();
            Links.Clear();

            foreach (var file in ExistingInputFiles)
            {
                Htmls.AddRange(JsonSerializer.Deserialize<List<string>>(File.ReadAllText(file)));
            }
        }

        public override void Work()
        {
            foreach (var html in Htmls)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var linkTags = doc.DocumentNode.Descendants("link");
                var linkedPages = doc.DocumentNode.Descendants("a")
                                                  .Select(a => a.GetAttributeValue("href", null))
                                                  .Where(u => !String.IsNullOrEmpty(u));

                Links.AddRange(linkedPages);
            }

        }

        public override void Save(string FilePath)
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(Links));
        }


    }
}
