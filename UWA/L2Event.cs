using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModularNetworkSimulator;

namespace UWA
{
    public class L2Event : IEvent
    {
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

        public bool SuppressReport
        {
            get { return false; }
            set { }
        }

        public IPhysicalProcessor PhysicalProcessor;
        public IEvent EncapsulatedEvent;

        public bool Start = true; // first event reception
        public double BaseReceivePower; // dB SPL

        public INode Sender;

        public bool HasClosure;

        public L2Event(IPhysicalProcessor referrer, INode sender, IEvent eve,
            double time, double duration, double receivePower, bool hasClosure)
        {
            this.PhysicalProcessor = referrer;
            this.Sender = sender;
            this.EncapsulatedEvent = eve;
            this.time = time;
            this.duration = duration;
            this.BaseReceivePower = receivePower;
            this.HasClosure = hasClosure;
        }

        public void Execute()
        {
            PhysicalProcessor.ExecuteAction(this);
        }

        public object Clone()
        {
            return new L2Event(this.PhysicalProcessor, this.Sender, this.EncapsulatedEvent,
                this.Time, this.Duration, this.BaseReceivePower, this.HasClosure);
        }
    }
}
