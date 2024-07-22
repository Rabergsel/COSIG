﻿namespace COSIG.Processing
{
    public abstract class Node
    {
        /// <summary>
        /// Where to get the Input Data from
        /// </summary>
        public string InputFile { get; set; } = "";

        /// <summary>
        /// Where to write the output to
        /// </summary>
        public string OutputFile { get; set; } = "";

        public Type InputType { get; internal set; } = typeof(object);
        public Type OutputType { get; internal set; } = typeof(object);


        /// <summary>
        /// Graph ID of node
        /// </summary>
        public string ID { get; set; } = "";

        /// <summary>
        /// Name of node
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Description of what the node does
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// A function to call whenever someone wants to report progress
        /// </summary>
        public virtual void ReportProgress()
        {
            Console.WriteLine($"Node {ID} reports progress");
        }

        /// <summary>
        /// The function doing the actual work of the node
        /// </summary>
        public abstract void Work();

        /// <summary>
        /// Loads all data from input file
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Saves all data to output file
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Runs the whole process of the node
        /// </summary>
       public void Run()
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting Node " + ID + "\tName = " + Name + "\tDescription = " + Description);
            Console.ResetColor();
#endif
            Load();
            Work();
            Save();

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ending Node " + ID + "\tName = " + Name + "\tDescription = " + Description);
            Console.ResetColor();
#endif

        }

        public Node() { }

        public Node(string InputFile, string OutputFile)
        {
            this.InputFile = InputFile;
            this.OutputFile = OutputFile;
        }

        internal Node(string inputFile, string outputFile, Type inputType, Type outputType, string iD, string name, string description) : this(inputFile, outputFile)
        {
            InputType = inputType;
            OutputType = outputType;
            ID = iD;
            Name = name;
            Description = description;
        }
    }
}
