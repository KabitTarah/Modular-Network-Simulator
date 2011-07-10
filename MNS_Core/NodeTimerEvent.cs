using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public class NodeTimerEvent : IEvent
    { // A Node Timer event lets the EventManager control any timers required inside the node (which would normally
        // be controlled by the node's internal clock). At timer expiry, the timer is sent to the node for processing.
        // Typical usage is to encapsulate another Event inside the Timer Events, indicating the action to be taken.
        double time;
        public double Time
        {
            get { return time; }
            set { time = value; }
        }

        double duration;
        public double Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        bool suppressReport = false;
        public bool SuppressReport
        {
            get { return suppressReport; }
            set { suppressReport = value; }
        }

        public INode node;
        public IEvent Event;

        virtual public void Execute()
        {
            this.node.ExecuteAction(this);
        }

        public object Clone()
        { // Makes an exact copy of the object without simply providing a reference to the object.
            NodeTimerEvent clone = new NodeTimerEvent();
            clone.time = time;
            clone.node = node;
            clone.Event = (IEvent)Event.Clone();
            return clone;
        }
    }
}
