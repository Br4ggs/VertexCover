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

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MatrixBuilder matrixBuilder = new MatrixBuilder();
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                GenerateDefaultGraph();
            }
            catch (Win32Exception e) //todo: fix this into proper error handling later
            {
                Console.WriteLine("Please install GraphViz");
            }
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

            Uri location = GraphViz.CreateGraphImage("graph", adjacencyMatrix, names);
            ImageBox.Source = new BitmapImage(location);
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
