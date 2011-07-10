using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_Reporting;
using MNS_GraphicsLib;
using Location;

namespace ModularNetworkSimulator
{
    public enum ReportLevel {Routine, Priority, Immediate, Flash, FlashOverride};

    public class ReporterIWF : IReportSubject
    {
        double maxBitDistance;              // meters
        public double MaxBitDistance  
        {
            get { return maxBitDistance; }
            set
            {
                maxBitDistance = value;
                maxBitTime = maxBitDistance / PropagationSpeed;
            }
        }

        double maxBitTime;
        public double MaxBitTime  
        {
            get { return maxBitTime; }
            set
            {
                maxBitTime = value;
                maxBitDistance = maxBitTime * PropagationSpeed;
            }
        }

        public double TimeScale;
        public double TransmissionSpeed = 0;    // bits / second
        public double PropagationSpeed = 0;     // meters / second

        public ReporterIWF(double TimeScale, double TransmissionSpeed, 
            double PropagationSpeed)
        {
            this.TimeScale = TimeScale;
            this.TransmissionSpeed = TransmissionSpeed;
            this.PropagationSpeed = PropagationSpeed;
        }

        public void SendWaveReport(double startTime, double messageSize, 
            ReportLevel messageLevel, INode RefNode)
        {
            AnnulusReport annReport;
            double size;
            double width;
            double currentTime = Math.Ceiling(startTime / TimeScale) * TimeScale;
            while (currentTime
                < startTime + maxBitTime + messageSize / TransmissionSpeed)
            {
                size = 2 * ((currentTime - startTime) 
                     * PropagationSpeed + RefNode.NodeSize / 2);
                if ((currentTime - startTime) * TransmissionSpeed < messageSize)
                    width = (currentTime - startTime) * PropagationSpeed;
                else width = messageSize / TransmissionSpeed * PropagationSpeed;

                annReport = new AnnulusReport(currentTime, RefNode.Location, 
                    (float)size, (float)width, RefNode.GradientPath);
                annReport.MaximumMessageSize = maxBitDistance;
                switch (messageLevel)
                {
                    case ReportLevel.Routine:
                        annReport.Color = ColorEnum.DarkBackgroundColor;
                        break;
                    case ReportLevel.Priority:
                        annReport.Color = ColorEnum.Priority;
                        break;
                    case ReportLevel.Immediate:
                        annReport.Color = ColorEnum.Immediate;
                        break;
                    case ReportLevel.FlashOverride:
                        annReport.Color = ColorEnum.FlashOverride;
                        break;
                    case ReportLevel.Flash:
                        annReport.Color = ColorEnum.Flash;
                        break;
                }
                annReport.FinalColor = ColorEnum.Transparent;
                Notify(annReport);
                currentTime = currentTime + TimeScale;
            }
        }

        public void SendWaveReport(double startTime, double messageSize,
            INode RefNode)
        {
            SendWaveReport(startTime, messageSize, ReportLevel.Routine, RefNode);
        }

        public void SendMessageReport(double startTime, double messageSize, 
            ReportLevel messageLevel, INode RefNode, INode DestNode)
        {
            if (RefNode == DestNode)
                return;
            if (!(RefNode.Location is XYDoubleLocation))
                return;
            if (!(DestNode.Location is XYDoubleLocation))
                return;

            ColorEnum color = ColorEnum.FlashOverride;
            switch (messageLevel)
            {
                case ReportLevel.Routine:
                    color = ColorEnum.Routine;
                    break;
                case ReportLevel.Priority:
                    color = ColorEnum.Priority;
                    break;
                case ReportLevel.Immediate:
                    color = ColorEnum.Immediate;
                    break;
                case ReportLevel.Flash:
                    color = ColorEnum.Flash;
                    break;
                case ReportLevel.FlashOverride:
                    color = ColorEnum.FlashOverride;
                    break;
            }

            double size;
            double width;
            double currentTime = Math.Ceiling(startTime / TimeScale) * TimeScale;
            while (currentTime < startTime + maxBitTime + messageSize / TransmissionSpeed)
            {
                double dist = RefNode.Location.Distance(DestNode.Location);
                if (dist > maxBitDistance)
                    return;
                
                size = 2 * ((currentTime - startTime) 
                     * PropagationSpeed + RefNode.NodeSize / 2);
                if ((currentTime - startTime) * TransmissionSpeed < messageSize)
                    width = (currentTime - startTime) * PropagationSpeed;
                else width = messageSize / TransmissionSpeed * PropagationSpeed;

                LineReport lineReport = new LineReport(currentTime, (XYDoubleLocation)RefNode.Location,
                    (XYDoubleLocation)DestNode.Location, size / 2.0d, width);
                lineReport.MaximumMessageSize = maxBitDistance;
                lineReport.Color = color;
                lineReport.FinalColor = color;
                lineReport.Layer = DrawLayer.Foreground;
                
                Notify(lineReport);

                currentTime = currentTime + TimeScale;
            }
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
        #endregion IReportSubject METHODS
    }
}
