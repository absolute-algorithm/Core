using System;
using System.Net.NetworkInformation;

namespace AbsoluteAlgorithm.Core.Networking;

/// <summary>
/// Provides helper methods for network-related tasks.
/// </summary>
public static class Connectivity
{
    /// <summary>
    /// Pings a host to check connectivity.
    /// </summary>
    public static bool PingHost(string host, int timeout = 1000)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(host);

        using var ping = new Ping();
        var reply = ping.Send(host, timeout);
        return reply?.Status == IPStatus.Success;
    }
}