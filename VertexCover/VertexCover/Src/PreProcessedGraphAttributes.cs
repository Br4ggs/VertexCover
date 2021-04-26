using System;
using System.Collections.Generic;
using System.Text;

namespace VertexCover
{
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
