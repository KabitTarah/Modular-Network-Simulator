using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MNS_Reporting
{
    public interface IReportObserver
    {
        void Update(IReport Report);        // This deviates from GoF!
    }
}
