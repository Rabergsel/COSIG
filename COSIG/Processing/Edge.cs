namespace COSIG.Processing
{
    public struct Edge
    {
        public string FromID { get; set; }
        public string ToID { get; set; }

        public Edge(string fromID, string toID)
        {
            FromID = fromID;
            ToID = toID;
        }
    }
}
