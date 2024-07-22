using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSIG.Nodes.Flow
{
    public class EmptyNode : Processing.Node
    {
        public EmptyNode(string InputFile, string OutputFile) : base(InputFile, OutputFile, typeof(string), typeof(string), "", "EmptyNode", "Passes through and helps as dummy node")
        {

        }

        public override void Load()
        {

        }

        public override void Work()
        {

        }

        public override void Save(string FilePath)
        {

                File.WriteAllText(FilePath, File.ReadAllText(InputFiles[0]));
            
        }
        public override void TidyUp()
        {
            
        }
    }
}
