using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace COSIG.Processing.Setup
{
    public class DefaultSetup : SetupBase
    {

        public override void Setup(ref List<Node> Nodes, ref List<Edge> Edges)
        {
            ChangeOutputFile(ref Nodes);
            InOutCoheerence(ref Nodes, ref Edges);
        }


        private void ChangeOutputFile(ref List<Node> Nodes)
        {
            foreach(var node in Nodes)
            {
                if (node.OutputFile == "")
                {
                    node.OutputFile = node.ID + ".out.json";
                }
            }
        }

        private void InOutCoheerence(ref List<Node> Nodes, ref List<Edge> Edges)
        {
            foreach(var edge in Edges)
            {
                var fromNode = GetNode(Nodes, edge.FromID);
                var toNode = GetNode(Nodes, edge.ToID);

                //The output File of the from-Node must be the input file of the to-Node
                Nodes[Nodes.IndexOf(toNode)].InputFile = Nodes[Nodes.IndexOf(fromNode)].OutputFile;

            }


        }

        private Node GetNode(List<Node> Nodes, string ID)
        {
            return Nodes.First(n => n.ID == ID);
        }

    }
}
