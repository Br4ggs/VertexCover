using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover
{
    public class MatrixBuilder
    {
        private Random random = new Random();

        /// <summary>
        /// Generate a complete adjacency matrix for a fully connected graph
        /// </summary>
        /// <param name="vertices">The number of vertices to include in the graph</param>
        /// <param name="edgeProbability">The probability of an edge being created between two vertices, expressed as an integer from 0 (no edges) to 100 (all edges)</param>
        /// <returns>An adjacency matrix for the generated fully connected graph</returns>
        public bool[,] GenerateCompleteAdjacencyMatrix(int vertices, int edgeProbability)
        {
            bool[,] matrix = GenerateAdjacencyMatrix(vertices, edgeProbability);
            List<List<int>> subGraphs = FindSubGraphsFromAdjacencyMatrix(matrix);
            ConnectSubGraphsInAdjacencyMatrix(matrix, subGraphs.Select(graph => graph.ToArray()).ToArray());

            return matrix;
        }

        /// <summary>
        /// Generate a complete adjaceny matrix for a graph
        /// </summary>
        /// <param name="vertices">The number of vertices to include in the graph</param>
        /// <param name="edgeProbability">The probability of an edge being created between two vertices, expressed as an integer from 0 (no edges) to 100 (all edges)</param>
        /// <returns>An adjacency matrix for the generated graph</returns>
        public bool[,] GenerateAdjacencyMatrix(int vertices, int edgeProbability)
        {
            bool[,] matrix = new bool[vertices, vertices];

            for (int i = 0; i < vertices; i++)
            {
                for (int j = 0; j < vertices; j++)
                {
                    if (i == j)
                    {
                        break;
                    }
                    matrix[i, j] = random.Next(0, 101) <= edgeProbability;
                    matrix[j, i] = matrix[i, j];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Finds the (sub)graphs in a graph
        /// </summary>
        /// <param name="adjacencyMatrix">The initial graph</param>
        /// <returns>A collection of all found (sub)graphs in the graph</returns>
        public List<List<int>> FindSubGraphsFromAdjacencyMatrix(bool[,] adjacencyMatrix)
        {
            List<List<int>> subGraphs = new List<List<int>>();
            List<int> nodes = new List<int>(Enumerable.Range(0, adjacencyMatrix.GetLength(0)));

            for (int i = 0; i < nodes.Count; i++)
            {
                int currentNode = nodes[i];
                List<int> currentSubGraph = new List<int>();
                currentSubGraph.Add(currentNode);

                for (int j = 0; j < currentSubGraph.Count; j++)
                {
                    currentNode = currentSubGraph[j];
                    for (int k = 0; k < adjacencyMatrix.GetLength(1); k++)
                    {
                        if (adjacencyMatrix[currentNode, k] && !currentSubGraph.Contains(k)) //todo: replace .contains() with memoization
                        {
                            currentSubGraph.Add(k);
                            nodes.Remove(k);
                        }
                    }
                }
                subGraphs.Add(currentSubGraph);
            }

            return subGraphs;
        }

        /// <summary>
        /// Turns a graph comprised of unconnected subgraphs into one singular conencted graph
        /// </summary>
        /// <param name="adjacencyMatrix">The initial graph</param>
        /// <param name="subGraphs">The set of subgraphs on the initial graph</param>
        public void ConnectSubGraphsInAdjacencyMatrix(bool[,] adjacencyMatrix, int[][] subGraphs)
        {
            for (int i = 0; i < subGraphs.Length - 1; i++)
            {
                adjacencyMatrix[subGraphs[i][0], subGraphs[i + 1][0]] = true;
                adjacencyMatrix[subGraphs[i + 1][0], subGraphs[i][0]] = true;
            }
        }
    }
}
