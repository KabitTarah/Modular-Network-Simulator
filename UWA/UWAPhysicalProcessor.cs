using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModularNetworkSimulator;
using MNS_GraphicsLib;
using MNS_Reporting;
using Location;

namespace UWA
{
    public class UWAPhysicalProcessor : IPhysicalProcessor
    {
        static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();

            PanelObj obj = new PanelObj();
            obj.name = "cLabel";
            obj.type = FormType.label;
            obj.text = "Speed of Sound (m/s)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "SoundSpeed";
            obj.type = FormType.doubleBox;
            obj.value = "1500.0";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "bitrateLabel";
            obj.type = FormType.label;
            obj.text = "Bitrate (bits/s)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "Bitrate";
            obj.type = FormType.doubleBox;
            obj.value = "2400.0";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "procdelayLabel";
            obj.type = FormType.label;
            obj.text = "Processing Delay (s)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 2;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "ProcessingDelay";
            obj.type = FormType.doubleBox;
            obj.value = "0.001";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 2;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "spreadingCoefLabel";
            obj.type = FormType.label;
            obj.text = "Spreading Coefficient (k)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 3;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "spreadingCoef";
            obj.type = FormType.doubleBox;
            obj.value = "1.5";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 3;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "transmitPowerLabel";
            obj.type = FormType.label;
            obj.text = "Xmit Power (dB SPL)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 4;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "transmitPower";
            obj.type = FormType.doubleBox;
            obj.value = "120";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 4;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "receivePowerLabel";
            obj.type = FormType.label;
            obj.text = "Rcv Min (dB SPL)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 5;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "receivePower";
            obj.type = FormType.doubleBox;
            obj.value = "80";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 5;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "freqLabel";
            obj.type = FormType.label;
            obj.text = "Frequency (Hz)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 6;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "frequency";
            obj.type = FormType.doubleBox;
            obj.value = "20000";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 6;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "noiseLevelLabel";
            obj.type = FormType.label;
            obj.text = "Amb. Noise (dB SPL)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 7;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "noiseLevel";
            obj.type = FormType.doubleBox;
            obj.value = "20";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 7;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "overheadBytesLabel";
            obj.type = FormType.label;
            obj.text = "L2 Overhead (Bytes)";
            obj.width = 70;
            obj.xSlot = 0;
            obj.ySlot = 8;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "OverheadBytes";
            obj.type = FormType.intBox;
            obj.value = "4";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 8;
            list.Add(obj);

            return list;
        }
        public List<PanelObj> SetupPanel
        { get { return setupPanel(); } }

        public List<PanelObj> panelObjs;
        public List<PanelObj> PanelObjs
        {
            get { return panelObjs; }
            set { panelObjs = value; }
        }
        PanelObjHelper panelObjsHelper;

        bool isInitialized;
        public bool IsInitialized
        {
            get { return isInitialized; }
        }

        double bitrate = 2400.0; // bit/s
        public double TransmissionSpeed
        {
            get { return bitrate; }
            set { bitrate = value; }
        }

        double c = 1500.0;  // m/s
        public double PropagationSpeed
        {
            get { return c; }
            set { c = value; }
        }

        double maxRange = double.MaxValue;
        public double MaximumRange
        {
            get { return maxRange; }
            set { maxRange = value; }
        }

        double freq = 20000;  // Hz
        double transmitPower = 120; // dB SPL
        double minReceivePower = 40; // dB SPL
        double k = 1.5;       // Spreading Coefficient.
        double noiseLevel = 20;  // dB / decade

        ReporterIWF repIWF;
        public ReporterIWF RepIWF
        {
            get { return repIWF; }
            set { repIWF = value; }
        }

        INodes nodes;
        IEventManager eventMgr;
        L2Nodes l2nodes;

        double processingDelay = 0;
        int overheadBytes = 0;


        public UWAPhysicalProcessor()
        {
            isInitialized = false;
        }

        public UWAPhysicalProcessor(INodes nodes, IEventManager eventMgr)
        {
            Initialize(nodes, eventMgr);
        }

        public void Initialize(INodes nodes, IEventManager eventMgr)
        {
            panelObjsHelper = new PanelObjHelper(panelObjs);

            c = panelObjsHelper.GetDoubleByName("SoundSpeed");
            bitrate = panelObjsHelper.GetDoubleByName("Bitrate");

            processingDelay = panelObjsHelper.GetDoubleByName("ProcessingDelay");

            overheadBytes = panelObjsHelper.GetIntByName("OverheadBytes");

            freq = panelObjsHelper.GetDoubleByName("frequency");

            transmitPower = panelObjsHelper.GetDoubleByName("transmitPower");

            minReceivePower = panelObjsHelper.GetDoubleByName("receivePower");

            noiseLevel = panelObjsHelper.GetDoubleByName("noiseLevel");

            k = panelObjsHelper.GetDoubleByName("spreadingCoef");

            this.nodes = nodes;
            this.eventMgr = eventMgr;

            maxRange = getMaxRange(transmitPower, minReceivePower);

            l2nodes = new L2Nodes();
            NodesIterator nodesIterator = new NodesIterator(this.nodes);
            while (!nodesIterator.IsDone())
                l2nodes.AddL2Node(new L2Node(nodesIterator.Next().ID, minReceivePower, noiseLevel));

            isInitialized = true;
        }

        public void ExecuteAction(IEvent e)
        {
            if (!isInitialized)
                throw new InvalidOperationException("UWAPhysicalProcessor not initialized!");

            NodesIterator iterator;

            INode currentNode;
            INode referrerNode;

            MessageEvent e_msg;
            MessageEvent MessageEveClone;
            L2Event e_L2;

            NodeTimerEvent beginTransmitTimerEvent;
            NodeTimerEvent endTransmitTimerEvent;
            NodeTimerEvent beginReceiveTimerEvent;
            InTransitEvent inTransitTimerEvent;

            ReportLevel msgLevel = ReportLevel.Routine;

            double currentDistance;
            double transmissionTime;
            double propagationTime;
            double receivePower;

            if (e is MessageEvent)
            {
                e_msg = (MessageEvent)e;
                transmissionTime = calculateTransmissionTime(e_msg.message.SizeBytes() 
                    + overheadBytes);

                if (e_msg.Referrer is INode)
                {
                    referrerNode = (INode)e_msg.Referrer;
                    iterator = new NodesIterator(this.nodes);
                    msgLevel = referrerNode.GetMessageLevel(e_msg.message);

                    // Begin Transmitting at referring node
                    referrerNode.PreTransmit(); // Perform pre-transmit activities at node.
                    beginTransmitTimerEvent = new NodeBeginTransmitEvent();
                    beginTransmitTimerEvent.Time = this.eventMgr.CurrentClock + processingDelay;
                    beginTransmitTimerEvent.node = referrerNode;
                    beginTransmitTimerEvent.Duration = transmissionTime;
                    this.eventMgr.AddEvent(beginTransmitTimerEvent);

                    // Generate Graphical Reports (Wave)
                    if (!e.SuppressReport)
                        repIWF.SendWaveReport(this.eventMgr.CurrentClock + processingDelay,
                            (e_msg.message.SizeBytes() + overheadBytes) * 8, msgLevel, (INode)e_msg.Referrer);

                    // End Transmitting at referring node
                    endTransmitTimerEvent = new NodeEndTransmitEvent();
                    endTransmitTimerEvent.Time = this.eventMgr.CurrentClock + processingDelay
                        + calculateTransmissionTime(e_msg.message.SizeBytes()
                             + overheadBytes);
                    endTransmitTimerEvent.node = referrerNode;
                    this.eventMgr.AddEvent(endTransmitTimerEvent);

                    while (!iterator.IsDone())
                    {
                        currentNode = iterator.Next();
                        currentDistance = referrerNode.Location.Distance(currentNode.Location);
                        propagationTime = calculatePropagationTime(currentDistance);
                        receivePower = transmitPower - transmissionLoss(currentDistance);
                        bool hasClosure = true;
                        if (receivePower < minReceivePower)
                            hasClosure = false;

                        if ((currentDistance <= this.maxRange) && (currentDistance > 0))
                        {
                            // Clone the message event
                            MessageEveClone = (MessageEvent)e.Clone();
                            MessageEveClone.Time = processingDelay + transmissionTime
                                + propagationTime + this.eventMgr.CurrentClock;
                            MessageEveClone.Referrer = currentNode;

                            // Begin Receving at transmitting node.
                            beginReceiveTimerEvent = new NodeBeginReceiveEvent();
                            beginReceiveTimerEvent.Time = this.eventMgr.CurrentClock + processingDelay
                                + propagationTime;  // FIRST bit arrives at receiver
                            beginReceiveTimerEvent.node = currentNode;
                            beginReceiveTimerEvent.Duration = transmissionTime;
                            this.eventMgr.AddEvent(beginReceiveTimerEvent);

                            // Create L2 Event
                            e_L2 = new L2Event(this, referrerNode, MessageEveClone, 
                                propagationTime, transmissionTime, receivePower, hasClosure);
                            this.eventMgr.AddEvent(e_L2);

                        } // if out of range, drop message
                    }

                } // if Referrer <> INode, drop message.
            }
            else if (e is L2Event)
            {
                e_L2 = (L2Event)e;

                e_msg = (MessageEvent)e_L2.EncapsulatedEvent;
                INode Receiver = (INode)e_msg.Referrer;
                L2Node tempL2node = l2nodes.GetNodeByID(Receiver.ID);
                msgLevel = e_L2.Sender.GetMessageLevel((IMessage)e_msg.message);

                if (e_L2.Start)
                {
                    tempL2node.StartEvent(e_L2.Sender.ID, e_L2.BaseReceivePower,
                        e_L2.Time, e_L2.Duration);

                    e_L2.Time = e_L2.Time + e_L2.Duration;
                    e_L2.Start = false;
                    eventMgr.AddEvent(e_L2);
                }
                else
                {
                    if (e_L2.HasClosure)
                    {
                        if (tempL2node.EndEvent(e_L2.Sender.ID, e_L2.Time))
                        { // Collision occurred
                            Receiver.ForceCollision();
                        }
                        else
                        { // Deliver Message
                            eventMgr.AddEvent(e_L2.EncapsulatedEvent);
                        }

                        // Generate Graphical Reports (Message)
                        if (!e_msg.SuppressReport)
                            if (!e_msg.Directed || (e_msg.NextHopID == tempL2node.ID))
                                repIWF.SendMessageReport(e_L2.Time,
                                    (e_msg.message.SizeBytes() + overheadBytes) * 8,
                                    msgLevel, e_L2.Sender, Receiver);
                    }
                }
            }
        }

        double calculateTransmissionTime(int sizeBytes)
        { // Calculates the delay in putting the message bits into the medium.
            return sizeBytes * 8 / bitrate;
        }

        double calculatePropagationTime(double distance)
        { // Calculates the delay in transmitting a message over the provided distance
            return distance / c;
        }

        double getMaxRange(double transmitPower, double receivePower)
        {
            double distance = 1;
            double power = transmitPower;

            while (power > receivePower)
            {
                distance = distance + 10;
                power = transmitPower - transmissionLoss(distance);
            }

            return distance - 1;
        }

        double thorpsApproximation()
        {
            return thorpsApproximation(freq);
        }

        double thorpsApproximation(double freq)
        {
            // freq in Hz...
            double f = freq / 1000; // Freq in kHz
            double f2 = Math.Pow(f, 2);

            return 0.11 * f2 / (1 + f2) + 44 * f2 / (4100 + f)
                + Math.Pow(2.75 * 10, -4) * f2 + 0.003;
        }

        double transmissionLoss(double distance)
        { // distance in meters
            return k * 10 * Math.Log10(distance) 
                + distance / 1000 * thorpsApproximation();
        }
    }
}
