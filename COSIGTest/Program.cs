using COSIG.Nodes;
using COSIG.Nodes.Flow;
using COSIG.Nodes.WWW;
using COSIG.Processing;
using System.Text.Json;

var URLs = new string[1] { "https://pyropixle.com/baxi/" };
File.WriteAllText("in.json", JsonSerializer.Serialize(URLs));

ProcessingGraph graph = new ProcessingGraph();

string id0 = graph.AddNode(new EmptyNode("in.json", ""));
string id1 = graph.AddNode(new HtmlDownloadNode("", ""));
string id2_1 = graph.AddNode(new HtmlLinkExtractorNode("", ""));
string id2_2 = graph.AddNode(new HtmlRemovalNode("", ""));
string id3 = graph.AddNode(new WaitNode("", "", 10000));


graph.AddEdge(new(id0, id1));
graph.AddEdge(new(id1, id2_1));
graph.AddEdge(new(id1, id2_2));
graph.AddEdge(new(id2_1, id3));
graph.AddEdge(new(id3, id1));


graph.Run();

Console.ReadLine();