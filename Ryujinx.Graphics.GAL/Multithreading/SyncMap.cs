﻿using System.Collections.Generic;
using System.Threading;

namespace Ryujinx.Graphics.GAL.Multithreading
{
    class SyncMap
    {
        private HashSet<ulong> _inFlight = new HashSet<ulong>();
        private AutoResetEvent _inFlightChanged = new AutoResetEvent(false);

        internal void CreateSyncHandle(ulong id)
        {
            lock (_inFlight)
            {
                _inFlight.Add(id);
            }
        }

        internal void AssignSync(ulong id)
        {
            lock (_inFlight)
            {
                _inFlight.Remove(id);
            }

            _inFlightChanged.Set();
        }

        internal void WaitSyncAvailability(ulong id)
        {
            // Blocks until the handle is available.

            bool signal = false;

            while (true)
            {
                lock (_inFlight)
                {
                    if (!_inFlight.Contains(id))
                    {
                        break;
                    }
                }

                _inFlightChanged.WaitOne();
                signal = true;
            }

            if (signal)
            {
                // Signal other threads which might still be waiting.
                _inFlightChanged.Set();
            }
        }
    }
}
