using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;
using MNS_Reporting;
using Location;

namespace ModularNetworkSimulator
{
    public interface IObserver
    {
        void Update();
    }
}
