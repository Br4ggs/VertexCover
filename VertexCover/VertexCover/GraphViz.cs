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
        public static Uri CreateGraphImage(string name, bool[,] adjacencyMatrix, string[] names)
        {
            CreateDotFile($"{name}.dot", adjacencyMatrix, names);
            Process dot = new Process
            {
                StartInfo =
                {
                    //TODO Fix
                    FileName = @"C:\Program Files\Graphviz\bin\dot.exe", Arguments = $"-T png -o {name}.png {name}.dot"
                }
            };

            dot.Start();
            dot.WaitForExit();
            return new Uri(@$"{Directory.GetCurrentDirectory()}/{name}.png");
        }

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

        private static string GenerateNode(int id, string label)
        {
            return $"node{id} [ label = \"{label}\"]\n";
        }

        private static string ConnectNodes(int id, int connectedID)
        {
            return $"node{id} -- node{connectedID}\n";
        }
    }
}