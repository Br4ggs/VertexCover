using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VertexCover
{
    public class VertexCoverPerformance
    {
        private readonly GraphPreprocessor graphPreprocessor;
        private readonly Stopwatch stopwatch;
        private readonly MatrixBuilder builder;
        private readonly Random random;

        public VertexCoverPerformance(GraphPreprocessor graphPreprocessor, MatrixBuilder builder)
        {
            this.graphPreprocessor = graphPreprocessor ?? throw new ArgumentNullException(nameof(graphPreprocessor));
            stopwatch = new Stopwatch();
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
            random = new Random();
        }

        public List<PerformanceTest> PerformanceTest(int maxSize, int increment, int repetitions, float percentageOfSizeForK)
        {
            List<PerformanceTest> values = new List<PerformanceTest>();
            for (int i = 1; i < maxSize; i += increment + 1)
            {
                var adjacencyMatrix = builder.GenerateAdjacencyMatrix(i, random.Next(100));
                Graph graph = new Graph(adjacencyMatrix);

                int size = (int)Math.Round(i * percentageOfSizeForK);
                TimeSpan preprocessedCoverPerformance = MeasurePerformance(() =>
                {
                    GeneratePreprocessedCover(graph, size);
                }, repetitions);

                TimeSpan vertexCoverPerformance = MeasurePerformance(() =>
                {
                    VertexCoverUtils.GetVertexCover(graph, size);
                }, repetitions);

                values.Add(new PerformanceTest(graph, size, vertexCoverPerformance, preprocessedCoverPerformance));
            }
            return values;
        }

        private void GeneratePreprocessedCover(Graph graph, int size)
        {
            PreProcessedGraphAttributes attributes = graphPreprocessor.GetProcessedGraph(graph, size);
            List<Vertex> baseCover = graphPreprocessor.GetBaseVertexCover(attributes);
            graph = attributes.ProcessedGraph;

            if (graph.Edges.Count <= 0)
            {
                return;
            }

            VertexCoverUtils.GetVertexCover(graph, size, baseCover);
        }

        private TimeSpan MeasurePerformance(Action performanceMeasurement, int repetitions)
        {
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < repetitions; i++)
            {
                performanceMeasurement.Invoke();
            }
            stopwatch.Stop();
            return stopwatch.Elapsed / repetitions;
        }
    }

    public readonly struct PerformanceTest
    {
        public Graph Graph { get; }
        public int Size { get; }

        public TimeSpan VertexCover { get; }
        public TimeSpan PreprocessedVertexCover { get; }

        public PerformanceTest(Graph graph, int size, TimeSpan vertexCover, TimeSpan preprocessedVertexCover)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Size = size;
            VertexCover = vertexCover;
            PreprocessedVertexCover = preprocessedVertexCover;
        }

        public override string ToString()
        {
            return
                $"{Graph.Vertices.Count},{Size},{VertexCover.TotalMilliseconds},{PreprocessedVertexCover.TotalMilliseconds}";
        }
    }

}
