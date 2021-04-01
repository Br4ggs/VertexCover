using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateWindow generateWindow = new GenerateWindow();
            generateWindow.ShowDialog();

            GenerateAdjacencyMatrix(generateWindow.Nodes, generateWindow.Density);
        }

        private bool[,] GenerateAdjacencyMatrix(int vertices, int edgeProbability)
        {
            Random random = new Random();
            bool[,] matrix = new bool[vertices, vertices];

            for(int i = 0; i < vertices; i++)
            {
                for(int j = 0; j < vertices; j++)
                {
                    matrix[i, j] = random.Next(0, 101) <= edgeProbability;
                }
            }

            return matrix;
        }
    }
}
