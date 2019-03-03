using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTheSpot.Services
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworConnection();
    }
}
