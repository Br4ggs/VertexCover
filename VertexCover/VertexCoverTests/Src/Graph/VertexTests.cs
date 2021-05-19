using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VertexCover.Tests
{
    [TestClass()]
    public class VertexTests
    {
        [TestMethod()]
        public void VertexTest()
        {
            const int ID = 0;
            Vertex vertex = new Vertex(ID);
            Assert.AreEqual(vertex.ID, ID);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            const int ID = 0;
            Vertex vertex = new Vertex(ID);
            Vertex vertex2 = new Vertex(ID);
            Assert.IsTrue(Equals(vertex, vertex2));
        }

        [TestMethod()]
        public void EqualsTest_NotEqual()
        {
            const int ID = 0;
            Vertex vertex = new Vertex(ID);
            Vertex vertex2 = new Vertex(3);
            Assert.IsFalse(Equals(vertex, vertex2));
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            const int ID = 2;
            Assert.AreEqual(ID, new Vertex(ID).GetHashCode());
        }
    }
}