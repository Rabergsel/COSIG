using COSIG;
using COSIG.Nodes;
using System.Text.Json;

var URLs = new string[3] { "http://www.example.com", "https://de.wikipedia.org", "https://en.wikipedia.org" };

File.WriteAllText("in.json", JsonSerializer.Serialize(URLs));
var node = new HtmlDownloadNode() { InputFile = "in.json", OutputFile = "out.json" };
node.Run();

Console.ReadLine();