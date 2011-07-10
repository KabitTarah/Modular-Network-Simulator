using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Location;
using ModularNetworkSimulator;
using MNS_GraphicsLib;
using MNS_Reporting;

namespace PEQ
{
    /* PEQ is the protocol defined in "A Fast and Reliable Protocol
     * for Wireless Sensor Networks in Critical Conditions Monitoring 
     * Applications," Azzedine Boukerche, et al., MSWiM October 2004
     */

    public class PEQNode : INode, IReferrer, IReportSubject
    {
        public int _NUM_ID_BYTES = 1;
        public double _TIMER_ACK = 5;
        public double _TIMER_BUILDTREE = 5;
        public double _TIMER_HELLO = 30;
        public double _TIMER_SEARCH = 5;
        public double _TIMER_SUBSCRIBE = 15;
        public double _TIMER_WAIT_SEND = 0.010; //secs (Wait before sending any message)
        public double _TIMER_RANDOM_WAIT_SEND = 0.250; //secs (random wait if busy)
        public bool _HELLO_SUPPRESS_REPORT = true;
        public bool _EXPLICIT_COLLISIONS = false;
        public bool _INFO_REPORTS = true;
        public bool _NODE_REPORTS = false;
        public bool _SINK_REPORTS = true;

        #region PEQNode Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        int _dataReceived = 0;

        List<PEQTableEntryConfig> _tableConfig;
        List<PEQTableEntryRouting> _tableRouting = 
            new List<PEQTableEntryRouting>();
        List<PEQTableEntrySubscription> _tableSubscription = 
            new List<PEQTableEntrySubscription>();

        List<VarID> _tableNeighbor = new List<VarID>();
        List<PEQTableEntryMessageAck> _tableAck = 
            new List<PEQTableEntryMessageAck>();
        List<PEQTableEntrySearch> _tableSearch = 
            new List<PEQTableEntrySearch>();

        Byte _sequenceNumber = 0x00;

        // The following to support L2
        int _busyCount = 0;
        public int Collisions = 0;
        public bool CollisionUpdate = false;

        // The following to support Visualization
        IReport currentRouteReport = null;
        INodes nodes;

        // The following to support output
        SearchAggregator _searchAggregator;

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQNode Variables

        #region INode Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        double _nodeSize = 1;
        public double NodeSize
        {
            get { return _nodeSize; }
        }

        // Determines whether a collision is occurring
        bool _collided = false;
        public bool Collided 
        {
            get { return _collided; }
        }

        ILocation _location;
        public ILocation Location 
        {
            get { return _location; }
            set { _location = value; }
        }

        IRandomizer _randomValue;
        public IRandomizer RandomValue 
        {
            get { return _randomValue; }
            set { _randomValue = value; }
        }

        bool _isSink = false;
        public bool IsSink 
        {
            get { return _isSink; }
            set { 
                _isSink = value;
                Notify(GenerateStaticReport());
            }
        }

        VarID _id;
        public int ID 
        {
            get { return _id.GetID(); }
            set { _id = new VarID(_NUM_ID_BYTES, value); }
        }

        GraphicsPath _gradientPath = new GraphicsPath();
        public GraphicsPath GradientPath 
        {
            get { return _gradientPath; }
            set { _gradientPath = value; }
        }

        IEventManager _eventManager;
        IPhysicalProcessor _physicalProcessor;

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion INode Variables

        #region IReportSubject Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        ArrayList _observers = new ArrayList();

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReportSubject Variables

        #region PEQNode Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public PEQNode(IEventManager EventManager,
            IPhysicalProcessor PhysicalProcessor,
            SearchAggregator aggregator)
        {
            this._eventManager = EventManager;
            this._physicalProcessor = PhysicalProcessor;
            
            // Any required PEQNode Initialization added here
            _searchAggregator = aggregator;
        }

        private void processHello(PEQMessage msg)
        {
            VarID neighborID = msg._SenderID;
            if (!_tableNeighbor.Contains(neighborID))
                _tableNeighbor.Add(neighborID);
        }

        private void processHelloTimer(PEQTimerHello timer)
        {
            PEQMessageHello msg = new PEQMessageHello(_id);
            msg._nextHopCheat = _location;

            PEQTimerMessage timerEvent = new PEQTimerMessage(msg,
                _eventManager.CurrentClock + _TIMER_WAIT_SEND, this);
            _eventManager.AddEvent(timerEvent);

            resetHelloTimer();
        }

        private void resetHelloTimer()
        {
            PEQTimerHello timer = 
                new PEQTimerHello(_eventManager.CurrentClock + _TIMER_HELLO, this);
            _eventManager.AddEvent(timer);
        }

        private void processAck(PEQMessageAck msg)
        {
            PEQTableEntryMessageAck ack;
            ack = null;
            foreach (PEQTableEntryMessageAck entry in _tableAck)
            {
                if ((entry._SequenceNumber == msg._SequenceNumber) &&
                    (entry._DestinationID == msg._SenderID))
                {
                    ack = entry;
                    break;
                }
            }
            if ((ack != null))
               _tableAck.Remove(ack);
        }

        private void processAckTimer(PEQTimerAck timer)
        {
            List<PEQTableEntryMessageAck> killList = 
                new List<PEQTableEntryMessageAck>();
            VarID sinkID = new VarID(_NUM_ID_BYTES);

            foreach (PEQTableEntryMessageAck entry in _tableAck)
            {
                if (entry._SequenceNumber == timer._AckInfo._SequenceNumber)
                {
                    // Add entry to list for deletion
                    killList.Add(entry);

                    bool search = false;
                    // Check neighbor - if `holds current route?` Then `Search.`
                    foreach (PEQTableEntryRouting route in _tableRouting)
                    {
                        if (route._DestinationID == entry._DestinationID)
                            if (route._Valid)
                            {
                                route._Valid = false;
                                search = true;
                                sinkID = route._SinkID;
                            }
                    }

                    // Check neighbor subscription entries
                    foreach (PEQTableEntrySubscription subscription in
                        _tableSubscription)
                    {
                        if (subscription._DestinationID == entry._DestinationID)
                        {
                            subscription._Valid = false;
                            search = true;
                        }
                    }

                    if (search)
                        startSearch(sinkID, entry._DataMessage);

                    // Delete neighbor
                    _tableNeighbor.Remove(entry._DestinationID);
                }
            }

            // Clear the ack list of all matched entries.
            foreach (PEQTableEntryMessageAck entry in killList)
                _tableAck.Remove(entry);
        }

        private void processBuildTree(PEQMessageBuildTree msg)
        {
            bool found = false;   // if the Sink was found in the table
            bool update = false;  // if the Sink was added / modified
            // Look for Sink ID in Subscription Table to update
            foreach (PEQTableEntrySubscription entry in _tableSubscription)
            {
                if ((entry._SinkID == msg._SinkID) &&
                    (entry._CriteriaType == 0x0000))
                {
                    found = true;
                    if (msg._HopCount + 1 < entry._HopCount)
                    {
                        update = true;
                        entry._HopCount = (byte)(msg._HopCount + 1);
                        entry._DestinationID = msg._SenderID;
                        entry._nextHopCheat = msg._nextHopCheat;
                    }
                }
            }

            if (!found)
            {
                update = true;
                PEQTableEntrySubscription entry = new PEQTableEntrySubscription();
                entry._CriteriaType = 0x0000;
                entry._DestinationID = msg._SenderID;
                entry._HopCount = (byte)(msg._HopCount + 1);
                entry._SinkID = msg._SinkID;
                entry._nextHopCheat = msg._nextHopCheat;
                _tableSubscription.Add(entry);
            }

            if (update)
            {
                updateRoutingTable();
                sendBuildTree(msg);
            }
        }
        
        private void processNotify(PEQMessageNotify msg)
        {
            if ((msg._SinkID == _id) && _isSink)
            {
                sendAck(msg);
                _dataReceived++;
                msg._Data._TotalDistance += msg._nextHopCheat.Distance(_location);
                if (_SINK_REPORTS)
                    Notify(GeneratePEQSinkInfoReport(_eventManager.CurrentClock - msg._Data._StartTime, msg));

                PEQDataInfoReport dataReport = new PEQDataInfoReport(this.ID, _eventManager.CurrentClock);
                dataReport._Received = 1;
                dataReport._DataID = msg._Data._DataID;
                Notify(dataReport);
            }
            else
            {
                // Find route to Sink
                foreach (PEQTableEntryRouting entry in _tableRouting)
                {
                    if ((entry._SinkID == msg._SinkID) && entry._Valid)
                    {
                        sendAck(msg);

                        sendNotify(entry, msg);
                    }
                }
            }
        }

        private void processResponse(PEQMessageResponse msg)
        {
            bool allReceived = true;
            PEQTableEntrySearch winner = null;
            Byte hopCount = 0xFF;
            foreach (PEQTableEntrySearch entry in _tableSearch)
            {
                if ((msg._SinkID == entry._SinkID)
                    && (msg._SenderID == entry._DestinationID))
                {
                    entry._ResponseReceived = true;
                    entry._HopCount = msg._HopCount;
                    entry._nextHopCheat = msg._nextHopCheat;

                    // Update Subscription
                    bool found = false;
                    foreach (PEQTableEntrySubscription subscription in
                        _tableSubscription)
                    {
                        if ((subscription._DestinationID == msg._SenderID)
                            && (subscription._SinkID == msg._SinkID))
                        {
                            found = true;
                            subscription._HopCount = msg._HopCount;
                            subscription._Valid = true;
                            subscription._nextHopCheat = msg._nextHopCheat;
                            subscription._Timestamp = _eventManager.CurrentClock;
                        }
                    }
                    if (!found && (msg._HopCount < 0xFF)) // add!
                    {
                        PEQTableEntrySubscription newSubscription = new PEQTableEntrySubscription();
                        newSubscription._CriteriaType = 0x0001;  // this should be included with Search/Response message
                        newSubscription._DestinationID = msg._SenderID;
                        newSubscription._HopCount = msg._HopCount;
                        newSubscription._nextHopCheat = msg._nextHopCheat;
                        newSubscription._SinkID = msg._SinkID;
                        newSubscription._Timestamp = _eventManager.CurrentClock;
                        newSubscription._Valid = true;

                        _tableSubscription.Add(newSubscription);
                    }
                }
                else if ((msg._SinkID == entry._SinkID)
                    && (!entry._ResponseReceived))
                    allReceived = false;
                if (entry._HopCount < hopCount)
                {
                    hopCount = entry._HopCount;
                    winner = entry;
                }
            }

            if (allReceived)
            {
                completeSearch(winner);
            }
        }

        private void completeSearch(VarID sinkID)
        {
            byte hopCount = 0xff;
            PEQTableEntrySearch winner = null;
            foreach (PEQTableEntrySearch entry in _tableSearch)
            {
                if ((entry._HopCount < hopCount) && (entry._SinkID == sinkID))
                {
                    hopCount = entry._HopCount;
                    winner = entry;
                }
            }

            if (winner == null)
            {
                clearTableSearch(sinkID);
                startSearch(sinkID);
            }
            else
                completeSearch(winner);
        }

        private void completeSearch(PEQTableEntrySearch winner)
        {
            PEQTableEntryRouting outRoute = null;
            if (winner == null)
                return; // should perform completeSearch(sinkID) first!
            bool found = false;
            // End Search, Update Route
            foreach (PEQTableEntryRouting route in _tableRouting)
            {
                if ((route._SinkID == winner._SinkID) && (!route._Valid))
                {
                    found = true;
                    route._Valid = true;
                    route._DestinationID = winner._DestinationID;
                    route._nextHopCheat = winner._nextHopCheat;
                    outRoute = route;
                }
            }

            // Add route
            if (!found)
            {
                PEQTableEntryRouting route = new PEQTableEntryRouting(_NUM_ID_BYTES);
                route._DestinationID = winner._DestinationID;
                route._SinkID = winner._SinkID;
                route._nextHopCheat = winner._nextHopCheat;
                _tableRouting.Add(route);
                outRoute = route;
            }

            updateRoutingTable();

            if (winner._DataMessage is PEQMessageNotify)
                sendNotify(outRoute, (PEQMessageNotify)winner._DataMessage);
        }

        private void processSearch(PEQMessageSearch msg)
        {
            PEQTableEntrySubscription subscriptionEntry = null;

            foreach (PEQTableEntrySubscription entry in _tableSubscription)
            {
                if ((entry._SinkID == msg._SinkID) && entry._Valid)
                {
                    if (subscriptionEntry == null)
                        subscriptionEntry = entry;
                    else if (subscriptionEntry._HopCount > entry._HopCount)
                        subscriptionEntry = entry;
                }
            }

            if (subscriptionEntry == null)
            {
                subscriptionEntry = new PEQTableEntrySubscription();
                subscriptionEntry._HopCount = 0xff;
                subscriptionEntry._Valid = false;
                subscriptionEntry._nextHopCheat = null;
                subscriptionEntry._CriteriaType = 0x0000;
            }

            sendResponse(subscriptionEntry, msg);
        }

        private void processSearchTimer(PEQTimerSearch timer)
        {
            // Find all Response entries and restart search if any are
            // not marked received in the table.

            if (timer._Count < 2)
            {
                foreach (PEQTableEntrySearch entry in _tableSearch)
                {
                    if (entry._SinkID == timer._SinkID)
                    {
                        if (!entry._ResponseReceived)
                        {
                            // Resend search message & reset timer
                            sendSearch(timer._SinkID, timer._Count);
                            return;
                        }
                    }
                }
            }
            else
            {
                completeSearch(timer._SinkID);
            }

            clearTableSearch(timer._SinkID);
        }

        private void processSubscribe(PEQMessageSubscribe msg)
        {
            bool found = false;   // if the Sink was found in the table
            bool update = false;  // if the Sink was added / modified
            // Look for Sink ID in Subscription Table to update
            foreach (PEQTableEntrySubscription entry in _tableSubscription)
            {
                if ((entry._SinkID == msg._SinkID) &&
                    (entry._CriteriaType == msg._SubscriptionInfo._CriteriaType))
                {
                    found = true;
                    if (msg._HopCount + 1 < entry._HopCount)
                    {
                        update = true;
                        entry._HopCount = (byte)(msg._HopCount + 1);
                        entry._DestinationID = msg._SenderID;
                        entry._nextHopCheat = msg._nextHopCheat;
                        updateRoutingTable();
                        sendSubscribe(msg);
                    }
                }
            }

            if (!found)
            {
                update = true;
                PEQTableEntrySubscription entry = new PEQTableEntrySubscription();
                entry._CriteriaType = msg._SubscriptionInfo._CriteriaType;
                entry._DestinationID = msg._SenderID;
                entry._HopCount = (byte)(msg._HopCount + 1);
                entry._SinkID = msg._SinkID;
                entry._nextHopCheat = msg._nextHopCheat;
                _tableSubscription.Add(entry);
                updateRoutingTable();
                sendSubscribe(msg);
            }
        }

        private void expectAck(PEQMessage msg)
        {
            // Add the ACK info to the ACK Table and create a timer event to kill
            // the neighbor entry upon ACK failure.
            PEQTableEntryMessageAck ackEntry = new 
                PEQTableEntryMessageAck(msg._SequenceNumber, msg._DestinationID, msg);
            _tableAck.Add(ackEntry);
            PEQTimerAck ackTimer = new PEQTimerAck(_eventManager.CurrentClock +
                _physicalProcessor.MaximumRange * _TIMER_ACK / _physicalProcessor.PropagationSpeed,
                this, ackEntry);
            _eventManager.AddEvent(ackTimer);
        }

        private void sendAck(PEQMessage msg)
        {
            if (msg._DestinationID == msg._SenderID)
                return;

            PEQMessageAck ackMessage = new PEQMessageAck(msg._SenderID,
                msg._DestinationID, msg._SequenceNumber);

            ackMessage._nextHopCheat = _location;   // Just add this to all msgs

            PEQTimerMessage timerEvent = new PEQTimerMessage(ackMessage,
                _eventManager.CurrentClock + _TIMER_WAIT_SEND, this);
            _eventManager.AddEvent(timerEvent);
        }

        private void updateRoutingTable()
        {
            List<VarID> sinkList = new List<VarID>(); // ONE route to each sink...

            _tableRouting.Clear();  // Start fresh. This list is a factor of valid data elsewhere.

            // Get all valid sink IDs
            foreach (PEQTableEntrySubscription subEntry in _tableSubscription)
            {
                if (!sinkList.Contains(subEntry._SinkID))
                    sinkList.Add(subEntry._SinkID);
            }

            // Find the best route to each sink.
            foreach (VarID sinkID in sinkList)
            {
                Byte best_hop = 0xFF; // Ensure that the best subEntry is added to the route.
                double best_time = 0; // Ensure that the most recent entry is used
                PEQTableEntrySubscription best_hopEntry = null;
                foreach (PEQTableEntrySubscription subEntry in _tableSubscription)
                {
                    if (subEntry._HopCount <= best_hop)
                    {
                        if (subEntry._Timestamp >= best_time)
                        {
                            best_hop = subEntry._HopCount;
                            best_time = subEntry._Timestamp;
                            best_hopEntry = subEntry;
                        }
                    }
                }

                if (best_hopEntry != null)
                {
                    PEQTableEntryRouting newRoute = new PEQTableEntryRouting(_NUM_ID_BYTES);
                    newRoute._DestinationID = best_hopEntry._DestinationID;
                    newRoute._SinkID = best_hopEntry._SinkID;
                    newRoute._nextHopCheat = best_hopEntry._nextHopCheat;
                    newRoute._Valid = best_hopEntry._Valid;
                    _tableRouting.Add(newRoute);

                    IGraphicalReport newRouteReport;
                    if (best_hopEntry._CriteriaType == 0x0000)
                        newRouteReport = GenerateDirectionReport(GetMessageLevel(new PEQMessageBuildTree()));
                    else
                        newRouteReport = GenerateDirectionReport(GetMessageLevel(new PEQMessageSubscribe()));
                    if (newRouteReport != null)
                    {
                        if (currentRouteReport != null)
                        {
                            newRouteReport.ReportAction = MNS_Reporting.Action.Modify;
                            newRouteReport.PreviousStatic = currentRouteReport;
                            currentRouteReport = newRouteReport;
                            Notify(newRouteReport);
                        }
                        else
                        {
                            currentRouteReport = GenerateDirectionReport();
                            if (currentRouteReport != null)
                                Notify(currentRouteReport);
                        }
                    }
                    else if (currentRouteReport != null)
                    {
                        LineReport dirReport = new LineReport(_eventManager.CurrentClock,
                        (XYDoubleLocation)_location,
                        (XYDoubleLocation)newRoute._nextHopCheat, 0, -250, true, -_id.GetID());
                        dirReport.PreviousStatic = currentRouteReport;
                        dirReport.ReportAction = MNS_Reporting.Action.Stop;
                        Notify(dirReport);

                        currentRouteReport = null;
                    }
                }
            }
        }

        private void sendBuildTree(PEQMessageBuildTree msg)
        {
            // Propagate BuildTree message
            PEQMessageBuildTree newMsg = (PEQMessageBuildTree)msg.Clone();
            newMsg._SenderID = _id;
            newMsg._SequenceNumber = 0x00;
            newMsg._HopCount = (byte)(msg._HopCount + 1);
            newMsg._nextHopCheat = _location;

            PEQTimerMessage timerEvent = new PEQTimerMessage(newMsg,
                _eventManager.CurrentClock + _TIMER_WAIT_SEND, this);
            _eventManager.AddEvent(timerEvent);
        }

        private void sendSubscribe(PEQMessageSubscribe msg)
        {
            PEQMessageSubscribe newMsg = (PEQMessageSubscribe)msg.Clone();
            newMsg._SenderID = _id;
            newMsg._SequenceNumber = 0x00;
            newMsg._HopCount = (byte)(msg._HopCount + 1);
            newMsg._nextHopCheat = _location;

            PEQTimerMessage timerEvent = new PEQTimerMessage(newMsg,
                _eventManager.CurrentClock + _TIMER_WAIT_SEND, this);
            _eventManager.AddEvent(timerEvent);
        }

        private void sendNotify(PEQTableEntryRouting Route, PEQMessageNotify msg)
        {
            PEQMessageNotify newMsg = (PEQMessageNotify)msg.Clone();
            newMsg._SenderID = _id;
            newMsg._SequenceNumber = _sequenceNumber++;
            newMsg._DestinationID = Route._DestinationID;
            newMsg._nextHopCheat = _location;
            newMsg._Data._NumHops++;
            newMsg._Data._TotalDistance += _location.Distance(msg._nextHopCheat);

            PEQTimerMessage timerEvent = new PEQTimerMessage(newMsg,
                _eventManager.CurrentClock + _TIMER_WAIT_SEND, this);
            _eventManager.AddEvent(timerEvent);
        }

        private void sendResponse(PEQTableEntrySubscription Subscription,
            PEQMessageSearch msg)
        {
            PEQMessageResponse newMsg = new PEQMessageResponse(msg._SenderID,
                _id, msg._SequenceNumber, Subscription._HopCount,
                msg._SinkID);
            newMsg._nextHopCheat = _location;

            PEQTimerMessage timerEvent = new PEQTimerMessage(newMsg,
                _eventManager.CurrentClock + _TIMER_RANDOM_WAIT_SEND 
                * RandomValue.NextDouble(), this);
            _eventManager.AddEvent(timerEvent);
        }

        private void processMessageTimer(PEQTimerMessage timer)
        {
            if (_busyCount > 0)
            {
                timer.Time = _eventManager.CurrentClock
                    + _randomValue.NextDouble() * _TIMER_RANDOM_WAIT_SEND;
                _eventManager.AddEvent(timer);
            }
            else
            {
                MessageEvent msgEve = (MessageEvent)timer.Event;
                PEQMessage msg = (PEQMessage)msgEve.message;
                if (msg._DestinationID == new VarID(msg._DestinationID.SizeOf()))
                    msgEve.Directed = false;
                else
                {
                    msgEve.Directed = true;
                    msgEve.NextHopID = msg._DestinationID.GetID();
                    if (msg is PEQMessageNotify)    // Also, directed subscription message when implemented
                        expectAck(msg);
                }

                if (_NODE_REPORTS)
                    Notify(GeneratePEQNodeInfoReport(true, msg));
                if (_HELLO_SUPPRESS_REPORT && (msg is PEQMessageHello))
                    msgEve.SuppressReport = true;

                _physicalProcessor.ExecuteAction(msgEve);
            }
        }

        private void startSearch(VarID SinkID, PEQMessage dataMessage)
        {
            clearTableSearch(SinkID);

            // Set all neighbors to 0xFF hops (unreachable)
            foreach (VarID neighborID in _tableNeighbor)
            {
                PEQTableEntrySearch entry = new PEQTableEntrySearch();
                entry._SinkID = SinkID;
                entry._HopCount = 0xFF;
                entry._DestinationID = neighborID;
                entry._DataMessage = dataMessage;

                _tableSearch.Add(entry);
            }

            sendSearch(SinkID);

            _searchAggregator.IncrementSearches();

            updateRoutingTable();
        }

        private void startSearch(VarID SinkID)
        {
            startSearch(SinkID, null);
        }

        private void sendSearch(VarID SinkID)
        {
            sendSearch(SinkID, 0);
        }

        private void sendSearch(VarID SinkID, int searchCount)
        {
            PEQMessageSearch msg = new PEQMessageSearch(new VarID(SinkID.SizeOf()), _id,
                _sequenceNumber++, SinkID);

            msg._nextHopCheat = _location;

            PEQTimerMessage timerEvent = new PEQTimerMessage(msg, 
                _eventManager.CurrentClock + _TIMER_RANDOM_WAIT_SEND * _randomValue.NextDouble(), this);
            _eventManager.AddEvent(timerEvent);

            PEQTimerSearch searchTimer
                = new PEQTimerSearch(_eventManager.CurrentClock +
                    _physicalProcessor.MaximumRange * _TIMER_SEARCH /
                    _physicalProcessor.PropagationSpeed, this);
            searchTimer._SinkID = SinkID;
            searchTimer._Count = ++searchCount;

            _eventManager.AddEvent(searchTimer);
        }

        private void clearTableSearch(VarID SinkID)
        {
            List<PEQTableEntrySearch> killList 
                = new List<PEQTableEntrySearch>();

            foreach (PEQTableEntrySearch entry in _tableSearch)
                if (entry._SinkID == SinkID)
                    killList.Add(entry);

            foreach (PEQTableEntrySearch entry in killList)
                _tableSearch.Remove(entry);
        }

        private void processApplicationMessage(PEQMessageApplication msg)
        {
            msg._Data._StartTime = _eventManager.CurrentClock;
            foreach (PEQTableEntryRouting routeEntry in _tableRouting)
            //foreach (PEQTableEntrySubscription subEntry in _tableSubscription)
            {
                //if ((subEntry._CriteriaType == msg._Criteria.CriteriaType) 
                  //  && subEntry._Valid)
                if (routeEntry._Valid)
                {
                    PEQMessageNotify notifyMsg = new PEQMessageNotify(_id, _id, 
                        0x00, routeEntry._SinkID, msg._Data);
                    notifyMsg._nextHopCheat = _location;
                    processNotify(notifyMsg);
                }
            }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQNode Methods

        #region INode Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public void Initialize()
        {
            Notify(GenerateStaticReport());  // For Visualization

            // Set initial hello timer to random value
            PEQTimerHello timer =
                new PEQTimerHello(_randomValue.NextDouble() * _TIMER_HELLO, this);
            _eventManager.AddEvent(timer);

            if (_isSink)
            {
                // Send build tree (at future time...)
                PEQMessageBuildTree msgBT = new PEQMessageBuildTree(new VarID(_NUM_ID_BYTES), _id,
                    _sequenceNumber++, _id, 0x00);
                double time = _randomValue.NextDouble() * _TIMER_BUILDTREE;
                msgBT._nextHopCheat = _location;
                PEQTimerMessage outputTimer = new PEQTimerMessage(msgBT, time, this);
                _eventManager.AddEvent(outputTimer);

                // Add to BT to subscription table (this is early........)
                PEQTableEntrySubscription sub = new PEQTableEntrySubscription();
                sub._SinkID = _id;
                sub._nextHopCheat = _location;
                sub._HopCount = 0;
                sub._DestinationID = _id;
                sub._CriteriaType = 0x0000;
                sub._Valid = true;
                _tableSubscription.Add(sub);

                // Send subscription (at future time...)
                PEQMessageSubscribe msgSub = new PEQMessageSubscribe(new VarID(_NUM_ID_BYTES), _id,
                    _sequenceNumber++, _id, 0x00,
                    new PEQSubscriptionInfo(0x0001,
                        new PEQSubscriptionCriteria()));
                time = time + _TIMER_SUBSCRIBE;
                msgSub._nextHopCheat = _location;
                outputTimer = new PEQTimerMessage(msgSub, time, this);
                _eventManager.AddEvent(outputTimer);

                // Add subscription to subscription table (this is very early........)
                sub = new PEQTableEntrySubscription();
                sub._SinkID = _id;
                sub._nextHopCheat = _location;
                sub._HopCount = 0;
                sub._DestinationID = _id;
                sub._CriteriaType = 0x0001;
                sub._Valid = true;
                _tableSubscription.Add(sub);

                updateRoutingTable();
            }
        }

        public void PreTransmit()
        {
            this._busyCount++;
        }

        public void BeginTransmit()
        {
            Notify(GenerateStaticReport());
        }

        public void EndTransmit()
        {
            if (this._busyCount > 0)
                this._busyCount--;
            if (this._busyCount != 0)
                this._collided = true;
            else if (this._collided)
            {
                this._collided = false;
                if (!_EXPLICIT_COLLISIONS)
                    this.Collisions++;
                if (_NODE_REPORTS)
                    Notify(GeneratePEQNodeInfoReport(true));
                this.CollisionUpdate = true;
            }
            Notify(GenerateStaticReport());
        }

        public void BeginReceive()
        {
            this._busyCount++;
            Notify(GenerateStaticReport());
        }

        public bool EndReceive()    // true if COLLISION OCCURRED, otherwise false.
        {
            if (this._busyCount > 0)
                this._busyCount--;
            if (this._busyCount != 0)
            {
                this._collided = true;
                if (!_EXPLICIT_COLLISIONS)
                    this.Collisions++;
                if (_NODE_REPORTS)
                    Notify(GeneratePEQNodeInfoReport(true));
                this.CollisionUpdate = true;
                Notify(GenerateStaticReport());
                return true;
            }
            else if (this._collided)
            {
                this._collided = false;
                if (!_EXPLICIT_COLLISIONS)
                    this.Collisions++;
                if (_NODE_REPORTS)
                    Notify(GeneratePEQNodeInfoReport(true));
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

        public void ForceCollision()
        {
            this.Collisions++;
            this.CollisionUpdate = true;
            if (_NODE_REPORTS)
                Notify(GeneratePEQNodeInfoReport(true));
        }

        public void EndSimulation()
        {
            _searchAggregator.EndSimulation();
        }

        public ReportLevel GetMessageLevel(IMessage msg)
        {
            if (msg is PEQMessage)
            {
                if (msg is PEQMessageBuildTree)
                    return ReportLevel.Priority;        // green
                else if (msg is PEQMessageSubscribe)
                    return ReportLevel.FlashOverride;   // red
                else if (msg is PEQMessageSearch)
                    return ReportLevel.Flash;           // orange
                else if (msg is PEQMessageResponse)
                    return ReportLevel.Priority;        // green
                else if (msg is PEQMessageNotify)
                    return ReportLevel.FlashOverride;   // red
                else return ReportLevel.Routine;        // gray
            }
            else return ReportLevel.Routine;            // gray
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion INode Methods

        #region IReferrer Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public void ExecuteAction(IEvent e)
        {
            if (e is MessageEvent)
            {
                MessageEvent msgEvent = (MessageEvent)e;
                IMessage imsg = msgEvent.message;

                if (!(imsg is PEQInternal))
                   if (this.EndReceive()) // End reception of msg... and if
                      return;             // collision end message processing.
                
                if (imsg is PEQMessage)
                {
                    PEQMessage PEQmsg = (PEQMessage)imsg;

                    if (_NODE_REPORTS)
                        Notify(GeneratePEQNodeInfoReport(false, PEQmsg));

                    processHello(PEQmsg); // Glean neighbors from every message.

                    if ((PEQmsg._DestinationID != _id) 
                        && (PEQmsg._DestinationID != new VarID(PEQmsg._DestinationID.SizeOf())))
                        return; // drop message if not destined for us (or bcast)
                }

                // Ignore hello messages... all messages get processed as hellos
                if (imsg is PEQMessageHello)
                { }
                else if (imsg is PEQMessageAck)
                {
                    PEQMessageAck msg = (PEQMessageAck)imsg;
                    processAck(msg);
                }
                else if (imsg is PEQMessageBuildTree)
                {
                    PEQMessageBuildTree msg = (PEQMessageBuildTree)imsg;
                    processBuildTree(msg);
                }
                else if (imsg is PEQMessageNotify)
                {
                    PEQMessageNotify msg = (PEQMessageNotify)imsg;
                    processNotify(msg);
                }
                else if (imsg is PEQMessageResponse)
                {
                    PEQMessageResponse msg = (PEQMessageResponse)imsg;
                    processResponse(msg);
                }
                else if (imsg is PEQMessageSearch)
                {
                    PEQMessageSearch msg = (PEQMessageSearch)imsg;
                    processSearch(msg);
                }
                else if (imsg is PEQMessageSubscribe)
                {
                    PEQMessageSubscribe msg = (PEQMessageSubscribe)imsg;
                    processSubscribe(msg);
                }
                else if (imsg is PEQMessageApplication)
                {
                    PEQMessageApplication msg = (PEQMessageApplication)imsg;
                    processApplicationMessage(msg);
                }
            }
            else if (e is NodeTimerEvent)
            {
                if (e is PEQTimerHello)
                {
                    PEQTimerHello timer = (PEQTimerHello)e;
                    processHelloTimer(timer);
                }
                else if (e is PEQTimerAck)
                {
                    PEQTimerAck timer = (PEQTimerAck)e;
                    processAckTimer(timer);
                }
                else if (e is PEQTimerSearch)
                {
                    PEQTimerSearch timer = (PEQTimerSearch)e;
                    processSearchTimer(timer);
                }
                else if (e is PEQTimerMessage)
                {
                    PEQTimerMessage timer = (PEQTimerMessage)e;
                    processMessageTimer(timer);
                }
                else if (e is PEQTimerInternal)
                {
                    PEQTimerInternal timer = (PEQTimerInternal)e;
                    ExecuteAction(timer.Event);
                }
            }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReferrer Methods

        #region IReportSubject Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public void Attach(IReportObserver observer)
        {
            if (_observers.Contains(observer))
                return;
            else _observers.Add(observer);

            _searchAggregator.Attach(observer);
        }

        public void Detach(IReportObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);
        }

        public void Notify(IReport report)
        {
            IReportObserver observer;
            foreach (Object obj in _observers)
            {
                observer = (IReportObserver)obj;
                observer.Update(report);
            }
        }

        public IGraphicalReport GenerateStaticReport()
        {
            CircleReport circReport = new CircleReport(_eventManager.CurrentClock,
                _location, _nodeSize, _gradientPath, true, _id.GetID());

            circReport.MaximumMessageSize = _physicalProcessor.MaximumRange * 2;

            if (_isSink)
            {
                circReport.Color = ColorEnum.Sink;
                circReport.FinalColor = ColorEnum.Sink;
            }
            else
            {
                circReport.Color = ColorEnum.Node;
                circReport.FinalColor = ColorEnum.Node;
            }
            
            if (_busyCount == 1)
            {
                circReport.Color = ColorEnum.Busy;
            }
            if ((_busyCount > 1) || _collided)
            {
                circReport.Color = ColorEnum.Collision;
            }

            return circReport;
        }

        public IGraphicalReport GenerateDirectionReport()
        {
            return GenerateDirectionReport(ColorEnum.Priority);
        }

        public IGraphicalReport GenerateDirectionReport(ColorEnum color)
        {
            foreach (PEQTableEntryRouting route in _tableRouting)
            {
                if (route._Valid) // take first valid route
                {
                    int node_dist = (int)_location.Distance(route._nextHopCheat);
                    int dist = -250;
                    if (node_dist/2 < 250)
                        dist = -node_dist/2;
                    LineReport dirReport = new LineReport(_eventManager.CurrentClock, 
                        (XYDoubleLocation)_location, 
                        (XYDoubleLocation)route._nextHopCheat, 0, dist, true, -_id.GetID());

                    dirReport.Color = color;
                    dirReport.IsArrow = true;

                    return dirReport;
                }
            }
            return null;
        }

        public IGraphicalReport GenerateDirectionReport(ReportLevel level)
        {
            switch (level)
            {
                case ReportLevel.Routine:
                    return GenerateDirectionReport(ColorEnum.Routine);
                case ReportLevel.Priority:
                    return GenerateDirectionReport(ColorEnum.Priority);
                case ReportLevel.Immediate:
                    return GenerateDirectionReport(ColorEnum.Immediate);
                case ReportLevel.Flash:
                    return GenerateDirectionReport(ColorEnum.Flash);
                case ReportLevel.FlashOverride:
                    return GenerateDirectionReport(ColorEnum.FlashOverride);
            }
            return GenerateDirectionReport(ColorEnum.Black);
        }

        IInfoReport GeneratePEQNodeInfoReport(bool isCollision)
        {
            return GeneratePEQNodeInfoReport(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1);
        }

        IInfoReport GeneratePEQNodeInfoReport(bool isSending, PEQMessage msg)
        {
            if (msg is PEQMessageAck)
                return GeneratePEQNodeInfoReport(isSending, PEQMessageType.ACK);
            if (msg is PEQMessageApplication)
                return GeneratePEQNodeInfoReport(isSending, PEQMessageType.Application);
            if (msg is PEQMessageBuildTree)
                return GeneratePEQNodeInfoReport(isSending, PEQMessageType.BuildTree);
            if (msg is PEQMessageHello)
                return GeneratePEQNodeInfoReport(isSending, PEQMessageType.Hello);
            if (msg is PEQMessageNotify)
                return GeneratePEQNodeInfoReport(isSending, PEQMessageType.Notify);
            if (msg is PEQMessageResponse)
                return GeneratePEQNodeInfoReport(isSending, PEQMessageType.Response);
            if (msg is PEQMessageSearch)
                return GeneratePEQNodeInfoReport(isSending, PEQMessageType.Search);
            if (msg is PEQMessageSubscribe)
                return GeneratePEQNodeInfoReport(isSending, PEQMessageType.Subscribe);
            else return null;
        }

        IInfoReport GeneratePEQNodeInfoReport(bool isSending, PEQMessageType messageType)
        {
            switch (messageType)
            {
                case PEQMessageType.Hello:
                    if (isSending)
                        return GeneratePEQNodeInfoReport(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    else
                        return GeneratePEQNodeInfoReport(0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                case PEQMessageType.BuildTree:
                    if (isSending)
                        return GeneratePEQNodeInfoReport(0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    else
                        return GeneratePEQNodeInfoReport(0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                case PEQMessageType.Subscribe:
                    if (isSending)
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    else
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                case PEQMessageType.Notify:
                    if (isSending)
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    else
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
                case PEQMessageType.ACK:
                    if (isSending)
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
                    else
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
                case PEQMessageType.Search:
                    if (isSending)
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
                    else
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
                case PEQMessageType.Response:
                    if (isSending)
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
                    else
                        return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
                case PEQMessageType.Application:
                    return GeneratePEQNodeInfoReport(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
            }
            return null;
        }

        IInfoReport GeneratePEQNodeInfoReport(int sentHellos, int rcvdHellos, int sentBuildTrees,
            int rcvdBuildTrees, int sentSubscribes, int rcvdSubscribes, int sentNotifys,
            int rcvdNotifys, int sentACKs, int rcvdACKs, int sentSearches, int rcvdSearches,
            int sentResponses, int rcvdResponses, int procApplications, int Collisions)
        {
            PEQNodeInfoReport report = new PEQNodeInfoReport(_id.GetID(), _eventManager.CurrentClock, 
                "PEQ_Node", "PEQ Node:");
            report._sentHellos = sentHellos;
            report._rcvdHellos = rcvdHellos;
            report._sentBuildTrees = sentBuildTrees;
            report._rcvdBuildTrees = rcvdBuildTrees;
            report._sentSubscribes = sentSubscribes;
            report._rcvdSubscribes = rcvdSubscribes;
            report._sentNotifys = sentNotifys;
            report._rcvdNotifys = rcvdNotifys;
            report._sentACKs = sentACKs;
            report._rcvdACKs = rcvdACKs;
            report._sentSearches = sentSearches;
            report._rcvdSearches = rcvdSearches;
            report._sentResponses = sentResponses;
            report._rcvdResponses = rcvdResponses;
            report._procApplications = procApplications;
            report._Collisions = Collisions;
            report.Loc = this.Location;

            return report;
        }

        IInfoReport GeneratePEQSinkInfoReport(double elapsedTime, PEQMessageNotify notifyMessage)
        {
            PEQSinkInfoReport report = new PEQSinkInfoReport(_id.GetID(), _eventManager.CurrentClock,
                "PEQ_Sink", "PEQ Sink:");
            report._elapsedTime = elapsedTime;
            report._numHops = notifyMessage._Data._NumHops;
            report._totalDistance = notifyMessage._Data._TotalDistance;
            report.Loc = this.Location;

            return report;
        }


        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReportSubject Methods

    }

    public class PEQNodeFactory : INodeFactory
    {
        #region PEQNodeFactory Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        IReportObserver _reporter;
        int _nodesCreated;

        int _NUM_ID_BYTES;

        double _RANDOM_WAIT, _TIMER_ACK, _TIMER_BUILDTREE,
            _TIMER_HELLO, _TIMER_SEARCH, _TIMER_SUBSCRIBE;

        bool _HELLO_SUPPRESS_REPORT = true;
        bool _EXPLICIT_COLLISIONS = false;
        bool _INFO_REPORTS = false;
        bool _NODE_REPORTS = false;
        bool _SINK_REPORTS = true;

        SearchAggregator _searchAggregator = new SearchAggregator();

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQNodeFactory Variables

        #region INodeFactory Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();

            PanelObj obj = new PanelObj();
            obj.name = "label_NUM_ID_BYTES";
            obj.type = FormType.label;
            obj.text = "ID Size (Bytes)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "NUM_ID_BYTES";
            obj.type = FormType.intBox;
            obj.value = "1";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "label_RANDOM_WAIT";
            obj.type = FormType.label;
            obj.text = "Random Wait Timer (s)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "RANDOM_WAIT";
            obj.type = FormType.doubleBox;
            obj.value = "0.250";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 1;
            list.Add(obj); 
            
            obj = new PanelObj();
            obj.name = "label_TIMER_ACK";
            obj.type = FormType.label;
            obj.text = "ACK Wait Timer (s)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 2;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "TIMER_ACK";
            obj.type = FormType.doubleBox;
            obj.value = "5.0";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 2;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "label_TIMER_BUILDTREE";
            obj.type = FormType.label;
            obj.text = "BuildTree Start Time (s)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 3;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "TIMER_BUILDTREE";
            obj.type = FormType.doubleBox;
            obj.value = "1.0";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 3;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "label_TIMER_HELLO";
            obj.type = FormType.label;
            obj.text = "Hello Timer (s)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 4;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "TIMER_HELLO";
            obj.type = FormType.doubleBox;
            obj.value = "1250.0";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 4;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "label_TIMER_SEARCH";
            obj.type = FormType.label;
            obj.text = "Search Wait Timer (s)";
            obj.xSlot = 0;
            obj.ySlot = 5;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "TIMER_SEARCH";
            obj.type = FormType.doubleBox;
            obj.value = "5.0";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 5;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "label_TIMER_SUBSCRIBE";
            obj.type = FormType.label;
            obj.text = "Subscribe Start Time (s)";
            obj.xSlot = 0;
            obj.ySlot = 6;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "TIMER_SUBSCRIBE";
            obj.type = FormType.doubleBox;
            obj.value = "15.0";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 6;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "CHECK_HELLOSUPPRESS";
            obj.type = FormType.check;
            obj.text = "Suppress Hello Reports";
            obj.value = "true";
            obj.width = 106;
            obj.xSlot = 0;
            obj.ySlot = 7;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "label_HELLOSUPPRESS";
            obj.type = FormType.label;
            obj.text = "(Also reduces text output)";
            obj.xSlot = 0;
            obj.ySlot = 8;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "CHECK_EXPLICIT_COLLISIONS";
            obj.type = FormType.check;
            obj.text = "Explicit Collisions (UWA Phys Proc)";
            obj.value = "false";
            obj.width = 106;
            obj.xSlot = 0;
            obj.ySlot = 9;
            list.Add(obj);

            /*obj = new PanelObj();
            obj.name = "INFO_REPORTS";
            obj.type = FormType.check;
            obj.text = "Informational Reports On/Off";
            obj.value = "true";
            obj.width = 106;
            obj.xSlot = 0;
            obj.ySlot = 10;
            list.Add(obj);*/

            obj = new PanelObj();
            obj.name = "NODE_REPORTS";
            obj.type = FormType.check;
            obj.text = "Node Reports On/Off";
            obj.value = "false";
            obj.width = 106;
            obj.xSlot = 0;
            obj.ySlot = 10;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "SINK_REPORTS";
            obj.type = FormType.check;
            obj.text = "Sink Reports On/Off";
            obj.value = "true";
            obj.width = 106;
            obj.xSlot = 0;
            obj.ySlot = 11;
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

        bool _isInitialized;
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        IEventManager _eventManager;
        public IEventManager EventManager
        {
            get { return _eventManager; }
        }

        IPhysicalProcessor _physicalProcessor;
        public IPhysicalProcessor PhysicalProcesser
        {
            get { return _physicalProcessor; }
        }

        IRandomizerFactory _randomizerFactory;
        public IRandomizerFactory RandomizerFactory
        {
            set { _randomizerFactory = value; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion INodeFactory Variables

        #region PEQNodeFactory Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public PEQNodeFactory()
        {
            _isInitialized = false;
        }

        public PEQNodeFactory(IEventManager eventMgr, IPhysicalProcessor physProc, 
            IRandomizerFactory randomizerFactory, IReportObserver reporter)
        {
            Initialize(eventMgr, physProc, randomizerFactory, reporter);
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQNodeFactory Methods

        #region INodeFactory Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public void Initialize(IEventManager eventMgr, IPhysicalProcessor physProc,
            IRandomizerFactory randomizerFactory, IReportObserver reporter)
        {
            _eventManager = eventMgr;
            _physicalProcessor = physProc;
            _randomizerFactory = randomizerFactory;
            _reporter = reporter;
            _isInitialized = true;

            panelObjsHelper = new PanelObjHelper(panelObjs);
            _NUM_ID_BYTES = panelObjsHelper.GetIntByName("NUM_ID_BYTES");
            _RANDOM_WAIT = panelObjsHelper.GetDoubleByName("RANDOM_WAIT");
            _TIMER_ACK = panelObjsHelper.GetDoubleByName("TIMER_ACK");
            _TIMER_BUILDTREE = panelObjsHelper.GetDoubleByName("TIMER_BUILDTREE");
            _TIMER_HELLO = panelObjsHelper.GetDoubleByName("TIMER_HELLO");
            _TIMER_SEARCH = panelObjsHelper.GetDoubleByName("TIMER_SEARCH");
            _TIMER_SUBSCRIBE = panelObjsHelper.GetDoubleByName("TIMER_SUBSCRIBE");
            _HELLO_SUPPRESS_REPORT = panelObjsHelper.GetBoolByName("CHECK_HELLOSUPPRESS");
            _EXPLICIT_COLLISIONS = panelObjsHelper.GetBoolByName("CHECK_EXPLICIT_COLLISIONS");
            //_INFO_REPORTS = panelObjsHelper.GetBoolByName("INFO_REPORTS");
            _NODE_REPORTS = panelObjsHelper.GetBoolByName("NODE_REPORTS");
            _SINK_REPORTS = panelObjsHelper.GetBoolByName("SINK_REPORTS");
        }

        public INode CreateNode(ILocation loc)
        { // Creates a PEQ Node at the specified location
            if (!_isInitialized)
                throw new InvalidOperationException("PEQNodeFactory not initialized!");

            PEQNode node = new PEQNode(_eventManager, _physicalProcessor, _searchAggregator);

            node._NUM_ID_BYTES = _NUM_ID_BYTES;
            node._TIMER_ACK = _TIMER_ACK;
            node._TIMER_BUILDTREE = _TIMER_BUILDTREE;
            node._TIMER_HELLO = _TIMER_HELLO;
            node._TIMER_RANDOM_WAIT_SEND = _RANDOM_WAIT;
            node._TIMER_SEARCH = _TIMER_SEARCH;
            node._TIMER_SUBSCRIBE = _TIMER_SUBSCRIBE;
            node._TIMER_WAIT_SEND = _RANDOM_WAIT;
            node._HELLO_SUPPRESS_REPORT = _HELLO_SUPPRESS_REPORT;
            node._EXPLICIT_COLLISIONS = _EXPLICIT_COLLISIONS;
            node._INFO_REPORTS = _INFO_REPORTS;
            node._NODE_REPORTS = _NODE_REPORTS;
            node._SINK_REPORTS = _SINK_REPORTS;

            node.Attach(_reporter);
            node.Location = loc;
            node.ID = ++_nodesCreated; // Reserve node #0
            node.RandomValue = _randomizerFactory.CreateRandomizer();

            return (INode)node;
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion INodeFactory Methods
    }

    public struct VarID
    {
        public byte[] _id;
        int _IDFieldSize;

        public VarID(int IDFieldSize)
        {
            _IDFieldSize = IDFieldSize;
            _id = new byte[IDFieldSize];
            SetMax();   // default to all 0xFF.
        }

        public VarID(int IDFieldSize, int id) : this(IDFieldSize)
        {
            SetID(id);
        }

        public void SetID(int id)
        {
            byte[] temp = System.BitConverter.GetBytes(id);
            for (int i = 0; i < _IDFieldSize; i++)
                _id[i] = temp[i];
        }

        public int GetID()
        {
            byte[] temp = new byte[5];
            for (int i = 0; i < _IDFieldSize; i++)
                temp[i] = _id[i];
            return System.BitConverter.ToInt32(temp, 0);
        }

        public void Add(int val)
        {
            SetID(GetID() + val);
        }

        public int SizeOf()
        {
            return _IDFieldSize;
        }

        public void SetMax()
        {
            for (int i = 0; i < _IDFieldSize; i++)
                _id[i] = 0xFF;
        }

        public static bool operator ==(VarID v1, VarID v2)
        {
            if (v1._IDFieldSize != v2._IDFieldSize)
                return false;

            for (int i = 0; i < v1._IDFieldSize; i++)
            {
                if (v1._id[i] != v2._id[i])
                    return false;
            }

            return true;
        }

        public static bool operator !=(VarID v1, VarID v2)
        {
            return !(v1 == v2);
        }
    }
}
