using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover.Src
{
    public class Graph
    {
        public IReadOnlyCollection<Edge> Edges => edges;
        private readonly List<Edge> edges;

        public IReadOnlyCollection<Vertex> Vertices => vertices;
        private readonly List<Vertex> vertices;

        public Graph(bool[,] adjacencyMatrix)
        {
            int size = adjacencyMatrix.GetLength(0);
            if (size != adjacencyMatrix.GetLength(1))
            {
                throw new ArgumentException($"{nameof(adjacencyMatrix)} can only be the same height and width");
            }
            edges = new List<Edge>();
            vertices = new List<Vertex>();
            CreateVertices(size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        break;
                    }

                    if (adjacencyMatrix[i, j])
                    {
                        edges.Add(new Edge(vertices[j], vertices[i]));
                    }
                }
            }
        }

        /// <summary>
        /// Create vertices for tis graph
        /// </summary>
        /// <param name="size">The amount of vertices you want to initialize</param>
        private void CreateVertices(int size)
        {
            for (int i = 0; i < size; i++)
            {
                vertices.Add(new Vertex(i));
            }
        }

        /// <summary>
        /// Check if two edges are connected in a graph
        /// </summary>
        /// <param name="vertex">One of the two tested vertices</param>
        /// <param name="otherVertex">The second of the two tested vertices</param>
        /// <returns>True if the two vertices are connected</returns>
        public bool AreConnected(Vertex vertex, Vertex otherVertex)
        {
            return Edges.Any(edge => (edge.StartVertex == vertex && edge.EndVertex == otherVertex) ||
                                     (edge.StartVertex == otherVertex && edge.EndVertex == vertex));
        }

        public IEnumerable<Edge> GetEdges(Vertex vertex)
        {
            return Edges.Where(edge => edge.StartVertex == vertex || edge.EndVertex == vertex);
        }

        public IEnumerable<Edge> GetEdgesStartingHere(Vertex vertex)
        {
            return Edges.Where(edge => edge.StartVertex == vertex);
        }
    }
}

