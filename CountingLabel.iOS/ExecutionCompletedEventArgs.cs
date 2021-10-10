using System;
namespace CountingLabel.iOS
{
    public class ExecutionCompletedEventArgs : EventArgs
    {
        public bool IsCancelled { get; }

        public double Duration { get; }

        internal ExecutionCompletedEventArgs(bool isCancelled, double duration)
        {
            IsCancelled = isCancelled;
            Duration = duration;
        }
    }
}
