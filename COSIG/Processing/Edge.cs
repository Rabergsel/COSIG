using COSIG.Nodes.Web;

namespace COSIG.Processing
{
    /// <summary>
    /// Represents a connection between two nodes
    /// </summary>
    public class Edge
    {
        public string FromID { get; set; }
        public string ToID { get; set; }

        public int MaxUses { get; set; } = -1;

        private int CurrentUses = 0;
        public bool IsEdgeExisting()
        {
            if (MaxUses == -1) return true;
            if(CurrentUses <= MaxUses) return true;
            return false;
        }
        public void UseEdge()
        {
            CurrentUses++;
        }

        /// <summary>
        /// Creates an Edge Instance
        /// </summary>
        /// <param name="fromID">ID of the node from where the connection starts</param>
        /// <param name="toID">ID of the node at which the connection ends</param>
        /// <param name="MaxUses">The maximal amounts of uses this edge has before rotting</param>
        /// <example>
        /// ProcessingGraph graph = new ProcessingGraph();
        /// string id1 = graph.AddNode(new HTMLDownloaderNode("", ""));
        /// string id2 = graph.AddNode(new HTMLRemoverNode("", ""));
        /// graph.AddEdge(new(id1, id2));
        /// </example>
        public Edge(string fromID, string toID, int MaxUses = -1)
        {
            FromID = fromID;
            ToID = toID;
            this.MaxUses = MaxUses;
        }
    }
}
