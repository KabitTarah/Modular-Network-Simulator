using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;

namespace ModularNetworkSimulator
{
    public interface IPhysicalProcessor : IReferrer
    {
        // static List<PanelObj> setupPanel(); // This IS required.
        List<PanelObj> SetupPanel { get; }

        List<PanelObj> PanelObjs { get; set; }

        bool IsInitialized { get; }
        double TransmissionSpeed { get; set; }
        double PropagationSpeed { get; set; }
        double MaximumRange { get; set; }

        ReporterIWF RepIWF { get; set; }

        void Initialize(INodes nodes, IEventManager eventMgr);
    }
}
