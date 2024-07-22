using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSIG.Processing
{
    public class ProcessingGraph
    {
        internal List<Edge> Edges { get; set; } = new List<Edge>();
        internal List<Node> Nodes { get; set; } = new List<Node>();

        /// <summary>
        /// Adds a node to the graph
        /// </summary>
        /// <param name="node">The node to add</param>
        /// <returns>The ID of the Node when added</returns>
        public string AddNode(Node node)
        {
            if(node.ID == "")
            {
                node.ID = node.GetType().ToString().Replace(" ", "_").ToLower() + "_" + Nodes.Count().ToString();
            }

            if(node.OutputFile == "")
            {
                node.OutputFile = node.ID + ".out.json";
            }

            Nodes.Add(node);
            return node.ID;
        }

        /// <summary>
        /// Adds edge to list after checking all conditions necessary
        /// </summary>
        /// <param name="edge">The edge to add</param>
        /// <exception cref="KeyNotFoundException">When a node is not found, this exception is thrown</exception>
        /// <exception cref="TypeLoadException">When the output and input types of the nodes are unequal, this exception is thrown</exception>
        public void AddEdge(Edge edge)
        {

            var fromNode = GetNode(edge.FromID);
            var toNode = GetNode(edge.ToID);

            //Check whether both Nodes are in Graph
            if (fromNode == null) throw new KeyNotFoundException("Node " + edge.FromID + " was not found! (Edge.FromID)");
            if (toNode == null) throw new KeyNotFoundException("Node " + edge.ToID + " was not found! (Edge.ToID)");

            //Check whether output and input are the same or not
            if (fromNode.OutputType != toNode.InputType) throw new TypeLoadException("The types of the nodes are not compatible\nOutput Type: " + fromNode.OutputType.ToString() + "\nInput Type: " + toNode.OutputType.ToString());

            //The output File of the from-Node must be the input file of the to-Node
            Nodes[Nodes.IndexOf(toNode)].InputFile = Nodes[Nodes.IndexOf(fromNode)].OutputFile;


            Edges.Add(edge);

        }

        /// <summary>
        /// Gets node by ID from list
        /// </summary>
        /// <param name="ID">The ID of the node</param>
        /// <returns>The Node with the ID</returns>
        private Node GetNode(string ID)
        {
            return Nodes.First(n => n.ID == ID);
        }

        /// <summary>
        /// Adds a node with edge
        /// </summary>
        /// <param name="node">The node to add</param>
        /// <param name="edge">The edge to add</param>
        public void AddNode(Node node, Edge edge)
        {
            AddNode(node);
            AddEdge(edge);
        }

        public void Run()
        {
            List<Node> CurrentNodes = new List<Node>();
            CurrentNodes = FindEntryNodes();

            while(true)
            {
                foreach(var n in CurrentNodes)
                {
                    n.Run();
                }

                CurrentNodes = GetAllChildrenNodes(CurrentNodes);
                if(CurrentNodes.Count == 0) break;
            }

        }

        /// <summary>
        /// Finds all Nodes where no edges are pointing to
        /// </summary>
        /// <returns>List of Nodes without incoming edges</returns>
        private List<Node> FindEntryNodes()
        {
            Dictionary<string, int> EntryCounter = new Dictionary<string, int>();

            //Register all Nodes
            foreach(var n in Nodes)
            {
                EntryCounter.Add(n.ID, 0);
            }

            //Count ToIDs
            foreach(var e in Edges)
            {
                //When adding edges the IDs are already checked
                //The ID exists guaranteed
                EntryCounter[e.ToID]++;
            }

            //Get all IDs where there were no Entry Edges
            var ids = EntryCounter.Keys.Where(k => EntryCounter[k] == 0).ToList();

            //Find all nodes to the IDs
            List<Node> entrynodes = new List<Node>();
            foreach(var  id in ids)
            {
                entrynodes.Add(GetNode(id));
            }

            return entrynodes;
        }

        private List<Node> GetAllChildrenNodes(IEnumerable<Node> nodes)
        {
            List<Node> children = new List<Node>();

            foreach(var node in nodes) { children.AddRange(GetAllChildrenNodes(node)); }

            return children;
        }

        private List<Node> GetAllChildrenNodes(Node node)
        {
            List<Node> children = new List<Node>();
            foreach(var e in Edges)
            {
                if(e.FromID == node.ID)
                {
                    children.Add(GetNode(e.ToID));
                }
            }
            return children;
        }

    }
}
