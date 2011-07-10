using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;
using MNS_Reporting;
using Location;

namespace ModularNetworkSimulator
{
    public interface INodeFactory
    {
        // static List<PanelObj> setupPanel(); // This IS required.
        List<PanelObj> SetupPanel { get; }

        List<PanelObj> PanelObjs { get; set; }

        bool IsInitialized { get; }
        IEventManager EventManager { get; }
        IPhysicalProcessor PhysicalProcesser { get; }
        IRandomizerFactory RandomizerFactory { set; }

        void Initialize(IEventManager eventMgr, IPhysicalProcessor physProc,
            IRandomizerFactory randomizerFactory, IReportObserver reporter);
        INode CreateNode(ILocation loc);
    }
}
