using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace VertexCover.Extensions.Tests
{
    [TestClass()]
    public class IEnumerableExtensionTests
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
        public void RandomTest()
        {
            Vertex vertex = graph.Vertices.Random();
            Assert.IsTrue(graph.Vertices.Contains(vertex));
        }


        [TestMethod()]
        public void IsEmptyTest_NullEnumerator()
        {
            List<int> testSet = null;
            Assert.IsTrue(testSet.IsEmpty());
        }

        [TestMethod()]
        public void IsEmptyTest_EmptyCollection()
        {
            List<int> testSet = new List<int>();
            Assert.IsTrue(testSet.IsEmpty());
        }

        [TestMethod()]
        public void FilterTest()
        {
            List<Vertex> filter = new List<Vertex>()
            {
                new Vertex(0),
                new Vertex(5),
                new Vertex(6),
            };
            var result = graph.Vertices.Filter(filter);
            foreach (var vertex in filter)
            {
                Assert.IsFalse(result.Contains(vertex));
            }
        }
    }
}