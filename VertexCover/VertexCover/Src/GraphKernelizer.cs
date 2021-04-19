using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;

namespace VertexCover
{
    public class GraphKernelizer
    {
        private Random random = new Random();

        /// <summary>
        /// Kernelize the graph for vertex cover
        /// </summary>
        /// <param name="graph">The graph you want to kernelize</param>
        /// <param name="k"></param>
        /// <returns></returns>
        public KernelizedAttributes FindKernelizedAttributes(Graph graph, int k)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return new KernelizedAttributes(FindPendantVertices(graph), FindTopVertices(graph, k), FindIsolatedVertices(graph), graph);
        }

        /// <summary>
        /// Get a set of all vertices with a specific amount of edges
        /// </summary>
        /// <param name="graph">The graph you want to query</param>
        /// <returns>The set of all vertices that are pendent</returns>
        public IReadOnlyCollection<Vertex> FindPendantVertices(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return (IReadOnlyCollection<Vertex>)graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() == 1);
        }

        /// <summary>
        /// Get a set of all vertices with less than k elements
        /// </summary>
        /// <param name="graph">The graph you want to query</param>
        /// <param name="k">The k weights required</param>
        /// <returns>The set of all top vertices</returns>
        public IReadOnlyCollection<Vertex> FindTopVertices(Graph graph, int k)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return (IReadOnlyCollection<Vertex>)graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() > k);
        }

        /// <summary>
        /// Get a set of all independent vertices
        /// </summary>
        /// <param name="graph">The graph you want to query</param>
        /// <returns>The set of all vertices with no edges</returns>
        public IReadOnlyCollection<Vertex> FindIsolatedVertices(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return (IReadOnlyCollection<Vertex>)graph.Vertices.Where(vertex => !graph.GetEdges(vertex).Any());
        }
    }

    public struct KernelizedAttributes
    {
        IReadOnlyCollection<Vertex> Pendants { get; }
        IReadOnlyCollection<Vertex> Tops { get; }
        IReadOnlyCollection<Vertex> Independents { get; }
        Graph Graph { get; }

        public KernelizedAttributes(
            IReadOnlyCollection<Vertex> pendants,
            IReadOnlyCollection<Vertex> tops,
            IReadOnlyCollection<Vertex> independents,
            Graph graph)
        {
            Pendants = pendants;
            Tops = tops;
            Independents = independents;
            Graph = graph;
        }
    }
}
