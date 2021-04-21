using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertexCover
{
    //todo:
    //-turn GraphKernelizer into an instance class
    public class GraphPreprocessor
    {
        public PreProcessedGraphAttributes GetVertexCoverProcessedGraph(Graph graph)
        {
            Graph preprocessedGraph = new Graph(graph);
            List<Vertex> includedVertices = new List<Vertex>();
            IEnumerable<Vertex> pendants = GraphKernelizer.FindPendantVertices(graph);
            IEnumerable<Vertex> independents = GraphKernelizer.FindIsolatedVertices(graph);
            foreach (Vertex pendent in pendants)
            {
                IEnumerable<Edge> connectedEdges = graph.GetEdges(pendent);
                includedVertices.AddRange(connectedEdges.Select(edge => pendent.Equals(edge.StartVertex) ? edge.EndVertex : edge.StartVertex));
            }

            //get connected edges of included vertices
            //remove edges
            //remove pendents
            //remove included vertices

            //possibly, also add top vertices to included vertices
            //possibly, remove top vertices from graph

            PreProcessedGraphAttributes preProcessedGraphAttributes = new PreProcessedGraphAttributes(includedVertices, preprocessedGraph);
            return preProcessedGraphAttributes;
        }
    }

    public struct PreProcessedGraphAttributes
    {
        public IEnumerable<Vertex> IncludedVertices { get; }
        public Graph ProcessedGraph { get; }

        public PreProcessedGraphAttributes(
            IEnumerable<Vertex> includedVertices,
            Graph processedGraph)
        {
            IncludedVertices = includedVertices;
            ProcessedGraph = processedGraph;
        }
    }
}
