using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace VertexCover
{
    public static class GraphViz
    {
        /// <summary>
        /// Create a graph image in the current directory
        /// </summary>
        /// <param name="name">'The name of the file</param>
        /// <param name="adjacencyMatrix">The adjacency matrix for the graph</param>
        /// <param name="names">The names of the vertices</param>
        /// <returns>The created images</returns>
        public static Uri CreateGraphImage(string name, bool[,] adjacencyMatrix, string[] names)
        {
            bool[,] copyMatrix = (bool[,])adjacencyMatrix.Clone();

            string imageFileName = $"{name}.png";
            CreateDotFile($"{name}.dot", copyMatrix, names);
            Process dot = new Process
            {
                StartInfo =
                {
                    //TODO Fix
                    FileName = @"C:\Program Files\Graphviz\bin\dot.exe", Arguments = $"-T png -o {imageFileName} {name}.dot"
                }
            };

            dot.Start();
            dot.WaitForExit();
            return new Uri(@$"{Directory.GetCurrentDirectory()}/{imageFileName}");
        }

        /// <summary>
        /// Create the dot file for a specific graph
        /// </summary>
        /// <param name="title">File name</param>
        /// <param name="adjacencyMatrix">The adjacency matrix for the graph</param>
        /// <param name="names">The names of the vertices</param>
        private static void CreateDotFile(string title, bool[,] adjacencyMatrix, string[] names)
        {
            string text = "graph my_graph {node [ fontname = Arial, style=\"filled,setlinewidth(4)\", shape=circle ]\n";

            for (int i = 0; i < names.Length; i++)
            {
                text += GraphViz.GenerateNode(i, names[i]);
            }

            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (!adjacencyMatrix[i, j])
                        continue;

                    text += ConnectNodes(i, j);
                    adjacencyMatrix[j, i] = false;
                }
            }

            text += "}";
            File.WriteAllText(title, text);
        }

        /// <summary>
        /// Generate the info for a node
        /// </summary>
        /// <param name="id">The node ID</param>
        /// <param name="label">'The label name</param>
        /// <returns>The graph viz for a node</returns>
        private static string GenerateNode(int id, string label)
        {
            return $"node{id} [ label = \"{label}\"]\n";
        }

        /// <summary>
        /// Generate the connection for two nodes
        /// </summary>
        /// <param name="id">The first node id</param>
        /// <param name="connectedID">The connected node idsss</param>
        /// <returns>The graph viz for a connection</returns>
        private static string ConnectNodes(int id, int connectedID)
        {
            return $"node{id} -- node{connectedID}\n";
        }
    }
}