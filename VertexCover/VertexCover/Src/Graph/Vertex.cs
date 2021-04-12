using VertexCover.Src.Graph;

namespace VertexCover
{
    public class Vertex : IGraphElement
    {
        public int ID { get; }

        public Vertex(int id)
        {
            ID = id;
        }


        protected bool Equals(Vertex other)
        {
            return ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            return obj is Vertex vertex &&
                   Equals(vertex);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
