using System;
using TSN.Utility.Exceptions;

namespace TSN.Utility.Entities
{
    public abstract class DisposableBase : IDisposable
    {
        protected DisposableBase()
        {
            _disposed = false;
        }
        ~DisposableBase()
        {
            Dispose(false);
        }


        protected readonly object _locker = new object();

        private bool _disposed;

        public bool IsDisposed
        {
            get
            {
                lock (_locker)
                    return _disposed;
            }
        }



        private void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;
            lock (_locker)
            {
                if (disposing)
                {
                    DisposeInstance();
                    GC.Collect();
                }
                _disposed = true;
            }
        }
        protected void ThrowIfDisposed(string objectName = null)
        {
            if (IsDisposed)
                if (string.IsNullOrEmpty(objectName))
                    throw new AlreadyDisposedException();
                else
                    throw new ObjectDisposedException(objectName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Do not lock/block the thread and do not call <see cref="GC.Collect()"/>; just free the memory and/or call <see cref="IDisposable.Dispose()"/> for compatible instances. Also implement as virtual; not sealed.
        /// </summary>
        /// <example>
        /// If a derived class is NOT directly deriving from <see cref="DisposableBase"/>, 'base.DisposeInstance();' code should be used at the top of its method which will be implemented from this. For example:
        /// <code>
        /// protected override void DisposeInstance()
        /// {
        ///     base.DisposeInstance();
        ///     ...
        /// }
        /// </code>
        /// </example>
        protected abstract void DisposeInstance();
    }
}