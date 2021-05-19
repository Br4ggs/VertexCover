namespace VertexCover
{
    /// <summary>
    /// Represents a vertex inside of a graph
    /// </summary>
    public class Vertex : IGraphElement
    {
        /// <summary>
        /// Get the id for a vertex
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Initialize a vertex for a graph
        /// </summary>
        /// <param name="id">The id you want to have it</param>
        public Vertex(int id)
        {
            ID = id;
        }

        /// <summary>
        /// Check if two vertices are equal
        /// </summary>
        /// <param name="other">The other vertex to compare with</param>
        /// <returns>True if equal false if not</returns>
        protected bool Equals(Vertex other)
        {
            return ID == other.ID;
        }

        /// <summary>
        /// Check if two vertices are equal
        /// </summary>
        /// <param name="obj">The other vertex to compare with</param>
        /// <returns>True if equal false if not</returns>
        public override bool Equals(object obj)
        {
            return obj is Vertex vertex &&
                   Equals(vertex);
        }

        /// <summary>
        /// Get the hash code for this vertex
        /// </summary>
        /// <returns>The has code</returns>
        public override int GetHashCode()
        {
            return ID;
        }
    }
}
