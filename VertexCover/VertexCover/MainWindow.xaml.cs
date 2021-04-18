using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using VertexCover.Src.GraphViz;

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
            foreach (var vertex in vertexCover)
            {
                attributes.AddAttribute(vertex, new Tuple<string, string>("color", "green"));
            }

            foreach (var edge in graph.Edges)
            {
                attributes.AddAttribute(edge, new Tuple<string, string>("color", "green"));
            }

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

        }

        private void RemoveTopVertex_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPendent_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() != 1);
            if (!vertices.Any())
            {
                VertexCoverOutput.Text = "This graph has only pendent vertices";
                return;
            }
            Vertex vertex = vertices.ElementAt(random.Next(vertices.Count()));

            IEnumerable<Edge> adjacentEdges = graph.GetEdges(vertex);
            var edges = adjacentEdges as Edge[] ?? adjacentEdges.ToArray();

            if (edges.Length > 0)
            {
                for (int i = 0; i < edges.Count() - 1; i++)
                {
                    graph.RemoveEdge(edges[i]);
                }
            }
            else
            {
                Edge edge = new Edge(graph.Vertices.ElementAt(random.Next(graph.Vertices.Count)), vertex);
                graph.AddEdge(edge);
            }
            DrawGraph(graph, attributes);
        }

        private void RemovePendent_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Vertex> vertices = graph.GetPendentVertices();
            if (!vertices.Any())
            {
                VertexCoverOutput.Text = "This graph has no pendent vertices";
                return;
            }
            Vertex vertex = vertices.ElementAt(random.Next(vertices.Count()));
            Edge edge = new Edge(graph.Vertices.ElementAt(random.Next(graph.Vertices.Count)), vertex);
            graph.AddEdge(edge);
            DrawGraph(graph, attributes);
        }
    }
}