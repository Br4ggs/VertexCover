using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bool[,] matrix = {
                {false, true, false, false},
                {true,false, true, false},
                {false,true, false, true},
                {false, false, true, false}
            };
            try
            {
                Uri location = GraphViz.CreateGraphImage("abc", matrix, new[] { "A", "B", "C", "D" });
                ImageBox.Source = new BitmapImage(location);
            }
            catch (Win32Exception e)
            {
                Console.WriteLine("Please install GraphViz");
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateWindow generateWindow = new GenerateWindow();
            generateWindow.ShowDialog();

            bool[,] matrix = GenerateAdjacencyMatrix(generateWindow.Nodes, generateWindow.Density);
            List<List<int>> subGraphs = FindSubGraphsFromAdjacencyMatrix(matrix);
        }

        private bool[,] GenerateAdjacencyMatrix(int vertices, int edgeProbability)
        {
            Random random = new Random();
            bool[,] matrix = new bool[vertices, vertices];

            for(int i = 0; i < vertices; i++)
            {
                for(int j = 0; j < vertices; j++)
                {
                    if(i == j)
                    {
                        break;
                    }
                    matrix[i, j] = random.Next(0, 101) <= edgeProbability;
                    matrix[j, i] = matrix[i, j];
                }
            }

            return matrix;
        }

        private List<List<int>> FindSubGraphsFromAdjacencyMatrix(bool[,] adjacencyMatrix)
        {
            List<List<int>> subGraphs = new List<List<int>>();
            List<int> nodes = new List<int>(Enumerable.Range(0, adjacencyMatrix.GetLength(0)));

            for(int i = 0; i < nodes.Count; i++)
            {
                int currentNode = nodes[i];
                List<int> currentSubGraph = new List<int>();
                currentSubGraph.Add(currentNode);

                for(int j = 0; j < currentSubGraph.Count; j++)
                {
                    currentNode = currentSubGraph[j];
                    for (int k = 0; k < adjacencyMatrix.GetLength(1); k++)
                    {
                        if (adjacencyMatrix[currentNode, k] && !currentSubGraph.Contains(k))
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

        private bool[,] ConnectSubGraphsInAdjacencyMatrix(bool[,] adjacencyMatrix, int[][] subGraphs)
        {
            for(int i = 0; i < subGraphs.Length - 1; i++)
            {
                adjacencyMatrix[subGraphs[i][0],subGraphs[i+1][0]] = true;
                adjacencyMatrix[subGraphs[i + 1][0], subGraphs[i][0]] = true;
            }

            return adjacencyMatrix;
        }
    }
}
