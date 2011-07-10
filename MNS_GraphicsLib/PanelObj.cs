using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using ModularNetworkSimulator;

namespace MNS_GraphicsLib
{
    public enum FormType { label,   // Has no "value"
                          intBox,   // Has no "text"
                        floatBox,   // Has no "text"
                       doubleBox,   // Has no "text"
                         //textBox,   // Has no "text"  ADD LATER
                         check };   // Has both "value" and "text"
    
    public class PanelObj
    {
        public string name;    // Name to identify the variable
        public FormType type;
        public string text;    // Text value to label the object. 
        public string value;   // Value (converted to string. Use Parse to recover).
        public int width;      // width of panel item. This may require some play.
        public int xSlot;      // slot in the x-position (valid 0-1) -- (more possible if extended panel used)
        public int ySlot;      // slot in the y-position (valid 0-16)
        public System.Windows.Forms.Control obj;      // contains the graphics object pointer.

        public PanelObj() { }

        public void UpdateGraphics()
        {
            if (type != FormType.check)
                obj.Text = value;
        }

        public void UpdateInfo()
        {
            text = obj.Text;

            if (type == FormType.check)
            {
                System.Windows.Forms.CheckBox item = (System.Windows.Forms.CheckBox)obj;
                value = item.Checked.ToString();
            }
            else
            {
                value = obj.Text;
            }
        }
    }

    public class PanelObjHelper
    {
        List<PanelObj> pobjList;

        public PanelObjHelper(List<PanelObj> PanelObjList)
        {
            pobjList = PanelObjList;
        }

        public void UpdateGraphics()
        {
            foreach (PanelObj obj in pobjList)
                obj.UpdateGraphics();
        }

        public void UpdateInfo()
        {
            foreach (PanelObj obj in pobjList)
                obj.UpdateInfo();
        }

        PanelObj getPanelObjByName(string name)
        {
            foreach (PanelObj p in pobjList)
            {
                if (p.name == name)
                    return p;
            }
            return null;
        }

        public void SetByName(string name, string value)
        {
            PanelObj p = getPanelObjByName(name);
            p.value = value;
            p.UpdateGraphics();
        }

        public void SetByName(string name, double value)
        {
            String strValue = value.ToString();
            SetByName(name, strValue);
        }

        public void SetByName(string name, float value)
        {
            String strValue = value.ToString();
            SetByName(name, strValue);
        }

        public void SetByName(string name, int value)
        {
            String strValue = value.ToString();
            SetByName(name, strValue);
        }

        public void SetByName(string name, bool value)
        {
            String strValue;
            if (value)
                strValue = "1.0";
            else
                strValue = "0.0";
            SetByName(name, strValue);
        }

        public double GetDoubleByName(string name)
        {
            double d;
            PanelObj p = getPanelObjByName(name);
            if (p == null)
                throw new Exception("Name Not Found");

            try
            {
                d = double.Parse(p.value);
            }
            catch (Exception ex)
            {
                throw new Exception("Parse Error: Not Double");
            }
            return d;
        }

        public float GetFloatByName(string name)
        {
            float f;
            PanelObj p = getPanelObjByName(name);
            if (p==null)
                throw new Exception("Name Not Found");
            try
            {
                f = float.Parse(p.value);
            }
            catch (Exception ex)
            {
                throw new Exception("Parse Error: Not Float");
            }
            return f;
        }

        public int GetIntByName(string name)
        {
            int i;
            PanelObj p = getPanelObjByName(name);
            if (p==null)
                throw new Exception("Name Not Found");
            try
            {
                i = int.Parse(p.value);
            }
            catch (Exception ex)
            {
                throw new Exception("Parse Error: Not Int");
            }
            return i;
        }

        public bool GetBoolByName(string name)
        {
            bool b;
            PanelObj p = getPanelObjByName(name);
            if (p == null)
                throw new Exception("Name Not Found");
            try
            {
                b = bool.Parse(p.value);
            }
            catch (Exception ex)
            {
                throw new Exception("Parse Error: Not Boolean");
            }
            return b;
        }

        public string GetStringValueByName(string name)
        {
            PanelObj p = getPanelObjByName(name);
            if (p==null)
                throw new Exception("Name Not Found");
            return p.value;
        }

        public string GetStringTextByName(string name)
        {
            foreach (PanelObj p in pobjList)
            {
                if (p.name == name)
                    return p.text;
            }
            throw new Exception("Name Not Found");
        }
    }
}