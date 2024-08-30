using COSIG.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace COSIG.Nodes.Data.Filter
{
    internal class CaseInsensitiveFilterNode : Node
    {
        List<APIObject> FilteredObjects = new List<APIObject>();

        public CaseInsensitiveFilterNode(string InputFile, string OutputFile, string Keyword) : base(InputFile, OutputFile, typeof(object[]), typeof(object[]), "", "CaseSensitiveFilterNode", "Only propagates Objects with the keyword " + Keyword + " (case insensitive)")
        {
            Configuration.TryAdd("filterword", Keyword);
        }

        public override void Load()
        {
            FilteredObjects.Clear();
        }

        public override void Work()
        {
            foreach (var apiobject in APIObjects)
            {
                if (apiobject.data.ToString().ToLower().Contains(Configuration["filterword"].ToLower()))
                {
                    FilteredObjects.Add(apiobject);
                }
            }
        }

        public override void Save(string FilePath)
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(FilteredObjects));
        }
    }
}
