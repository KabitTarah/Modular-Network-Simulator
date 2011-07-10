using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public interface IRandomizer
    {
        Int32 Next();
        Int32 Next(Int32 maxValue);
        Int32 Next(Int32 minValue, Int32 maxValue);
        Double NextDouble();

        Double NextGaussian();
        Double NextGaussian(Double mean, Double variance);
    }

}
