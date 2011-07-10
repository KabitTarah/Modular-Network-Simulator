using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Location;

namespace ModularNetworkSimulator
{
    public interface IFieldDefinition : ICloneable
    {
        Queue<ILocation> PointList { get; }

        bool IsIn(ILocation loc);
        bool AddPoint(ILocation loc);
        bool IsValid();

        void Convert(FieldDefinition square);
    }

    public struct FieldDefinition
    { // Deprecated form of IFieldDefinition for backward compatibility
        public ILocation InitialCorner, FinalCorner;
    }
}
