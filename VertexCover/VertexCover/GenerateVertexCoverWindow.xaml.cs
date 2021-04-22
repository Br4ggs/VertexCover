using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
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

        private GraphPreprocessor graphPreProcessor = new GraphPreprocessor();

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
            Graph coveredGraph = graph;
            //show loading screen
            if (UsePreprocessing)
            {
                PreProcessedGraphAttributes attributes = graphPreProcessor.GetVertexCoverProcessedGraph(graph);
                VertexCover.AddRange(attributes.IncludedVertices);
                coveredGraph = attributes.ProcessedGraph;
                vertexCoverSize -= attributes.IncludedVertices.Count();

                foreach(Vertex discardedVertex in attributes.DiscardedVertices)
                {
                    if(coveredGraph.Vertices.Count() >= vertexCoverSize)
                    {
                        break;
                    }

                    VertexCover.Add(discardedVertex);
                    vertexCoverSize--;
                }
            }

            Stack<Vertex> cover = VertexCoverUtils.GetVertexCover(coveredGraph, vertexCoverSize);

            if (cover == null)
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
