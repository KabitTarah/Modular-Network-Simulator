using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public class MessageEvent : IEvent
    { // This event encapsulates a message for transmission through the relevant medium. This event passes through
        // intermediate handlers (such as the IPhysicalProcessor) before reaching the end node(s). The IMessage implementation
        // will be defined for the specific protocol being studied.
        double time;
        public double Time
        {
            get { return time; }
            set { time = value; }
        }

        double duration = 0;
        public double Duration
        {
            get { return duration; }
            set { duration = 0; }
        }

        bool suppressReport = false;
        public bool SuppressReport
        {
            get { return suppressReport; }
            set { suppressReport = value; }
        }

        public IMessage message;

        public IReferrer Referrer; // Either an INode or IPhysicalProcessor... or some 3rd party implementation of this intf

        public bool Directed = false;
        public int NextHopID = int.MaxValue;

        public MessageEvent(IMessage message)
        {
            this.message = message;
        }

        public void Execute()
        {
            this.Referrer.ExecuteAction(this);
        }

        public object Clone()
        {
            IMessage message = (IMessage)this.message.Clone();
            MessageEvent clone = new MessageEvent(message);
            clone.time = time;
            clone.Referrer = Referrer;
            clone.SuppressReport = suppressReport;
            return clone;
        }
    }
}
