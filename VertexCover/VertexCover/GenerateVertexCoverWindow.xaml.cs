using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using VertexCover.Src;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for GenerateVertexCoverWindow.xaml
    /// </summary>
    public partial class GenerateVertexCoverWindow : Window
    {
        public bool Completed { get; private set; }
        public int Nodes { get; private set; }
        public Stack<Vertex> VertexCover { get; private set; }

        private readonly bool[,] adjacencyMatrix;

        public GenerateVertexCoverWindow(bool[,] adjacencyMatrix)
        {
            Completed = false;
            Nodes = 0;
            this.adjacencyMatrix = adjacencyMatrix;
            InitializeComponent();
        }

        private void NodesTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NodesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Nodes = Int32.Parse((sender as TextBox).Text);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //show loading screen
            Graph graph = new Graph(adjacencyMatrix);
            VertexCover = VertexCoverUtils.GetVertexCover(graph, Nodes);
            Completed = true;
            Close();
        }
    }
}
