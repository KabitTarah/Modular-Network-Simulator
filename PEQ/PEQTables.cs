using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Location;

namespace PEQ
{
    public class PEQTableEntryConfig
    {
        /* Contains Sink Configuration Parameters. Only used if the node is
         * a sink and no specific parameters are provided. Unused.
         */
    }

    public class PEQTableEntryRouting
    {
        // For routing to/from Sink Node.

        public VarID _SinkID;
        public VarID _SenderID; // 0xFF
        public VarID _DestinationID;
        public ILocation[] _Coordinates;

        public bool _Valid = true;

        public ILocation _nextHopCheat;    // Used for visual purposes only

        public PEQTableEntryRouting(int VarIDSize)
        {
            _SenderID = new VarID(VarIDSize);
            _nextHopCheat = new XYDoubleLocation(0, 0);
        }
    }

    public class PEQTableEntrySubscription
    {
        // Containing various Sink subscriptions for which this node
        // can provide.

        public VarID _SinkID;
        public double _Timestamp = 0;

        // In the "real" PEQ this should probably be PEQSubscriptionCriteria
        public Int16 _CriteriaType; // also found in PEQMessageSubscribe.
                                    // 0x0000 reserved for BuildTree hop count
        public Byte _HopCount;
        public VarID _DestinationID;

        public bool _Valid = true;

        public ILocation _nextHopCheat;    // Used for visual purposes only
    }

    public class PEQTableEntryMessageAck
    {
        public Byte _SequenceNumber;
        public VarID _DestinationID;
        public PEQMessage _DataMessage;

        public PEQTableEntryMessageAck()
        { }

        public PEQTableEntryMessageAck(Byte SequenceNumber, VarID DestinationID)
        {
            _SequenceNumber = SequenceNumber;
            _DestinationID = DestinationID;
        }

        public PEQTableEntryMessageAck(Byte SequenceNumber, VarID DestinationID, PEQMessage dataMessage)
        {
            _SequenceNumber = SequenceNumber;
            _DestinationID = DestinationID;
            _DataMessage = dataMessage;
        }
    }

    public class PEQTableEntrySearch
    {
        public VarID _SinkID;
        public VarID _DestinationID;
        public Byte _HopCount;
        public PEQMessage _DataMessage;

        public bool _ResponseReceived = false;

        public ILocation _nextHopCheat;    // Used for visual purposes only
    }
}
