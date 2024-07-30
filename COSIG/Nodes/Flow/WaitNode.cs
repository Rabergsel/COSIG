using COSIG.Processing;

namespace COSIG.Nodes.Flow
{
    public class WaitNode : Node
    {

        public WaitNode(string InputFile, string OutputFile, int MillisecondsWait) :
            base(InputFile, OutputFile, typeof(object[]), typeof(object[]), "", "WaitNode", "Waits for " + MillisecondsWait + " milliseconds")
        {
            Configuration.Add("ms", MillisecondsWait.ToString());
        }

        public override void Load()
        {

        }

        public override void Work()
        {
            Thread.Sleep(int.Parse(Configuration["ms"]));
        }

        public override void Save(string FilePath)
        {
            File.WriteAllText(FilePath, System.Text.Json.JsonSerializer.Serialize(APIObjects));
        }

    }
}
