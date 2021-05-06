using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VertexCover;

//TODO: use mock for vertexcoverkernelizer
namespace VertexCover.Tests
{
    [TestClass]
    public class GraphPreProcessorTests
    {
        private readonly bool[,] matrixWithTwoPendants =
        {
            { false, true, true, false, false },
            { true, false, true, false, false },
            { true, true, false, true, true },
            { false, false, true, false, false },
            { false, false, true, false, false }
        };

        private readonly bool[,] matrixWithNoPendants =
        {
            { false, true, true, false },
            { true, false, false, true },
            { true, false, false, true },
            { false, true, true, false }
        };

        private readonly bool[,] matrixWithThreePendants =
        {
            { false, true, false, false, false, false },
            { true, false, true, false, true, false },
            { false, true, false, true, true, false },
            { false, false, true, false, false, false },
            { false, true, true, false, false, true },
            { false, false, false, false, true, false }
        };

        private GraphPreprocessor graphPreprocessor;

        [TestInitialize]
        public void BeforeEach()
        {
            graphPreprocessor = new GraphPreprocessor(new VertexCoverGraphKernelizer());
        }

        [TestMethod()]
        public void PreprocessesGraphAndReturnsPreProcessedGraphAttributes()
        {
            Graph graphWithTwoPendants = new Graph(matrixWithTwoPendants);
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetProcessedGraph(graphWithTwoPendants, 5);
            
            Assert.AreEqual(2, graphAttributes.IncludedVertices.Count());
            Assert.AreEqual(2, graphAttributes.IncludedVertices.ElementAt(0).ID);
            Assert.AreEqual(1, graphAttributes.IncludedVertices.ElementAt(1).ID);

            Assert.AreEqual(3, graphAttributes.DiscardedVertices.Count());

            Assert.AreEqual(0, graphAttributes.ProcessedGraph.Vertices.Count());
            Assert.AreEqual(0, graphAttributes.ProcessedGraph.Edges.Count());
        }

        [TestMethod()]
        public void PreprocessesGraphAndReturnsSameGraph()
        {
            Graph graphWithNoPendants = new Graph(matrixWithNoPendants);
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetProcessedGraph(graphWithNoPendants, 4);

            Assert.AreEqual(0, graphAttributes.IncludedVertices.Count());

            CollectionAssert.AreEqual(graphAttributes.ProcessedGraph.Vertices.ToList(), graphWithNoPendants.Vertices.ToList());
            CollectionAssert.AreEqual(graphAttributes.ProcessedGraph.Edges.ToList(), graphWithNoPendants.Edges.ToList());
        }

        [TestMethod()]
        public void PreprocessesGraphAndReturnsAllVertices()
        {
            Graph graphWithThreePendants = new Graph(matrixWithThreePendants);
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetProcessedGraph(graphWithThreePendants, 6);

            Assert.AreEqual(3, graphAttributes.IncludedVertices.Count());
            Assert.AreEqual(1, graphAttributes.IncludedVertices.ElementAt(0).ID);
            Assert.AreEqual(2, graphAttributes.IncludedVertices.ElementAt(1).ID);
            Assert.AreEqual(5, graphAttributes.IncludedVertices.ElementAt(2).ID);

            Assert.AreEqual(0, graphAttributes.ProcessedGraph.Vertices.Count);
            Assert.AreEqual(0, graphAttributes.ProcessedGraph.Edges.Count);
        }
    }
}
