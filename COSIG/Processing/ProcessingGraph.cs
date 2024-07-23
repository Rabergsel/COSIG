using ConsoleTables;
using COSIG.Processing.Setup;

namespace COSIG.Processing
{
    public class ProcessingGraph
    {
        internal List<Edge> _edges = new List<Edge>();
        internal List<Node> _nodes = new List<Node>();

        private SetupBase Setup = new DefaultSetup();

        /// <summary>
        /// Adds a node to the graph
        /// </summary>
        /// <param name="node">The node to add</param>
        /// <returns>The ID of the Node when added</returns>
        public string AddNode(Node node)
        {
            if (node.ID == "")
            {
                node.ID = node.Name.ToString().Replace(" ", "_").ToLower() + "_" + _nodes.Count().ToString();
            }

            _nodes.Add(node);
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

            //Check whether both _nodes are in Graph
            if (fromNode == null)
            {
                throw new KeyNotFoundException("Node " + edge.FromID + " was not found! (Edge.FromID)");
            }

            if (toNode == null)
            {
                throw new KeyNotFoundException("Node " + edge.ToID + " was not found! (Edge.ToID)");
            }

            //Check whether output and input are the same or not
            //if (fromNode.OutputType != toNode.InputType) throw new TypeLoadException("The types of the nodes are not compatible\nOutput Type: " + fromNode.OutputType.ToString() + "\nInput Type: " + toNode.OutputType.ToString());



            _edges.Add(edge);

        }

        /// <summary>
        /// Gets node by ID from list
        /// </summary>
        /// <param name="ID">The ID of the node</param>
        /// <returns>The Node with the ID</returns>
        private Node GetNode(string ID)
        {
            return _nodes.First(n => n.ID == ID);
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
            Setup.Setup(ref _nodes, ref _edges);

            List<Node> CurrentNodes = new List<Node>();
            CurrentNodes = FindEntryNodes();
            ToTable();
            while (true)
            {
                foreach (var n in CurrentNodes)
                {
                    n.Run();
                }

                CurrentNodes = GetAllChildrenNodes(CurrentNodes);
                if (CurrentNodes.Count == 0)
                {
                    break;
                }
            }

        }

        /// <summary>
        /// Finds all _nodes where no edges are pointing to
        /// </summary>
        /// <returns>List of _nodes without incoming edges</returns>
        private List<Node> FindEntryNodes()
        {
            Dictionary<string, int> EntryCounter = new Dictionary<string, int>();

            //Register all _nodes
            foreach (var n in _nodes)
            {
                EntryCounter.Add(n.ID, 0);
            }

            //Count ToIDs
            foreach (var e in _edges)
            {
                //When adding edges the IDs are already checked
                //The ID exists guaranteed
                EntryCounter[e.ToID]++;
            }

            //Get all IDs where there were no Entry _edges
            var ids = EntryCounter.Keys.Where(k => EntryCounter[k] == 0).ToList();

            //Find all nodes to the IDs
            List<Node> entrynodes = new List<Node>();
            foreach (var id in ids)
            {
                entrynodes.Add(GetNode(id));
            }

            return entrynodes;
        }

        private List<Node> GetAllChildrenNodes(IEnumerable<Node> nodes)
        {
            List<Node> children = new List<Node>();

            foreach (var node in nodes) { children.AddRange(GetAllChildrenNodes(node)); }

            return children;
        }

        private List<Node> GetAllChildrenNodes(Node node)
        {
            List<Node> children = new List<Node>();
            foreach (var e in _edges)
            {
                if (e.FromID == node.ID)
                {
                    children.Add(GetNode(e.ToID));
                }
            }
            return children;
        }

        public void ToTable()
        {
            var table = new ConsoleTable("#", "ID", "Name", "Description", "Input", "Output");

            int i = 0;
            foreach (var node in _nodes)
            {
                var ins = "";
                var o = "";
                foreach (var inp in node.InputFiles) { ins += inp + " | "; }
                foreach (var outp in node.OutputFiles) { o += outp + " | "; }

                table.AddRow(i, node.ID, node.Name, node.Description, ins, o);
                i++;
            }

            table.Write();

        }

    }
}
