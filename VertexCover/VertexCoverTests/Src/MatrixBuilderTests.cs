using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using VertexCover;

namespace VertexCover.Tests
{
    //TODO: do input parsing and null-checking
    [TestClass()]
    public class MatrixBuilderTests
    {
        private readonly bool[,] matrixOfTwoSubGraphs =
        {
            { false, false, true, false, true },
            { false, false, false, true, false },
            { true, false, false, false, true },
            { false, true, false, false, false },
            { true, false, true, false, false }
        };

        private readonly bool[,] matrixOfAllSubGraphs =
        {
            { false, false, false, false },
            { false, false, false, false },
            { false, false, false, false },
            { false, false, false, false }
        };

        private readonly bool[,] matrixOfOneSubGraph =
        {
            { false, true, true },
            { true, false, true },
            { true, true, false }
        };


        private MatrixBuilder matrixBuilder;

        [TestInitialize]
        public void BeforeEach()
        {
            matrixBuilder = new MatrixBuilder();
        }

        [TestMethod()]
        public void FindSubGraphsInAdjacencyMatrixReturnsFullList()
        {
            List<List<int>> subgraphs = matrixBuilder.FindSubGraphsFromAdjacencyMatrix(matrixOfAllSubGraphs);

            Assert.AreEqual(subgraphs.Count, 4);
            CollectionAssert.AreEqual(subgraphs[0], new List<int> { 0 });
            CollectionAssert.AreEqual(subgraphs[1], new List<int> { 1 });
            CollectionAssert.AreEqual(subgraphs[2], new List<int> { 2 });
            CollectionAssert.AreEqual(subgraphs[3], new List<int> { 3 });
        }

        [TestMethod()]
        public void FindSubGraphsInAdjacencyMatrixReturnsListWithOneElement()
        {
            List<List<int>> subgraph = matrixBuilder.FindSubGraphsFromAdjacencyMatrix(matrixOfOneSubGraph);

            Assert.AreEqual(subgraph.Count, 1);
            CollectionAssert.AreEqual(subgraph[0], new List<int> { 0, 1, 2 });
        }

        [TestMethod()]
        public void FindSubGraphsInAdjacencyMatrixReturnsListWithTwoElements()
        {
            List<List<int>> subgraphs = matrixBuilder.FindSubGraphsFromAdjacencyMatrix(matrixOfTwoSubGraphs);

            Assert.AreEqual(subgraphs.Count, 2);
            CollectionAssert.AreEqual(subgraphs[0], new List<int> { 0, 2, 4 });
            CollectionAssert.AreEqual(subgraphs[1], new List<int> { 1, 3 });
        }

        [TestMethod()]
        public void ConnectSubGraphsInAdjacencyMatrixDoesNotModifyMatrixWithOneSubGraph()
        {
            bool[,] adjacencyMatrix = matrixOfOneSubGraph;

            int[][] subgraphs =
            {
                new int[] { 0, 1, 2 }
            };

            matrixBuilder.ConnectSubGraphsInAdjacencyMatrix(adjacencyMatrix, subgraphs);

            CollectionAssert.AreEqual(adjacencyMatrix, matrixOfOneSubGraph);
        }

        [TestMethod()]
        public void ConnectSubGraphsInAdjacencyMatrixConnectsMatrixWithNoEdges()
        {
            bool[,] adjacencyMatrix = matrixOfAllSubGraphs;

            int[][] subgraphs =
            {
                new int[] { 0 },
                new int[] { 1 },
                new int[] { 2 },
                new int[] { 3 }
            };

            matrixBuilder.ConnectSubGraphsInAdjacencyMatrix(adjacencyMatrix, subgraphs);

            bool[,] fixedMatrix =
            {
                { false, true, false, false},
                { true, false, true, false},
                { false, true, false, true},
                { false, false, true, false}
            };

            CollectionAssert.AreEqual(adjacencyMatrix, fixedMatrix);
        }

        [TestMethod()]
        public void ConnectSubGraphsInAdjacencyMatrixConnectsTwoSubgraphs()
        {
            bool[,] adjacencyMatrix = matrixOfTwoSubGraphs;

            int[][] subgraphs =
            {
                new int[] { 0, 2, 4 },
                new int[] { 1, 3 }
            };

            matrixBuilder.ConnectSubGraphsInAdjacencyMatrix(adjacencyMatrix, subgraphs);

            bool[,] fixedMatrix =
            {
                { false, true, true, false, true },
                { true, false, false, true, false },
                { true, false, false, false, true },
                { false, true, false, false, false },
                { true, false, true, false, false }
            };

            CollectionAssert.AreEqual(adjacencyMatrix, fixedMatrix);
        }
    }
}
