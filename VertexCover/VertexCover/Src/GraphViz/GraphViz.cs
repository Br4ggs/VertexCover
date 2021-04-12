using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VertexCover.Src.GraphViz;

namespace VertexCover
{
    public static class GraphViz
    {
        /// <summary>
        /// Create a graph image in the current directory
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="graph">The graph you want an image for</param>
        /// <param name="attributes">The attributes for the corresponding graph</param>
        /// <returns>The created images</returns>
        public static Uri CreateGraphImage(string name, Graph graph, GraphVizAttributes attributes)
        {
            string imageFileName = $"{name}.png";
            string dotFileName = $"{name}.dot";
            File.WriteAllText(dotFileName, ConvertToDotFile(graph, attributes));
            Process dot = new Process
            {
                StartInfo =
                {
                    //TODO Fix
                    FileName = @"C:\Program Files\Graphviz\bin\dot.exe", Arguments = $"-T png -o {imageFileName} {dotFileName}"
                }
            };

            dot.Start();
            dot.WaitForExit();
            return new Uri(@$"{Directory.GetCurrentDirectory()}/{imageFileName}");
        }

        /// <summary>
        /// Create the dot file for a specific graph
        /// </summary>
        /// 
        private static string ConvertToDotFile(Graph graph, GraphVizAttributes attributes)
        {
            string text = $"graph {attributes.Name} {{ node [fontname = {attributes.FontName}, style =\"{attributes.Style}\", shape={attributes.Shape} ]\n";

            foreach (var vertex in graph.Vertices)
            {
                text += VertexToString(vertex);
                if (attributes.HasAttributes(vertex))
                {
                    text += AttributesToString(attributes.GetAttributes(vertex));
                }
                text += Environment.NewLine;
            }

            foreach (var edge in graph.Edges)
            {
                text += EdgeToString(edge);
                if (attributes.HasAttributes(edge))
                {
                    text += AttributesToString(attributes.GetAttributes(edge));
                }
                text += Environment.NewLine;
            }

            text += "}";
            return text;
        }

        /// <summary>
        /// Generate the info for a vertex
        /// </summary>
        /// <param name="vertex">The vertex you want</param>
        /// <returns>The graph viz for a node</returns>
        private static string VertexToString(Vertex vertex)
        {
            return $"node{vertex.ID}";
        }
        /// <summary>
        /// Convert list of attributes to graph viz attributes
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static string AttributesToString(IEnumerable<Tuple<string, string>> values)
        {
            string text = " [";
            foreach (var attribute in values)
            {
                text += $" {attribute.Item1} = \"{attribute.Item2}\"";
            }
            text += " ]";
            return text;
        }

        /// <summary>
        /// Edge to graph viz string format
        /// <param name="edge">The edge you want to convert to string </param>
        /// <returns>The graph viz string for an edge</returns>
        private static string EdgeToString(Edge edge)
        {
            return $"{VertexToString(edge.StartVertex)} -- {VertexToString(edge.EndVertex)}";
        }
    }
}