using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VertexCover.Src;
using VertexCover.Utils;

namespace VertexCover.Tests
{
    [TestClass()]
    public class VertexUtilsTests
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
        public void UpdateVertexDegree_AboveCurrentWeight()
        {
            Vertex vertex = graph.Vertices.ElementAt(1);
            VertexUtils.TransformVertexDegree(graph, vertex, 3);
            Assert.AreEqual(graph.GetEdges(vertex).Count(), 3);
        }

        [TestMethod()]
        public void UpdateVertexDegree_BelowCurrentWeight()
        {
            Vertex vertex = graph.Vertices.ElementAt(1);
            VertexUtils.TransformVertexDegree(graph, vertex, 0);
            Assert.AreEqual(graph.GetEdges(vertex).Count(), 0);
        }

        [TestMethod()]
        public void UpdateVertexDegree_ToTheSameValue()
        {
            Vertex vertex = graph.Vertices.ElementAt(1);
            VertexUtils.TransformVertexDegree(graph, vertex, 1);
            Assert.AreEqual(graph.GetEdges(vertex).Count(), 1);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateVertexDegree_GraphNull()
        {
            Vertex vertex = graph.Vertices.ElementAt(1);
            VertexUtils.TransformVertexDegree(null, vertex, 1);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateVertexDegree_VertexNull()
        {
            Vertex vertex = graph.Vertices.ElementAt(1);
            VertexUtils.TransformVertexDegree(null, vertex, 1);
        }
    }
}