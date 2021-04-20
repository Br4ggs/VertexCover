using Microsoft.VisualStudio.TestTools.UnitTesting;
using VertexCover;
using System;
using System.Collections.Generic;
using System.Text;

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