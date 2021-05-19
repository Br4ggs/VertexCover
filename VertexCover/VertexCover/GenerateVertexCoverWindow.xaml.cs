using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VertexCover.Utils;

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
        public bool UseVertexTops { get; private set; }

        private Graph graph;

        private GraphPreprocessor graphPreProcessor = new GraphPreprocessor(new VertexCoverGraphKernelizer());
        private OptimizedProgressBar progressBar;

        public GenerateVertexCoverWindow(Graph graph)
        {
            Completed = false;
            Nodes = 0;
            VertexCover = new List<Vertex>();
            UsePreprocessing = false;

            this.graph = graph;
            InitializeComponent();
            progressBar = new OptimizedProgressBar(VertexCoverProgressBar);
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
            ConfirmButton.Visibility = Visibility.Hidden;
            progressBar.StartProgressBar((ulong)Math.Pow(2, graph.Vertices.Count), .05);

            Task task = new Task(() => FindVertexCover(CloseWindow));
            task.Start();

            progressBar.UI.Visibility = Visibility.Visible;
        }

        private void FindVertexCover(Action onVertexCoverFound)
        {
            int vertexCoverSize = Nodes;
            Graph coveredGraph = graph;
            List<Vertex> baseCover = new List<Vertex>();
            if (UsePreprocessing)
            {
                PreProcessedGraphAttributes attributes = graphPreProcessor.GetProcessedGraph(coveredGraph, vertexCoverSize);
                baseCover = graphPreProcessor.GetBaseVertexCover(attributes);
                coveredGraph = attributes.ProcessedGraph;

                if (coveredGraph.Edges.Count <= 0)
                {
                    Application.Current.Dispatcher.Invoke(onVertexCoverFound);
                    return;
                }
            }

            VertexCover = VertexCoverUtils.GetVertexCover(coveredGraph, vertexCoverSize, baseCover, OnVertexProcessed);
            Application.Current.Dispatcher.Invoke(onVertexCoverFound);
        }

        private void OnVertexProcessed()
        {
            Application.Current.Dispatcher.Invoke(() => progressBar.TakeStep());
        }

        private void CloseWindow()
        {
            progressBar.UI.Value = 100;
            progressBar.UI.UpdateLayout();
            Completed = true;
            Close();
        }
    }
}
