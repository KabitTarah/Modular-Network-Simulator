using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;

namespace ModularNetworkSimulator
{
    public interface IRandomizerFactory
    {
        // static List<PanelObj> setupPanel(); // This IS required.
        List<PanelObj> SetupPanel { get; }

        List<PanelObj> PanelObjs { get; set; }

        bool IsInitialized { get; }
        Int32 InitialSeed { get; }

        IRandomizer InitialRandomVariable { get; set; }

        void SetSeed(Int32 Seed);
        void Initialize();
        IRandomizer CreateRandomizer();
    }
}
