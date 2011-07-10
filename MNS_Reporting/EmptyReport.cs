using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Location;

namespace MNS_Reporting
{
    public class EmptyReport : IReport
    {
        public bool IsStatic { get { return true; } }

        IReport previousStatic = null;
        public IReport PreviousStatic
        {
            get { return previousStatic; }
            set { previousStatic = value; }
        }

        Action reportAction = Action.Stop;
        public Action ReportAction
        {
            get { return reportAction; }
            set { reportAction = value; }
        }
        
        public ILocation Loc { get { return null; } }
        public int ID { get { return 0; } }
        public Double Time { get { return 0; } }

        public DrawLayer Layer { get { return DrawLayer.Static; } set { } }

        public EmptyReport() { }
    }
}
