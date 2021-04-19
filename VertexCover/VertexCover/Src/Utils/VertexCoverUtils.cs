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
        /// <returns>The vertex cover</returns>
        public static Stack<Vertex> GetVertexCover(Graph graph, int size)
        {
            Stack<Vertex> cover = new Stack<Vertex>();
            return size <= graph.Vertices.Count && IsVertexCoverPossible(graph, cover, 0, size) ? cover : null;
        }

        /// <summary>
        /// Make sure that is possible to create a vertex cover of a specific size.
        /// The reference to cover will be the valid vertex cover
        /// </summary>
        /// <param name="graph">The graph you wanted to use</param>
        /// <param name="cover">A reference to the cover</param>
        /// <param name="depth">The depth you are at</param>
        /// <param name="requestSize">The size that you want</param>
        /// <returns>True if the vertex cover is possible</returns>
        private static bool IsVertexCoverPossible(Graph graph, Stack<Vertex> cover, int depth, int requestSize)
        {

            int values = cover.Count;
            if (values == requestSize)
            {
                return Validate(graph, cover);
            }

            if (graph.Vertices.Count <= depth || values >= requestSize)
            {
                return false;
            }

            cover.Push(graph.Vertices.ElementAt(depth));
            if (IsVertexCoverPossible(graph, cover, depth + 1, requestSize))
                return true;


            cover.Pop();
            return IsVertexCoverPossible(graph, cover, depth + 1, requestSize);
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
