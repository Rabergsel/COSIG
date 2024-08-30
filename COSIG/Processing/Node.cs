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

        /// <summary>
        /// Currently existing Input Files
        /// </summary>
        internal List<string> ExistingInputFiles { get; set; } = new List<string>();



        public Dictionary<string, string> Configuration { get; set; } = new Dictionary<string, string>();

        internal List<APIObject> APIObjects { get; set; } = new List<APIObject>();

        /// <summary>
        /// The type for input
        /// </summary>
        public Type InputType { get; internal set; } = typeof(object);

        /// <summary>
        /// The type for Output
        /// </summary>
        public Type OutputType { get; internal set; } = typeof(object);


        /// <summary>
        /// Counter of how often this node has been executed
        /// </summary>
        private int RunCount = 0;


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

        public void ReportProgress(string info)
        {
            Console.WriteLine($"[{ID}]:\t" + info);
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

        /// <summary>
        /// Making a list of which Input Files are existing
        /// </summary>
        internal void CheckInputFiles()
        {
            foreach (var f in InputFiles)
            {
                if (File.Exists(f) & !ExistingInputFiles.Contains(f))
                {
                    ExistingInputFiles.Add(f);
                    APIObjects.AddRange(System.Text.Json.JsonSerializer.Deserialize<List<APIObject>>(File.ReadAllText(f)));
                }
            }

        }

        /// <summary>
        /// A virtual method to check whether this node should be executed or not
        /// </summary>
        /// <returns>True when execution should start</returns>
        public virtual bool CheckForStart()
        {
            if (ExistingInputFiles.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Procedure to call after execution
        /// </summary>
        public virtual void TidyUp()
        {
            foreach (var f in InputFiles)
            {
                if (File.Exists(f))
                {
                    File.Delete(f);
                }
            }
            ExistingInputFiles.Clear();
            APIObjects.Clear();
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
                if (CheckForStart())
                {
                    break;
                }

                Thread.Sleep(1000);
            }

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting Node " + ID + "\tName = " + Name + "\tDescription = " + Description);
            Console.ResetColor();
#endif

            Load();
            Work();
            foreach (var output in OutputFiles)
            {
                Save(output.Replace("{RUN_INDEX}", RunCount.ToString()));
            }

            TidyUp();

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ending   Node " + ID + "\tName = " + Name + "\tDescription = " + Description);
            Console.ResetColor();
#endif
            RunCount++;


        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Node() { }

        /// <summary>
        /// Constructor for users
        /// </summary>
        /// <param name="InputFile">The path to the input file</param>
        /// <param name="OutputFile">The path to the output file</param>
        public Node(string InputFile, string OutputFile)
        {
            if (InputFile != "")
            {
                InputFiles.Add(InputFile);
            }

            if (OutputFile != "")
            {
                OutputFiles.Add(OutputFile);
            }
        }

        /// <summary>
        /// Internal constructor for setting node specific settings
        /// </summary>
        /// <param name="inputFile">Path to input file</param>
        /// <param name="outputFile">Path to output file</param>
        /// <param name="inputType">The type for input</param>
        /// <param name="outputType">The type for output</param>
        /// <param name="iD">ID of node</param>
        /// <param name="name">Name of node</param>
        /// <param name="description">Description of Nodes work</param>
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
