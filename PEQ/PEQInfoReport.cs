using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_Reporting;
using Location;

namespace PEQ
{
    class PEQInfoReport : IInfoReport
    {
        #region IReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        ILocation _location = null;
        public ILocation Loc
        {
            get { return _location; }
            set { _location = value; }
        }

        int _id;
        public int ID
        {
            get { return _id; }
        }

        double _timestamp;
        public double Time
        {
            get { return _timestamp; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReport Variables

        #region IInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        string _key = "PEQ_Info";
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        string _label = "PEQ_Info:";
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public String CSVHeader
        {
            get
            {
                return "Node ID, Seed";
            }
        }

        bool _isSummable = true;
        public bool IsSummable
        {
            get { return _isSummable; }
        }

        bool _isWritten = false;
        public bool IsWritten
        {
            get { return _isWritten; }
            set { _isWritten = value; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Variables

        #region PEQInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public double _seed;
        
        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQInfoReport Variables

        #region PEQInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public PEQInfoReport(int id, double timestamp)
        { // Minimum required information to create report
            _id = id;
            _timestamp = timestamp;
        }

        public PEQInfoReport(int id, double timestamp,
            string key, string label)
        {
            _id = id;
            _timestamp = timestamp;
            _key = key;
            _label = label;

            // Values to be set manually...
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQInfoReport Methods

        #region IInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public IInfoReport Add(IInfoReport withTarget)
        {
            if (withTarget is PEQNodeInfoReport)
            {
                int newID;
                double newTimestamp;
                ILocation newLocation;
                bool sameNode;
                if (withTarget.ID == _id)
                {
                    sameNode = true;
                    newID = _id;
                    newLocation = _location;
                }
                else
                {
                    sameNode = false;
                    newID = int.MaxValue;
                    newLocation = null;
                }

                if (withTarget.Time > _timestamp)
                    newTimestamp = withTarget.Time;
                else newTimestamp = _timestamp;

                PEQInfoReport target = (PEQInfoReport)withTarget;
                PEQInfoReport output = new PEQInfoReport(newID, newTimestamp,
                    _key, _label);
                output._location = newLocation;
                return output;
            }
            else return null;
        }

        public IInfoReport Subtract(IInfoReport targetFromThisObject)
        { // Probably will be unused for this report type.
            if (targetFromThisObject is PEQNodeInfoReport)
            {
                int newID;
                double newTimestamp;
                ILocation newLocation;
                bool sameNode = false;

                if (targetFromThisObject.ID == _id)
                {
                    sameNode = true;
                    newID = _id;
                    newLocation = _location;
                }
                else
                {
                    sameNode = false;
                    newID = int.MaxValue;
                    newLocation = null;
                }

                if (targetFromThisObject.Time > _timestamp)
                    newTimestamp = targetFromThisObject.Time;
                else newTimestamp = _timestamp;

                PEQInfoReport target = (PEQInfoReport)targetFromThisObject;
                PEQInfoReport output = new PEQInfoReport(newID, newTimestamp,
                    _key, _label);
                output._location = newLocation;
                return output;
            }
            else return null;
        }

        public String ToValueString()
        {
            return null; // Not possible for this record type...
        }

        public override String ToString()
        {
            return "Node " + _id.ToString() + " Time: "
                + _timestamp.ToString("0.0000000") + " "
                + _label + " (all sent/rcvd)";
        }

        public String ToCSV()
        {
            return _id.ToString()
                + ", " + _seed.ToString();
        }

        /*        | 
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Methods
    }

    class PEQNodeInfoReport : IInfoReport
    {
        #region IReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        ILocation _location = null;
        public ILocation Loc
        {
            get { return _location; }
            set { _location = value; }
        }

        int _id;
        public int ID
        {
            get { return _id; }
        }

        double _timestamp;
        public double Time
        {
            get { return _timestamp; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReport Variables

        #region IInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        string _key = "PEQ_Node";
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        string _label = "PEQ_Node:";
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public String CSVHeader
        {
            get
            {
                return "Node ID, Timestamp, Hellos Sent, Hellos Received, "
                    + "BuildTrees Sent, BuildTrees Received, Subscribes Sent, "
                    + "Subscribes Received, Notifys Sent, Notifys Received, "
                    + "ACKs Sent, ACKs Received, Searches Sent, Searches Received, "
                    + "Responses Sent, Responses Received, Application Messages Processed, "
                    + "Collisions";
            }
        }

        bool _isSummable = true;
        public bool IsSummable
        {
            get { return _isSummable; }
        }

        bool _isWritten = false;
        public bool IsWritten
        {
            get { return _isWritten; }
            set { _isWritten = value; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Variables

        #region PEQInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public int _sentHellos, _sentBuildTrees, _sentSubscribes, _sentNotifys, _sentACKs,
            _sentSearches, _sentResponses;
        public int _rcvdHellos, _rcvdBuildTrees, _rcvdSubscribes, _rcvdNotifys, _rcvdACKs,
            _rcvdSearches, _rcvdResponses;
        public int _procApplications, _Collisions;

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQInfoReport Variables
        
        #region PEQInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public PEQNodeInfoReport(int id, double timestamp)
        { // Minimum required information to create report
            _id = id;
            _timestamp = timestamp;
        }

        public PEQNodeInfoReport(int id, double timestamp,
            string key, string label)
        {
            _id = id;
            _timestamp = timestamp;
            _key = key;
            _label = label;

            // Values to be set manually...
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQInfoReport Methods

        #region IInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public IInfoReport Add(IInfoReport withTarget)
        {
            if (withTarget is PEQNodeInfoReport)
            {
                int newID;
                double newTimestamp;
                ILocation newLocation;
                bool sameNode;
                if (withTarget.ID == _id)
                {
                    sameNode = true;
                    newID = _id;
                    newLocation = _location;
                }
                else
                {
                    sameNode = false;
                    newID = int.MaxValue;
                    newLocation = null;
                }

                if (withTarget.Time > _timestamp)
                    newTimestamp = withTarget.Time;
                else newTimestamp = _timestamp;

                PEQNodeInfoReport target = (PEQNodeInfoReport)withTarget;
                PEQNodeInfoReport output = new PEQNodeInfoReport(newID, newTimestamp,
                    _key, _label);
                output._location = newLocation;
                output._sentHellos = _sentHellos + target._sentHellos;
                output._sentBuildTrees = _sentBuildTrees + target._sentBuildTrees;
                output._sentSubscribes = _sentSubscribes + target._sentSubscribes;
                output._sentNotifys = _sentNotifys + target._sentNotifys;
                output._sentACKs = _sentACKs + target._sentACKs;
                output._sentSearches = _sentSearches + target._sentSearches;
                output._sentResponses = _sentResponses + target._sentResponses;
                output._rcvdHellos = _rcvdHellos + target._rcvdHellos;
                output._rcvdBuildTrees = _rcvdBuildTrees + target._rcvdBuildTrees;
                output._rcvdSubscribes = _rcvdSubscribes + target._rcvdSubscribes;
                output._rcvdNotifys = _rcvdNotifys + target._rcvdNotifys;
                output._rcvdACKs = _rcvdACKs + target._rcvdACKs;
                output._rcvdSearches = _rcvdSearches + target._rcvdSearches;
                output._rcvdResponses = _rcvdResponses + target._rcvdResponses;
                output._procApplications = _procApplications + target._procApplications;
                output._Collisions = _Collisions + target._Collisions;
                return output;
            }
            else return null;
        }

        public IInfoReport Subtract(IInfoReport targetFromThisObject)
        { // Probably will be unused for this report type.
            if (targetFromThisObject is PEQNodeInfoReport)
            {
                int newID;
                double newTimestamp;
                ILocation newLocation;
                bool sameNode;
                if (targetFromThisObject.ID == _id)
                {
                    sameNode = true;
                    newID = _id;
                    newLocation = _location;
                }
                else
                {
                    sameNode = false;
                    newID = int.MaxValue;
                    newLocation = null;
                }

                if (targetFromThisObject.Time > _timestamp)
                    newTimestamp = targetFromThisObject.Time;
                else newTimestamp = _timestamp;

                PEQNodeInfoReport target = (PEQNodeInfoReport)targetFromThisObject;
                PEQNodeInfoReport output = new PEQNodeInfoReport(newID, newTimestamp,
                    _key, _label);
                output._location = newLocation;
                output._sentHellos = _sentHellos - target._sentHellos;
                output._sentBuildTrees = _sentBuildTrees - target._sentBuildTrees;
                output._sentSubscribes = _sentSubscribes - target._sentSubscribes;
                output._sentNotifys = _sentNotifys - target._sentNotifys;
                output._sentACKs = _sentACKs - target._sentACKs;
                output._sentSearches = _sentSearches - target._sentSearches;
                output._sentResponses = _sentResponses - target._sentResponses;
                output._rcvdHellos = _rcvdHellos - target._rcvdHellos;
                output._rcvdBuildTrees = _rcvdBuildTrees - target._rcvdBuildTrees;
                output._rcvdSubscribes = _rcvdSubscribes - target._rcvdSubscribes;
                output._rcvdNotifys = _rcvdNotifys - target._rcvdNotifys;
                output._rcvdACKs = _rcvdACKs - target._rcvdACKs;
                output._rcvdSearches = _rcvdSearches - target._rcvdSearches;
                output._rcvdResponses = _rcvdResponses - target._rcvdResponses;
                output._procApplications = _procApplications - target._procApplications;
                output._Collisions = _Collisions - target._Collisions;
                return output;
            }
            else return null;
        }

        public String ToValueString()
        {
            return null; // Not possible for this record type...
        }

        public override String ToString()
        {
            return "Node " + _id.ToString() + " Time: "
                + _timestamp.ToString("0.0000000") + " "
                + _label + " (all sent/rcvd)"
                + "\n         Hello: " + _sentHellos.ToString() + " / " + _rcvdHellos.ToString()
                + "\n     BuildTree: " + _sentBuildTrees.ToString() + " / " + _rcvdBuildTrees.ToString()
                + "\n     Subscribe: " + _sentSubscribes.ToString() + " / " + _rcvdSubscribes.ToString()
                + "\n        Notify: " + _sentNotifys.ToString() + " / " + _rcvdNotifys.ToString()
                + "\n           ACK: " + _sentACKs.ToString() + " / " + _rcvdACKs.ToString()
                + "\n        Search: " + _sentSearches.ToString() + " / " + _rcvdSearches.ToString()
                + "\n      Response: " + _sentResponses.ToString() + " / " + _rcvdResponses.ToString()
                + "\n   Application: " + _procApplications
                + "\n     Collision: " + _Collisions.ToString();
        }

        public String ToCSV()
        {
            return _id.ToString()
                + ", " + _timestamp.ToString("0.0000000000")
                + ", " + _sentHellos.ToString() + ", " + _rcvdHellos.ToString()
                + ", " + _sentBuildTrees.ToString() + ", " + _rcvdBuildTrees.ToString()
                + ", " + _sentSubscribes.ToString() + ", " + _rcvdSubscribes.ToString()
                + ", " + _sentNotifys.ToString() + ", " + _rcvdNotifys.ToString()
                + ", " + _sentACKs.ToString() + ", " + _rcvdACKs.ToString()
                + ", " + _sentSearches.ToString() + ", " + _rcvdSearches.ToString()
                + ", " + _sentResponses.ToString() + ", " + _rcvdResponses.ToString()
                + ", " + _procApplications + ", " + _Collisions.ToString();
        }

        /*        | 
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Methods
    }

    class PEQSinkInfoReport : IInfoReport
    {
        #region IReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        ILocation _location = null;
        public ILocation Loc
        {
            get { return _location; }
            set { _location = value; }
        }

        int _id;
        public int ID
        {
            get { return _id; }
        }

        double _timestamp;
        public double Time
        {
            get { return _timestamp; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReport Variables

        #region IInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        string _key = "PEQ_Sink";
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        string _label = "PEQ_Sink:";
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public String CSVHeader
        {
            get
            {
                return "Node ID, Timestamp, Elapsed Time (Data to Sink), # Hops, Total Distance";
            }
        }

        bool _isSummable = true;
        public bool IsSummable
        {
            get { return _isSummable; }
        }

        bool _isWritten = false;
        public bool IsWritten
        {
            get { return _isWritten; }
            set { _isWritten = value; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Variables

        #region PEQInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public double _elapsedTime;
        public int _numHops;
        public double _totalDistance;

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQInfoReport Variables

        #region PEQInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public PEQSinkInfoReport(int id, double timestamp)
        { // Minimum required information to create report
            _id = id;
            _timestamp = timestamp;
        }

        public PEQSinkInfoReport(int id, double timestamp,
            string key, string label)
        {
            _id = id;
            _timestamp = timestamp;
            _key = key;
            _label = label;

            // Values to be set manually...
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQInfoReport Methods

        #region IInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public IInfoReport Add(IInfoReport withTarget)
        {
            if (withTarget is PEQNodeInfoReport)
            {
                int newID;
                double newTimestamp;
                ILocation newLocation;
                bool sameNode;
                if (withTarget.ID == _id)
                {
                    sameNode = true;
                    newID = _id;
                    newLocation = _location;
                }
                else
                {
                    sameNode = false;
                    newID = int.MaxValue;
                    newLocation = null;
                }

                if (withTarget.Time > _timestamp)
                    newTimestamp = withTarget.Time;
                else newTimestamp = _timestamp;

                PEQSinkInfoReport target = (PEQSinkInfoReport)withTarget;
                PEQSinkInfoReport output = new PEQSinkInfoReport(newID, newTimestamp,
                    _key, _label);
                output._location = newLocation;
                output._elapsedTime = _elapsedTime;
                return output;
            }
            else return null;
        }

        public IInfoReport Subtract(IInfoReport targetFromThisObject)
        { // Probably will be unused for this report type.
            if (targetFromThisObject is PEQNodeInfoReport)
            {
                int newID;
                double newTimestamp;
                ILocation newLocation;
                bool sameNode = false;

                if (targetFromThisObject.ID == _id)
                {
                    sameNode = true;
                    newID = _id;
                    newLocation = _location;
                }
                else
                {
                    sameNode = false;
                    newID = int.MaxValue;
                    newLocation = null;
                }

                if (targetFromThisObject.Time > _timestamp)
                    newTimestamp = targetFromThisObject.Time;
                else newTimestamp = _timestamp;

                PEQSinkInfoReport target = (PEQSinkInfoReport)targetFromThisObject;
                PEQSinkInfoReport output = new PEQSinkInfoReport(newID, newTimestamp,
                    _key, _label);
                output._location = newLocation;
                output._elapsedTime = _elapsedTime;
                return output;
            }
            else return null;
        }

        public String ToValueString()
        {
            return null; // Not possible for this record type...
        }

        public override String ToString()
        {
            return "Node " + _id.ToString() + " Time: "
                + _timestamp.ToString("0.0000000") + " "
                + _label + " (all sent/rcvd)"
                + "\n  Elapsed Time: " + _elapsedTime.ToString("0.000")
                + "\n        # Hops: " + _numHops.ToString()
                + "\n    Total Dist: " + _totalDistance.ToString("0.00") + " m";
        }

        public String ToCSV()
        {
            return _id.ToString()
                + ", " + _timestamp.ToString("0.0000000000")
                + ", " + _elapsedTime.ToString("0.0000000000")
                + ", " + _numHops.ToString()
                + ", " + _totalDistance.ToString("0.00");
        }

        /*        | 
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Methods
    }

    class PEQDataInfoReport : IInfoReport
    {
        #region IReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        ILocation _location = null;
        public ILocation Loc
        {
            get { return _location; }
            set { _location = value; }
        }

        int _id;
        public int ID
        {
            get { return _id; }
        }

        double _timestamp;
        public double Time
        {
            get { return _timestamp; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReport Variables

        #region IInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        string _key = "PEQ_Data";
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        string _label = "PEQ_Data:";
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public String CSVHeader
        {
            get
            {
                return "Data ID, Timestamp, Sent, Received";
            }
        }

        bool _isSummable = true;
        public bool IsSummable
        {
            get { return _isSummable; }
        }

        bool _isWritten = false;
        public bool IsWritten
        {
            get { return _isWritten; }
            set { _isWritten = value; }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Variables

        #region PEQInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public double _DataID;
        public int _Sent=0;     // usually 0 or 1
        public int _Received=0; // usually 0 or 1, greater if multiple receptions occur

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQInfoReport Variables

        #region PEQInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public PEQDataInfoReport(int id, double timestamp)
        { // Minimum required information to create report
            _id = id;
            _timestamp = timestamp;
        }

        public PEQDataInfoReport(int id, double timestamp,
            string key, string label)
        {
            _id = id;
            _timestamp = timestamp;
            _key = key;
            _label = label;

            // Values to be set manually...
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQInfoReport Methods

        #region IInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public IInfoReport Add(IInfoReport withTarget)
        {
            if (withTarget is PEQDataInfoReport)
            {
                PEQDataInfoReport target = (PEQDataInfoReport)withTarget;
                if (target._DataID != _DataID)
                    return null;
                
                int newID;
                double newTimestamp;
                ILocation newLocation;
                bool sameNode;
                if (withTarget.ID == _id)
                {
                    sameNode = true;
                    newID = _id;
                    newLocation = _location;
                }
                else
                {
                    sameNode = false;
                    newID = int.MaxValue;
                    newLocation = null;
                }

                if (withTarget.Time < _timestamp)
                    newTimestamp = withTarget.Time;
                else newTimestamp = _timestamp;

                PEQDataInfoReport output = new PEQDataInfoReport(newID, newTimestamp,
                    _key, _label);
                output._location = newLocation;
                output._DataID = _DataID;
                output._Received = _Received + target._Received;
                output._Sent = _Sent + target._Sent;
                return output;
            }
            else return null;
        }

        public IInfoReport Subtract(IInfoReport targetFromThisObject)
        { // Probably will be unused for this report type.
            if (targetFromThisObject is PEQDataInfoReport)
            {
                PEQDataInfoReport target = (PEQDataInfoReport)targetFromThisObject;
                if (target._DataID != _DataID)
                    return null;
                
                int newID;
                double newTimestamp;
                ILocation newLocation;
                bool sameNode = false;

                if (targetFromThisObject.ID == _id)
                {
                    sameNode = true;
                    newID = _id;
                    newLocation = _location;
                }
                else
                {
                    sameNode = false;
                    newID = int.MaxValue;
                    newLocation = null;
                }

                if (targetFromThisObject.Time > _timestamp)
                    newTimestamp = targetFromThisObject.Time;
                else newTimestamp = _timestamp;

                PEQDataInfoReport output = new PEQDataInfoReport(newID, newTimestamp,
                    _key, _label);
                output._location = newLocation;
                output._DataID = _DataID;
                output._Received = _Received - target._Received;
                output._Sent = _Sent - target._Sent;
                return output;
            }
            else return null;
        }

        public String ToValueString()
        {
            return null; // Not possible for this record type...
        }

        public override String ToString()
        {
            return "DataID " + _DataID.ToString() + " Time: "
                + _timestamp.ToString("0.0000000") + " "
                + _label + " (all sent/rcvd)"
                + "\n        # Sent: " + _Sent.ToString()
                + "\n    # Received: " + _Received.ToString();
        }

        public String ToCSV()
        {
            return _DataID.ToString()
                + ", " + _timestamp.ToString("0.0000000000")
                + ", " + _Sent.ToString()
                + ", " + _Received.ToString();
        }

        /*        | 
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Methods
    }
}
