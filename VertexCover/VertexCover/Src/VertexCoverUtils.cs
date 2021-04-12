using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup.Localizer;
using VertexCover.Src;

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
        public static bool[] GetVertexCover(Graph graph, uint size)
        {
            bool[] cover = new bool[graph.Vertices.Count];
            return size < graph.Vertices.Count && IsVertexCoverPossible(graph, cover, 0, size) ? cover : null;
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
        private static bool IsVertexCoverPossible(Graph graph, bool[] cover, uint depth, uint requestSize)
        {
            if (cover.Length <= depth)
                return false;

            int values = cover.Count(value => value);
            if (values == requestSize)
            {
                return Validate(graph, cover);
            }
            else if (values > requestSize)
            {
                return false;
            }

            cover[depth] = true;
            bool valid = IsVertexCoverPossible(graph, cover, depth + 1, requestSize);
            cover[depth] = valid;
            return valid || IsVertexCoverPossible(graph, cover, depth + 1, requestSize);

        }

        /// <summary>
        /// Check if the vertex cover is valid
        /// </summary>
        /// <param name="graph">The adjacency matrix</param>
        /// <param name="cover">The cover you want to test the adjacency matrix on</param>
        /// <returns>True if the cover is valid</returns>
        private static bool Validate(Graph graph, bool[] cover)
        {
            List<Edge> edges = new List<Edge>();
            for (int i = 0; i < cover.Length; i++)
            {
                if (cover[i])
                {
                    edges.AddRange(graph.GetEdges(graph.Vertices.ElementAt(i)));
                }
            }

            int test = edges.Distinct().Count();
            return test == graph.Edges.Count;
        }

        /// <summary>
        /// Get all the edges for a specific vertex
        /// </summary>
        /// <param name="adjacencyMatrix">The adjacency Matrix</param>
        /// <param name="vertex">The vertex you want its neighbors for</param>
        /// <returns>The vertices that are adjacent</returns>
        private static int[] GetEdges(bool[,] adjacencyMatrix, int vertex)
        {
            var values = new List<int>();
            for (int i = 0; i < adjacencyMatrix.GetLength(vertex); i++)
            {
                if (adjacencyMatrix[vertex, i])
                {
                    values.Add(i);
                }
            }
            return values.ToArray();
        }
    }
}
