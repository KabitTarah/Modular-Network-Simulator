using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;

namespace Location
{
    // This may require significant changes to fit in with the MNS (in place of its ILocation)

    public interface ILocation
    {
        // static List<PanelObj> setupPanel(); // This IS required.
        List<PanelObj> SetupPanel { get; }
        List<PanelObj> PanelObjs { get; set; }

        ILocation[] Field { get; }          // Two ILocations defining a rectangular field
        bool FieldSet { get; }              // Determines whether the Field has been defined
        double Aspect { get; }              // Aspect Ratio (X:Y)

        NormalLocation Normalize();         // Creates a NormalLocation on [0,1] using the point (location) and field
        bool SetField(ILocation[] field);   // Sets the field
        bool SetField(ILocation point1, ILocation point2);
        bool InField();                     // Determines whether the Location exists within the Field
        double FieldMultiplier(double field_x);   // Aspect ratios must match for this method to work. FieldSet must be true.
        double Distance(ILocation toLocation);
    }
}
