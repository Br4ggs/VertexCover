using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VertexCover.Utils;

namespace VertexCover
{
    public class GraphPreprocessor
    {
        private IGraphKernelizer graphKernelizer;

        public GraphPreprocessor(IGraphKernelizer graphKernelizer)
        {
            this.graphKernelizer = graphKernelizer;
        }

        public PreProcessedGraphAttributes GetProcessedGraph(Graph graph, int k)
        {
            Graph preprocessedGraph = new Graph(graph);
            List<Vertex> includedVertices = new List<Vertex>();
            List<Vertex> discardedVertices = new List<Vertex>();

            int processedK = k;

            while (processedK > 0)
            {
                IEnumerable<Vertex> tops = graphKernelizer.FindTopVertices(preprocessedGraph, processedK);
                IEnumerable<Vertex> pendants = graphKernelizer.FindPendantVertices(preprocessedGraph);

                if (tops.Any())
                {
                    Vertex top = tops.First();
                    includedVertices.Add(top);
                    preprocessedGraph.RemoveVertex(top);
                    processedK--;
                }
                else if (pendants.Any())
                {
                    Vertex pendant = pendants.First();
                    Edge edge = preprocessedGraph.GetEdges(pendant).First();

                    Vertex neighbour = (pendant.Equals(edge.StartVertex)) ? edge.EndVertex : edge.StartVertex;
                    includedVertices.Add(neighbour);
                    preprocessedGraph.RemoveVertex(neighbour);
                    processedK--;
                }
                else
                {
                    break;
                }

                IEnumerable<Vertex> isolated = graphKernelizer.FindIsolatedVertices(preprocessedGraph);
                discardedVertices.AddRange(isolated);

                for (int i = isolated.Count() - 1; i >= 0; i--)
                {
                    preprocessedGraph.RemoveVertex(isolated.ElementAt(i));
                }
            }

            PreProcessedGraphAttributes preProcessedGraphAttributes = new PreProcessedGraphAttributes(includedVertices.Distinct(), discardedVertices.Distinct(), preprocessedGraph, processedK);
            return preProcessedGraphAttributes;
        }
        /// <summary>
        /// Get the base vertices from the preprocessed 
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public List<Vertex> GetBaseVertexCover(PreProcessedGraphAttributes attributes)
        {
            List<Vertex> vertexCover = attributes.IncludedVertices.ToList();

            var coveredGraph = attributes.ProcessedGraph;
            var vertexCoverSize = attributes.ProcessedDegree;

            foreach (Vertex discardedVertex in attributes.DiscardedVertices)
            {
                if (coveredGraph.Vertices.Count() >= vertexCoverSize)
                {
                    break;
                }

                vertexCover.Add(discardedVertex);
                vertexCoverSize--;
            }

            return vertexCover;
        }
    }
}
