using System;

namespace VertexCover
{
    public class Edge
    {
        public Vertex StartVertex { get; }
        public Vertex EndVertex { get; }

        public Edge(Vertex starVertex, Vertex endVertex)
        {
            StartVertex = starVertex;
            EndVertex = endVertex;
        }

        protected bool Equals(Edge other)
        {
            return StartVertex == other.StartVertex && EndVertex == other.EndVertex;
        }

        public override bool Equals(object obj)
        {
            return obj is Edge vertex &&
                   Equals(vertex);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartVertex, EndVertex);
        }
    }
}