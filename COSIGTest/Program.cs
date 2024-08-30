using COSIG;
using COSIG.Nodes.Data;
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
string id1_1 = graph.AddNode(new HTMLLinkExtractorNode("", ""));
string id3 = graph.AddNode(new CollectorNode("", ""));

graph.AddEdge(new(id0, id1));
graph.AddEdge(new(id1, id2));
graph.AddEdge(new(id1, id1_1, 2));
graph.AddEdge(new(id1_1, id1));
graph.AddEdge(new(id2, id3));

graph.Run();

Console.ReadLine();