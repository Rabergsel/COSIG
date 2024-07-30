namespace COSIG.Processing
{
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

        public Edge(string fromID, string toID, int MaxUses = -1)
        {
            FromID = fromID;
            ToID = toID;
            this.MaxUses = MaxUses;
        }
    }
}
