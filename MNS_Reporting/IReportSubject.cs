using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MNS_Reporting
{
    public interface IReportSubject
    {
        void Attach(IReportObserver observer);
        void Detach(IReportObserver observer);
        void Notify(IReport report);
    }
}
