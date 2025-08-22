using System;

namespace TSN.FileToVideo
{
    internal class CancellableEventArgs : EventArgs
    {
        public CancellableEventArgs()
        {
            Cancel = false;
        }


        public bool Cancel { get; private set; }



        public void CancelOperation() => Cancel = true;
    }
    internal class ProgressChangedEventArgs : CancellableEventArgs
    {
        public ProgressChangedEventArgs(int progress, int count)
        {
            if (progress > count)
                throw new ArgumentOutOfRangeException();
            _progress = progress;
            _count = count;
        }


        private readonly int _progress;
        private readonly int _count;

        public int Progress => _progress;
        public int Count => _count;
    }
    internal class CalculatedFileEventArgs : CancellableEventArgs
    {
        public CalculatedFileEventArgs(long bufferLengthInBytes, int framesCount, TimeSpan minimumVideoLength)
        {
            _bufferLengthInBytes = bufferLengthInBytes;
            _framesCount = framesCount;
            _minimumVideoLength = minimumVideoLength;
        }


        private readonly long _bufferLengthInBytes;
        private readonly int _framesCount;
        private readonly TimeSpan _minimumVideoLength;

        public long BufferLengthInBytes => _bufferLengthInBytes;
        public int FramesCount => _framesCount;
        public TimeSpan MinimumVideoLength => _minimumVideoLength;
    }
}