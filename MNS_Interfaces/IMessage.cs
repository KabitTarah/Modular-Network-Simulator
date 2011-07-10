using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public interface IMessage : ICloneable
    {
        int SizeBytes();
    }
}
