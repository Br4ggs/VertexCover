using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover
{
    /// <summary>
    /// Holds all data for a specific graph
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Get all edges in the graph
        /// </summary>
        public IReadOnlyCollection<Edge> Edges => edges;
        private readonly List<Edge> edges;

        /// <summary>
        /// Get all vertices in the graph
        /// </summary>
        public IReadOnlyCollection<Vertex> Vertices => vertices;
        private readonly List<Vertex> vertices;

        private readonly Dictionary<Vertex, List<Edge>> edgeCache;

        /// <summary>
        /// Generate Graph from adjacencyMatrix
        /// </summary>
        /// <param name="adjacencyMatrix">The adjacencyMatrix that you want to create a graph with</param>
        public Graph(bool[,] adjacencyMatrix)
        {
            int size = adjacencyMatrix.GetLength(0);
            if (size != adjacencyMatrix.GetLength(1))
            {
                throw new ArgumentException($"{nameof(adjacencyMatrix)} can only be the same height and width");
            }

            edges = new List<Edge>();
            vertices = new List<Vertex>();
            edgeCache = new Dictionary<Vertex, List<Edge>>();
            CreateVertices(size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size && i != j; j++)
                {
                    if (adjacencyMatrix[i, j])
                        AddEdge(new Edge(vertices[j], vertices[i]));
                }
            }
        }

        /// <summary>
        /// Create a copy of an existing graph
        /// </summary>
        /// <param name="graph">The graph you want to copy</param>
        public Graph(Graph graph)
        {
            edges = new List<Edge>();
            vertices = new List<Vertex>();
            edgeCache = new Dictionary<Vertex, List<Edge>>();
            foreach (var vertex in graph.Vertices)
            {
                AddVertex(new Vertex(vertex.ID));
            }

            foreach (var edge in graph.Edges)
            {
                Vertex vertex = vertices.Find(vertex1 => vertex1.ID == edge.StartVertex.ID);
                Vertex vertex2 = vertices.Find(vertex1 => vertex1.ID == edge.EndVertex.ID);
                AddEdge(new Edge(vertex, vertex2));
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
                AddVertex(new Vertex(i));
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
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (otherVertex == null)
                throw new ArgumentNullException(nameof(otherVertex));

            return Edges.Any(edge => (Equals(edge.StartVertex, vertex) && Equals(edge.EndVertex, otherVertex)) ||
                                     (Equals(edge.StartVertex, otherVertex) && Equals(edge.EndVertex, vertex)));
        }

        /// <summary>
        /// From a vertex get all edges that go to or start at this vertex
        /// </summary>
        /// <param name="vertex">The vertex you want to check with</param>
        /// <returns>All adjacent edges</returns>
        public IEnumerable<Edge> GetEdges(Vertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!edgeCache.ContainsKey(vertex))
                throw new ArgumentException($"{nameof(vertex)} does not belong to this graph.");

            return edgeCache[vertex];
        }

        /// <summary>
        /// Add a single new edge
        /// </summary>
        /// <param name="edge">The edge you want to add</param>
        public void AddEdge(Edge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (!vertices.Contains(edge.StartVertex) || !vertices.Contains(edge.EndVertex))
                throw new ArgumentException();

            edgeCache[edge.StartVertex].Add(edge);
            if (!Equals(edge.StartVertex, edge.EndVertex))
                edgeCache[edge.EndVertex].Add(edge);
            edges.Add(edge);
        }

        /// <summary>
        /// Remove a specific edge
        /// </summary>
        /// <param name="edge">The edge you want to delete</param>
        /// <returns>True if the edge was removed false if it was not</returns>
        public bool RemoveEdge(Edge edge)
        {
            if (!Edges.Contains(edge))
                return false;

            edgeCache[edge.StartVertex].Remove(edge);
            edgeCache[edge.EndVertex].Remove(edge);

            return edges.Remove(edge);
        }

        /// <summary>
        /// Add a vertex to the graph. Only if the vertex is not inside of the graph.
        /// </summary>
        /// <param name="vertex">The vertex you want to add</param>
        public void AddVertex(Vertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (edgeCache.ContainsKey(vertex))
                throw new ArgumentException($"{nameof(vertex)} is already inside of the graph");

            edgeCache.Add(vertex, new List<Edge>());
            vertices.Add(vertex);
        }

        /// <summary>
        /// Remove the vertex from a graph. All edges to the vertex will also be removed.
        /// </summary>
        /// <param name="vertex">The vertex you want to remove</param>
        /// <returns>True it was successfully removed</returns>
        public bool RemoveVertex(Vertex vertex)
        {
            if (!edgeCache.ContainsKey(vertex))
                return false;

            var invalidEdges = GetEdges(vertex).ToArray();
            foreach (var edge in invalidEdges)
            {
                RemoveEdge(edge);
            }
            edgeCache.Remove(vertex);
            return vertices.Remove(vertex);
        }
    }
}

