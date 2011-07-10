using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriorityQueue;

namespace MNS_Reporting
{
    public class Reporter : IReportObserver
    {
        #region IReportObserver Variables

        bool enableGraphics = true;
        public bool EnableGraphics
        {
            get { return enableGraphics; }
            set { enableGraphics = value; }
        }

        #endregion IReportObserver Variables

        #region VARIABLES
        double secPerTick;              // Set upon construction. This tells the Subject when to send
        public Double SecPerTick        // periodic Reports to the Reporter. (Based in SimTime).  
        {
            get { return secPerTick; }
        }

        // reportDB is the collection where the reports are held until the viewer requests them
        PriorityQueue<IReport, double> reportDB = new PriorityQueue<IReport, double>();
        PriorityQueue<IReport, double> reverseDB = new PriorityQueue<IReport, double>();
        #endregion VARIABLES

        #region CONSTRUCTOR(S)
        public Reporter(double secPerTick)
        {
            this.secPerTick = secPerTick;
        }
        #endregion CONSTRUCTOR(S)

        #region IReportObserver METHOD(S)
        public void Update(IReport Report)
        { // There's not much to this method. Enqueue the Reports provided by the ISubject
            if (enableGraphics || !(Report is IGraphicalReport))
                reportDB.Enqueue(Report, -Report.Time);
        }
        #endregion IReportObserver METHOD(S)

        #region VIEWER INTERACTION METHOD(S)

        public IReport GetPrev(int Tick)
        { // Exact reverse of GetNext.
            PriorityQueueItem<IReport, double> item;
            if (reverseDB.Count > 0)
            {       // Priority is now positive. Tick is becoming smaller.
                if (reverseDB.Peek().Priority >= Tick * secPerTick)
                {
                    item = reverseDB.Dequeue();
                    reportDB.Enqueue(item.Value, -item.Priority);
                    return item.Value;
                }
                else return null;
            }
            else return new EmptyReport();
        }

        public IReport GetNext(int Tick)
        { // When provided a Tick time (discrete frame), this provides the next report occurring in that time.
            PriorityQueueItem<IReport, double> item;
            if (reportDB.Count > 0)
            {
                if (reportDB.Peek().Priority >= -Tick * secPerTick)
                {
                    // Dequeue and move to reverse queue
                    item = reportDB.Dequeue();
                    reverseDB.Enqueue(item.Value, -item.Priority);
                    return item.Value;    // Just provide the Value -- We don't care about the Priority.
                }
                else return null;         // Return a null when all reports have been doled out!
            }
            else return new EmptyReport();
        }
        #endregion VIEWER INTERACTION METHOD(S)

        #region METHODS
        public double NextTick(double time)
        { // Returns the next Tick time given an input time
            return ((Math.Round(time / secPerTick, 0) + 1) * secPerTick);
        }

        public int TickNumber(double time)
        { // Returns the Tick ordinal given an input time
            return (int)(Math.Round(time / secPerTick, 0));
        }
        #endregion METHODS
    }
}
