using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MNS_Reporting
{
    public interface IInfoReport : IReport
    {
        String Key { get; set; }        // To match like types
        String Label { get; set; }      // For printing
        String CSVHeader { get; }       // First line of CSV output
        bool IsSummable { get; }        // Is it numeric (summable)?
        bool IsWritten { get; set; }    // Has this report already been printed?

        IInfoReport Add(IInfoReport withTarget);
        IInfoReport Subtract(IInfoReport targetFromThisObject);
        String ToValueString(); // Just the value --> String
        String ToString();      // Entire report --> Textual String
        String ToCSV();         // To Comma Separated Value
    }
}
