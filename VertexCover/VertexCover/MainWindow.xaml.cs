using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VertexCover.Extensions;
using VertexCover.Src.GraphViz;
using VertexCover.Utils;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MatrixBuilder matrixBuilder;
        private readonly VertexCoverGraphKernelizer graphKernelizer;
        private readonly GraphPreprocessor graphPreProcessor;

        private readonly VertexCoverPerformance performance;

        private Graph graph;
        private GraphVizAttributes attributes = new GraphVizAttributes("my_graph", "Arial", "filled,setlinewidth(4)", "circle");

        private int imagesGenerated;
        private uint kSize;
        public MainWindow()
        {
            InitializeComponent();
            matrixBuilder = new MatrixBuilder();
            graphKernelizer = new VertexCoverGraphKernelizer();
            graphPreProcessor = new GraphPreprocessor(graphKernelizer);
            performance = new VertexCoverPerformance(graphPreProcessor, matrixBuilder);

            GenerateDefaultGraph();
        }

        private void GenerateGraphButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateMatrixWindow generateWindow = new GenerateMatrixWindow(matrixBuilder);
            generateWindow.ShowDialog();

            if (!generateWindow.Completed)
                return;

            graph = new Graph(generateWindow.Matrix);
            attributes.Clear();
            attributes.LabelElementsNumeric(graph.Vertices);
            DrawGraph(graph, attributes);
        }

        private void GenerateVertexCoverButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateVertexCoverWindow generateVertexCoverWindow = new GenerateVertexCoverWindow(graph);
            generateVertexCoverWindow.ShowDialog();

            if (!generateVertexCoverWindow.Completed)
                return;

            List<Vertex> vertexCover = generateVertexCoverWindow.VertexCover;

            if (vertexCover.IsEmpty())
            {
                VertexCoverOutput.Text = "No suitable vertex cover could be found";
                return;
            }

            string edges = vertexCover.Aggregate("", (current, vertex) => current + (vertex.ID + " "));

            attributes.Clear();
            attributes.LabelElementsNumeric(graph.Vertices);
            attributes.ColorElements(vertexCover, Color.Green);
            attributes.ColorElements(graph.Edges, Color.Green);

            VertexCoverOutput.Text = "Vertices: " + edges + "form biggest vertex cover for graph";
            DrawGraph(graph, attributes);
        }

        private void DrawGraph(Graph graph, GraphVizAttributes attributes)
        {
            try
            {
                Uri location = GraphViz.CreateGraphImage($"graph{imagesGenerated++}", graph, attributes);
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
            graph = new Graph(matrixBuilder.GenerateCompleteAdjacencyMatrix(5, 50));
            attributes.LabelElementsNumeric(graph.Vertices);
            DrawGraph(graph, attributes);
        }

        private void AddTopVertex_Click(object sender, RoutedEventArgs e)
        {
            if (kSize >= graph.Vertices.Count() - 1)
            {
                VertexCoverOutput.Text = "There are no vertices that can have that many connections";
                return;
            }

            IEnumerable<Vertex> vertices = graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() < kSize);
            if (vertices.IsEmpty())
            {
                VertexCoverOutput.Text = "This graph has only top vertices";
                return;
            }

            VertexUtils.TransformVertexDegree(graph, vertices.Random(), kSize + 1);
            DrawGraph(graph, attributes);
        }

        private void RemoveTopVertex_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graphKernelizer.FindTopVertices(graph, (int)kSize);

            if (vertices.IsEmpty())
            {
                VertexCoverOutput.Text = "This graph has no top vertices";
                return;
            }

            VertexUtils.TransformVertexDegree(graph, vertices.Random(), Math.Max(kSize - 1, 0));
            DrawGraph(graph, attributes);
        }

        private void AddPendent_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() != 1);
            if (vertices.IsEmpty())
            {
                VertexCoverOutput.Text = "This graph has only pendent vertices";
                return;
            }

            Vertex independentVertex = vertices.FirstOrDefault(vertex => !graph.GetEdges(vertex).Any());
            VertexUtils.TransformVertexDegree(graph, independentVertex ?? vertices.Random(), 1);
            DrawGraph(graph, attributes);
        }

        private void RemovePendent_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graphKernelizer.FindPendantVertices(graph);
            if (vertices.IsEmpty())
            {
                VertexCoverOutput.Text = "This graph has no pendent vertices";
                return;
            }

            VertexUtils.TransformVertexDegree(graph, vertices.Random(), 2);
            DrawGraph(graph, attributes);
        }

        private void NodesTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NodesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (uint.TryParse((sender as TextBox)?.Text, out var value))
                kSize = value;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorKernelization();
            DrawGraph(graph, attributes);
        }

        private void ColorKernelization()
        {
            attributes.Clear();
            attributes.LabelElementsNumeric(graph.Vertices);
            KernelizedAttributes kernelized = graphKernelizer.FindKernelizedAttributes(graph, (int)kSize);

            // Color neighbours of pendants
            List<Vertex> vertices = new List<Vertex>();
            foreach (var pendent in kernelized.Pendants)
            {
                var edges = graph.GetEdges(pendent);
                vertices.AddRange(edges.Select(edge => Equals(edge.StartVertex, pendent) ? edge.EndVertex : edge.StartVertex));
            }
            attributes.ColorElements(vertices.Distinct(), Color.LightBlue);
            attributes.ColorElements(kernelized.Pendants, Color.Blue);
            attributes.ColorElements(kernelized.Tops, Color.Red);

            attributes.ColorElements(kernelized.Independents, Color.Green);
        }

        private void PerformanceTestButton_Click(object sender, RoutedEventArgs e)
        {

            VertexCoverPerformanceWindow generateVertexCoverWindow = new VertexCoverPerformanceWindow(performance);
            generateVertexCoverWindow.ShowDialog();
        }
    }
}