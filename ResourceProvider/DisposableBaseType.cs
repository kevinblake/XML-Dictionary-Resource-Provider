using System;

namespace Localisation
{
    public class DisposableBaseType : IDisposable
    {
        private bool _mDisposed;

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (_mDisposed) return;
                Cleanup();
                _mDisposed = true;

                GC.SuppressFinalize(this);
            }
        }

        #endregion

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        protected virtual void Cleanup()
        {
            // override to provide cleanup
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DisposableBaseType"/> is reclaimed by garbage collection.
        /// </summary>
        ~DisposableBaseType()
        {
            Cleanup();
        }

    }

}
