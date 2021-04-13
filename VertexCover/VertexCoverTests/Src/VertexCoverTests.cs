using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VertexCover.Src;

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
            Stack<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, 3);
            Stack<Vertex> answer = new Stack<Vertex>(new Vertex[]
            {
                new Vertex(1),
                new Vertex(3),
                new Vertex(4)
            });

            CollectionAssert.AreEqual(cover, answer);
        }

        [TestMethod()]
        public void GetVertexCoverSizeFour()
        {
            Stack<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, 4);
            Stack<Vertex> answer = new Stack<Vertex>(new Vertex[]
            {
                new Vertex(0),
                new Vertex(1),
                new Vertex(4),
                new Vertex(5)
            });

            CollectionAssert.AreEqual(cover, answer);
        }

        [TestMethod()]
        public void GetVertexCoverAllVertices()
        {
            Stack<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, graph.Vertices.Count);
            Stack<Vertex> answer = new Stack<Vertex>(new Vertex[]
            {
                new Vertex(0),
                new Vertex(1),
                new Vertex(2),
                new Vertex(3),
                new Vertex(4),
                new Vertex(5),
                new Vertex(6),
            });

            CollectionAssert.AreEqual(cover, answer);
        }


        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_NoValidVertexCoverPossible()
        {
            Stack<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, 1);

            CollectionAssert.AreEqual(cover, null);
        }

        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_NoValidVertexCoverPossible_Size2()
        {
            Stack<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, 2);

            CollectionAssert.AreEqual(cover, null);
        }


        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_LargerSizeThanGraph()
        {
            Stack<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, graph.Vertices.Count + 1);
            Assert.AreEqual(cover, null);
        }
    }
}