using COSIG.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace COSIG.Nodes.Data
{
    public class CollectorNode : Node
    {

        public CollectorNode(string InputFile, string OutputFile) : base(InputFile, OutputFile, typeof(object[]), typeof(string), "", "CollectorNode", "Saves all objects when called multiple times")
        {
            Configuration.TryAdd("MaxObjects", "20000");
        }

        public CollectorNode(string InputFile, string OutputFile, int MaxObjects) : base(InputFile, OutputFile, typeof(object[]), typeof(string), "", "CollectorNode", "Saves all objects when called multiple times")
        {
            Configuration.TryAdd("MaxObjects", MaxObjects.ToString());
        }

        List<APIObject> Objects = new List<APIObject>();

        public override void Load()
        {

        }

        public override void Work()
        {

            if (int.Parse(Configuration["MaxObjects"]) < (Objects.Count + APIObjects.Count))
            {
                ReportProgress("Couldn't save objects as it would exceed maximum object count");
                return;
            }

            Objects.AddRange(APIObjects);
            ReportProgress("Saved " + Objects.Count + " objects");
        }

        public override void Save(string FilePath)
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(Objects));
        }

    }
}
