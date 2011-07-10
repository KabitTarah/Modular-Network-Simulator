using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public interface IReferrer
    { // An interface describing a class that can create an event (Command Pattern) and be called by that
        // event.
        void ExecuteAction(IEvent e);
    }
}
