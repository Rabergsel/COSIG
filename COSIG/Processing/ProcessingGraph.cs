using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSIG.Processing
{
    internal class ProcessingGraph
    {
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public List<Node> Nodes { get; set; } = new List<Node>();

    }
}
