using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VertexCover
{
    public class GraphKernelizer
    {
        private Random random = new Random();

        public KernelizedAttributes FindKernelizedAttributes(Graph graph, int k)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Vertex> FindPendantVertices(Graph graph)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Vertex> FindTopVertices(Graph graph, int k)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Vertex> FindIsolatedVertices(Graph graph)
        {
            throw new NotImplementedException();
        }
    }

    public struct KernelizedAttributes
    {
        IReadOnlyCollection<Vertex> Pendants { get; }
        IReadOnlyCollection<Vertex> Tops { get; }
        IReadOnlyCollection<Vertex> Independents { get; }
        Graph Graph { get; }

        public KernelizedAttributes(
            IReadOnlyCollection<Vertex> pendants, 
            IReadOnlyCollection<Vertex> tops,
            IReadOnlyCollection<Vertex> independents,
            Graph graph)
        {
            Pendants = pendants;
            Tops = tops;
            Independents = independents;
            Graph = graph;
        }
    }
}
