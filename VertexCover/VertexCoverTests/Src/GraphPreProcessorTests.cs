﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetProcessedGraph(graphWithTwoPendants);
            
            Assert.AreEqual(1, graphAttributes.IncludedVertices.Count());
            Assert.AreEqual(2, graphAttributes.IncludedVertices.ElementAt(0).ID);

            Assert.AreEqual(2, graphAttributes.ProcessedGraph.Vertices.Count());
            Assert.AreEqual(0, graphAttributes.ProcessedGraph.Vertices.ElementAt(0).ID);
            Assert.AreEqual(1, graphAttributes.ProcessedGraph.Vertices.ElementAt(1).ID);
            Assert.AreEqual(1, graphAttributes.ProcessedGraph.Edges.Count());
            Assert.AreEqual(graphAttributes.ProcessedGraph.Vertices.ElementAt(0), graphAttributes.ProcessedGraph.Edges.ElementAt(0).StartVertex);
            Assert.AreEqual(graphAttributes.ProcessedGraph.Vertices.ElementAt(1), graphAttributes.ProcessedGraph.Edges.ElementAt(0).EndVertex);
        }

        [TestMethod()]
        public void PreprocessesGraphAndReturnsSameGraph()
        {
            Graph graphWithNoPendants = new Graph(matrixWithNoPendants);
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetProcessedGraph(graphWithNoPendants);

            Assert.AreEqual(graphAttributes.IncludedVertices.Count(), 0);

            CollectionAssert.AreEqual(graphAttributes.ProcessedGraph.Vertices.ToList(), graphWithNoPendants.Vertices.ToList());
            CollectionAssert.AreEqual(graphAttributes.ProcessedGraph.Edges.ToList(), graphWithNoPendants.Edges.ToList());
        }

        [TestMethod()]
        public void PreprocessesGraphAndReturnsAllVertices()
        {
            Graph graphWithThreePendants = new Graph(matrixWithThreePendants);
            PreProcessedGraphAttributes graphAttributes = graphPreprocessor.GetProcessedGraph(graphWithThreePendants);

            Assert.AreEqual(graphAttributes.IncludedVertices.Count(), 3);
            Assert.AreEqual(graphAttributes.IncludedVertices.ElementAt(0).ID, 1);
            Assert.AreEqual(graphAttributes.IncludedVertices.ElementAt(1).ID, 2);
            Assert.AreEqual(graphAttributes.IncludedVertices.ElementAt(2).ID, 4);

            Assert.AreEqual(graphAttributes.ProcessedGraph.Vertices.Count, 0);
            Assert.AreEqual(graphAttributes.ProcessedGraph.Edges.Count, 0);
        }
    }
}
