using System;
using System.Collections.Generic;
using System.Text;

namespace VertexCover
{
    public interface IGraphKernelizer
    {
        public KernelizedAttributes FindKernelizedAttributes(Graph graph, int k);
        public IEnumerable<Vertex> FindTopVertices(Graph graph, int k);
        public IEnumerable<Vertex> FindPendantVertices(Graph graph);
        public IEnumerable<Vertex> FindIsolatedVertices(Graph graph);
    }
}
