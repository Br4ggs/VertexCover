using System;
using System.Collections.Generic;
using System.Text;

namespace VertexCover
{
    public interface IGraphKernelizer
    {
        public KernelizedAttributes FindKernelizedAttributes(Graph graph, int k);
    }
}
