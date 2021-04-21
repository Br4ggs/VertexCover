using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace VertexCover.Tests
{
    [TestClass()]
    public class GraphTests
    {
        private readonly bool[,] adjacencyMatrix =
        {
            { false, true, false, false, false, false, false},
            { true, false, true, false, false, false, false},
            { false, true, false, true, true, false, false},
            { false, false, true, false, true, true, true},
            { false, false, true, true, false, true, false},
            { false, false, false, true, true, false, false},
            { false, false, false, true, false, false, false},
        };

        private Graph graph;

        [TestInitialize]
        public void BeforeEach()
        {
            graph = new Graph(adjacencyMatrix);
        }

        [TestMethod()]
        public void GraphTest_WithAdjecancyMatrix()
        {
            List<Vertex> vertices = new List<Vertex>();
            for (int i = 0; i < 7; i++)
            {
                vertices.Add(new Vertex(i));
            }

            List<Edge> edges = new List<Edge>()
            {
                new Edge(vertices[0], vertices[1]),
                new Edge(vertices[1], vertices[2]),
                new Edge(vertices[2], vertices[3]),
                new Edge(vertices[2], vertices[4]),
                new Edge(vertices[3], vertices[4]),
                new Edge(vertices[3], vertices[5]),
                new Edge(vertices[4], vertices[5]),
                new Edge(vertices[3], vertices[6]),

            };

            CollectionAssert.AreEqual((ICollection)graph.Vertices, vertices);
            CollectionAssert.AreEqual((ICollection)graph.Edges, edges);
        }

        public void GraphTest_CopyGraph()
        {
            List<Vertex> vertices = new List<Vertex>();
            for (int i = 0; i < 7; i++)
            {
                vertices.Add(new Vertex(i));
            }

            List<Edge> edges = new List<Edge>()
            {
                new Edge(vertices[0], vertices[1]),
                new Edge(vertices[1], vertices[2]),
                new Edge(vertices[2], vertices[3]),
                new Edge(vertices[2], vertices[4]),
                new Edge(vertices[3], vertices[4]),
                new Edge(vertices[3], vertices[5]),
                new Edge(vertices[4], vertices[5]),
                new Edge(vertices[3], vertices[6]),

            };
            Graph graph1 = new Graph(graph);
            CollectionAssert.AreEqual((ICollection)graph1.Vertices, vertices);
            CollectionAssert.AreEqual((ICollection)graph1.Edges, edges);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void GraphTest_InvalidSizeForAdjacencyMatrix()
        {
            bool[,] values = new bool[1, 2];
            _ = new Graph(values);
        }

        [TestMethod()]
        public void GraphTest_AreConnected()
        {
            Assert.IsTrue(graph.AreConnected(graph.Vertices.ElementAt(1), graph.Vertices.ElementAt(2)));
        }

        [TestMethod()]
        public void GraphTest_AreConnected_Reversed()
        {
            Assert.IsTrue(graph.AreConnected(graph.Vertices.ElementAt(2), graph.Vertices.ElementAt(1)));
        }

        [TestMethod()]
        public void GraphTest_AreConnected_WhenNotConnected()
        {
            Assert.IsFalse(graph.AreConnected(graph.Vertices.ElementAt(0), graph.Vertices.ElementAt(2)));
        }

        [TestMethod()]
        public void GraphTest_GetEdges()
        {
            Vertex[] vertices = graph.Vertices.ToArray();
            List<Edge> edges = new List<Edge>()
            {
                new Edge(vertices[2], vertices[4]),
                new Edge(vertices[3], vertices[4]),
                new Edge(vertices[4], vertices[5]),
            };

            CollectionAssert.AreEqual(graph.GetEdges(vertices[4]).ToArray(), edges);
        }

        [TestMethod()]
        public void GraphTest_AddEdge()
        {
            int count = graph.Edges.Count;
            Edge edge = new Edge(graph.Vertices.ElementAt(4), graph.Vertices.ElementAt(2));
            graph.AddEdge(edge);
            Assert.IsTrue(graph.Edges.Contains(edge));
            Assert.AreEqual(count + 1, graph.Edges.Count);
        }

        [TestMethod()]
        public void GraphTest_RemoveEdge()
        {
            int count = graph.Edges.Count;
            Edge edge = graph.Edges.First();
            graph.RemoveEdge(edge);
            Assert.IsFalse(graph.Edges.Contains(edge));
            Assert.AreEqual(count - 1, graph.Edges.Count);
        }
    }
}