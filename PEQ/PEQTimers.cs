using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModularNetworkSimulator;

namespace PEQ
{
    class PEQTimerMessage : NodeTimerEvent
    {
     // For generic messages that need to be sent in the future
     // (Required because Physical Processor only works in the present)

        public PEQTimerMessage()
        { }

        public PEQTimerMessage(IMessage message, double time, INode referrer)
        {
            MessageEvent msgEvent = new MessageEvent(message);
            msgEvent.Referrer = referrer;
            this.node = referrer;
            this.Time = time;
            this.Event = msgEvent;
        }
    }

    class PEQTimerInternal : NodeTimerEvent
    {
     // For generic internal messages that need to be processed in the future

        public PEQTimerInternal()
        { }

        public PEQTimerInternal(IMessage message, double time, INode referrer)
        {
            MessageEvent msgEvent = new MessageEvent(message);
            msgEvent.Referrer = referrer;
            this.node = referrer;
            this.Time = time;
            this.Event = msgEvent;
        }
    }

    class PEQTimerHello : NodeTimerEvent
    {
        public PEQTimerHello()
        { }

        public PEQTimerHello(double time, INode node)
        {
            this.Time = time;
            this.node = node;
        }
    }

    class PEQTimerAck : NodeTimerEvent
    {
        public PEQTableEntryMessageAck _AckInfo;

        public PEQTimerAck()
        { }

        public PEQTimerAck(double time, INode node, PEQTableEntryMessageAck AckInfo)
        {
            this.Time = time;
            this.node = node;
            _AckInfo = AckInfo;
        }

        new public void Execute()
        {
            this.node.ExecuteAction(this);
        }
    }

    class PEQTimerSearch : NodeTimerEvent
    {
        public VarID _SinkID;

        public int _Count = 0;

        public PEQTimerSearch()
        { }

        public PEQTimerSearch(double time, INode node)
        {
            this.Time = time;
            this.node = node;
        }

        new public void Execute()
        {
            this.node.ExecuteAction(this);
        }
    }
}
