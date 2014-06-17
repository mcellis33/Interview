using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public static class Dijkstra
    {
        public static void Test()
        {
            Test1();
            Test2();
        }

        private static void Test1()
        {
            var G = new Graph();
            var a = G.AddNode("a");
            var b = G.AddNode("b");
            var c = G.AddNode("c");
            var d = G.AddNode("d");
            G.AddEdge(a, b, 1);
            G.AddEdge(b, d, 3);
            G.AddEdge(a, c, 3);
            G.AddEdge(c, d, 1);
            G.AddEdge(b, c, 1);
            Console.WriteLine(G.ToString());
            LinkedList<Graph.Node> shortestPath;
            int shortestPathLength = G.Dijkstra(a, d, out shortestPath);
            Console.WriteLine("Shortest path length: {0}", shortestPathLength);
            foreach (var n in shortestPath)
            {
                if (n != d)
                    Console.Write("{0}-->", n);
                else
                    Console.Write(n);
            }
        }

        private static void Test2()
        {
            var G = new Graph();
            var a = G.AddNode("a");
            var b = G.AddNode("b");
            var c = G.AddNode("c");
            var d = G.AddNode("d");
            var e = G.AddNode("e");
            G.AddEdge(a, c, -1);
            G.AddEdge(a, e, 10);
            G.AddEdge(b, a, 1);
            G.AddEdge(b, d, -1);
            G.AddEdge(c, b, 1);
            G.AddEdge(c, d, 2);
            G.AddEdge(d, c, 2);
            G.AddEdge(d, e, 3);
            G.AddEdge(e, b, -1);
            Console.WriteLine(G.ToString());
            LinkedList<Graph.Node> shortestPath;
            int shortestPathLength = G.Dijkstra(a, e, out shortestPath);
            Console.WriteLine("Shortest path length: {0}", shortestPathLength);
            foreach (var n in shortestPath)
            {
                if (n != e)
                    Console.Write("{0}-->", n);
                else
                    Console.Write(n);
            }
        }
    }

    public class Graph
    {
        // Wrapper for the Node class that holds information used for Dijkstra's algorithm.
        // The current best offer and the node that made that offer are recorded.
        private class DijkstraInfo : IComparable<DijkstraInfo>
        {
            public int BestOffer;
            public Node BestOfferNode;
            public Node MyNode;

            public DijkstraInfo(Node myNode)
            {
                MyNode = myNode;
                BestOffer = int.MaxValue;
                BestOfferNode = null;
            }

            public int CompareTo(DijkstraInfo other)
            {
                return this.BestOffer.CompareTo(other.BestOffer);
            }
        }

        // Returns the shortest path length from start to end
        // int.MaxValue indicates an "infinite" path length, that is, there is no path from start to end
        // shortestPath is the list of nodes that are on the shortest path, from start to end inclusive
        public int Dijkstra(Node start, Node end, out LinkedList<Node> shortestPath)
        {
            // Wrap all of the nodes with Dijkstra info
            var info = new Dictionary<Node, DijkstraInfo>();
            foreach (var n in nodes)
            {
                info.Add(n, new DijkstraInfo(n));
            }

            // Store the unvisited node in a priority queue organized by best offer score
            Heap<DijkstraInfo> unvisited = new Heap<DijkstraInfo>(info.Values);

            // Set the start node with a best offer of 0
            Node current = start;
            info[current].BestOffer = 0;

            while (unvisited.Count > 0 && current != end)
            {
                // The current node makes an offer to each neighbor: the shortest path from start to that neighbor
                // If the offer is better than the offer that the neighbor currently holds, then update the neighbor with that offer
                foreach (Edge e in current.Edges)
                {
                    int offer = info[current].BestOffer + e.Weight;

                    if (offer < info[e.Target].BestOffer)
                    {
                        info[e.Target].BestOffer = offer;
                        info[e.Target].BestOfferNode = current;

                        // Update the changed info's position in the heap by removing and re-adding it
                        unvisited.Remove(info[e.Target]);
                        unvisited.Add(info[e.Target]);
                    }
                }
                current = unvisited.Pop().MyNode;
            }

            int shortestPathLength = info[end].BestOffer;

            // If the end node has not been given any offers,
            // Then its score will remain int.MaxValue,
            // which indicates that there is no path from the start node to the end node.
            if (shortestPathLength == int.MaxValue)
            {
                shortestPath = null;
            }
            // Backtrack through the best offers from end to start to reconstruct the shortest path
            else
            {
                shortestPath = new LinkedList<Node>();
                current = end;
                while (current != start)
                {
                    shortestPath.AddFirst(current);
                    current = info[current].BestOfferNode;
                }
                shortestPath.AddFirst(start);
            }
            return shortestPathLength;
        }

        public List<Node> nodes { get; private set; }

        public Graph()
        {
            nodes = new List<Node>();
        }

        public Node AddNode(string label)
        {
            Node n = new Node(label);
            nodes.Add(n);
            return n;
        }

        public Edge AddEdge(Node source, Node target, int weight)
        {
            Edge e = new Edge(source, target, weight);
            source.Edges.Add(e);
            return e;
        }

        public class Node
        {
            public string Label { get; private set; }
            public List<Edge> Edges { get; private set; }

            public Node(string label)
            {
                Label = label;
                Edges = new List<Edge>();
            }

            public override string ToString()
            {
                return Label;
            }
        }

        public class Edge
        {
            public Node Source { get; private set; }
            public Node Target { get; private set; }
            public int Weight { get; private set; }

            public Edge(Node source, Node target, int weight)
            {
                Source = source;
                Target = target;
                Weight = weight;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var n in nodes)
            {
                sb.AppendLine(n.ToString());
                foreach (var e in n.Edges)
                {
                    sb.AppendFormat("  -{0}-> {1}", e.Weight, e.Target.ToString());
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
