using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Location;

namespace MNS_Reporting
{
    public class StringInfoReport : IInfoReport
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

        string _key = "default";
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        string _label = "Default:";
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

        bool _isSummable = false;
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

        #region StringInfoReport Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public String Value;

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion StringInfoReport Variables
        
        #region StringInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public StringInfoReport(int id, double timestamp)
        { // Minimum required information to create report
            _id = id;
            _timestamp = timestamp;
        }

        public StringInfoReport(int id, double timestamp,
            string key, string label, string value)
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
        #endregion StringInfoReport Methods

        #region IInfoReport Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public IInfoReport Add(IInfoReport withTarget)
        {
            return null;
        }

        public IInfoReport Subtract(IInfoReport targetFromThisObject)
        {
            return null;
        }

        public String ToValueString()
        {
            return Value;
        }

        public String ToString()
        {
            return "Node " + _id.ToString() + " Time: "
                + _timestamp.ToString("0.0000000") + " " 
                + _label + " " + Value;
        }

        public String ToCSV()
        {
            return _id.ToString()
                + ", " + _timestamp.ToString("0.0000000000")
                + ", " + _label
                + ", " + Value;
        }

        /*        | 
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Methods
    }
}
