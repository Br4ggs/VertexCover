using System.Collections.Generic;

namespace VertexCover
{
    public struct PreProcessedGraphAttributes
    {
        public IEnumerable<Vertex> IncludedVertices { get; }
        public IEnumerable<Vertex> DiscardedVertices { get; }
        public Graph ProcessedGraph { get; }
        public int ProcessedDegree { get; }

        public PreProcessedGraphAttributes(
            IEnumerable<Vertex> includedVertices,
            IEnumerable<Vertex> discardedVertices,
            Graph processedGraph,
            int processedDegree)
        {
            IncludedVertices = includedVertices;
            DiscardedVertices = discardedVertices;
            ProcessedGraph = processedGraph;
            ProcessedDegree = processedDegree;
        }
    }
}
