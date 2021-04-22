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

            foreach(Vertex independent in independents)
            {
                preprocessedGraph.RemoveVertex(independent);
            }
            foreach(Vertex pendant in pendants)
            {
                preprocessedGraph.RemoveVertex(pendant);
            }
            foreach(Vertex includedVertex in includedVertices)
            {
                preprocessedGraph.RemoveVertex(includedVertex);
            }

            //possibly, also add top vertices to included vertices
            //possibly, remove top vertices from graph

            PreProcessedGraphAttributes preProcessedGraphAttributes = new PreProcessedGraphAttributes(includedVertices.Distinct(), pendants.Distinct(), preprocessedGraph);
            return preProcessedGraphAttributes;
        }
    }

    public struct PreProcessedGraphAttributes
    {
        public IEnumerable<Vertex> IncludedVertices { get; }
        
        public IEnumerable<Vertex> DiscardedVertices { get; }
        public Graph ProcessedGraph { get; }

        public PreProcessedGraphAttributes(
            IEnumerable<Vertex> includedVertices,
            IEnumerable<Vertex> discardedVertices,
            Graph processedGraph)
        {
            IncludedVertices = includedVertices;
            DiscardedVertices = discardedVertices;
            ProcessedGraph = processedGraph;
        }
    }
}
