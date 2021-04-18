﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover
{
    public class Graph
    {
        public IReadOnlyCollection<Edge> Edges => edges;
        private readonly List<Edge> edges;

        public IReadOnlyCollection<Vertex> Vertices => vertices;
        private readonly List<Vertex> vertices;

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
            CreateVertices(size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        break;
                    }

                    if (!adjacencyMatrix[i, j])
                        continue;

                    edges.Add(new Edge(vertices[j], vertices[i]));
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
            return Edges.Where(edge => Equals(edge.StartVertex, vertex) || Equals(edge.EndVertex, vertex));
        }

        /// <summary>
        /// From a vertex get all edges that start at this vertex
        /// </summary>
        /// <param name="vertex">The vertex you want to check with</param>
        /// <returns>All adjacent edges</returns>
        public IEnumerable<Edge> GetEdgesStartingHere(Vertex vertex)
        {
            return Edges.Where(edge => Equals(edge.StartVertex, vertex));
        }

        /// <summary>
        /// Get a set of all vertices that are independent
        /// </summary>
        /// <returns>The independent vertices set</returns>
        public IEnumerable<Vertex> GetIndependentVerticesSet()
        {
            return Vertices.Where(vertex => !GetEdges(vertex).Any());
        }

        /// <summary>
        /// Get a set of all vertices with a specific amount of edges
        /// </summary>
        /// <returns>The set of all vertices who satisfies the values</returns>
        public IEnumerable<Vertex> GetVerticesSetWithEdgeCount(int edgeCount)
        {
            return Vertices.Where(vertex => GetEdges(vertex).Count() == edgeCount);
        }

        /// <summary>
        /// Remove a specific edge
        /// </summary>
        /// <param name="edge">The edge you want to delete</param>
        public void RemoveEdge(Edge edge)
        {
            edges.Remove(edge);
        }
    }
}

