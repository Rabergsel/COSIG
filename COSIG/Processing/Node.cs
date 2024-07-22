namespace COSIG.Processing
{
    public abstract class Node
    {
        /// <summary>
        /// Where to get the Input Data from
        /// </summary>
        public List<string> InputFiles { get; set; } = new List<string>();

        /// <summary>
        /// Where to write the output to
        /// </summary>
        public List<string> OutputFiles { get; set; } = new List<string>();

        internal List<string> ExistingInputFiles { get; set; } = new List<string>();


        public Type InputType { get; internal set; } = typeof(object);
        public Type OutputType { get; internal set; } = typeof(object);

        int run = 0;


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
        public abstract void Save(string FilePath);

        internal void CheckInputFiles()
        {
            foreach (var f in InputFiles)
            {
                if (File.Exists(f)) ExistingInputFiles.Add(f);
            }
           
        }

        public virtual bool CheckForStart()
        {
            if(ExistingInputFiles.Count == 0) return false;
            return true;
        }

        public virtual void TidyUp()
        {
            foreach (var f in InputFiles)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            ExistingInputFiles.Clear();
        }


        /// <summary>
        /// Runs the whole process of the node
        /// </summary>
       public void Run()
        {

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Awaiting Node " + ID + "\tName = " + Name + "\tDescription = " + Description);
            Console.ResetColor();
#endif

            while (true)
            {
                CheckInputFiles();
                if (CheckForStart()) break;
                Thread.Sleep(1000);
            }

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting Node " + ID + "\tName = " + Name + "\tDescription = " + Description);
            Console.ResetColor();
#endif

            Load();
            Work();
            foreach(var output in OutputFiles)
            {
                Save(output.Replace("{RUN_INDEX}", run.ToString()));
            }

            TidyUp();

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ending   Node " + ID + "\tName = " + Name + "\tDescription = " + Description);
            Console.ResetColor();
#endif
            run++;


        }

        public Node() { }

        public Node(string InputFile, string OutputFile)
        {
            if(InputFile != "") InputFiles.Add(InputFile);
            if (OutputFile != "") OutputFiles.Add(OutputFile);
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
