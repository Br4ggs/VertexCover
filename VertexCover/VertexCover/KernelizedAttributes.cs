using System;
using System.Collections.Generic;
using System.Text;

namespace VertexCover
{
    public struct KernelizedAttributes
    {
        public IEnumerable<Vertex> Pendants { get; }
        public IEnumerable<Vertex> Tops { get; }
        public IEnumerable<Vertex> Independents { get; }
        public Graph Graph { get; }

        public KernelizedAttributes(
            IEnumerable<Vertex> pendants,
            IEnumerable<Vertex> tops,
            IEnumerable<Vertex> independents,
            Graph graph)
        {
            Pendants = pendants;
            Tops = tops;
            Independents = independents;
            Graph = graph;
        }
    }
}
