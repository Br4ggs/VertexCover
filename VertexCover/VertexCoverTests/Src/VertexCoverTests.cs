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
            bool[] cover = VertexCoverUtils.GetVertexCover(graph, 3);
            bool[] answer =
            {
                false,
                true,
                false,
                true,
                true,
                false,
                false
            };

            CollectionAssert.AreEqual(cover, answer);
        }

        [TestMethod()]
        public void GetVertexCoverSizeFour()
        {
            bool[] cover = VertexCoverUtils.GetVertexCover(graph, 4);
            bool[] answer =
            {
                true,
                true,
                false,
                true,
                true,
                false,
                false
            };

            CollectionAssert.AreEqual(cover, answer);
        }

        [TestMethod()]
        public void GetVertexCoverAllVertices()
        {
            bool[] cover = VertexCoverUtils.GetVertexCover(graph, (uint)graph.Vertices.Count);
            bool[] answer =
            {
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };

            CollectionAssert.AreEqual(cover, answer);
        }


        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_NoValidVertexCoverPossible()
        {
            bool[] cover = VertexCoverUtils.GetVertexCover(graph, 1);

            CollectionAssert.AreEqual(cover, null);
        }

        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_NoValidVertexCoverPossible_Size2()
        {
            bool[] cover = VertexCoverUtils.GetVertexCover(graph, 2);

            CollectionAssert.AreEqual(cover, null);
        }


        [TestMethod()]
        public void GetVertexCoverThatIsNotPossible_LargerSizeThanGraph()
        {
            bool[] cover = VertexCoverUtils.GetVertexCover(graph, (uint)graph.Vertices.Count + 1);
            Assert.AreEqual(cover, null);
        }
    }
}