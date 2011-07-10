using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;

namespace ModularNetworkSimulator
{
    public class BasicUWAPhysicalProcessor : IPhysicalProcessor
    { // Large sections are commented out. For now, no actual underwater propagation is used.

        static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();

            PanelObj obj = new PanelObj();
            obj.name = "cLabel";
            obj.type = FormType.label;
            obj.text = "Speed of Sound (m/s)";
            obj.width = 58;
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
            obj.width = 58;
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
            obj.width = 58;
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
            obj.name = "maxrangeLabel";
            obj.type = FormType.label;
            obj.text = "Maximum Range (m)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 3;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "MaxRange";
            obj.type = FormType.doubleBox;
            obj.value = "1000";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 3;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "overheadBytesLabel";
            obj.type = FormType.label;
            obj.text = "L2 Overhead (Bytes)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 4;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "OverheadBytes";
            obj.type = FormType.intBox;
            obj.value = "4";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 4;
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

        INodes nodes;
        IEventManager eventMgr;

        // Speed of Sound. In a more advanced UWA Phys Proc, this could be a sound speed profile.
        double c = 1500.0;  // m/s
        public double PropagationSpeed 
        {
            get { return c; }
            set { c = value; }
        }

        // Channel / Modulation bit rate. This should be user settable.
        double bitrate = 2400.0; // bit/s
        public double TransmissionSpeed 
        {
            get { return bitrate; }
            set { bitrate = value; }
        }

        // Static delay added to every message event
        double processingDelay = 0.001; // s
        public double ProcessingDelay
        {
            get { return processingDelay; }
            set { processingDelay = value; }
        }

        /*        double SPL_1m = 150.0; // dB re: 1 uPa
                public double dB_SPL
                {
                    get { return SPL_1m; }
                    set { SPL_1m = value; }
                }

                double noiseLevel = 45; // dB re: 1 uPa (approx Sea State 3 @ 10 kHz
                public double NL
                {
                    get { return noiseLevel; }
                    set { noiseLevel = value; }
                }

                double minSNR = 30;
                public double MinSNRdB
                {
                    get { return minSNR; }
                    set { minSNR = value; }
                }
        */
        // Maximum range of a transmission. No interference exists in this "ideal" model
        double maxRange = 1001; // m (for now, ignoring dB calculations)
        public double MaximumRange 
        {
            get { return maxRange; }
            set { maxRange = value; }
        }

        int overheadBytes;

        ReporterIWF repIWF;
        public ReporterIWF RepIWF
        {
            get { return repIWF; }
            set { repIWF = value; }
        }

        /*double freq = 10000.0; // Hz
        public double Frequency
        {
            get { return freq; }
            set { freq = value; }
        }

        double absorptionCoefficient; // dB/km */

        public BasicUWAPhysicalProcessor()
        {
            isInitialized = false;
        }

        public BasicUWAPhysicalProcessor(INodes nodes, IEventManager eventMgr)
        {
            Initialize(nodes, eventMgr);
        }

        public void Initialize(INodes nodes, IEventManager eventMgr)
        { // Does nothing at this point...
            /*    double freq_khz = freq / 1000;
                absorptionCoefficient = Math.Pow(freq_khz, 2) * (0.08/Math.Pow(0.9 + freq_khz, 2)
                    + 30/Math.Pow(3000 + freq_khz, 2) + 4*Math.Pow(10,(-4)));*/
            panelObjsHelper = new PanelObjHelper(panelObjs);
            c = panelObjsHelper.GetDoubleByName("SoundSpeed");
            bitrate = panelObjsHelper.GetDoubleByName("Bitrate");
            processingDelay = panelObjsHelper.GetDoubleByName("ProcessingDelay");
            maxRange = panelObjsHelper.GetDoubleByName("MaxRange");
            overheadBytes = panelObjsHelper.GetIntByName("OverheadBytes");
            this.nodes = nodes;
            this.eventMgr = eventMgr;
            isInitialized = true;
        }

        /*double TL(double range_m)
        {
            double range_km = range_m / 1000;

            return 20 * Math.Log10(range_m) + absorptionCoefficient * range_km;
        }*/

        public void ExecuteAction(IEvent e)
        {
            if (!isInitialized)
                throw new InvalidOperationException("BasicUWAPhysicalProcessor not initialized!");
            NodesIterator iterator;
            INode currentNode;
            MessageEvent e_msg;
            INode referrerNode;
            MessageEvent MessageEveClone;
            NodeTimerEvent beginTransmitTimerEvent;
            NodeTimerEvent endTransmitTimerEvent;
            NodeTimerEvent beginReceiveTimerEvent;
            InTransitEvent inTransitTimerEvent;
            ReportLevel msgLevel = ReportLevel.Routine;

            double currentDistance;
            double transmissionTime;
            double propagationTime;

            // Determine the type of event
            if (e is MessageEvent)
            {
                e_msg = (MessageEvent)e;
                transmissionTime = calculateTransmissionTime(e_msg.message.SizeBytes() 
                    + overheadBytes);
                if (e_msg.Referrer is INode)
                {
                    referrerNode = (INode)e_msg.Referrer;
                    iterator = new NodesIterator(this.nodes);
                    /*******************************************************\
                    |*********************** WARNING ***********************|
                    |*******************************************************|
                    |* The following section needs to be made into a       *|
                    |* module that gets attached to the Physical Processor *|
                    |* Currently, this is counter to the philospohy put    *|
                    |* forth for the simulator. (e.g. This must be         *|
                    |* modified for each protocol).                        *|
                    \*******************************************************/
                    if (e_msg.message is FloodingQueryMessage)
                    {
                        FloodingQueryMessage msg = (FloodingQueryMessage)e_msg.message;
                        switch (msg.Type)
                        {
                            case FloodingQueryMessage.MessageType.hello:
                                msgLevel = ReportLevel.Routine;
                                break;
                            case FloodingQueryMessage.MessageType.query:
                                msgLevel = ReportLevel.Immediate;
                                break;
                            case FloodingQueryMessage.MessageType.data:
                                msgLevel = ReportLevel.FlashOverride;
                                break;
                        }
                    }
                    else
                    {
                        msgLevel = referrerNode.GetMessageLevel(e_msg.message);
                    }

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
                            (e_msg.message.SizeBytes()+overheadBytes)*8, msgLevel, (INode)e_msg.Referrer);

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
                        if ((currentDistance <= this.maxRange) && (currentDistance > 0))
                        {
                            MessageEveClone = (MessageEvent)e.Clone();
                            MessageEveClone.Time = processingDelay + transmissionTime
                                + propagationTime + this.eventMgr.CurrentClock;
                            MessageEveClone.Referrer = currentNode;
                            this.eventMgr.AddEvent(MessageEveClone);

                            // Begin Receving at transmitting node.
                            beginReceiveTimerEvent = new NodeBeginReceiveEvent();
                            beginReceiveTimerEvent.Time = this.eventMgr.CurrentClock + processingDelay
                                + propagationTime;  // FIRST bit arrives at receiver
                            beginReceiveTimerEvent.node = currentNode;
                            beginReceiveTimerEvent.Duration = transmissionTime;
                            this.eventMgr.AddEvent(beginReceiveTimerEvent);

                            // Record Complete Transit
                            inTransitTimerEvent = new InTransitEvent();
                            inTransitTimerEvent.Time = this.eventMgr.CurrentClock + processingDelay;
                            inTransitTimerEvent.Duration = transmissionTime + propagationTime;
                            inTransitTimerEvent.Source = referrerNode;
                            inTransitTimerEvent.Destination = currentNode;
                            inTransitTimerEvent.PropagationTime = propagationTime;
                            inTransitTimerEvent.TransmissionTime = transmissionTime;
                            inTransitTimerEvent.Message = e_msg.message;
                            this.eventMgr.AddEvent(inTransitTimerEvent);

                            // Generate Graphical Reports (Message)
                            if (!e.SuppressReport)
                                if (!e_msg.Directed || (e_msg.NextHopID == currentNode.ID))
                                    repIWF.SendMessageReport(this.eventMgr.CurrentClock, 
                                        (e_msg.message.SizeBytes()+overheadBytes)*8,
                                        msgLevel, (INode)e_msg.Referrer, currentNode);
                        }
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
    }
}
