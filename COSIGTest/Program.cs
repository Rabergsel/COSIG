using COSIG;

using COSIG.Nodes;
using COSIG.Nodes.WWW;
using COSIG.Nodes.Flow;

using COSIG.Processing;
using System.Text.Json;

var URLs = new string[3] { "http://www.example.com", "https://de.wikipedia.org", "https://en.wikipedia.org" };
File.WriteAllText("in.json", JsonSerializer.Serialize(URLs));

ProcessingGraph graph = new ProcessingGraph();

string id1 = graph.AddNode(new HtmlDownloadNode("in.json", ""));
string id2 = graph.AddNode(new WaitNode("", "", 5000));
string id3 = graph.AddNode(new HtmlRemovalNode("", ""));

graph.AddEdge(new Edge(id1, id2));
graph.AddEdge(new Edge(id2, id3));

graph.Run();

Console.ReadLine();