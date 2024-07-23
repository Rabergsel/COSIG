namespace COSIG.Nodes.Flow
{
    public class WaitNode : Processing.Node
    {
        public string InputContent = "";
        public int MillisecondsWait = 1000;

        public WaitNode(string InputFile, string OutputFile, int WaitMS) : base(InputFile, OutputFile, typeof(string), typeof(string), "", "WaitNode", "Waits " + WaitMS + " ms")
        {
            MillisecondsWait = WaitMS;
        }

        public override void Load()
        {
            InputContent = File.ReadAllText(InputFiles[0]);
        }

        public override void Work()
        {
            Thread.Sleep(MillisecondsWait);

        }

        public override void Save(string FilePath)
        {

            File.WriteAllText(FilePath, InputContent);

        }


    }
}
