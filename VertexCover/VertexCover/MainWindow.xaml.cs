using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MatrixBuilder matrixBuilder = new MatrixBuilder();
        private int imagesGenerated = 0;

        public MainWindow()
        {
            InitializeComponent();

            GenerateDefaultGraph();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateWindow generateWindow = new GenerateWindow();
            generateWindow.ShowDialog();

            bool[,] matrix = matrixBuilder.GenerateCompleteAdjacencyMatrix(generateWindow.Nodes, generateWindow.Density);
            DrawGraph(matrix);
        }

        private void DrawGraph(bool[,] adjacencyMatrix)
        {
            string[] names = Enumerable.Range(1, adjacencyMatrix.GetLength(0))
                .Select(num => num.ToString())
                .ToArray();
            try
            {
                Uri location = GraphViz.CreateGraphImage($"graph{imagesGenerated++}", adjacencyMatrix, names);
                ImageBox.Source = new BitmapImage(location);
            }
            catch (Win32Exception)
            {
                MessageBox.Show("Please install GraphViz at C:\\Program Files\\Graphviz\\bin\\dot.exe");
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("The maximum size has been reached please create a smaller graph");
            }
        }

        private void GenerateDefaultGraph()
        {
            bool[,] matrix = {
                {false, true, false, false},
                {true,false, true, false},
                {false,true, false, true},
                {false, false, true, false}
            };

            DrawGraph(matrix);
        }
    }
}