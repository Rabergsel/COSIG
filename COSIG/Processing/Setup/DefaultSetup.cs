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
        public string DirectoryPath = "./wf/";
        public override void Setup(ref List<Node> Nodes, ref List<Edge> Edges)
        {
            CheckForDirectory();
            SetUpFiles(ref Nodes, ref Edges);
        }

        private void CheckForDirectory()
        {
            if(!Directory.Exists(DirectoryPath)) { Directory.CreateDirectory(DirectoryPath); }
            if (!Directory.Exists(DirectoryPath + "out/")) { Directory.CreateDirectory(DirectoryPath + "out/"); }
        }

        private void SetUpFiles(ref List<Node> Nodes, ref List<Edge> Edges)
        {

            foreach(var Node in Nodes)
            {
                if(Node.OutputFiles.Count == 0)
                {
                    Node.OutputFiles.Add(DirectoryPath + "out/" + Node.ID + ".{RUN_INDEX}.out.json");
                }
            }

            foreach(var edge in Edges)
            {
                var index_from = Nodes.IndexOf(GetNode(Nodes, edge.FromID));
                var index_to = Nodes.IndexOf(GetNode(Nodes, edge.ToID));

                string newOutputFile = DirectoryPath + Nodes[index_from].ID + "_" + Nodes[index_to].ID + ".in.json";

                Nodes[index_from].OutputFiles.Add(newOutputFile);
                Nodes[index_to].InputFiles.Add(newOutputFile);

            }

        }



        private Node GetNode(List<Node> Nodes, string ID)
        {
            return Nodes.First(n => n.ID == ID);
        }

    }
}
