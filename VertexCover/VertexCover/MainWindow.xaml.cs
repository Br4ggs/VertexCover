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
using VertexCover.Src.Graph;
using VertexCover.Src.GraphViz;
using VertexCover.Utils;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MatrixBuilder matrixBuilder = new MatrixBuilder();
        private int imagesGenerated = 0;
        private bool[,] matrix = new bool[0, 0];
        private Graph graph;
        private GraphVizAttributes attributes;
        private Random random = new Random();
        private uint kSize = 0;
        public MainWindow()
        {
            InitializeComponent();

            GenerateDefaultGraph();
        }

        private void GenerateGraphButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateMatrixWindow generateWindow = new GenerateMatrixWindow(matrixBuilder);
            generateWindow.ShowDialog();

            if (!generateWindow.Completed)
                return;

            matrix = generateWindow.Matrix;
            graph = new Graph(matrix);
            GenerateAttributes();
            DrawGraph(graph, attributes);
        }

        private void GenerateVertexCoverButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateVertexCoverWindow generateVertexCoverWindow = new GenerateVertexCoverWindow(matrix);
            generateVertexCoverWindow.ShowDialog();

            if (!generateVertexCoverWindow.Completed)
                return;

            Stack<Vertex> vertexCover = generateVertexCoverWindow.VertexCover;

            if (vertexCover == null)
            {
                VertexCoverOutput.Text = "No suitable vertex cover could be found";
                return;
            }

            string edges = vertexCover.Aggregate("", (current, vertex) => current + (vertex.ID + " "));

            GenerateAttributes();
            ColorValues(vertexCover, Color.Green);
            ColorValues(graph.Edges, Color.Green);

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

        private void GenerateAttributes()
        {
            attributes =
                new GraphVizAttributes("my_graph", "Arial", "filled,setlinewidth(4)", "circle");

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                attributes.AddAttribute(graph.Vertices.ElementAt(i), new Tuple<string, string>("label", i.ToString()));
            }
        }

        private void GenerateDefaultGraph()
        {
            matrix = matrixBuilder.GenerateCompleteAdjacencyMatrix(5, 50);
            graph = new Graph(matrix);
            GenerateAttributes();
            DrawGraph(graph, attributes);
        }

        private void AddTopVertex_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() < kSize);
            TransformVertexDegree(vertices, $"All vertices have more than {kSize} elements", (int)kSize);
        }

        private void RemoveTopVertex_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graph.GetVerticesSetWithHigherThanEdgeCount((int)kSize);
            TransformVertexDegree(vertices, "This graph has no top vertices", 1);
        }

        private void AddPendent_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() != 1);
            TransformVertexDegree(vertices, "This graph has only pendent vertices", 1);
        }

        private void RemovePendent_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graph.GetPendentVertices();
            TransformVertexDegree(vertices, "This graph has no pendent vertices", 2);
        }

        private void TransformVertexDegree(IEnumerable<Vertex> vertices, string failedMessage, int edges)
        {
            if (!vertices.Any())
            {
                VertexCoverOutput.Text = failedMessage;
                return;
            }
            Vertex vertex = vertices.ElementAt(random.Next(vertices.Count()));
            VertexUtils.TransformVertexDegree(graph, vertex, (uint)edges);
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
            GenerateAttributes();
            IEnumerable<Vertex> pendents = graph.GetPendentVertices();
            List<Vertex> vertices = new List<Vertex>();
            foreach (var pendent in pendents)
            {
                var edges = graph.GetEdges(pendent);
                foreach (var edge in edges)
                {
                    vertices.Add(edge.StartVertex);
                    vertices.Add(edge.EndVertex);
                }
            }
            ColorValues(vertices.Distinct(), Color.Blue);
            ColorValues(graph.GetVerticesSetWithHigherThanEdgeCount((int)kSize), Color.Red);

            ColorValues(graph.GetIndependentVerticesSet(), Color.Green);
        }

        private void ColorValues(IEnumerable<IGraphElement> elements, Color color)
        {
            string colorString = color.Name.ToLower();

            foreach (var element in elements)
            {
                attributes.AddAttribute(element, new Tuple<string, string>("color", colorString));
            }
        }


    }
}