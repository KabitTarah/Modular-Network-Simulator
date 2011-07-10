using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UWA
{
    public struct SPL
    {
        public double Value;

        public SPL(double value)
        {
            this.Value = value;
        }

        public SPL(int value)
        {
            this.Value = (double)value;
        }

        public static SPL operator +(SPL u1, SPL u2)
        { // Addition adds pressure levels, then converts back to SPL
            return new SPL(10 * Math.Log10(Math.Pow(10, (u1.Value / 10))
                + Math.Pow(10, (u2.Value / 10))));
        }

        public static SPL operator -(SPL u1, SPL u2)
        { // Subtraction first converts, then subtracts (negate value after conversion)
            if (u1 > u2)
                return new SPL(10 * Math.Log10(Math.Pow(10, (u1.Value / 10))
                    - Math.Pow(10, (u2.Value / 10))));
            else
                return (SPL)double.NegativeInfinity;
        }

        public static SPL operator -(SPL u1)
        { // Negation works the same way...
            return new SPL(-u1.Value);
        }

        public static SPL operator *(SPL u1, SPL u2)
        { // To directly add two SPLs
            return new SPL(u1.Value + u2.Value);
        }

        public static SPL operator /(SPL u1, SPL u2)
        { // To directly subtract one SPL from another
            return new SPL(u1.Value - u2.Value);
        }

        public static implicit operator SPL(double doubleValue)
        {
            return new SPL(doubleValue);
        }

        public static implicit operator double(SPL u1)
        {
            return u1.Value;
        }

        public static implicit operator SPL(int intValue)
        {
            return new SPL(intValue);
        }

        public static explicit operator int(SPL u1)
        {
            return (int)u1.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
