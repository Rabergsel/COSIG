﻿using COSIG.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSIG.Nodes.Flow
{
    public class EmptyNode : Node
    {

        public EmptyNode(string InputFile, string OutputFile) : base(InputFile, OutputFile, typeof(object), typeof(object), "", "EmptyNode", "Empty Node")
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

    }
}
