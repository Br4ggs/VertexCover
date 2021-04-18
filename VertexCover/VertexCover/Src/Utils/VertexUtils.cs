using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover.Utils
{
    public static class VertexUtils
    {
        private static readonly Random random = new Random();

        public static void TransformVertexWeight(Graph graph, Vertex vertex, uint value)
        {
            IEnumerable<Edge> adjacentEdges = graph.GetEdges(vertex);
            var edges = adjacentEdges as Edge[] ?? adjacentEdges.ToArray();
            int difference = edges.Length - (int)value;
            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    graph.RemoveEdge(edges[i]);
                }
            }
            else if (difference < 0)
            {
                for (int i = 0; i < Math.Abs(difference); i++)
                {
                    Edge edge = new Edge(graph.Vertices.ElementAt(random.Next(graph.Vertices.Count)), vertex);
                    graph.AddEdge(edge);
                }

            }
        }
    }
}
