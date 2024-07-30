using COSIG.Processing;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace COSIG.Nodes.Web
{
    /// <summary>
    /// A Node that will remove all HTML Tags
    /// </summary>
    public class HTMLRemoverNode : Node
    {

        public HTMLRemoverNode(string InputFile, string OutputFile) : base(InputFile, OutputFile, typeof(string), typeof(string), "", "HtmlRemoverNode", "Removes HTML Tags from a HTML String")
        {

        }

        List<APIObject> CleanPages = new List<APIObject>();

        public override void Load()
        {
            CleanPages.Clear();
        }


        public override void Work()
        {
            foreach(var p in APIObjects) 
            {
                if (!p.IsType(typeof(Tuple<string, string>))) throw new FormatException("Expected type <string, string>, got " + p.type + " instead");

                var tuple = JsonSerializer.Deserialize<Tuple<string, string>>(p.data.ToString());

                var doc = new HtmlDocument();
                doc.LoadHtml(tuple.Item2);
                var text = doc.DocumentNode.InnerText;

                CleanPages.Add(new(new Tuple<string, string>(tuple.Item1, text)));

            }
        }

        public override void Save(string FilePath)
        {
            File.WriteAllText(FilePath, System.Text.Json.JsonSerializer.Serialize(CleanPages));
        }

    }
}
