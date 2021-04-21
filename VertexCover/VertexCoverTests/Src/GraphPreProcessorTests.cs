using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VertexCover;

namespace VertexCoverTests.Tests
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
            graphPreprocessor = new GraphPreprocessor();
        }

        [TestMethod()]
        public void PreprocessesGraphAndReturnsPreProcessedGraphAttributes()
        {
            Graph graphWithTwoPendants = new Graph(matrixWithTwoPendants);
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetVertexCoverProcessedGraph(graphWithTwoPendants);
            
            Assert.AreEqual(graphAttributes.IncludedVertices.Count(), 1);
            Assert.AreEqual(graphAttributes.IncludedVertices.ElementAt(0).ID, 2);

            Assert.AreEqual(graphAttributes.ProcessedGraph.Vertices.Count(), 2);
            Assert.AreEqual(graphAttributes.ProcessedGraph.Vertices.ElementAt(0).ID, 0);
            Assert.AreEqual(graphAttributes.ProcessedGraph.Vertices.ElementAt(1).ID, 1);
            Assert.AreEqual(graphAttributes.ProcessedGraph.Edges.Count(), 1);
            Assert.AreEqual(graphAttributes.ProcessedGraph.Edges.ElementAt(0).StartVertex, graphAttributes.ProcessedGraph.Vertices.ElementAt(0));
            Assert.AreEqual(graphAttributes.ProcessedGraph.Edges.ElementAt(0).EndVertex, graphAttributes.ProcessedGraph.Vertices.ElementAt(1));
        }

        [TestMethod()]
        public void PreprocessesGraphAndReturnsSameGraph()
        {
            Graph graphWithNoPendants = new Graph(matrixWithNoPendants);
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetVertexCoverProcessedGraph(graphWithNoPendants);

            Assert.AreEqual(graphAttributes.IncludedVertices.Count(), 0);

            CollectionAssert.AreEqual(graphAttributes.ProcessedGraph.Vertices.ToList(), graphWithNoPendants.Vertices.ToList());
            CollectionAssert.AreEqual(graphAttributes.ProcessedGraph.Edges.ToList(), graphWithNoPendants.Edges.ToList());
        }

        [TestMethod()]
        public void PreprocessesGraphAndReturnsAllVertices()
        {
            Graph graphWithThreePendants = new Graph(matrixWithThreePendants);
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetVertexCoverProcessedGraph(graphWithThreePendants);

            Assert.AreEqual(graphAttributes.IncludedVertices.Count(), 3);
            Assert.AreEqual(graphAttributes.IncludedVertices.ElementAt(0).ID, 1);
            Assert.AreEqual(graphAttributes.IncludedVertices.ElementAt(1).ID, 2);
            Assert.AreEqual(graphAttributes.IncludedVertices.ElementAt(2).ID, 4);

            Assert.AreEqual(graphAttributes.ProcessedGraph.Vertices.Count, 0);
            Assert.AreEqual(graphAttributes.ProcessedGraph.Edges.Count, 0);
        }
    }
}
