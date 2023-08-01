using System;
using System.Collections.Generic;
using Wlan;

internal class WlanClient
{
    public IEnumerable<WlanInterface> Interfaces { get; internal set; }

    internal class WlanInterface
    {
        internal WlanBssEntry[] GetNetworkBssList()
        {
            throw new NotImplementedException();
        }
    }
}