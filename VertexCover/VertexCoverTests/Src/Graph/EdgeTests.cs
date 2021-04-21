using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace VertexCover.Tests
{
    [TestClass()]
    public class EdgeTests
    {
        [TestMethod()]
        public void EdgeTest()
        {
            Vertex startVertex = new Vertex(0);
            Vertex endVertex = new Vertex(1);

            Edge edge = new Edge(startVertex, endVertex);
            Assert.AreEqual(edge.StartVertex, startVertex);
            Assert.AreEqual(edge.EndVertex, endVertex);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]

        public void EdgeTest_NullStartVertex()
        {
            _ = new Edge(null, new Vertex(1));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EdgeTest_NullEndVertex()
        {
            _ = new Edge(new Vertex(0), null);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Vertex startVertex = new Vertex(0);
            Vertex endVertex = new Vertex(1);
            Edge edge = new Edge(startVertex, endVertex);
            Edge edge2 = new Edge(startVertex, endVertex);
            Assert.IsTrue(Equals(edge, edge2));
        }

        [TestMethod()]
        public void EqualsTest_NotEqual()
        {
            Vertex startVertex = new Vertex(0);
            Vertex endVertex = new Vertex(1);
            Edge edge = new Edge(startVertex, endVertex);
            Edge edge2 = new Edge(startVertex, new Vertex(2));
            Assert.IsFalse(Equals(edge, edge2));
        }

        [TestMethod()]
        public void EqualsTest_ReverseStartAndEndVertex()
        {
            Vertex startVertex = new Vertex(0);
            Vertex endVertex = new Vertex(1);
            Edge edge = new Edge(startVertex, endVertex);
            Edge edge2 = new Edge(endVertex, startVertex);
            Assert.IsFalse(Equals(edge, edge2));
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Vertex startVertex = new Vertex(0);
            Vertex endVertex = new Vertex(1);
            Edge edge = new Edge(startVertex, endVertex);
            Assert.AreEqual(HashCode.Combine(startVertex, endVertex), edge.GetHashCode());
        }
    }
}