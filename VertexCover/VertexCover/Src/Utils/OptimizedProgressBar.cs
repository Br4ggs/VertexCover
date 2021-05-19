using System.Windows.Controls;

namespace VertexCover.Utils
{
    public class OptimizedProgressBar
    {
        /// <summary>
        /// Get the base UI element that is used
        /// </summary>
        public ProgressBar UI { get; }

        public ulong Steps { get; private set; }
        public double ProgressPerChunk { get; private set; }
        public ulong ChunkSize { get; private set; }

        public ulong CurrentStep { get; private set; }

        private const int DecimalToPercentage = 100;

        /// <summary>
        /// Link a progress bar to chunk based variant
        /// </summary>
        /// <param name="ui"></param>
        public OptimizedProgressBar(ProgressBar ui)
        {
            UI = ui;
        }

        /// <summary>
        /// Start the progress bar
        /// </summary>
        /// <param name="steps">The amount of steps it will take</param>
        /// <param name="chunkPercentage">What percentage will be a single chunk</param>
        public void StartProgressBar(ulong steps, double chunkPercentage)
        {
            StartProgressBar(steps, (ulong)System.Math.Ceiling(steps * chunkPercentage));
        }

        /// <summary>
        /// Start the progress bar 
        /// </summary>
        /// <param name="steps">The amount of steps it will take</param>
        /// <param name="chunkSize">A specifc amount that will correspond to the chunk</param>
        public void StartProgressBar(ulong steps, ulong chunkSize)
        {
            Steps = steps;
            ChunkSize = chunkSize;
            ProgressPerChunk = (ChunkSize / (double)Steps) * DecimalToPercentage;
        }

        /// <summary>
        /// Take a single step. If a chunk has been finished we update the UI
        /// </summary>
        public void TakeStep()
        {
            if (++CurrentStep < ChunkSize)
                return;

            UI.Value += ProgressPerChunk;
            UI.UpdateLayout();
            CurrentStep = 0;
        }
    }
}
