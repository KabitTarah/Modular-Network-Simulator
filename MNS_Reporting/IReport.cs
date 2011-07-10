using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Location;

namespace MNS_Reporting
{
    public interface IReport
    {
        ILocation Loc { get; }
        int ID { get; }
        Double Time { get; }
    }
}
