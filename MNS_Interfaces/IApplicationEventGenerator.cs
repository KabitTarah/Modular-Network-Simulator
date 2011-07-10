using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;
using MNS_Reporting;
using Location;

namespace ModularNetworkSimulator
{
    public interface IApplicationEventGenerator : IReportSubject
    { // This module generates a physical event (detection, threshold, etc) and provides this event
        // to the application layer of the Node. This module can be considered completely optional,
        // but may be necessary for sensor networks to operate.

        // static List<PanelObj> setupPanel(); // This IS required.
        List<PanelObj> SetupPanel { get; }
        List<PanelObj> PanelObjs { get; set; }

        bool IsInitialized { get; }

        IEventManager EventMgr { set; }
        INodes Nodes { set; }
        IRandomizer Randomizer { set; }
        ILocation[] Field { set; }

        void Initialize(IEventManager eventMgr, INodes nodes,
            IRandomizer randomizer, ILocation[] field);
        void GenerateEvent();
        void GenerateEvent(ILocation location);
    }
}
