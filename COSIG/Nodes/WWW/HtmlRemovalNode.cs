using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COSIG.Nodes.WWW
{
    public class HtmlRemovalNode : Processing.Node
    {
        public List<string> HTMLPages { get; set; } = new List<string>();
        public List<string> CleanStrings { get; set; } = new List<string>();

        public HtmlRemovalNode(string inputfile, string outputfile) : base(inputfile, outputfile,
            typeof(string[]), typeof(string[]), "", "HtmlRemoval", "Removes HTML tags from string") { }

        public override void Load()
        {
            HTMLPages.Clear();
            CleanStrings.Clear();

            foreach(var existingFile in ExistingInputFiles)
            {
                HTMLPages.AddRange(JsonSerializer.Deserialize<List<string>>(File.ReadAllText(existingFile))); 
            }

        }

        public override void Save(string FilePath)
        {

                File.WriteAllText(FilePath, JsonSerializer.Serialize(CleanStrings, new JsonSerializerOptions() { WriteIndented = true }));
            
            
        }


        public override void Work()
        {
            foreach(var html in HTMLPages)
            {
                CleanStrings.Add(Regex.Replace(html, "<.*?>", String.Empty));
                ReportProgress();
            }
        }

        public override void ReportProgress()
        {
            Console.WriteLine($"Node ID = " + ID + "; Stripped " + CleanStrings.Count() + "/" + HTMLPages.Count());
        }


    }
}
