using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertexCover
{
    public class GraphPreprocessor
    {
        private IGraphKernelizer graphKernelizer;

        public GraphPreprocessor(IGraphKernelizer graphKernelizer)
        {
            this.graphKernelizer = graphKernelizer;
        }

        public PreProcessedGraphAttributes GetProcessedGraph(Graph graph)
        {
            Graph preprocessedGraph = new Graph(graph);
            List<Vertex> includedVertices = new List<Vertex>();

            KernelizedAttributes attributes = graphKernelizer.FindKernelizedAttributes(graph, -1);

            //IEnumerable<Vertex> pendants = GraphKernelizer.FindPendantVertices(graph);
            //IEnumerable<Vertex> independents = GraphKernelizer.FindIsolatedVertices(graph);

            IEnumerable<Vertex> pendants = attributes.Pendants;
            IEnumerable<Vertex> independents = attributes.Independents;

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

            PreProcessedGraphAttributes preProcessedGraphAttributes = new PreProcessedGraphAttributes(includedVertices.Distinct(), preprocessedGraph);
            return preProcessedGraphAttributes;
        }
    }
}
