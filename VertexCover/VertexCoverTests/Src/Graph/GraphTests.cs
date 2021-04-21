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

        [TestMethod()]
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
        public void AreConnectedTest()
        {
            Assert.IsTrue(graph.AreConnected(graph.Vertices.ElementAt(1), graph.Vertices.ElementAt(2)));
        }

        [TestMethod()]
        public void AreConnectedTest_Reversed()
        {
            Assert.IsTrue(graph.AreConnected(graph.Vertices.ElementAt(2), graph.Vertices.ElementAt(1)));
        }

        [TestMethod()]
        public void AreConnectedTest_WhenNotConnected()
        {
            Assert.IsFalse(graph.AreConnected(graph.Vertices.ElementAt(0), graph.Vertices.ElementAt(2)));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AreConnectedTest_NullCheckFirstVertex()
        {
            Assert.IsTrue(graph.AreConnected(null, graph.Vertices.ElementAt(2)));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AreConnectedTest_NullCheckSecondVertex()
        {
            Assert.IsTrue(graph.AreConnected(graph.Vertices.ElementAt(2), null));
        }

        [TestMethod()]
        public void GetEdgesTest()
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
        public void GetEdgesTest_UnknownVertex()
        {
            List<Edge> edges = new List<Edge>();
            CollectionAssert.AreEqual(graph.GetEdges(new Vertex(2398)).ToArray(), edges);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEdgesTest_NullCheckVertex()
        {
            graph.GetEdges(null);
        }

        [TestMethod()]
        public void AddEdgeTest()
        {
            int count = graph.Edges.Count;
            Edge edge = new Edge(graph.Vertices.ElementAt(4), graph.Vertices.ElementAt(2));
            graph.AddEdge(edge);
            Assert.IsTrue(graph.Edges.Contains(edge));
            Assert.AreEqual(count + 1, graph.Edges.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddEdgeTest_NullCheckVertex()
        {
            graph.AddEdge(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEdgeTest_InvalidStartVertex()
        {
            Edge edge = new Edge(graph.Vertices.First(), new Vertex(838383));
            graph.AddEdge(edge);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEdgeTest_InvalidEndVertex()
        {
            Edge edge = new Edge(new Vertex(838383), graph.Vertices.First());
            graph.AddEdge(edge);
        }

        [TestMethod()]
        public void RemoveEdgeTest()
        {
            int count = graph.Edges.Count;
            Edge edge = graph.Edges.First();
            Assert.IsTrue(graph.RemoveEdge(edge));
            Assert.IsFalse(graph.Edges.Contains(edge));
            Assert.AreEqual(count - 1, graph.Edges.Count);
        }

        [TestMethod()]
        public void RemoveEdgeTest_NullAsParameter()
        {
            int count = graph.Edges.Count;
            Assert.IsFalse(graph.RemoveEdge(null));
            Assert.AreEqual(count, graph.Edges.Count);
        }

        [TestMethod()]
        public void RemoveEdgeTest_UnknownEdge()
        {
            int count = graph.Edges.Count;
            Edge edge = new Edge(new Vertex(444), new Vertex(231));
            Assert.IsFalse(graph.RemoveEdge(edge));
            Assert.AreEqual(count, graph.Edges.Count);
        }

        [TestMethod()]
        public void AddVertexTest()
        {
            int count = graph.Vertices.Count;
            Vertex vertex = new Vertex(2000);
            graph.AddVertex(vertex);
            Assert.IsTrue(graph.Vertices.Contains(vertex));
            Assert.AreEqual(count + 1, graph.Edges.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddVertexTest_NullAsVertexParameter()
        {
            graph.AddVertex(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void AddVertexTest_AlreadyAddedVertex()
        {
            Vertex vertex = graph.Vertices.First();
            graph.AddVertex(vertex);
        }

        [TestMethod()]
        public void RemoveVertexTest()
        {
            int count = graph.Vertices.Count;
            Vertex vertex = graph.Vertices.First();

            int edgeCount = graph.Edges.Count - graph.GetEdges(vertex).Count();

            Assert.IsTrue(graph.RemoveVertex(vertex));
            Assert.IsFalse(graph.Vertices.Contains(vertex));
            Assert.AreEqual(count - 1, graph.Vertices.Count);
            Assert.AreEqual(edgeCount, graph.Edges.Count);
        }

        [TestMethod()]
        public void RemoveVertexTest_UnknownVertex()
        {
            int count = graph.Vertices.Count;
            Vertex vertex = new Vertex(23123);

            int edgeCount = graph.Edges.Count;

            Assert.IsFalse(graph.RemoveVertex(vertex));
            Assert.AreEqual(count, graph.Vertices.Count);
            Assert.AreEqual(edgeCount, graph.Edges.Count);
        }
    }
}