using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover
{
    public static class VertexCoverUtils
    {
        /// <summary>
        /// Get a vertex cover
        /// </summary>
        /// <param name="graph">The graph you want to use to generate the vertex cover</param>
        /// <param name="size">The size of the vertex cover</param>
        /// <param name="onVertexProcessed">A callback for when a vertex has been processed</param>
        /// <returns>The vertex cover</returns>
        public static List<Vertex> GetVertexCover(Graph graph, int size, Action onVertexProcessed = null)
        {
            Stack<Vertex> cover = new Stack<Vertex>();
            if (size <= graph.Vertices.Count)
                IsVertexCoverPossible(graph, cover, 0, size, onVertexProcessed);


            return cover.ToList();
        }

        /// <summary>
        /// Make sure that is possible to create a vertex cover of a specific size.
        /// The reference to cover will be the valid vertex cover
        /// </summary>
        /// <param name="graph">The graph you wanted to use</param>
        /// <param name="cover">A reference to the cover</param>
        /// <param name="depth">The depth you are at</param>
        /// <param name="requestSize">The size that you want</param>
        /// <param name="onVertexProcessed">A callback for when a vertex has been processed</param>
        /// <returns>True if the vertex cover is possible</returns>
        private static bool IsVertexCoverPossible(Graph graph, Stack<Vertex> cover, int depth, int requestSize, Action onVertexProcessed)
        {
            int values = cover.Count;
            if (values == requestSize)
            {
                return Validate(graph, cover);
            }
            onVertexProcessed?.Invoke();

            if (graph.Vertices.Count <= depth || values >= requestSize)
            {
                return false;
            }

            cover.Push(graph.Vertices.ElementAt(depth));
            if (IsVertexCoverPossible(graph, cover, depth + 1, requestSize, onVertexProcessed))
                return true;


            cover.Pop();
            return IsVertexCoverPossible(graph, cover, depth + 1, requestSize, onVertexProcessed);
        }

        /// <summary>
        /// Check if the vertex cover is valid
        /// </summary>
        /// <param name="graph">The adjacency matrix</param>
        /// <param name="cover">The cover you want to test the adjacency matrix on</param>
        /// <returns>True if the cover is valid</returns>
        private static bool Validate(Graph graph, IEnumerable<Vertex> cover)
        {
            List<Edge> edges = new List<Edge>();
            foreach (var vertex in cover)
            {
                edges.AddRange(graph.GetEdges(vertex));
            }

            int coveredEdges = edges.Distinct().Count();
            return coveredEdges == graph.Edges.Count;
        }

    }
}
