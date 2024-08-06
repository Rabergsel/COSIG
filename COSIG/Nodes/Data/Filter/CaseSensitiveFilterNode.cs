using COSIG.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace COSIG.Nodes.Data.Filter
{
    internal class CaseSensitiveFilterNode : Node
    {

        List<APIObject> FilteredObjects = new List<APIObject>();

        public CaseSensitiveFilterNode(string InputFile, string OutputFile, string Keyword) : base(InputFile, OutputFile, typeof(object[]), typeof(object[]), "", "CaseSensitiveFilterNode", "Only propagates Objects with the keyword " + Keyword + " (case sensitive)")
        {
            Configuration.TryAdd("filterword", Keyword);
        }

        public override void Load()
        {
            FilteredObjects.Clear();            
        }

        public override void Work()
        {
            foreach(var apiobject in APIObjects)
            {
                if (apiobject.data.ToString().Contains(Configuration["filterword"]))
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
