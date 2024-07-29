using COSIG;
using COSIG.Nodes;
using COSIG.Nodes.Flow;
using COSIG.Nodes.Web;
using COSIG.Processing;
using System.Text.Json;

var URLs = new List<APIObject>();

URLs.Add(new("https://www.example.com"));

File.WriteAllText("in.json", JsonSerializer.Serialize(URLs));

ProcessingGraph graph = new ProcessingGraph();

string id0 = graph.AddNode(new EmptyNode("in.json", ""));
string id1 = graph.AddNode(new HTMLDownloaderNode("", ""));
string id2 = graph.AddNode(new HTMLRemoverNode("", ""));

graph.AddEdge(new(id0, id1));
graph.AddEdge(new(id1, id2));

graph.Run();

Console.ReadLine();