using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for ApproximateVertexCoverWindow.xaml
    /// </summary>
    public partial class ApproximateVertexCoverWindow : Window
    {
        public bool Completed { get; private set; }
        public int Nodes { get; private set; }
        public List<Vertex> VertexCover { get; private set; }

        private Graph graph;

        public ApproximateVertexCoverWindow(Graph graph)
        {
            Completed = false;
            Nodes = 0;
            VertexCover = new List<Vertex>();

            this.graph = graph;
            InitializeComponent();
        }

        private void NodesTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NodesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse((sender as TextBox)?.Text, out var value))
                Nodes = value;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmButton.IsEnabled = false;
            VertexCover = VertexCoverUtils.ApproximateVertexCover(graph);
            Completed = true;
            Close();
        }
    }
}
