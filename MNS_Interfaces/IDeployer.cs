using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;
using Location;

namespace ModularNetworkSimulator
{
    public interface IDeployer
    {
        // static List<PanelObj> setupPanel(); // This IS required.
        List<PanelObj> SetupPanel { get; }
        List<PanelObj> PanelObjs { get; set; }

        bool IsInitialized { get; }

        INodes Nodes { get; }
        INodeFactory NodeFactory { get; }
        ILocation[] Field { get; }

        IRandomizerFactory RandFactory { set; }

        void Initialize(INodes nodes, INodeFactory nodeFactory, ILocation[] field,
            IRandomizerFactory randFactory);
        void Deploy(); // The Deploy function sets up the INodes array.
    }
}
