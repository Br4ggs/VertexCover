using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover
{
    public class VertexCoverGraphKernelizer : IGraphKernelizer
    {
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

            IEnumerable<Vertex> isolatedVertices = FindIsolatedVertices(graph); //these will be discarded outright

            IEnumerable<Vertex> pendantVertices = FindPendantVertices(graph); //these will be added to the vertex cover, decrease k by the amount found

            IEnumerable<Vertex> topVertices = FindTopVertices(graph, k);

            return new KernelizedAttributes(pendantVertices, topVertices, isolatedVertices, graph);
        }

        /// <summary>
        /// Get a set of all vertices with a specific amount of edges
        /// </summary>
        /// <param name="graph">The graph you want to query</param>
        /// <returns>The set of all vertices that are pendent</returns>
        public IEnumerable<Vertex> FindPendantVertices(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() == 1);
        }

        /// <summary>
        /// Get a set of all vertices with less than k elements
        /// </summary>
        /// <param name="graph">The graph you want to query</param>
        /// <param name="k">The k weights required</param>
        /// <returns>The set of all top vertices</returns>
        public IEnumerable<Vertex> FindTopVertices(Graph graph, int k)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return graph.Vertices.Where(vertex => graph.GetEdges(vertex).Count() > k);
        }

        /// <summary>
        /// Get a set of all independent vertices
        /// </summary>
        /// <param name="graph">The graph you want to query</param>
        /// <returns>The set of all vertices with no edges</returns>
        public IEnumerable<Vertex> FindIsolatedVertices(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return graph.Vertices.Where(vertex => !graph.GetEdges(vertex).Any());
        }
    }
}
