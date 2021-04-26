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
using System.Linq;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for GenerateVertexCoverWindow.xaml
    /// </summary>
    public partial class GenerateVertexCoverWindow : Window
    {
        public bool Completed { get; private set; }
        public int Nodes { get; private set; }
        public List<Vertex> VertexCover { get; private set; }
        public bool UsePreprocessing { get; private set; }

        private Graph graph;

        private GraphPreprocessor graphPreProcessor = new GraphPreprocessor(new VertexCoverGraphKernelizer());

        public GenerateVertexCoverWindow(Graph graph)
        {
            Completed = false;
            Nodes = 0;
            VertexCover = new List<Vertex>();
            UsePreprocessing = false;

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

        private void PreprocessingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UsePreprocessing = true;
        }

        private void PreprocessingCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            UsePreprocessing = false;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int vertexCoverSize = Nodes;
            //show loading screen
            if(UsePreprocessing)
            {
                PreProcessedGraphAttributes attributes = graphPreProcessor.GetProcessedGraph(graph);
                VertexCover.AddRange(attributes.IncludedVertices);
                graph = attributes.ProcessedGraph;
                vertexCoverSize -= attributes.IncludedVertices.Count();
            }

            Stack<Vertex> cover = VertexCoverUtils.GetVertexCover(graph, vertexCoverSize);

            if(cover == null)
            {
                VertexCover = new List<Vertex>();
            }
            else
            {
                VertexCover.AddRange(cover);
            }

            Completed = true;
            Close();
        }

    }
}
