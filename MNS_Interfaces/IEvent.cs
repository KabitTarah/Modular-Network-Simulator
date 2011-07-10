using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public interface IEvent : ICloneable   // Command Pattern (GOF p.233)
    { // The IEvent requires a Time variable and Execute function, which is invoked by the Event Manager
        // when its Time is due.
        // This is ICloneable (built in interface) because most events will be cloned during processing.
        double Time { get; set; }
        double Duration { get; set; }

        bool SuppressReport { get; set; }   // Default false.

        void Execute();
    }
}
