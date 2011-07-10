using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Location;
using ModularNetworkSimulator;
using MNS_Reporting;

namespace PEQ
{
    class PEQAggregateInfoReport : IInfoReport
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

        int _id = 0;
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

        string _key = "PEQ_Agg";
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        string _label = "PEQ_Aggregate:";
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public String CSVHeader
        {
            get
            {
                return "Node ID, # Searches Started";
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

        public double _numSearches;

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

        public PEQAggregateInfoReport(double timestamp)
        { // Minimum required information to create report
            _timestamp = timestamp;
        }

        public PEQAggregateInfoReport(double timestamp,
            string key, string label)
        {
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
            if (withTarget is PEQAggregateInfoReport)
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

                PEQAggregateInfoReport target = (PEQAggregateInfoReport)withTarget;
                PEQAggregateInfoReport output = new PEQAggregateInfoReport(newTimestamp,
                    _key, _label);
                output._location = newLocation;
                output._numSearches = this._numSearches + target._numSearches;
                return output;
            }
            else return null;
        }

        public IInfoReport Subtract(IInfoReport targetFromThisObject)
        { // Probably will be unused for this report type.
            if (targetFromThisObject is PEQAggregateInfoReport)
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

                PEQAggregateInfoReport target = (PEQAggregateInfoReport)targetFromThisObject;
                PEQAggregateInfoReport output = new PEQAggregateInfoReport(newTimestamp,
                    _key, _label);
                output._location = newLocation;
                output._numSearches = this._numSearches - target._numSearches;
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
                + _label
                + "\n    Searches Started: " + _numSearches.ToString();
        }

        public String ToCSV()
        {
            return _id.ToString()
                + ", " + _numSearches.ToString();
        }

        /*        | 
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IInfoReport Methods
    }

    public class SearchAggregator : IReportSubject
    {
        private int _numSearches = 0;
        private bool _sentReport = false;

        public void IncrementSearches()
        {
            _numSearches++;
        }

        public void EndSimulation()
        {
            if (_sentReport)
                return;

            PEQAggregateInfoReport report = new PEQAggregateInfoReport(1);
            report._numSearches = _numSearches;

            Notify(report);
            _sentReport = true;
        }

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

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReportSubject Methods
    }
}
