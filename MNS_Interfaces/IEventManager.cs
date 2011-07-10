using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public interface IEventManager
    { // The interface for the raw guts of this simulator. The simulator has been created to be modular
        // and this interface can certainly be implemented in some other way. It is quite possible, with some
        // customization, for the simulator clock to be driven by an external process (e.g. an HLA / DIS
        // simulation protocol). 
        double CurrentClock { get; }

        void AddEvent(IEvent e);
        void StartSimulation();
    }
}
