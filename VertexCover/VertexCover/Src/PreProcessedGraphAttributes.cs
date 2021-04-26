using System;
using System.Collections.Generic;
using System.Text;

namespace VertexCover
{
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
