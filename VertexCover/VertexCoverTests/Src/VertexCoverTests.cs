using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VertexCover.Extensions;

namespace VertexCover.Tests
{
    [TestClass()]
    public class VertexCoverTests
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
        public void GetVertexCoverSizeThree()
        {
            List<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, 3);
            List<Vertex> answer = new List<Vertex>()
            {
                new Vertex(1),
                new Vertex(3),
                new Vertex(4)
            };
            answer.Reverse();
            CollectionAssert.AreEqual(cover, answer);
        }

        [TestMethod()]
        public void GetVertexCoverSizeFour()
        {
            List<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, 4);
            List<Vertex> answer = new List<Vertex>()
            {
                new Vertex(0),
                new Vertex(1),
                new Vertex(3),
                new Vertex(4)
            };
            answer.Reverse();
            CollectionAssert.AreEqual(cover, answer);
        }

        [TestMethod()]
        public void GetVertexCoverAllVertices()
        {
            List<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, graph.Vertices.Count);
            List<Vertex> answer = new List<Vertex>()
            {
                new Vertex(0),
                new Vertex(1),
                new Vertex(2),
                new Vertex(3),
                new Vertex(4),
                new Vertex(5),
                new Vertex(6),
            };
            answer.Reverse();
            CollectionAssert.AreEqual(cover, answer);
        }


        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_NoValidVertexCoverPossible()
        {
            List<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, 1);
            Assert.IsTrue(cover.IsEmpty());
        }

        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_NoValidVertexCoverPossible_Size2()
        {
            List<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, 2);
            Assert.IsTrue(cover.IsEmpty());
        }


        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_LargerSizeThanGraph()
        {
            List<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, graph.Vertices.Count + 1);
            Assert.IsTrue(cover.IsEmpty());
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApproximateVertexCoverTest_GraphNull()
        {
            VertexCoverUtils.ApproximateVertexCover(null);
        }

        [TestMethod()]
        public void ApproximateVertexCoverTest()
        {
            List<Vertex> answer = new List<Vertex>()
            {
                new Vertex(0),
                new Vertex(1),
                new Vertex(2),
                new Vertex(3),
                new Vertex(4),
                new Vertex(5),
            };

            CollectionAssert.AreEqual(answer, VertexCoverUtils.ApproximateVertexCover(graph));
        }
    }
}