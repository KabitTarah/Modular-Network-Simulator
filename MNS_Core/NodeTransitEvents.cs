using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public class InTransitEvent : NodeTimerEvent
    {
        public INode Source;
        public INode Destination;

        public IMessage Message;

        public double TransmissionTime;
        public double PropagationTime;

        public override void Execute()
        { } // Do nothing... really! -- This only gets sent to the Model for visual processing.
    }

    public class NodeBeginTransmitEvent : NodeTimerEvent
    { // Indicates that the Node has begun transmitting. When this occurs, a busy counter is incremented.
        override public void Execute()
        {
            this.node.BeginTransmit();
        }
    }

    public class NodeEndTransmitEvent : NodeTimerEvent
    { // Indicates that the Node has stopped transmitting. When this occurs, the busy counter is decremented and, 
        // if greater than zero (after decrementing), a collision flag is set. If zero, the collision flag is cleared.
        override public void Execute()
        {
            this.node.EndTransmit();
        }
    }

    public class NodeBeginReceiveEvent : NodeTimerEvent
    { // Indicates that the Node has begun receiving. When this occurs, the busy counter is incremented.
        override public void Execute()
        {
            this.node.BeginReceive();
        }
    }
    // note that NodeEndReceiveEvent, the logical conclusion, is not required as it occurs at the same time as the MessageEvent.
}
