using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSIG.Processing
{
    public abstract class SetupBase
    {
        public abstract void Setup(ref List<Node> Nodes, ref List<Edge> Edges);

    }
}
