﻿using System;
using System.Linq;

namespace VertexCover.Utils
{
    public static class VertexUtils
    {
        /// <summary>
        /// Transform a vertex's edges so that it has a specific 
        /// </summary>
        /// <param name="graph">The graph you want to modify</param>
        /// <param name="vertex">The vertex you want to change its edges for</param>
        /// <param name="weight">The weight of the vertex</param>
        public static void TransformVertexDegree(Graph graph, Vertex vertex, uint weight)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            var edges = graph.GetEdges(vertex).ToArray();
            int difference = edges.Length - (int)weight;

            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    graph.RemoveEdge(edges[i]);
                }
            }
            else if (difference < 0)
            {
                var vertices = graph.Vertices.Where(neighbor => !Equals(vertex, neighbor) && !graph.AreConnected(vertex, neighbor)).ToArray();

                for (int i = 0; i < Math.Abs(difference) && i < vertices.Length; i++)
                {
                    Edge edge = new Edge(vertices[i], vertex);
                    graph.AddEdge(edge);
                }

            }
        }
    }
}
