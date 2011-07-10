using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Location;
using ModularNetworkSimulator;

namespace PEQ
{
    public enum PEQMessageType { Hello, BuildTree, Subscribe, Notify, ACK, Search, Response, Application };

    public class PEQMessage : ICloneable
    {
     // Base message. Contains all header info required in all other messages

        public VarID _DestinationID;
        public VarID _SenderID;
        public Byte _SequenceNumber;
        public Byte _MessageType;       // Not necessary... 
                                        // added for size completeness.

        public ILocation _nextHopCheat; // Used for visual purposes only

        public PEQMessage()
        { }

        public PEQMessage(VarID DestinationID, VarID SenderID, Byte SequenceNumber)
        {
            _DestinationID = DestinationID;
            _SenderID = SenderID;
            _SequenceNumber = SequenceNumber;
        }

        PEQMessage(VarID DestinationID, VarID SenderID, Byte SequenceNumber,
            Byte MessageType) : this(DestinationID, SenderID, SequenceNumber)
        {
            _MessageType = MessageType;
        }

        public Int32 SizeBytes()
        {
            return 2 + _SenderID.SizeOf() + _DestinationID.SizeOf();
        }

        /***********************************************\
        |******************* WARNING *******************|
        |***********************************************|
        |* See warning in PEQMessageHello!!             |
        \***********************************************/
        public object Clone()
        {
            PEQMessage outmsg = new PEQMessage(_DestinationID, _SenderID, 
                _SequenceNumber, _MessageType);
            outmsg._nextHopCheat = _nextHopCheat;
            return outmsg;
        }
    }

    public class PEQInternal : ICloneable
    {
     // Empty class defining internal messages (node/application)
        public PEQInternal()
        { }

        public Int32 SizeBytes()
        {
            return 0;
        }

        public object Clone()
        {
            return new PEQInternal();
        }
    }

    public class PEQMessageHello : PEQMessage, IMessage
    { 
     // Hello Message: For Neighbor Discovery
        
     /* Note: This is added and not formally specified in the PEQ article.
      * The article states that neighbors know about each other, for which
      * some type of discovery must take place. */

        public PEQMessageHello()
        {
            _MessageType = 0xFF;
            _SequenceNumber = 0x00;
        }

        public PEQMessageHello(VarID SenderID)
        {
            _DestinationID = new VarID(SenderID.SizeOf());
            _SenderID = SenderID;
            _MessageType = 0xFF;
            _SequenceNumber = 0x00;
        }


        /***********************************************\
        |******************* WARNING *******************|
        |***********************************************|
        |* This Clone() method is never called unless   |
        |* the type (PEQMessageHello) is explicitly     |
        |* known. The base Clone() will be called. It   |
        |* seems as though it should use the most       |
        |* specific class, but the least specific is    |
        |* chosen.                                      |
        \***********************************************/
        new public object Clone()
        {
            PEQMessageHello msg = new PEQMessageHello();
            msg._DestinationID = this._DestinationID;
            msg._MessageType = this._MessageType;
            msg._SenderID = this._SenderID;
            msg._SequenceNumber = this._SequenceNumber;
            msg._nextHopCheat = this._nextHopCheat;
            return msg;
        }
    }

    public class PEQMessageBuildTree : PEQMessage, IMessage
    {
     // Build Tree Message: Generates the initial routing tree (Sink Initiated)

        public VarID _SinkID;
        public Byte _HopCount;

        public PEQMessageBuildTree()
        {
            _MessageType = 0x00;
        }

        public PEQMessageBuildTree(VarID DestinationID, VarID SenderID,
            Byte SequenceNumber, VarID SinkID, Byte HopCount) :
            base(DestinationID, SenderID, SequenceNumber)
        {
            _MessageType = 0x00;
            _SinkID = SinkID;
            _HopCount = HopCount;
        }

        public PEQMessageBuildTree(PEQMessage BaseMessage, VarID SinkID,
            Byte HopCount)
            : base(BaseMessage._DestinationID, BaseMessage._SenderID, 
              BaseMessage._SequenceNumber)
        {
            _MessageType = 0x00;
            _SinkID = SinkID;
            _HopCount = HopCount;
        }

        new public Int32 SizeBytes()
        {
            return base.SizeBytes() + 1 + _SinkID.SizeOf();
        }

        new public object Clone()
        {
            PEQMessageBuildTree msg =
                new PEQMessageBuildTree(_DestinationID, _SenderID, _SequenceNumber,
                _SinkID, _HopCount);
            msg._nextHopCheat = _nextHopCheat;
            return msg;
        }
    }

    public class PEQSubscriptionInfo : ICloneable
    {
     // Subscription Info: Defines what criteria are desired by the Sink

        public Int16 _CriteriaType;
        public PEQSubscriptionCriteria _Criteria;

        public PEQSubscriptionInfo()
        { }

        public PEQSubscriptionInfo(Int16 CriteriaType,
            PEQSubscriptionCriteria Criteria)
        {
            _CriteriaType = CriteriaType;
            _Criteria = Criteria;
        }

        public Int32 SizeBytes()
        {
            return 2 + _Criteria.SizeBytes();
        }

        public object Clone()
        {
            return new PEQSubscriptionInfo(_CriteriaType,
                (PEQSubscriptionCriteria)_Criteria.Clone());
        }
    }

    public class PEQSubscriptionCriteria : ICloneable
    {
     // A Base type, different per CriteriaType (Int16)

        public Int16 CriteriaType = 0x0001;

        public PEQSubscriptionCriteria()
        { }

        public Int32 SizeBytes()
        {
            return 2;
        }

        public object Clone()
        {
            return new PEQSubscriptionCriteria();
        }
    }

    public class PEQMessageSubscribe : PEQMessage, IMessage
    {
     // Subscribe Message: Sent by the Sink to define its interest criteria

        public VarID _SinkID;
        public Byte _HopCount;
        public PEQSubscriptionInfo _SubscriptionInfo;

        public PEQMessageSubscribe()
        {
            _MessageType = 0x01;
        }

        public PEQMessageSubscribe(VarID DestinationID, VarID SenderID,
            Byte SequenceNumber, VarID SinkID, Byte HopCount,
            PEQSubscriptionInfo SubscriptionInfo) :
            base(DestinationID, SenderID, SequenceNumber)
        {
            _MessageType = 0x01;
            _SinkID = SinkID;
            _HopCount = HopCount;
            _SubscriptionInfo = SubscriptionInfo;
        }

        public PEQMessageSubscribe(PEQMessage BaseMessage, VarID SinkID,
            Byte HopCount, PEQSubscriptionInfo SubscriptionInfo)
            : base(BaseMessage._DestinationID, BaseMessage._SenderID, 
              BaseMessage._SequenceNumber)
        {
            _MessageType = 0x01;
            _SinkID = SinkID;
            _HopCount = HopCount;
            _SubscriptionInfo = SubscriptionInfo;
        }

        new public Int32 SizeBytes()
        {
            return base.SizeBytes() + 1 + _SinkID.SizeOf() + _SubscriptionInfo.SizeBytes();
        }

        new public object Clone()
        {
            PEQMessageSubscribe msg = 
                new PEQMessageSubscribe(_DestinationID, _SenderID, _SequenceNumber,
                _SinkID, _HopCount, (PEQSubscriptionInfo)_SubscriptionInfo.Clone());
            msg._nextHopCheat = _nextHopCheat;
            return msg;
        }
    }

    public class PEQData : ICloneable
    {
     // A payload carried by the Notify message

        public Byte[] _Data;

        public double _StartTime;         // for statistics gathering.
        public int _NumHops = 0;          // ...
        public double _TotalDistance = 0; // ...
        public double _DataID = 0;        // ...

        public PEQData()
        {
            _Data = new Byte[10];
        }

        public PEQData(Byte[] Data)
        {
            _Data = Data;
        }

        public PEQData(Byte[] Data, double startTime)
        {
            _Data = Data;
            _StartTime = startTime;
        }

        public Int32 SizeBytes()
        {
            return _Data.Length;
        }

        public object Clone()
        {
            PEQData clone = new PEQData(_Data, _StartTime);
            clone._NumHops = _NumHops;
            clone._TotalDistance = _TotalDistance;
            clone._DataID = _DataID;
            return clone;
        }
    }

    public class PEQMessageNotify : PEQMessage, IMessage
    {
     // Notify Message: Sends data from Source, routed to Sink

        public VarID _SinkID;
        public PEQData _Data;

        public PEQMessageNotify()
        {
            _MessageType = 0x02;
        }

        public PEQMessageNotify(VarID DestinationID, VarID SenderID,
            Byte SequenceNumber, VarID SinkID, PEQData Data) :
            base(DestinationID, SenderID, SequenceNumber)
        {
            _MessageType = 0x02;
            _SinkID = SinkID;
            _Data = Data;
        }

        public PEQMessageNotify(PEQMessage BaseMessage, VarID SinkID, 
            PEQData Data)
            : base(BaseMessage._DestinationID, BaseMessage._SenderID, 
              BaseMessage._SequenceNumber)
        {
            _MessageType = 0x02;
            _SinkID = SinkID;
            _Data = Data;
        }

        new public Int32 SizeBytes()
        {
            return base.SizeBytes() + _SinkID.SizeOf() + _Data.SizeBytes();
        }

        new public object Clone()
        {
            PEQMessageNotify msg = 
                new PEQMessageNotify(_DestinationID, _SenderID,
                _SequenceNumber, _SinkID, (PEQData)_Data.Clone());
            msg._nextHopCheat = _nextHopCheat;
            return msg;
        }
    }

    public class PEQMessageAck : PEQMessage, IMessage
    {
     // Ack message: General acknowledgement (sent when Received Dest ID 
     //              == Local Node ID)

        public PEQMessageAck(VarID DestinationID, VarID SenderID, 
            Byte SequenceNumber) : base(DestinationID, SenderID, SequenceNumber)
        {
            _MessageType = 0x03;
        }

        new public object Clone()
        {
            PEQMessageAck msg = new PEQMessageAck(_DestinationID, _SenderID,
                _SequenceNumber);
            msg._nextHopCheat = _nextHopCheat;
            return msg;
        }
    }

    public class PEQMessageSearch : PEQMessage, IMessage
    {
     // Search Message: Sent when no ACK is received, looking for the best path
     //                 back to the (missing) sink.

        public VarID _SinkID;

        public PEQMessageSearch()
        {
            _MessageType = 0x04;
        }

        public PEQMessageSearch(VarID DestinationID, VarID SenderID,
            Byte SequenceNumber, VarID SinkID) :
            base(DestinationID, SenderID, SequenceNumber)
        {
            _MessageType = 0x04;
            _SinkID = SinkID;
        }

        public PEQMessageSearch(PEQMessage BaseMessage, VarID SinkID)
            : base(BaseMessage._DestinationID, BaseMessage._SenderID, 
              BaseMessage._SequenceNumber)
        {
            _MessageType = 0x04;
            _SinkID = SinkID;
        }

        new public Int32 SizeBytes()
        {
            return base.SizeBytes() + _SinkID.SizeOf();
        }

        new public object Clone()
        {
            PEQMessageSearch msg = 
                new PEQMessageSearch(_DestinationID, _SenderID, _SequenceNumber,
                _SinkID);
            msg._nextHopCheat = _nextHopCheat;
            return msg;
        }
    }

    public class PEQMessageResponse : PEQMessage, IMessage
    {
     // Response Message: Sent in response to a Search

        public Byte _HopCount;
        public VarID _SinkID;

        public PEQMessageResponse()
        {
            _MessageType = 0x05;
        }

        public PEQMessageResponse(VarID DestinationID, VarID SenderID,
            Byte SequenceNumber, Byte HopCount, VarID SinkID) :
            base(DestinationID, SenderID, SequenceNumber)
        {
            _MessageType = 0x05;
            _HopCount = HopCount;
            _SinkID = SinkID;
        }

        public PEQMessageResponse(PEQMessage BaseMessage, Byte HopCount, VarID SinkID)
            : base(BaseMessage._DestinationID, BaseMessage._SenderID, 
              BaseMessage._SequenceNumber)
        {
            _MessageType = 0x05;
            _HopCount = HopCount;
            _SinkID = SinkID;
        }

        new public Int32 SizeBytes()
        {
            return base.SizeBytes() + 1 + _SinkID.SizeOf();
        }

        new public object Clone()
        {
            PEQMessageResponse msg = 
                new PEQMessageResponse(_DestinationID, _SenderID, _SequenceNumber,
                _HopCount, _SinkID);
            msg._nextHopCheat = _nextHopCheat;
            return msg;
        }
    }

    public class PEQMessageApplication : PEQInternal, IMessage
    {
        public PEQSubscriptionCriteria _Criteria;
        public PEQData _Data;

        public PEQMessageApplication()
        {
            _Criteria = new PEQSubscriptionCriteria();
            _Data = new PEQData();
        }

        public PEQMessageApplication(PEQSubscriptionCriteria criteria,
            PEQData data)
        {
            _Criteria = criteria;
            _Data = data;
        }

        public Int32 SizeBytes()
        {
            return _Criteria.SizeBytes() + _Data.SizeBytes();
        }

        public object Clone()
        {
            PEQMessageApplication msg = new PEQMessageApplication(
                (PEQSubscriptionCriteria)_Criteria.Clone(),
                (PEQData)_Data.Clone());
            return msg;
        }
    }
}
