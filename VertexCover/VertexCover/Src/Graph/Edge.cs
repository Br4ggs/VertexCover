using System;

namespace VertexCover
{
    /// <summary>
    /// The class that represent an edge
    /// </summary>
    public class Edge : IGraphElement
    {
        /// <summary>
        /// The start vertex for this edge
        /// </summary>
        public Vertex StartVertex { get; }

        /// <summary>
        /// The end vertex for this edge
        /// </summary>
        public Vertex EndVertex { get; }

        /// <summary>
        /// Initialize a new edge
        /// </summary>
        /// <param name="starVertex">The start vertex for this edge</param>
        /// <param name="endVertex">The end vertex for this edge</param>
        public Edge(Vertex starVertex, Vertex endVertex)
        {
            StartVertex = starVertex ?? throw new ArgumentNullException(nameof(starVertex));
            EndVertex = endVertex ?? throw new ArgumentNullException(nameof(endVertex));
        }

        /// <summary>
        /// Check if two edges are equal
        /// </summary>
        /// <param name="other">The other edge to compare with</param>
        /// <returns>True if equal false if not</returns>
        protected bool Equals(Edge other)
        {
            return Equals(StartVertex, other.StartVertex) && Equals(EndVertex, other.EndVertex);
        }

        /// <summary>
        /// Check if two edges are equal
        /// </summary>
        /// <param name="obj">The other vertex to compare with</param>
        /// <returns>True if equal false if not</returns>
        public override bool Equals(object obj)
        {
            return obj is Edge vertex &&
                   Equals(vertex);
        }

        /// <summary>
        /// Get the hash code for this Edge
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(StartVertex, EndVertex);
        }
    }
}