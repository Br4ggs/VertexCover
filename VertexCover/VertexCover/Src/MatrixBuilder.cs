using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover
{
    public class MatrixBuilder
    {
        private Random random = new Random();

        public bool[,] GenerateCompleteAdjacencyMatrix(int vertices, int edgeProbability)
        {
            bool[,] matrix = GenerateAdjacencyMatrix(vertices, edgeProbability);
            List<List<int>> subGraphs = FindSubGraphsFromAdjacencyMatrix(matrix);
            ConnectSubGraphsInAdjacencyMatrix(matrix, subGraphs.Select(graph => graph.ToArray()).ToArray());

            return matrix;
        }

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
