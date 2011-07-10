using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Location;
using MNS_GraphicsLib;
using MNS_Reporting;

namespace ModularNetworkSimulator
{
    // The following class implements a Message format for the "Flooding Query" Node class.
    // This Message format includes three message types. Message types may be set in an individual message class
    // or, if messages match well, in a single class. It is also possible, given some specific (real) Node code,
    // for a message class to be set-up to use a single string or byte array (resulting in a simpler Message,
    // but a much more complex Node).

    public class FloodingQueryMessage : IMessage
    {
        int messageID;              // Add 2 Bytes when used
        public int MessageID
        {
            get { return messageID; }
        }

        int originatorID;           // Add 2 Bytes when used
        public int OriginatorID
        {
            get { return originatorID; }
            set { originatorID = value; }
        }

        int srcID;                  // Assume 2 Bytes, REQUIRED
        public int SourceID
        {
            get { return srcID; }
        }
        //INode source;               // only for use in FloodingQueryMessage (Lazy way to include a source
        //public INode Source 
        //{ get { return source; } set { source = value; } }     // without assigning IDs to all nodes.
        public ILocation SourceLoc;

        int dstID;                  // Assume 2 Bytes, REQUIRED
        public int DestinationID
        {
            get { return dstID; }
            set { dstID = value; }
        }
        //INode destination;          // See "source" -- only for FloodingQueryMessage
        //public INode Destination 
        //{ get { return destination; } set { destination = value; } }

        int hopCount = 0;           // Assume 2 Bytes
        public int Hops             // ONLY USE IN: query message
        {
            get { return hopCount; }
        }

        public enum MessageType { hello, query, data }  // Assume 1 Byte
        MessageType type;
        public MessageType Type
        {
            get { return this.type; }
        }

        string data;    // ONLY USE IN DATA message
        string Data     // If you really want to see / change the meesage data.
        {
            get { if (this.type == MessageType.data) return data; else return ""; }
            set { if (this.type == MessageType.data) data = value; else data = ""; }
        }

        int size;       // Size of message (bytes)

        public FloodingQueryMessage(int SourceID, int DestinationID, MessageType type)
        {
            size = 25; // set the base size - this will be incremented depending on message type
            this.srcID = SourceID;
            this.dstID = DestinationID;
            this.type = type;
            if (type == MessageType.data)
                this.data = "abcdefghijklmnopqrstuvwxyz"; // Garbage data
        }

        public FloodingQueryMessage(int SourceID, int DestinationID, MessageType type, int MessageID, int OriginatorID)
        { // Overloaded FQM where MessageID and OriginatorID are needed. This is any Data or Query message... (see INode.cs)
            size = 90;
            this.srcID = SourceID;
            this.dstID = DestinationID;
            this.type = type;
            if (type == MessageType.data)
            {
                this.data = "abcdefghijklmnopqrstuvwxyz";
                size = size + this.data.Length;
            }
            this.messageID = MessageID;
            this.originatorID = OriginatorID;
        }

        public void IncrementHops()
        {
            hopCount++;
        }

        public void UpdateSource(int id)
        { // must occur for each query hop (this source field is then used for the path back to the sink).
            if (this.type == MessageType.query)
                this.srcID = id;
        }

        public int SizeBytes()
        { // Required for PhysicalProcessor calculations
            return size;
        }

        public object Clone()
        {
            FloodingQueryMessage clone = new FloodingQueryMessage(this.srcID, this.dstID, this.type);
            clone.data = this.data;
            clone.hopCount = this.hopCount;
            clone.SourceLoc = this.SourceLoc;
            clone.size = this.size;
            return clone;
        }
    }

    public class FloodingQueryApplicationMessage : IMessage, ICloneable
    { // This is mostly empty... simply exists to indicate that a physical event was detected (or threshold reached).
        bool isEvent = true;
        public bool IsEventOccurrance
        {
            get { return isEvent; }
            set { isEvent = value; }
        }

        public int SizeBytes()
        {
            return 0;
        }

        public object Clone()
        {
            FloodingQueryApplicationMessage clone = new FloodingQueryApplicationMessage();
            return clone;
        }
    }

    public class FloodingQueryNode : INode, IReferrer, IReportSubject
    {
        double nodeSize = 1;                    // use same units as ILocation
        public double NodeSize
        {
            get { return nodeSize; }
            set { nodeSize = value; }
        }

        bool collided = false;                  // True if overlap exists during Receive events (See Begin/End Xmit/Rcv methods)
        public bool Collided
        {
            get { return collided; }
        }
        int busyCount = 0;                      // Incremented for each Begin Xmit/Rcv event
        public int Collisions = 0;              // Counter of number of collisions during simulation
        public bool CollisionUpdate = false;    // Tells the Model whether it needs to recalculate the total number of collisions

        IRandomizer randomizer;                 // Random Variable
        public IRandomizer RandomValue
        {
            get { return randomizer; }
            set { randomizer = value; }
        }

        int currentMessageID = 0;               // This is sent for every broadcast / long-distance packet. Other nodes will
        int CurrentMessageID                    // record the originator ID and message ID to determine whether the packet
        {                                       // has already been seen and rebroadcasted. (Prevents broadcast storms)
            get
            {
                if (currentMessageID == 0xffff)
                {
                    currentMessageID = 0;
                    return 0xffff;
                }
                else
                    return currentMessageID++;
            }
        }

        ILocation location;                     // The location of this node.
        public ILocation Location
        {
            get { return location; }
            set { location = value; }
        }

        int id;                                 // Unique node identifier.
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public IEventManager EventMgr;          // Link to the Event Manager class - for enqueuing Timers

        public IPhysicalProcessor PhysProc;     // Link to the Physical Processor class

        GraphicsPath gradientPath = new GraphicsPath();
        public GraphicsPath GradientPath 
        {
            get { return gradientPath; }
            set { gradientPath = value; }
        }

        int nextHop;                            // Next Hop toward Sink
        int distance;                           // Distance to Sink
        ILocation nextHopCheat;     // The "cheater's" way with a direct link to the next node.
                                    // (Used for visual purposes only).

        bool isSink = false;                      // Is this node a sink?
        public bool IsSink
        {
            get { return isSink; }
            set { 
                isSink = value;
                Notify(GenerateStaticReport());
            }
        }

        protected Hashtable _messagesHeard = new Hashtable();   // Contains a list of heard Message IDs and Originator IDs

        public FloodingQueryNode(IEventManager eventMgr, IPhysicalProcessor physProc)
        { // CONSTRUCTOR
            this.nextHop = int.MaxValue;        // Set the nextHop node ID and
            this.distance = int.MaxValue;       // distance to sink equal to an invalid number (int.MaxValue)
            this.EventMgr = eventMgr;
            this.PhysProc = physProc;
        }

        public void Initialize()
        {
            Notify(GenerateStaticReport());

            FloodingQueryMessage message = new FloodingQueryMessage(this.id, 0xffff,
                FloodingQueryMessage.MessageType.hello);  // All new nodes create a hello message to be broadcast
            message.SourceLoc = this.Location;
            MessageEvent initialEvent = new MessageEvent(message); // An event is created with the message
            initialEvent.Referrer = this;
            // Initial Hello randomly occurs 0 - 10 minutes after deployment:

            // Create a timer event for the first Hello packet
            NodeTimerEvent helloTimerEvent = new NodeTimerEvent();
            helloTimerEvent.Event = initialEvent;
            helloTimerEvent.node = this;
            helloTimerEvent.Time = this.EventMgr.CurrentClock + this.randomizer.NextDouble() * 10;

            // Add timer to the Event Queue
            this.EventMgr.AddEvent(helloTimerEvent);

            // If this node is a Sink, add a Timer (Query) event to the event queue, starting in 0 - 5 seconds.
            if (this.IsSink)
            {
                // Create the Query Message
                message = new FloodingQueryMessage(this.id, 0xffff, FloodingQueryMessage.MessageType.query,
                    CurrentMessageID, this.id);
                message.SourceLoc = this.Location;
                // Create the MessageEvent
                MessageEvent QueryEvent = new MessageEvent(message);
                QueryEvent.Referrer = this;

                // Create the Timer event
                NodeTimerEvent queryTimerEvent = new NodeTimerEvent();
                queryTimerEvent.Event = QueryEvent;
                queryTimerEvent.node = this;
                queryTimerEvent.Time = this.EventMgr.CurrentClock + this.randomizer.NextDouble() * 5;

                // Add the TimerEvent to the Event Queue
                this.EventMgr.AddEvent(queryTimerEvent);
            }
        }

        public void ExecuteAction(IEvent e)  // *** THIS SHOULD BE VERY SIMPLE AND DELEGATE TO OTHER METHODS!!! ***
        {                                    // *** CURRENTLY, THIS METHOD IS TOO COMPLEX!!!                    ***
            // This example class will not have sophisticated actions. No major tables or timers are kept.
            // All events will be Message type events.

            MessageEvent e_msg;
            FloodingQueryMessage message;

            NodeTimerEvent e_timer;

            // Check the type of the event to classify it and take the appropriate actions.
            if (e is MessageEvent)
            {
                e_msg = (MessageEvent)e;
                if (!this.EndReceive())  // Checks that no collision has occurred.
                {
                    if (e_msg.message is FloodingQueryMessage)
                    {
                        message = (FloodingQueryMessage)e_msg.message;
                        if (((message.DestinationID == this.id) || (message.DestinationID == 0xffff)) // Ensure destination is self or broadcast
                            && (message.SourceID != this.id)) // and ensure we're not hearing ourself.
                            switch (message.Type)
                            {
                                case FloodingQueryMessage.MessageType.hello:    // HELLO Messages indicate that a node exists
                                    // (usually used to form neighbor tables, links, etc)
                                    // just drop hellos... added to increase statistics
                                    break;
                                case FloodingQueryMessage.MessageType.query:    // QUERY Messages are generated by the sink and flooded
                                    this.processQueryMessage(message);
                                    break;
                                case FloodingQueryMessage.MessageType.data:     // DATA Messages carry Phys.Event data back to the sink
                                    this.processDataMessage(message);
                                    break;
                            }
                        // else drop the packet.
                    }
                    else if (e_msg.message is FloodingQueryApplicationMessage)
                        processApplicationMessage((FloodingQueryApplicationMessage)e_msg.message);
                }
                else
                {
                    e_msg.message = null;
                }
            }
            else if (e is NodeTimerEvent)
            { // A node timer event will be a hello for our purposes. We take the message encapsulated within the Event
                // and send a hello. We then also generate a new NodeTimerEvent for the expiry of the next hello timer.
                e_timer = (NodeTimerEvent)e;

                if (e_timer.Event is MessageEvent)
                {
                    if (this.busyCount > 0)     // We're already doing something... need to set a random wait
                    { // update NodeTimerEvent time and requeue.
                        e_timer.Time = e_timer.Time + this.randomizer.NextDouble() * 0.5; // wait up to 0.5 sec
                        this.EventMgr.AddEvent(e_timer);
                        return;  // do not process further (deferred)
                    }

                    e_msg = (MessageEvent)e_timer.Event;

                    if (e_msg.message is FloodingQueryMessage)
                    {
                        message = (FloodingQueryMessage)e_msg.message;
                        switch (message.Type)
                        {
                            case FloodingQueryMessage.MessageType.hello:
                                processHelloTimer(e_msg);
                                break;
                            case FloodingQueryMessage.MessageType.query:
                                processMessageTimer(e_msg);
                                break;
                            case FloodingQueryMessage.MessageType.data:
                                processMessageTimer(e_msg);
                                break;
                        }
                    }
                    else if (e_msg.message is FloodingQueryApplicationMessage)
                    {
                        processApplicationMessage((FloodingQueryApplicationMessage)e_msg.message);
                    }
                }
            }
        }

        public void PreTransmit()
        {
            this.busyCount++;
        }

        public void BeginTransmit()
        { // When a node begins transmitting, increment the Busy counter. No collision occurs at a begin event!
            Notify(GenerateStaticReport());
        }

        public void EndTransmit()
        { // When a node ends transmitting, decrement the busy counter and, if not zero, set the collided flag.
            // No collision occurs during transmitting... Transmitting can occur during message reception, but the
            // received packet will be corrupted / not-heard.
            this.busyCount--;
            if (this.busyCount != 0)
            {
                this.collided = true;
            }
            else if (this.collided)
            {
                this.collided = false;
                this.Collisions++;
                this.CollisionUpdate = true;
            }
            Notify(GenerateStaticReport());
        }

        public void ForceCollision()
        {
            if (this.busyCount > 0)
                this.busyCount--;
            if (this.busyCount == 0)
                this.collided = false;
            this.Collisions++;
            this.CollisionUpdate = true;
            Notify(GenerateStaticReport());
        }

        public void BeginReceive()
        { // When a node begins receiving, increment the busy counter. No collision occurs 
          // at a begin event!
            this.busyCount++;
            Notify(GenerateStaticReport());
        }

        public bool EndReceive()      // true if COLLISION OCCURRED, otherwise false. 
        { // When a node ends receiving, decrement the busy counter and, if not zero, set the collided flag. If 
            // not zero or if the collided flag was previously set, increment the number of collisions. If zero and collided,
            // clear the collided flag.
            this.busyCount--;
            if (this.busyCount != 0)
            {
                this.collided = true;
                this.Collisions++;
                this.CollisionUpdate = true;
                Notify(GenerateStaticReport());
                return true;
            }
            else if (this.collided)
            {
                this.collided = false;
                this.Collisions++;
                this.CollisionUpdate = true;
                Notify(GenerateStaticReport());
                return true;
            }
            else
            {
                Notify(GenerateStaticReport());
                return false;
            }
        }

        public void EndSimulation() 
        { }

        void processQueryMessage(FloodingQueryMessage message)
        { // Called when a Query Message has been received
            NodeTimerEvent outputTimer;
            MessageEvent outputEvent;

            if (this.isSink) return;

            // Create a string uniquely identifying the message
            string idString = message.OriginatorID.ToString() + "-" + message.MessageID.ToString();

            if (!_messagesHeard.ContainsKey(idString)) // (For fun, get rid of this check and watch the broadcast storm!)
            { // If the _messagesHeard hash table doesn't contain the unique packet identifying string...
                _messagesHeard.Add(idString, null);         // Add the unique message string to the hash table
                if (message.Hops < this.distance)           // If the query we got is better than our previous
                {
                    this.nextHop = message.SourceID;        // Update the path back to the sink
                    this.distance = message.Hops;           // Update the distance back to the sink
                    this.nextHopCheat = ((FloodingQueryMessage)message).SourceLoc;  // ONLY FOR DISPLAY PURPOSES
                    Notify(GenerateDirectionReport());
                }
                message.UpdateSource(this.id);              // Update the message source to be the current node
                message.SourceLoc = this.Location;          // Update the message location to the current node.
                message.IncrementHops();                    // Increment the number of hops back to the sink

                outputEvent = new MessageEvent(message);    // create a new event to send out
                outputEvent.Referrer = this;

                outputTimer = new NodeTimerEvent();
                outputTimer.Event = outputEvent;
                outputTimer.node = this;
                     // Wait up to 2 seconds before sending (stand-off time).
                outputTimer.Time = this.EventMgr.CurrentClock + 2 * this.randomizer.NextDouble();
                this.EventMgr.AddEvent(outputTimer);

                //this.PhysProc.ExecuteAction(outputEvent);   // Send the event to our Physical Processor
            } // ELSE DROP MESSAGE
        }

        void processDataMessage(FloodingQueryMessage message)
        { // Called when a Data Message has been received
            NodeTimerEvent outputTimer;
            MessageEvent outputEvent;

            if ((message.DestinationID != this.ID) && (message.DestinationID != 0xffff))
                return;

            if (this.isSink) return;

            // ---- NOTE, NEARLY SAME CODE AS "processQueryMessage"
                if (this.nextHop != int.MaxValue)           // If a next hop exists...
                    message.DestinationID = this.nextHop;   // set the destination ID to the next hop address
                else
                    message.DestinationID = 0xffff;         // otherwise flood <-- this is not efficient!!!
                outputEvent = new MessageEvent(message);    // create a new event
                outputEvent.Referrer = this;

                outputTimer = new NodeTimerEvent();
                outputTimer.Event = outputEvent;
                outputTimer.node = this;
                // Wait up to 2 seconds before sending (stand-off time).
                outputTimer.Time = this.EventMgr.CurrentClock + 2 * this.randomizer.NextDouble();
                this.EventMgr.AddEvent(outputTimer);
        }

        void processHelloTimer(MessageEvent e)
        { // When the timer expires, send the Hello message and regenerate the timer.
            MessageEvent outputEvent;
            FloodingQueryMessage message = (FloodingQueryMessage)e.message;

            outputEvent = new MessageEvent(message);
            outputEvent.Referrer = this;
            this.PhysProc.ExecuteAction(outputEvent);

            this.resetHelloTimer();
        }

        void processMessageTimer(MessageEvent e)
        { // When the timer expires, send the Message
            MessageEvent outputEvent;

            outputEvent = new MessageEvent(e.message);
            outputEvent.Referrer = this;

            this.PhysProc.ExecuteAction(outputEvent);
        }

        void resetHelloTimer()
        { // Set the Hello Timer and insert into the Event Queue
            NodeTimerEvent outputTimer;
            FloodingQueryMessage message = new FloodingQueryMessage(this.id, 0xffff, FloodingQueryMessage.MessageType.hello);
            message.SourceLoc = this.Location;
            MessageEvent e = new MessageEvent(message);
            double Clock = this.EventMgr.CurrentClock;
            Clock += 30;

            outputTimer = new NodeTimerEvent();
            outputTimer.Event = e;
            outputTimer.node = this;
            outputTimer.Time = Clock;
            this.EventMgr.AddEvent(outputTimer);
        }

        void processApplicationMessage(FloodingQueryApplicationMessage message)
        { // When an application message is received, create and send a data message toward the Sink. If no Sink path
            // exists, broadcast the data message.
            FloodingQueryMessage dataMessage;
            MessageEvent outputEvent;
            if (message.IsEventOccurrance)
            {
                if (this.nextHop != int.MaxValue)
                    dataMessage = new FloodingQueryMessage(this.id, this.nextHop, FloodingQueryMessage.MessageType.data,
                        CurrentMessageID, this.id);
                else
                    dataMessage = new FloodingQueryMessage(this.id, 0xffff, FloodingQueryMessage.MessageType.data,
                        CurrentMessageID, this.id);
                
                dataMessage.SourceLoc = this.Location;

                outputEvent = new MessageEvent(dataMessage);
                outputEvent.Referrer = this;
                this.PhysProc.ExecuteAction(outputEvent);
            }
        }

        public ReportLevel GetMessageLevel(IMessage msg)
        {
            FloodingQueryMessage fqm = (FloodingQueryMessage)msg;
            switch (fqm.Type)
            {
                case FloodingQueryMessage.MessageType.hello:
                    return ReportLevel.Routine;
                    break;
                case FloodingQueryMessage.MessageType.query:
                    return ReportLevel.Immediate;
                    break;
                case FloodingQueryMessage.MessageType.data:
                    return ReportLevel.FlashOverride;
                    break;
            }
            return ReportLevel.Routine;
        }

        #region IReportSubject METHODS
        ArrayList observers = new ArrayList();

        public void Attach(IReportObserver observer)
        {
            if (observers.Contains(observer))
                return;
            else observers.Add(observer);
        }

        public void Detach(IReportObserver observer)
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }

        public void Notify(IReport report)
        {
            IReportObserver observer;
            foreach (Object obj in observers)
            {
                observer = (IReportObserver)obj;
                observer.Update(report);
            }
        }

        public IReport GenerateStaticReport()
        {
            CircleReport circReport = new CircleReport(EventMgr.CurrentClock, 
                location, nodeSize, gradientPath, true, id);

            circReport.MaximumMessageSize = PhysProc.MaximumRange * 2;

            if (this.isSink)
            {
                circReport.Color = ColorEnum.Sink;
                circReport.FinalColor = ColorEnum.Sink;
            }
            else
            {
                circReport.Color = ColorEnum.Node;
                circReport.FinalColor = ColorEnum.Node;
            }

            if (busyCount == 1)
            {
                circReport.Color = ColorEnum.Busy;
            }
            if ((busyCount > 1) || collided)
            {
                circReport.Color = ColorEnum.Collision;
            }
            return circReport;
        }

        public IReport GenerateDirectionReport()
        {
            LineReport dirReport = new LineReport(this.EventMgr.CurrentClock, (XYDoubleLocation)this.Location,
                (XYDoubleLocation)this.nextHopCheat, 0, -250, true, -this.id);

            dirReport.Color = ColorEnum.Priority;
            dirReport.IsArrow = true;

            return dirReport;
        }

        /*public IReport GenerateReport(double time)
        {
            CircleReport circReport = new CircleReport(time, location, nodeSize,
                gradientPath, false, id);
            
            if (this.isSink)
            {
                circReport.Color = ColorEnum.Immediate;
                circReport.FinalColor = ColorEnum.Immediate;
            }
            else
            {
                circReport.Color = ColorEnum.Node;
                circReport.FinalColor = ColorEnum.Node;
            }

            if (collided)
            {
                circReport.Color = ColorEnum.FlashOverride;
            }
        }*/ // Unused...
        #endregion IReportSubject METHODS
    }

    public class FloodingQueryNodeFactory : INodeFactory
    { // A concrete factory class to create FloodingQueryNode objects.

        public static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();

            PanelObj obj = new PanelObj();
            obj.name = "label_EMPTY";
            obj.type = FormType.label;
            obj.text = "";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 0;
            list.Add(obj);

            return list;
        }
        public List<PanelObj> SetupPanel
        {
            get { return setupPanel(); }
        }

        public List<PanelObj> panelObjs;
        public List<PanelObj> PanelObjs
        {
            get { return panelObjs; }
            set { panelObjs = value; }
        }

        int nodesCreated = 0;
        IReportObserver reporter;

        bool isInitialized;
        public bool IsInitialized
        {
            get { return isInitialized; }
        }

        IEventManager eventMgr;
        public IEventManager EventManager
        {
            get { return eventMgr; }
        }

        IPhysicalProcessor physProc;
        public IPhysicalProcessor PhysicalProcesser
        {
            get { return physProc; }
        }

        IRandomizerFactory randomizerFactory;
        public IRandomizerFactory RandomizerFactory
        {
            set { randomizerFactory = value; }
        }

        public FloodingQueryNodeFactory()
        {
            isInitialized = false;
        }

        public FloodingQueryNodeFactory(IEventManager eventMgr, IPhysicalProcessor physProc, 
            IRandomizerFactory randomizerFactory, IReportObserver reporter)
        {
            Initialize(eventMgr, physProc, randomizerFactory, reporter);
        }

        public void Initialize(IEventManager eventMgr, IPhysicalProcessor physProc, 
            IRandomizerFactory randomizerFactory, IReportObserver reporter)
        {
            this.eventMgr = eventMgr;
            this.physProc = physProc;
            this.randomizerFactory = randomizerFactory;
            this.reporter = reporter;
            isInitialized = true;
        }

        public INode CreateNode(ILocation loc)
        { // Creates a FloodingQueryNode at the specified location
            if (!isInitialized)
                throw new InvalidOperationException("FloodingQueryNodeFactory not initialized!");

            FloodingQueryNode node = new FloodingQueryNode(eventMgr, physProc);
            node.Attach(this.reporter);
            node.Location = loc;
            node.ID = ++nodesCreated; // Reserve node #0
            node.RandomValue = randomizerFactory.CreateRandomizer();
            return node;
        }
    }
}
