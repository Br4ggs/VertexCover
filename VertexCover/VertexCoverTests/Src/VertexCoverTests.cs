using Microsoft.VisualStudio.TestTools.UnitTesting;
using VertexCover.Src;

namespace VertexCover.Tests
{
    [TestClass()]
    public class VertexCoverTests
    {
        private bool[,] adjacencyMatrix =
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
        public void GetVertexCover()
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
        public void GetVertexCoverThatIsNotPossible()
        {
            bool[] cover = VertexCoverUtils.GetVertexCover(graph, (uint)graph.Vertices.Count);
            Assert.AreEqual(cover, null);
        }
    }
}