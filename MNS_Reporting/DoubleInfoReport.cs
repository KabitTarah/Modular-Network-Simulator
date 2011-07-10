using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Location;

namespace MNS_Reporting
{
    public class DoubleInfoReport : IInfoReport
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

        string _key = "int_default";
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        string _label = "Int Default:";
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public String CSVHeader
        {
            get
            {
                return "Node ID, Timestamp, Label, Value";
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

        #region DoubleInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public double Value;

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion DoubleInfoReport Variables

        #region DoubleInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public DoubleInfoReport(int id, double timestamp)
        { // Minimum required information to create report
            _id = id;
            _timestamp = timestamp;
        }

        public DoubleInfoReport(int id, double timestamp,
            string key, string label, double value)
        {
            _id = id;
            _timestamp = timestamp;
            _key = key;
            _label = label;
            Value = value;
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion DoubleInfoReport Methods

        #region IInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public IInfoReport Add(IInfoReport withTarget)
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

            if (withTarget is DoubleInfoReport)
            {
                DoubleInfoReport target = (DoubleInfoReport)withTarget;
                DoubleInfoReport output = new DoubleInfoReport(newID, 
                    newTimestamp, _key, _label, Value + target.Value);
                output._location = newLocation;
                return output;
            }
            else return null;
        }

        public IInfoReport Subtract(IInfoReport targetFromThisObject)
        { // Value - targetFromThisObject.Value
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

            if (targetFromThisObject is DoubleInfoReport)
            {
                DoubleInfoReport target = (DoubleInfoReport)targetFromThisObject;
                DoubleInfoReport output = new DoubleInfoReport(newID, newTimestamp,
                    _key, _label, Value - target.Value);
                output._location = newLocation;
                return output;
            }
            else return null;
        }

        public String ToValueString()
        {
            return Value.ToString("0.0000000000");
        }

        public String ToString()
        {
            return "Node " + _id.ToString() + " Time: "
                + _timestamp.ToString("0.0000000") + " "
                + _label + " " + Value.ToString("0.0000000000");
        }

        public String ToCSV()
        {
            return _id.ToString()
                + ", " + _timestamp.ToString("0.0000000000")
                + ", " + _label
                + ", " + Value.ToString("0.0000000000");
        }

        /*        | 
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Methods
    }
}
