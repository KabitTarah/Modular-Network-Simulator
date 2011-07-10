using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using Location;
using ModularNetworkSimulator;
using MNS_GraphicsLib;
using MNS_Reporting;
using StopWatch;

namespace MNS_Visualizer
{
    public partial class Form1 : Form
    {
        #region FORM GLOBAL VARIABLES
        /*    // \\    */
        /*   //   \\   */
        /*  //     \\  */

        Color fieldColor;                   // Stores the base Panel field color. This is taken from the FORM default.
        int PanelMaxWidth, PanelMaxHeight;  // Records the maximum width and height of the drawing Panel.
        int LabelInfo_MaxSize;              // Maximum size of the label_Info WPF object

        ColorEnumStatics colorDef = new ColorEnumStatics(); // Just some useful functions for custom colors

        Graphics gfxStaticBuffer;           // Points to bufferStatic
        Graphics gfxBackBuffer;             // Points to bufferBackground
        Graphics gfxForeBuffer;             // Points to bufferForeground
        Graphics gfxFlatBuffer;             // Points to bufferFlattened
        Graphics gfxDraw;                   // Points to Panel

        private Bitmap bufferStatic;        // Bitmap will hold static (non-moving) objects and seldom updated.
        private Bitmap bufferBackground;    // Bitmap will hold drawn (moving/changing) objects in the background. 
        // Bitmap staticBuffer will be copied over drawBuffer to clear.
        private Bitmap bufferForeground;    // Bitmap will hold drawn (moving/changing) objects in the foreground.
        private Bitmap bufferFlattened;     // Bitmap will hold image before drawn to screen

        Timer timerDraw = new Timer();      // Draws the screen every 1/FPS (Frame rate)

        XYIntLocation[] panelDrawField = new XYIntLocation[2];

        List<IReport> staticQueue = new List<IReport>();  // Stores all static reports for redraw as required.

        // SIM Setup
        bool enableRun = true;
        bool isSimRunning = false;
        bool isSimPaused = false;
        Reporter reporter;
        bool firstResize = false;
        int currTimeTick;
        double secPerTick;
        StopWatch.StopWatch sw = new StopWatch.StopWatch();

        // IO
        Dictionary<string, StreamWriter> _tableIO = new Dictionary<string, StreamWriter>();

        /*  \\     //  */
        /*   \\   //   */
        /*    \\ //    */
        #endregion FORM GLOBAL VARIABLES
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

        #region FORM SET-UP / TEAR-DOWN
        /*    // \\    */
        /*   //   \\   */
        /*  //     \\  */

        public Form1()
        {
            InitializeComponent();          // Sets up the FORM. (DEFAULT -- All customization occurs AFTER this method)
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            this.cb_IApplicationEventGenerator.BackColorChanged += new EventHandler(cb_IApplicationEventGenerator_BackColorChanged);
            this.cb_IDeployer.BackColorChanged += new EventHandler(cb_IDeployer_BackColorChanged);
            this.cb_ILocation.BackColorChanged += new EventHandler(cb_ILocation_BackColorChanged);
            //this.cb_INode.BackColorChanged += new EventHandler(cb_INode_BackColorChanged);
            this.cb_IPhysicalProcessor.BackColorChanged += new EventHandler(cb_IPhysicalProcessor_BackColorChanged);
            this.cb_IRandomizerFactory.BackColorChanged += new EventHandler(cb_IRandomizerFactory_BackColorChanged);

            this.btn_Stop_Sim.Click += new System.EventHandler(this.btn_Stop_Sim_Click);

            this.text_JumpSec.KeyDown += new KeyEventHandler(text_JumpSec_KeyDown);

            //this.btn_Save_Sim.MouseEnter += new System.EventHandler(this.toolTipBar_On);
            //this.btn_Save_Sim.MouseLeave += new System.EventHandler(this.toolTipBar_Off);

            this.Panel.SizeChanged += new EventHandler(Panel_SizeChanged);  // When the Panel size changes, call this method

            fieldColor = Color.FromArgb(232, 247, 249);
            colorDef.BackgroundColor = fieldColor;
            colorDef.DarkBackgroundColor = Color.FromArgb(136, 144, 145);
            colorDef.Node = Color.FromArgb(0, 0, 255);
            PanelMaxWidth = Panel.Width;                                // Store the Panel Width
            PanelMaxHeight = Panel.Height;                              // Store the Panel Height
            LabelInfo_MaxSize = label_Info.Text.Length;                 // Store the maximum size of the info label
            label_Info.Text = "";                                       // Clear the text of the info label

            bufferStatic = new Bitmap(Panel.Width, Panel.Height);       // Set up the Static Buffer (backmost layer)
            bufferBackground = new Bitmap(Panel.Width, Panel.Height);   // Set up the Background Buffer (middle layer)
            bufferForeground = new Bitmap(Panel.Width, Panel.Height);   // Set up the Foreground Buffer (top layer)
            bufferFlattened = new Bitmap(Panel.Width, Panel.Height);    // Set up the Flattened Buffer

            gfxStaticBuffer = Graphics.FromImage(bufferStatic);         // attach the Graphics objects to their 
            gfxBackBuffer = Graphics.FromImage(bufferBackground);       // corresponding buffers
            gfxForeBuffer = Graphics.FromImage(bufferForeground);       // ...
            gfxFlatBuffer = Graphics.FromImage(bufferFlattened);        // ...
            gfxDraw = this.Panel.CreateGraphics();                      // attach this Graphics object to the Panel

            gfxBackBuffer.CompositingMode = CompositingMode.SourceOver;
            gfxBackBuffer.CompositingQuality = CompositingQuality.HighQuality;
            gfxBackBuffer.InterpolationMode = InterpolationMode.HighQualityBicubic;

            gfxBackBuffer.Clear(fieldColor);                            // Clear the Background Buffer to the Field Color
            gfxStaticBuffer.Clear(Color.Transparent);                   // Clear the other Buffers to a Transparent
            gfxForeBuffer.Clear(Color.Transparent);                     // color, allowing lower layers to appear through

            timerDraw.Tick += new EventHandler(DrawBuffer);             // Set the timer to call DrawBuffer every Tick
            timerDraw.Interval = (int)(1000f / int.Parse(FPS.Text));    // Set the timer interval (in ms)
            FPS.Enabled = false;            // Disable the FPS text box on the form (default 15 FPS)
            labelFPS.Enabled = false;       // Disable the FPS label on the form
            timerDraw.Start();              // Start the timer

            // Set output folder
            text_IOFolder.Text =
                Path.GetDirectoryName(Application.ExecutablePath)
                + "\\output";
            if (!System.IO.Directory.Exists(text_IOFolder.Text))
                System.IO.Directory.CreateDirectory(text_IOFolder.Text);

            addLocalDLLs();

            Panel.Click += new EventHandler(Panel_Click);
        }

        void Panel_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseEve = (MouseEventArgs)e;
            Point loc = mouseEve.Location;
            //loc.Offset(-this.Bounds.Location.X, -this.Bounds.Location.Y);
            //loc.Offset(-Panel.Location.X, -Panel.Location.Y);

            IReport report = null;
            double distance = double.MaxValue;
            foreach (IReport currentReport in staticQueue)
            {
                double currentDistance = currentReport.Loc.Distance(new XYIntLocation(loc.X, loc.Y));
                if (currentDistance < distance)
                {
                    report = currentReport;
                    distance = currentDistance;
                }
            }

            if (report != null)
            {
                textClick_ID.Text = report.ID.ToString();
            }
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (StreamWriter sw in _tableIO.Values)
                sw.Close();
        }

        /*  \\     //  */
        /*   \\   //   */
        /*    \\ //    */
        #endregion FORM SET-UP / TEAR-DOWN

        #region PANEL SIZE CHANGES
        /*    // \\    */
        /*   //   \\   */
        /*  //     \\  */

        private void ResizePanel(int Width, int Height)
        { // Set Panel dimensions & clean up
            Panel.Width = Width;                                    // Set Panel Width
            Panel.Height = Height;                                  // Set Panel Height

            bufferStatic = new Bitmap(Width, Height);               // Reset Static Buffer
            bufferBackground = new Bitmap(Width, Height);           // Reset Background Buffer
            bufferForeground = new Bitmap(Width, Height);           // Reset Foreground Buffer

            gfxStaticBuffer = Graphics.FromImage(bufferStatic);     // Reattach Buffers to their respective 
            gfxBackBuffer = Graphics.FromImage(bufferBackground);   // Graphics objects
            gfxForeBuffer = Graphics.FromImage(bufferForeground);   // ... 

            gfxBackBuffer.Clear(fieldColor);                        // Clear Background Buffer to default color.
            gfxStaticBuffer.Clear(Color.Transparent);               // Make Static & Foreground
            gfxForeBuffer.Clear(Color.Transparent);                 // buffers transparent.
        }

        private void Panel_SizeChanged(object sender, EventArgs e)
        { // Every time the panel's size changes, this occurs...
            int x = Panel.Width; int y = Panel.Height;                   // get width & height
            //PanelX.Text = x.ToString(); PanelY.Text = y.ToString();      // Set width/height debug fields
            //PanelAspect.Text = ((float)x / (float)y).ToString("0.0000"); // Set Aspect Ration debug field

            panelDrawField[0] = new XYIntLocation(0, 0);                 // Set Field for new Panel size
            panelDrawField[1] = new XYIntLocation(x, y);

            this.Panel.Invalidate();                                     // Invalidate Panel (force redraw)
        }

        private void btn_ResizetabControl_Click(object sender, EventArgs e)
        {
            if (btn_ResizetabControl.Text == "ç")
                btn_ResizetabControl.Text = "è";
            else btn_ResizetabControl.Text = "ç";
            ResizeTabControl();
        }

        int tabControlMinWidth = 246;
        int tabControlMaxWidth = 725;
        private void ResizeTabControl()
        { ResizeTabControl(false); }
        private void ResizeTabControl(bool forceSmall)
        {
            int tabControlDiffWidth = tabControlMaxWidth - tabControlMinWidth;
            if ((tab_INode.Width == tabControlMinWidth) && !forceSmall)
            {
                tab_INode.Width = tabControlMaxWidth;
                tab_INode.Location = new Point(tab_INode.Location.X - tabControlDiffWidth, 
                    tab_INode.Location.Y);
                // Increase component widths (new components must be added here)
                lb_DLLs.Width = lb_DLLs.Width + tabControlDiffWidth;
                cb_IApplicationEventGenerator.Width = cb_IApplicationEventGenerator.Width 
                    + tabControlDiffWidth;
                cb_IDeployer.Width = cb_IDeployer.Width + tabControlDiffWidth;
                cb_ILocation.Width = cb_ILocation.Width + tabControlDiffWidth;
                cb_INode.Width = cb_INode.Width + tabControlDiffWidth;
                cb_InvertScale.Width = cb_InvertScale.Width + tabControlDiffWidth;
                cb_IPhysicalProcessor.Width = cb_IPhysicalProcessor.Width + tabControlDiffWidth;
                cb_IRandomizerFactory.Width = cb_IRandomizerFactory.Width + tabControlDiffWidth;
            }
            else
            {
                tab_INode.Width = tabControlMinWidth;
                tab_INode.Location = new Point(tab_INode.Location.X + tabControlDiffWidth, 
                    tab_INode.Location.Y);
                // Decrease component widths (new components must be added here)
                lb_DLLs.Width = lb_DLLs.Width - tabControlDiffWidth;
                cb_IApplicationEventGenerator.Width = cb_IApplicationEventGenerator.Width 
                    - tabControlDiffWidth;
                cb_IDeployer.Width = cb_IDeployer.Width - tabControlDiffWidth;
                cb_ILocation.Width = cb_ILocation.Width - tabControlDiffWidth;
                cb_INode.Width = cb_INode.Width - tabControlDiffWidth;
                cb_InvertScale.Width = cb_InvertScale.Width - tabControlDiffWidth;
                cb_IPhysicalProcessor.Width = cb_IPhysicalProcessor.Width - tabControlDiffWidth;
                cb_IRandomizerFactory.Width = cb_IRandomizerFactory.Width - tabControlDiffWidth;
            }
        }

        /*  \\     //  */
        /*   \\   //   */
        /*    \\ //    */
        #endregion PANEL SIZE CHANGES

        #region FORM DRAW / TOGGLE DRAW
        /*    // \\    */
        /*   //   \\   */
        /*  //     \\  */

        private void DrawBuffer(Object o, EventArgs eve)
        { // Gets called every (1/FPS) seconds
            gfxFlatBuffer.DrawImage(bufferBackground, new Point(0, 0));   // Apply Background buffer to Flattened buffer
            gfxFlatBuffer.DrawImage(bufferStatic, new Point(0, 0));       // Apply Static buffer to Flattened buffer
            gfxFlatBuffer.DrawImage(bufferForeground, new Point(0, 0));   // Apply Foreground buffer to Flattened buffer
            gfxDraw.DrawImage(bufferFlattened, new Point(0, 0));          // Apply Flattened buffer to Panel

            if (!isSimPaused)
            {
                gfxBackBuffer.Clear(fieldColor);                          // Clear Background & Foreground
                gfxForeBuffer.Clear(Color.Transparent);                   // buffers to Transparent.
            }

            if (((currTimeTick - resetTimeTick) * secPerTick > 1) && !cb_GraphicsOff.Checked)
            {
                long sw_time = sw.Peek();
                float simTime = (float)(sw_time / 10000f);
                labelTimeFactor.Text = simTime.ToString("0.0");
            }
        }

        private void btnToggleGraphics_Click(object sender, EventArgs e)
        { // Occurs when "btnToggleGraphics" button is clicked ("Stop Graphics" / "Start Graphics" button)
            switch (btnToggleGraphics.Text) // Check the button text
            {
                case "Start Graphics":      // If the text is "Start Graphics" ...
                    if (FPS.BackColor == Color.Yellow)
                        break;
                    timerDraw.Interval = (int)(1000f / int.Parse(FPS.Text));    // get the FPS setting 
                    // & set at timer interval
                    FPS.Enabled = false;                        // Disable FPS text box
                    labelFPS.Enabled = false;                   // Disable FPS label
                    btnToggleGraphics.Text = "Stop Graphics";   // Change button name
                    timerDraw.Start();                          // Start timer
                    selected_GraphicsRunning.Text = "Graphics: Running";
                    selected_GraphicsRunning.ForeColor = Color.Green;
                    break;
                case "Stop Graphics":       // If the text is "Stop Graphics" ...
                    timerDraw.Stop();                           // Stop the timer
                    FPS.Enabled = true;                         // Enable the FPS text box
                    labelFPS.Enabled = true;                    // Enable the FPS label
                    btnToggleGraphics.Text = "Start Graphics";  // Change button name
                    selected_GraphicsRunning.Text = "Graphics: Stopped";
                    selected_GraphicsRunning.ForeColor = Color.Red;
                    break;
            }
        }

        private void btn_Pause_Sim_Click(object sender, EventArgs e)
        {
            if (isSimRunning)
            {
                isSimPaused = true;
                // Change from Pause to Play icon
                this.btn_Pause_Sim.Image = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("MNS_Visualizer._icons.play.png"));
                System.Threading.Thread.Sleep(50);

                timerDraw.Tick -= new EventHandler(runVisualizer);
                btn_Pause_Sim.Click -= new EventHandler(btn_Pause_Sim_Click);
                btn_Pause_Sim.Click += new EventHandler(btn_Play_Sim_Click);
                btn_Pause_Sim.Tag = "play";
            }
            else
            {
                this.btn_Pause_Sim.Image = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("MNS_Visualizer._icons.play.png"));
                btn_Pause_Sim.Click -= new EventHandler(btn_Pause_Sim_Click);
                btn_Pause_Sim.Click += new EventHandler(btn_Run_Sim_Click);
                btn_Pause_Sim.Tag = "run";
            }
        }

        private void btn_Run_Sim_Click(object sender, EventArgs e)
        {
            this.btn_Pause_Sim.Image = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("MNS_Visualizer._icons.pause.png"));
            btn_Pause_Sim.Click -= new EventHandler(btn_Run_Sim_Click);
            btn_Pause_Sim.Click +=new EventHandler(btn_Pause_Sim_Click);
            btn_Pause_Sim.Tag = "pause";

            this.tab_INode.SelectTab(this.tab_Run);
            btn_RunSim_Click(sender, e);
        }

        private void btn_Play_Sim_Click(object sender, EventArgs e)
        {
            if (isSimRunning)
            {
                isSimPaused = false;
                // Change from Play to Pause icon
                this.btn_Pause_Sim.Image = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("MNS_Visualizer._icons.pause.png"));
                System.Threading.Thread.Sleep(50);

                timerDraw.Tick += new EventHandler(runVisualizer);
                btn_Pause_Sim.Click -= new EventHandler(btn_Play_Sim_Click);
                btn_Pause_Sim.Click += new EventHandler(btn_Pause_Sim_Click);
                btn_Pause_Sim.Tag = "pause";
            }
            else
            {
                this.btn_Pause_Sim.Image = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("MNS_Visualizer._icons.play.png"));
                btn_Pause_Sim.Click -= new EventHandler(btn_Play_Sim_Click);
                btn_Pause_Sim.Click += new EventHandler(btn_Run_Sim_Click);
                btn_Pause_Sim.Tag = "run";
            }
        }

        private void btn_Stop_Sim_Click(object sender, EventArgs e)
        {
            if (isSimRunning)
            {
                if (!(e is EventArgsMessage))
                    if (!(MessageBox.Show("Do you really want to stop this simulation?",
                        "STOP SIM?", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        return;

                run = int.MaxValue - 1000;

                timerDraw.Tick -= new EventHandler(runVisualizer);
                enableConfiguration();
                enableConfigurationMultirun();
                lock (reporter)
                    reporter = null;
                lock (_tableIO)
                {
                    foreach (StreamWriter sw in _tableIO.Values)
                        sw.Close();
                    _tableIO = new Dictionary<string, StreamWriter>();
                }
                if (btnToggleGraphics.Text == "Start Graphics")
                    btnToggleGraphics_Click(sender, e);
                isSimRunning = false;
                firstResize = false;
                staticQueue.Clear();
                gfxBackBuffer.Clear(fieldColor);
                gfxStaticBuffer.Clear(Color.Transparent);
                gfxForeBuffer.Clear(Color.Transparent);
                if (e is EventArgsMessage)
                {
                    EventArgsMessage args = (EventArgsMessage)e;
                    this.label_Info.Text = args.Message;
                }
                else
                    this.label_Info.Text = "Simulation Stopped!";

                this.btn_Pause_Sim.Image = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("MNS_Visualizer._icons.play.png"));
                if ((string)btn_Pause_Sim.Tag == "pause")
                    btn_Pause_Sim.Click -= new EventHandler(btn_Pause_Sim_Click);
                else if ((string)btn_Pause_Sim.Tag == "play")
                    btn_Pause_Sim.Click -= new EventHandler(btn_Play_Sim_Click);
                else if ((string)btn_Pause_Sim.Tag == "run")
                    btn_Pause_Sim.Click -= new EventHandler(btn_Run_Sim_Click);
                btn_Pause_Sim.Click += new EventHandler(btn_Run_Sim_Click);
            }
        }

        /*  \\     //  */
        /*   \\   //   */
        /*    \\ //    */
        #endregion FORM DRAW / TOGGLE DRAW

        #region REPORT HANDLING
        /*    // \\    */
        /*   //   \\   */
        /*  //     \\  */

        private void ProcessStaticReport(IGraphicalReport report)
        {
            // Forward Direction...
            if (report.ReportAction != MNS_Reporting.Action.Start)
                staticQueue.Remove(report.PreviousStatic);
            if (report.ReportAction != MNS_Reporting.Action.Stop)
                staticQueue.Add(report);
        }

        private void ProcessStaticReport(IGraphicalReport report, bool TimeForward)
        {
            if (TimeForward)
            {
                ProcessStaticReport(report);
            }
            else
            {
                if (report.ReportAction != MNS_Reporting.Action.Stop)
                    staticQueue.Remove(report);
                if (report.ReportAction != MNS_Reporting.Action.Start)
                    staticQueue.Add(report.PreviousStatic);
            }
        }

        private void RedrawStatics()
        {
            gfxStaticBuffer.Clear(Color.Transparent);
            IGraphicalReport graphicalReport;
            foreach (IReport rep in staticQueue)
            {
                if (rep is IGraphicalReport)
                {
                    graphicalReport = (IGraphicalReport)rep;
                    graphicalReport.Draw(gfxStaticBuffer, colorDef);
                }
            }
        }

        double simAspect, visAspect;
        double fieldMultiplier;
        private void setAspect(IReport report)
        {
            Panel.Width = PanelMaxWidth;
            Panel.Height = PanelMaxHeight;

            simAspect = report.Loc.Aspect;
            int x = Panel.Width; int y = Panel.Height;  // Get the Panel Width / Height (locally)
            if ((double)x / (double)y > simAspect)      // If Panel Aspect is bigger than Field Aspect,
                ResizePanel((int)(y * simAspect), y);   // Resize the panel Width
            else if ((double)x / (double)y < simAspect) // else If Panel Aspect is less than Field Aspect,
                ResizePanel(x, (int)(x / simAspect));   // Resize the panel Height

            x = Panel.Width; y = Panel.Height;
            visAspect = (double)x / (double)y;
            fieldMultiplier = report.Loc.FieldMultiplier(x);
            firstResize = true;
        }

        private void DrawSimTime(int currTimeTick)
        { // Outputs the current Sim Time to the screen into a label.
            double timeSec = currTimeTick * secPerTick;
            int timeHrs = (int)Math.Floor(timeSec / 3600);
            int timeMin = (int)Math.Floor(timeSec / 60);
            int timeSecInt = (int)Math.Floor(timeSec - timeHrs * 3600 - timeMin * 60);

            string simTime = timeHrs.ToString() + ":";
            if (timeMin < 10) simTime = simTime + "0" + timeMin.ToString() + ":";
            else simTime = simTime + timeMin.ToString() + ":";
            if (timeSecInt < 10) simTime = simTime + "0" + timeSecInt.ToString();
            else simTime = simTime + timeSecInt.ToString();
            label_SimTime.Text = simTime;
        }

        /*  \\     //  */
        /*   \\   //   */
        /*    \\ //    */
        #endregion REPORT HANDLING

        #region OPEN DLL METHODS
        /*    // \\    */
        /*   //   \\   */
        /*  //     \\  */

        private void btn_LoadDLL_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = "dll";
            file.Filter = "DLL Files (*.dll)|*.DLL";
            file.CheckFileExists = true;
            file.Multiselect = false;
            file.ShowDialog();
            addDLL(file.FileName);
        }

        private void addLocalDLLs()
        {
            String homePath = Path.GetDirectoryName(Application.ExecutablePath);

            if (!Directory.Exists(homePath + "\\lib"))
                Directory.CreateDirectory(homePath + "\\lib");

            string[] filePaths = Directory.GetFiles(homePath, "*.dll", SearchOption.AllDirectories);

            for (int i = 0; i < filePaths.Length; i++)
            {
                if (!this.lb_DLLs.Items.Contains(filePaths[i]))
                    addDLL(filePaths[i]);
            }
        }

        private void addDLL(string Filename)
        { // Modified from http://blogs.msdn.com/abhinaba/archive/2005/11/14/492458.aspx
            try
            {
                Assembly assembly = Assembly.LoadFile(Filename);
                Type[] types = assembly.GetTypes();
                this.lb_DLLs.Items.Add(Filename);
                foreach (Type type in types)
                {
                    if (!type.IsClass || type.IsNotPublic) continue;
                    Type[] interfaces = type.GetInterfaces();
                    bool contains = false;
                    foreach (Type intf in interfaces)
                    {
                        if (intf.ToString() == typeof(INodeFactory).ToString())
                        {
                            contains = true;

                        }
                    }
                    if (contains)
                    {
                        //object obj = Activator.CreateInstance(type);
                        addINode(type);
                    }

                    contains = false;
                    foreach (Type intf in interfaces)
                    {
                        if (intf.ToString() == typeof(IPhysicalProcessor).ToString())
                        {
                            contains = true;

                        }
                    }
                    if (contains)
                    {
                        //object obj = Activator.CreateInstance(type);
                        addIPhysicalProcessor(type);
                    }

                    contains = false;
                    foreach (Type intf in interfaces)
                    {
                        if (intf.ToString() == typeof(IDeployer).ToString())
                        {
                            contains = true;

                        }
                    }
                    if (contains)
                    {
                        //object obj = Activator.CreateInstance(type);
                        addIDeployer(type);
                    }

                    contains = false;
                    foreach (Type intf in interfaces)
                    {
                        if (intf.ToString() == typeof(IApplicationEventGenerator).ToString())
                        {
                            contains = true;

                        }
                    }
                    if (contains)
                    {
                        //object obj = Activator.CreateInstance(type);
                        addIApplicationEventGenerator(type);
                    }

                    contains = false;
                    foreach (Type intf in interfaces)
                    {
                        if (intf.ToString() == typeof(IRandomizerFactory).ToString())
                        {
                            contains = true;

                        }
                    }
                    if (contains)
                    {
                        //object obj = Activator.CreateInstance(type);
                        addIRandomizerFactory(type);
                    }

                    contains = false;
                    foreach (Type intf in interfaces)
                    {
                        if (intf.ToString() == typeof(ILocation).ToString())
                        {
                            contains = true;

                        }
                    }
                    if (contains)
                    {
                        //object obj = Activator.CreateInstance(type);
                        addILocation(type);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void addINode(object obj)
        {
            this.cb_INode.Items.Add(obj);
            this.cb_INode.SelectedIndex = 0;
        }

        private void addIPhysicalProcessor(object obj)
        {
            this.cb_IPhysicalProcessor.Items.Add(obj);
            this.cb_IPhysicalProcessor.SelectedIndex = 0;
        }

        private void addIDeployer(object obj)
        {
            this.cb_IDeployer.Items.Add(obj);
            this.cb_IDeployer.SelectedIndex = 0;
        }

        private void addIApplicationEventGenerator(object obj)
        {
            this.cb_IApplicationEventGenerator.Items.Add(obj);
            this.cb_IApplicationEventGenerator.SelectedIndex = 0;
        }

        private void addIRandomizerFactory(object obj)
        {
            this.cb_IRandomizerFactory.Items.Add(obj);
            this.cb_IRandomizerFactory.SelectedIndex = 0;
        }

        private void addILocation(object obj)
        {
            this.cb_ILocation.Items.Add(obj);
            this.cb_ILocation.SelectedIndex = 0;
        }

        /*  \\     //  */
        /*   \\   //   */
        /*    \\ //    */
        #endregion OPEN DLL METHODS

        #region RUN SIMULATION
        private void tabControl_SelectedIndexChangedRunning(Object o, EventArgs eve)
        {
            if (tab_INode.SelectedTab == tab_File)
                return;
            if (tab_INode.SelectedTab == tab_Run)
                return;
            if (tab_INode.SelectedTab == tab_Statistics)
                return;
            if (tab_INode.SelectedTab == tab_IO)
                return;
            //tab_INode.SelectedTab = tab_Statistics;
        }

        private void disableConfiguration()
        {
            tab_INode.SelectedIndexChanged += new 
                EventHandler(tabControl_SelectedIndexChangedRunning);

            // Enable Sim Interaction Buttons
            btn_Advance_Sim.Enabled = true;
            btn_Back_Sim.Enabled = true;
            btn_Jump_Sim.Enabled = true;
            btn_RestartSim.Enabled = true;
            btn_Stop_Sim.Enabled = true;

            // Disable Tab Controls
            disableTab(tab_IApplicationEventGenerator);
            disableTab(tab_IDeployer);
            disableTab(tab_ILocation);
            disableTab(tab_INodeFactory);
            disableTab(tab_IPhysicalProcessor);
            disableTab(tab_IRandomizerFactory);
            disableTab(tab_IO);
        }

        private void disableConfigurationMultirun()
        {
            btn_Pause_Sim.Enabled = false;
            btn_Back_Sim.Enabled = false;
            btn_Advance_Sim.Enabled = false;
            btn_RestartSim.Enabled = false;
            btn_Jump_Sim.Enabled = false;
        }

        private void enableConfigurationMultirun()
        {
            btn_Pause_Sim.Enabled = true;
        }

        private void enableConfiguration()
        {
            tab_INode.SelectedIndexChanged -= new 
                EventHandler(tabControl_SelectedIndexChangedRunning);

            // Disable Sim Interaction Buttons
            btn_Advance_Sim.Enabled = false;
            btn_Back_Sim.Enabled = false;
            btn_Jump_Sim.Enabled = false;
            btn_RestartSim.Enabled = false;
            btn_Stop_Sim.Enabled = false;

            // Enable Tab Controls
            enableTab(tab_IApplicationEventGenerator);
            enableTab(tab_IDeployer);
            enableTab(tab_ILocation);
            enableTab(tab_INodeFactory);
            enableTab(tab_IPhysicalProcessor);
            enableTab(tab_IRandomizerFactory);
            enableTab(tab_IO);
        }

        StreamWriter outputFile = null;
        int run;
        private void btn_RunSim_Click(object sender, EventArgs e)
        {
            isSimPaused = false;
            if (isSimRunning)
                return;
            disableConfiguration();

            label_Info.Text = "Configuring Simulation.";
            label_Info.Refresh();
            System.Threading.Thread.Sleep(250);
            // ********** VERIFY CONFIGURATION **********
            // This could be moved to click/change events
            // and the Begin button disabled until config
            // is validated.
            bool valid = true;

            if (selected_IApplicationEventGenerator.ForeColor == Color.Red)
                valid = false;
            if (selected_IDeployer.ForeColor == Color.Red)
                valid = false;
            if (selected_ILocation.ForeColor == Color.Red)
                valid = false;
            if (selected_INodeFactory.ForeColor == Color.Red)
                valid = false;
            if (selected_IPhysicalProcessor.ForeColor == Color.Red)
                valid = false;
            if (selected_IRandomizerFactory.ForeColor == Color.Red)
                valid = false;
            if (selected_GraphicsRunning.ForeColor == Color.Red)
                valid = false;

            string filename = text_IOFolder.Text + "\\"
                    + text_IOFileTag.Text + "_SimInfo.txt";

            if (filename.IndexOfAny(System.IO.Path.GetInvalidPathChars()) > -1)
                valid = false;

            if (!valid)
            {
                enableConfiguration();
                label_Info.Text = "ERR: Fix Highlighted Red/Yellow Options.";
                return;
            }

            // To prevent SW from getting stuck. Seems to happen when restarting many visualizations.
            sw = new StopWatch.StopWatch();
            sw.Reset();
            resetTimeTick = 0;

            // Set up modules & settings
            Type INodeFactoryType = (Type)this.cb_INode.SelectedItem;
            Type IPhysicalProcessorType = (Type)this.cb_IPhysicalProcessor.SelectedItem;
            Type IDeployerType = (Type)this.cb_IDeployer.SelectedItem;
            Type IApplicationEventGeneratorType = (Type)this.cb_IApplicationEventGenerator.SelectedItem;
            Type IRandomizerFactoryType = (Type)this.cb_IRandomizerFactory.SelectedItem;
            Type ILocationType = (Type)this.cb_ILocation.SelectedItem;
            double x1 = double.Parse(text_FieldX1.Text);
            double y1 = double.Parse(text_FieldY1.Text);
            double x2 = double.Parse(text_FieldX2.Text);
            double y2 = double.Parse(text_FieldY2.Text);

            double timeScale = double.Parse(TimeScale.Text);
            if (!cb_InvertScale.Checked)
                timeScale = 1 / timeScale;                          // Sim seconds per real second
            secPerTick = timeScale / double.Parse(FPS.Text);        // Sim seconds per frame

            bool multirun = false;
            int numruns = 1;
            if (cb_Multirun.Checked && (tb_Multirun.BackColor == Color.White))
            {
                multirun = true;
                numruns = int.Parse(tb_Multirun.Text);
                disableConfigurationMultirun();
            }

            outputFile = null;

            // write to Run Info File
            string indent = "     ";
            StreamWriter simInfoFile = new StreamWriter(filename, false);
            simInfoFile.WriteLine("Simulation Tag: " + text_IOFileTag.Text);
            simInfoFile.Write("Run Flags: ");
            if (cb_AppSetsSink.Checked)
                simInfoFile.Write("[Application Sets Sink Node] ");
            if (cb_randomSink.Checked)
                simInfoFile.Write("[Randomized Sink Node] ");
            if (cb_GraphicsOff.Checked)
                simInfoFile.Write("[Graphics Off] ");
            else
                simInfoFile.Write("[Graphics On] ");
            if (cb_Multirun.Checked)
                simInfoFile.Write("[Multirun with " + tb_Multirun.Text + " Runs]");
            simInfoFile.Write("\n");
            
            simInfoFile.WriteLine("ILocation: " + ILocationType.ToString());
            simInfoFile.WriteLine(indent + "Initial Corner: (" 
                + text_FieldX1.Text + ", " + text_FieldY1.Text + ")");
            simInfoFile.WriteLine(indent + "Final Corner:   ("
                + text_FieldX2.Text + ", " + text_FieldY2.Text + ")");

            // Initial randomizer factory
            IRandomizerFactory randomFactoryMultirun
                        = (IRandomizerFactory)Activator.CreateInstance(IRandomizerFactoryType);
            randomFactoryMultirun.PanelObjs = setPanelObjValues(PanelObjs_IRandomizerFactory);
            randomFactoryMultirun.Initialize();

            simInfoFile.WriteLine("IRandomizerFactory: " + IRandomizerFactoryType.ToString());
            foreach (PanelObj pObj in PanelObjs_IRandomizerFactory)
            {
                pObj.UpdateInfo();
                simInfoFile.WriteLine(indent + pObj.name + ": Text = " + pObj.text
                    + "; Value = " + pObj.value);
            }

            simInfoFile.WriteLine("IDeployer: " + IDeployerType.ToString());
            foreach (PanelObj pObj in PanelObjs_IDeployer)
            {
                pObj.UpdateInfo();
                simInfoFile.WriteLine(indent + pObj.name + ": Text = " + pObj.text
                    + "; Value = " + pObj.value);
            }

            simInfoFile.WriteLine("IPhysicalProcessor: " + IPhysicalProcessorType.ToString());
            foreach (PanelObj pObj in PanelObjs_IPhysProc)
            {
                pObj.UpdateInfo();
                simInfoFile.WriteLine(indent + pObj.name + ": Text = " + pObj.text
                    + "; Value = " + pObj.value);
            }

            simInfoFile.WriteLine("INodeFactory: " + INodeFactoryType.ToString());
            foreach (PanelObj pObj in PanelObjs_INode)
            {
                pObj.UpdateInfo();
                simInfoFile.WriteLine(indent + pObj.name + ": Text = " + pObj.text
                    + "; Value = " + pObj.value);
            }

            simInfoFile.WriteLine("IApplicationEventGenerator: " + IApplicationEventGeneratorType.ToString());
            foreach (PanelObj pObj in PanelObjs_IApplicationEventGenerator)
            {
                pObj.UpdateInfo();
                simInfoFile.WriteLine(indent + pObj.name + ": Text = " + pObj.text
                    + "; Value = " + pObj.value);
            }

            simInfoFile.WriteLine();

            // Start running simulation
            label_Info.Text = "Simulation Running";
            label_Info.Refresh();
            System.Threading.Thread.Sleep(250);

            for (run = 0; run < numruns; run++)
            {
                // SIM START
                isSimRunning = true;
                reporter = new Reporter(secPerTick);
                reporter.EnableGraphics = !cb_GraphicsOff.Checked;

                IRandomizerFactory randomFactory = (IRandomizerFactory)Activator.CreateInstance(IRandomizerFactoryType);
                randomFactory.PanelObjs = setPanelObjValues(PanelObjs_IRandomizerFactory);
                if (run == 0)
                    randomFactory.SetSeed(randomFactoryMultirun.InitialSeed);
                else
                    randomFactory.SetSeed(randomFactoryMultirun.CreateRandomizer().Next());
                
                simInfoFile.WriteLine("Run #" + run + ": Randomizer Seed Info");
                simInfoFile.WriteLine(indent + "Random Seed: " + randomFactory.InitialSeed);
                
                Nodes nodes = new Nodes(randomFactory.CreateRandomizer());

                EventManager eventMgr = new EventManager();

                IPhysicalProcessor physProc
                    = (IPhysicalProcessor)Activator.CreateInstance(IPhysicalProcessorType);
                physProc.PanelObjs = setPanelObjValues(PanelObjs_IPhysProc);

                ReporterIWF repIWF = new ReporterIWF(secPerTick, physProc.TransmissionSpeed,
                    physProc.PropagationSpeed);
                repIWF.Attach(reporter);
                physProc.RepIWF = repIWF;

                INodeFactory nodeFactory = (INodeFactory)Activator.CreateInstance(INodeFactoryType);
                nodeFactory.PanelObjs = setPanelObjValues(PanelObjs_INode);
                nodeFactory.Initialize(eventMgr, physProc, randomFactory, reporter);

                XYDoubleLocation[] field = new XYDoubleLocation[2];
                field[0] = new XYDoubleLocation(x1, y1);
                field[1] = new XYDoubleLocation(x2, y2);

                IDeployer deployer = (IDeployer)Activator.CreateInstance(IDeployerType);
                deployer.PanelObjs = setPanelObjValues(PanelObjs_IDeployer);
                deployer.Initialize(nodes, nodeFactory, field, randomFactory);

                //SimulationCompleteEvent finalEvent = new SimulationCompleteEvent();
                //finalEvent.Time = 0 /*hours*/   * 60 * 60
                //                + 10 /*minutes*/ * 60
                //                + 0 /*seconds*/;
                //eventMgr.AddEvent(finalEvent);

                deployer.Deploy();

                // Physical Processor needs to be initialized after the nodes are deployed.
                physProc.Initialize(nodes, eventMgr);
                repIWF.MaxBitDistance = physProc.MaximumRange;

                IApplicationEventGenerator eventGen = (IApplicationEventGenerator)
                        Activator.CreateInstance(IApplicationEventGeneratorType);
                eventGen.PanelObjs = setPanelObjValues(PanelObjs_IApplicationEventGenerator);
                eventGen.Attach(reporter);
                eventGen.Initialize(eventMgr, nodes, randomFactory.CreateRandomizer(),
                    field);

                if (cb_AppSetsSink.Checked)
                {
                    if (cb_randomSink.Checked)
                    {
                        eventGen.GenerateEvent();
                        nodes.SetRandomSinkNode();
                    }
                    else
                    {
                        INode[] furthestPair = nodes.FindFurthestNodes();
                        eventGen.GenerateEvent(furthestPair[0].Location);
                        furthestPair[1].IsSink = true;
                    }
                }
                else
                    eventGen.GenerateEvent();

                nodes.InitializeNodes();

                tab_INode.SelectedTab = tab_Statistics;

                backgroundWorker_RunSim.RunWorkerAsync(eventMgr);
                while (backgroundWorker_RunSim.IsBusy)
                    Application.DoEvents();

                if (cb_GraphicsOff.Checked)
                {
                    label_Info.Text = "Run " + (run + 1).ToString() + " of " + numruns.ToString()
                        + " is running.";
                    label_Info.Refresh();

                    runSimStruct runSimArgs;
                    runSimArgs.outputFile = outputFile;
                    runSimArgs.run = run;

                    backgroundWorker_RunSim.RunWorkerAsync(runSimArgs);
                    while (backgroundWorker_RunSim.IsBusy)
                        Application.DoEvents();
                }
                else
                {
                    label_Info.Text = "Visualizer Running";
                    label_Info.Refresh();
                    currTimeTick = 0;
                    timerDraw.Tick += new EventHandler(runVisualizer);
                    timerDraw.Start();
                }
            }

            if (cb_GraphicsOff.Checked)
            {// Press "stop"
                EventArgsMessage args = new EventArgsMessage("Simulation Complete.");
                btn_Stop_Sim_Click(sender, args);
                enableConfigurationMultirun();
            }

            simInfoFile.Close();
        }

        class EventArgsMessage : EventArgs
        {
            public String Message;

            public EventArgsMessage()
            { }

            public EventArgsMessage(string Message)
            {
                this.Message = Message;
            }
        }

        struct runSimStruct
        {
            public StreamWriter outputFile;
            public int run;
        }

        private void backgroundWorker_RunSim_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is EventManager)
            {
                EventManager em = (EventManager)e.Argument;
                em.StartSimulation();
            }
            else if (e.Argument is runSimStruct)
            {
                runSimStruct args = (runSimStruct)e.Argument;

                outputFile = runInfoReporting(args.run, args.outputFile);
            }
        }

        int resetTimeTick = 0;
        private void runVisualizer(Object o, EventArgs eve)
        {
            // Variable Set-up
            IGraphicalReport graphicalReport;
            CircleReport circReport;
            AnnulusReport annReport;
            XYIntLocation intLoc;

            // Reset Stopwatch to calculate actual time scale (real:sim)
            if ((currTimeTick - resetTimeTick) * secPerTick > 1)
            {
                sw.Reset();
                resetTimeTick = currTimeTick;
            }

            // Output current Sim Time to a label on screen
            DrawSimTime(currTimeTick);
            
            bool redrawNeeded = false;
            currTimeTick++;

            // Check for nulls (Stop pressed)
            if (reporter == null)
                return;
            // Cycle through reports and draw... (This takes lots of time! ref: FPS)
            IReport report = reporter.GetNext(currTimeTick);
            while ((report != null) && !(report is EmptyReport))
            {
                // Set aspect ratio on screen
                if (!firstResize)
                {
                    setAspect(report);
                }

                if (report is IGraphicalReport)
                {
                    graphicalReport = (IGraphicalReport)report;
                    graphicalReport.ConvertTo(panelDrawField);
                    graphicalReport.ConvertTo(fieldMultiplier);

                    intLoc = (XYIntLocation)report.Loc;
                    if (graphicalReport is CircleReport)
                    {
                        circReport = (CircleReport)graphicalReport;
                        circReport.GradientPath.AddEllipse((float)(intLoc.X - circReport.MaximumMessageSize / 2),
                            (float)(intLoc.Y - circReport.MaximumMessageSize / 2), (float)circReport.MaximumMessageSize,
                            (float)circReport.MaximumMessageSize);
                    }
                    else if (graphicalReport is AnnulusReport)
                    {
                        annReport = (AnnulusReport)graphicalReport;
                    }

                    if (graphicalReport.IsStatic)
                    {
                        ProcessStaticReport(graphicalReport);
                        redrawNeeded = true;
                    }
                    else
                    {
                        if (graphicalReport.Layer == DrawLayer.Background)
                            graphicalReport.Draw(gfxBackBuffer, colorDef);
                        else if (graphicalReport.Layer == DrawLayer.Foreground)
                            graphicalReport.Draw(gfxForeBuffer, colorDef);
                    }
                }
                else if (report is IInfoReport)
                {
                    processInfoReport((IInfoReport)report);
                }
                // Check for nulls (Stop pressed)
                if (reporter == null)
                    return;
                report = reporter.GetNext(currTimeTick);
            }

            if (report is EmptyReport)
            {
                label_Info.Text = "Visualization Complete.";
            }

            if (redrawNeeded)
                RedrawStatics();
        }

        private StreamWriter runInfoReporting()
        {
            return runInfoReporting(0, null);
        }
        private StreamWriter runInfoReporting(int runID, StreamWriter stream)
        {
            // Check for nulls (Stop pressed)
            if (reporter == null)
                return stream;

            IReport report = reporter.GetNext(int.MaxValue);
            while ((report != null) && !(report is EmptyReport))
            {
                if (report is IInfoReport)
                {
                    stream = processInfoReport((IInfoReport)report, runID, stream);
                }

                report = reporter.GetNext(int.MaxValue);
                /* if ((report is EmptyReport) || (report == null))
                {
                    label_Info.Text = "Simulation Complete.";
                } */
            }
            return stream;
        }

        private StreamWriter processInfoReport(IInfoReport report)
        {
            return processInfoReport(report, 0);
        }

        private StreamWriter processInfoReport(IInfoReport report, bool timeForward)
        {
            return processInfoReport(report); // for now...
        }

        private StreamWriter processInfoReport(IInfoReport report, int runID, StreamWriter stream)
        {
            IInfoReport infoReport = (IInfoReport)report;
            String line;
            if (!_tableIO.TryGetValue(infoReport.Key, out stream))
            {
                string filename = text_IOFolder.Text + "\\"
                    + text_IOFileTag.Text + "_" + infoReport.Key + ".csv";

                if (filename.IndexOfAny(System.IO.Path.GetInvalidPathChars()) > -1)
                    return null;

                stream = new StreamWriter(filename);

                _tableIO.Add(infoReport.Key, stream);
                line = "runID, " + infoReport.CSVHeader;
                stream.WriteLine(line);
            }

            if (!infoReport.IsWritten)
            {
                line = runID.ToString() + ", " + infoReport.ToCSV();
                stream.WriteLine(line);
                infoReport.IsWritten = true;
            }
            return stream;
        }

        private StreamWriter processInfoReport(IInfoReport report, int runID)
        {
            StreamWriter stream = null;
            return processInfoReport(report, runID, stream);
        }

        #endregion RUN SIMULATION

        private void createPanelObj(List<PanelObj> PanelObjs, TabPage tab)
        {
            PanelObj q;
            foreach (PanelObj p in PanelObjs)
            {
                switch (p.type)
                {
                    case FormType.check:
                        q = p;
                        CheckBox cb = new CheckBox();
                        q.obj = cb;
                        tab.Controls.Add(cb);
                        cb.AutoSize = true;
                        cb.Location = new Point(p.xSlot * (125 - 6) + 6,
                            p.ySlot * 25 + 69);
                        cb.Name = tab.Name + '_' + p.name.Replace(" ", "");
                        cb.Size = new Size(p.width, 17);
                        cb.Text = p.text;
                        cb.Checked = bool.Parse(p.value);
                        cb.UseVisualStyleBackColor = true;
                        break;
                    case FormType.doubleBox:
                    case FormType.floatBox:
                    case FormType.intBox:
                        q = p;
                        TextBox tb = new TextBox();
                        q.obj = tb;
                        tab.Controls.Add(tb);
                        tb.AutoSize = true;
                        tb.Location = new Point(p.xSlot * (125 - 6) + 6,
                            p.ySlot * 25 + 67);
                        tb.Name = tab.Name + '_' + p.name.Replace(" ", "");
                        tb.Text = p.value;
                        tb.Size = new Size(p.width, 20);
                        break;
                    case FormType.label:
                        q = p;
                        Label lab = new Label();
                        q.obj = lab;
                        tab.Controls.Add(lab);
                        lab.AutoSize = true;
                        lab.Location = new Point(p.xSlot * (125 - 6) + 6,
                            p.ySlot * 25 + 70);
                        lab.Name = tab.Name + '_' + p.name.Replace(" ", "");
                        lab.Text = p.text;
                        lab.Size = new Size(p.width, 13);
                        break;
                }
            }
        }

        private void disableTab(TabPage tab)
        {
            foreach (Control ctrl in tab.Controls)
            {
                ctrl.Enabled = false;
            }
        }

        private void enableTab(TabPage tab)
        {
            foreach (Control ctrl in tab.Controls)
            {
                ctrl.Enabled = true;
            }
        }

        private void disablePanelObj(List<PanelObj> panelObjs)
        {
            foreach (PanelObj obj in panelObjs)
            {
                obj.obj.Enabled = false;
            }
        }

        private void enablePanelObj(List<PanelObj> panelObjs)
        {
            foreach (PanelObj obj in panelObjs)
            {
                obj.obj.Enabled = true;
            }
        }

        #region FORM VALIDATION
        /*    // \\    */
        /*   //   \\   */
        /*  //     \\  */

        private void Validate_IntBox(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                ComboBox cb = (ComboBox)box.Parent.GetChildAtPoint(new Point(6, 23));
                int checkValue;
                try { checkValue = int.Parse(box.Text); }
                catch (Exception ex)
                {
                    box.BackColor = Color.Yellow;
                    cb.BackColor = Color.Red;
                    return;
                }
            }
        }

        private bool Validate_IntTextBox(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                int checkValue;
                try { checkValue = int.Parse(box.Text); }
                catch (Exception ex)
                {
                    box.BackColor = Color.Yellow;
                    return false;
                }
                box.BackColor = Color.White;
                return true;
            }
            return false;
        }

        private bool Validate_DoubleTextBox(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                double checkValue;
                try { checkValue = double.Parse(box.Text); }
                catch
                {
                    box.BackColor = Color.Yellow;
                    return false;
                }
                box.BackColor = Color.White;
                return true;
            }
            return false;
        }

        private void cb_IApplicationEventGenerator_BackColorChanged(object sender, EventArgs e)
        {
            if (cb_IApplicationEventGenerator.BackColor == Color.Yellow)
                selected_IApplicationEventGenerator.ForeColor = Color.Red;
            else if (cb_IApplicationEventGenerator.BackColor == Color.White)
                selected_IApplicationEventGenerator.ForeColor = Color.Green;
        }

        private void cb_IDeployer_BackColorChanged(object sender, EventArgs e)
        {
            if (cb_IDeployer.BackColor == Color.Yellow)
                selected_IDeployer.ForeColor = Color.Red;
            else if (cb_IDeployer.BackColor == Color.White)
                selected_IDeployer.ForeColor = Color.White;
        }

        private void cb_ILocation_BackColorChanged(object sender, EventArgs e)
        {
            if (cb_ILocation.BackColor == Color.Yellow)
                selected_ILocation.ForeColor = Color.Red;
            else if (cb_ILocation.BackColor == Color.White)
                selected_ILocation.ForeColor = Color.White;
        }

/*        private void cb_INode_BackColorChanged(object sender, EventArgs e)
        {
            if (cb_INode.BackColor == Color.Yellow)
                selected_INode.ForeColor = Color.Red;
            if (cb_INode.BackColor == Color.White)
                selected_INode.ForeColor = Color.White;
        } */

        private void cb_IPhysicalProcessor_BackColorChanged(object sender, EventArgs e)
        {
            if (cb_IPhysicalProcessor.BackColor == Color.Yellow)
                selected_IPhysicalProcessor.ForeColor = Color.Red;
            else if (cb_IPhysicalProcessor.BackColor == Color.White)
                selected_IPhysicalProcessor.ForeColor = Color.White;
        }

        private void cb_IRandomizerFactory_BackColorChanged(object sender, EventArgs e)
        {
            if (cb_IRandomizerFactory.BackColor == Color.Yellow)
                selected_IRandomizerFactory.ForeColor = Color.Red;
            if (cb_IRandomizerFactory.BackColor == Color.White)
                selected_IRandomizerFactory.ForeColor = Color.White;
        }

        private void cb_InvertScale_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_InvertScale.Checked)
                label_TimeScale.Text = "Real Sec : Sim Sec";
            else label_TimeScale.Text = "Sim Sec : Real Sec";
        }

        private void TimeScale_TextChanged(object sender, EventArgs e)
        {
            if (Validate_DoubleTextBox(sender, e))
            {
                double d;
                d = double.Parse(TimeScale.Text);
                
                if (d > 0)
                    TimeScale.BackColor = Color.White;
                else TimeScale.BackColor = Color.Yellow;
            }
        }

        private void FPS_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (Validate_IntTextBox(sender, e))
            {
                i = int.Parse(FPS.Text);
                if (i > 0)
                    FPS.BackColor = Color.White;
                else FPS.BackColor = Color.Yellow;
            }
        }

        List<PanelObj> PanelObjs_INode = new List<PanelObj>();
        private void cb_INode_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (PanelObj p in PanelObjs_INode)
            {
                tab_INode.Controls.Remove(p.obj);
            }

            Type INodeFactoryType = (Type)this.cb_INode.SelectedItem;
            if (INodeFactoryType == null)
            {
                cb_INode.BackColor = Color.Yellow;
                selected_INodeFactory.ForeColor = Color.Red;
            }
            else
            {
                cb_INode.BackColor = Color.White;
                selected_INodeFactory.Text = cb_INode.SelectedItem.ToString();
                selected_INodeFactory.ForeColor = Color.Green;

                INodeFactory nodeFactory = (INodeFactory)Activator.CreateInstance(INodeFactoryType);
                PanelObjs_INode = nodeFactory.SetupPanel;
                createPanelObj(PanelObjs_INode, this.tab_INodeFactory);
            }
        }

        List<PanelObj> PanelObjs_IPhysProc = new List<PanelObj>();
        private void cb_IPhysicalProcessor_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (PanelObj p in PanelObjs_IPhysProc)
            {
                tab_IPhysicalProcessor.Controls.Remove(p.obj);
            }

            Type IPhysicalProcessorType = (Type)this.cb_IPhysicalProcessor.SelectedItem;
            if (IPhysicalProcessorType == null)
            {
                cb_IPhysicalProcessor.BackColor = Color.Yellow;
                selected_IPhysicalProcessor.ForeColor = Color.Red;
            }
            else
            {
                cb_IPhysicalProcessor.BackColor = Color.White;
                selected_IPhysicalProcessor.Text = cb_IPhysicalProcessor.SelectedItem.ToString();
                selected_IPhysicalProcessor.ForeColor = Color.Green;

                IPhysicalProcessor physProc = (IPhysicalProcessor)Activator.CreateInstance(IPhysicalProcessorType);
                PanelObjs_IPhysProc = physProc.SetupPanel;
                createPanelObj(PanelObjs_IPhysProc, this.tab_IPhysicalProcessor);
            }
        }

        List<PanelObj> PanelObjs_IApplicationEventGenerator = new List<PanelObj>();
        bool IApplicationEventGeneratorSelected = false;
        private void cb_IApplicationEventGenerator_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (PanelObj p in PanelObjs_IApplicationEventGenerator)
            {
                tab_IApplicationEventGenerator.Controls.Remove(p.obj);
            }

            Type IApplicationEventGeneratorType = (Type)this.cb_IApplicationEventGenerator.SelectedItem;
            if (IApplicationEventGeneratorType == null)
            {
                cb_IApplicationEventGenerator.BackColor = Color.Yellow;
                selected_IApplicationEventGenerator.ForeColor = Color.Red;
                IApplicationEventGeneratorSelected = false;
            }
            else
            {
                cb_IApplicationEventGenerator.BackColor = Color.White;
                selected_IApplicationEventGenerator.Text
                    = cb_IApplicationEventGenerator.SelectedItem.ToString();
                selected_IApplicationEventGenerator.ForeColor = Color.Green;
                IApplicationEventGeneratorSelected = true;

                IApplicationEventGenerator appGen = (IApplicationEventGenerator)Activator.CreateInstance(IApplicationEventGeneratorType);
                PanelObjs_IApplicationEventGenerator = appGen.SetupPanel;
                createPanelObj(PanelObjs_IApplicationEventGenerator, this.tab_IApplicationEventGenerator);
            }
        }

        List<PanelObj> PanelObjs_IRandomizerFactory = new List<PanelObj>();
        bool IRandomizerFactorySelected = false;
        private void cb_IRandomizerFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (PanelObj p in PanelObjs_IRandomizerFactory)
            {
                tab_IRandomizerFactory.Controls.Remove(p.obj);
            }

            Type IRandomizerFactoryType = (Type)this.cb_IRandomizerFactory.SelectedItem;
            if (IRandomizerFactoryType == null)
            {
                cb_IRandomizerFactory.BackColor = Color.Yellow;
                selected_IRandomizerFactory.ForeColor = Color.Red;
                IRandomizerFactorySelected = false;
            }
            else
            {
                cb_IRandomizerFactory.BackColor = Color.White;
                selected_IRandomizerFactory.Text = cb_IRandomizerFactory.SelectedItem.ToString();
                IRandomizerFactorySelected = true;

                IRandomizerFactory randFact = (IRandomizerFactory)Activator.CreateInstance(IRandomizerFactoryType);
                PanelObjs_IRandomizerFactory = randFact.SetupPanel;
                createPanelObj(PanelObjs_IRandomizerFactory, this.tab_IRandomizerFactory);
            }
        }

        List<PanelObj> PanelObjs_ILocation = new List<PanelObj>();
        bool ILocationSelected = false;
        private void cb_ILocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (PanelObj p in PanelObjs_ILocation)
            {
                tab_ILocation.Controls.Remove(p.obj);
            }

            Type ILocationType = (Type)this.cb_ILocation.SelectedItem;
            if (ILocationType == null)
            {
                cb_ILocation.BackColor = Color.Yellow;
                selected_ILocation.ForeColor = Color.Red;
                ILocationSelected = false;
            }
            else if (this.cb_ILocation.Text == "Location.XYDoubleLocation")
            {
                cb_ILocation.BackColor = Color.White;
                selected_ILocation.Text = cb_ILocation.SelectedItem.ToString();
                selected_ILocation.ForeColor = Color.Green;
                ILocationSelected = true;

                ILocation loc = (ILocation)Activator.CreateInstance(ILocationType);
                PanelObjs_ILocation = loc.SetupPanel;
                createPanelObj(PanelObjs_ILocation, this.tab_ILocation);
            }
            else
            {
                cb_ILocation.BackColor = Color.Yellow;
                selected_ILocation.ForeColor = Color.Red;
                ILocationSelected = false;
            }
        }

        List<PanelObj> PanelObjs_IDeployer = new List<PanelObj>();
        private void cb_IDeployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (PanelObj p in PanelObjs_IDeployer)
            {
                tab_IDeployer.Controls.Remove(p.obj);
            }

            Type IDeployerType = (Type)this.cb_IDeployer.SelectedItem;
            if (IDeployerType == null)
            {
                cb_IDeployer.BackColor = Color.Yellow;
                selected_IDeployer.ForeColor = Color.Red;
            }
            else
            {
                cb_IDeployer.BackColor = Color.White;
                selected_IDeployer.Text = cb_IDeployer.SelectedItem.ToString();
                selected_IDeployer.ForeColor = Color.Green;

                IDeployer deployer = (IDeployer)Activator.CreateInstance(IDeployerType);
                PanelObjs_IDeployer = deployer.SetupPanel;
                createPanelObj(PanelObjs_IDeployer, this.tab_IDeployer);
            }
        }

        private List<PanelObj> setPanelObjValues(List<PanelObj> panelObjs)
        {
            PanelObj pobj;
            List<PanelObj> outList = new List<PanelObj>();
            foreach (PanelObj p in panelObjs)
            {
                switch (p.type)
                {
                    case FormType.check:
                        pobj = p;
                        pobj.value = ((CheckBox)pobj.obj).Checked.ToString();
                        outList.Add(pobj);
                        break;
                    case FormType.doubleBox:
                    case FormType.floatBox:
                    case FormType.intBox:
                        pobj = p;
                        pobj.value = ((TextBox)pobj.obj).Text;
                        outList.Add(pobj);
                        break;
                    case FormType.label:
                        break;
                }
            }
            return outList;
        }

        

        private void text_JumpHrs_TextChanged(object sender, EventArgs e)
        {
            Validate_IntTextBox(sender, e);
        }

        private void text_JumpMin_TextChanged(object sender, EventArgs e)
        {
            Validate_IntTextBox(sender, e);
        }

        private void text_JumpSec_TextChanged(object sender, EventArgs e)
        {
            Validate_DoubleTextBox(sender, e);
        }

        private void text_JumpSec_KeyDown(object sender, KeyEventArgs e)
        {
            if (isSimRunning)
                if (e.KeyCode == Keys.Enter)
                    btn_Jump_Sim_Click(sender, e);
        }

        private void text_JumpStepSize_TextChanged(object sender, EventArgs e)
        {
            Validate_DoubleTextBox(sender, e);
        }

        /*  \\     //  */
        /*   \\   //   */
        /*    \\ //    */
        #endregion FORM VALIDATION

        private void btn_Jump_Sim_Click(object sender, EventArgs e)
        {
            string hrsString = this.text_JumpHrs.Text;
            string minString = this.text_JumpMin.Text;
            string secString = this.text_JumpSec.Text;
            double jumpTime = (double)(int.Parse(hrsString) * 60 * 60 + int.Parse(minString) * 60 + double.Parse(secString));

            jumpToTime(jumpTime);
        }

        private void btn_Back_Sim_Click(object sender, EventArgs e)
        {
            double jumpStep = double.Parse(text_JumpStepSize.Text);
            double jumpTime = currTimeTick * secPerTick - jumpStep;

            if (jumpTime < 0)
                jumpTime = 0;

            jumpToTime(jumpTime);
        }

        private void btn_Advance_Sim_Click(object sender, EventArgs e)
        {
            double jumpStep = double.Parse(text_JumpStepSize.Text);
            double jumpTime = currTimeTick * secPerTick + jumpStep;

            jumpToTime(jumpTime);
        }

        private void jumpToTime(double jumpTime)
        {
            if (!isSimRunning)
                return;

            IGraphicalReport graphicalReport;
            XYIntLocation intLoc;
            bool timeForward;
            bool redrawNeeded = false;
            double timeNow = currTimeTick * secPerTick;
            IReport report;
            int jumpTick;

            if (timeNow < jumpTime)
            {
                timeForward = true;
                jumpTick = (int)Math.Floor(jumpTime / secPerTick);
            }
            else
            {
                timeForward = false;
                jumpTick = (int)Math.Ceiling(jumpTime / secPerTick);
            }

            if (timeForward)
                report = reporter.GetNext(jumpTick);
            else
                report = reporter.GetPrev(jumpTick);
            while ((report != null) && !(report is EmptyReport))
            {
                // Set aspect ratio on screen
                if (!firstResize)
                {
                    setAspect(report);
                }

                if (report is IGraphicalReport)
                {
                    graphicalReport = (IGraphicalReport)report;
                    graphicalReport.ConvertTo(panelDrawField);
                    graphicalReport.ConvertTo(fieldMultiplier);

                    intLoc = (XYIntLocation)report.Loc;

                    if (graphicalReport.IsStatic)
                    {
                        ProcessStaticReport(graphicalReport, timeForward);
                        redrawNeeded = true;
                    }
                }
                else if (report is IInfoReport)
                {
                    processInfoReport((IInfoReport)report, timeForward);
                }

                if (timeForward)
                    report = reporter.GetNext(jumpTick);
                else
                    report = reporter.GetPrev(jumpTick);
            }

            if (redrawNeeded)
                RedrawStatics();

            currTimeTick = jumpTick;
            sw.Reset();
            resetTimeTick = currTimeTick;

            string textTime = "Jumped to time ";
            int hours = (int)(Math.Floor(jumpTime / (60 * 60)));
            textTime = textTime + hours.ToString() + ":";
            int min = (int)(Math.Floor((jumpTime - hours * 60 * 60) / 60));
            if (min < 10)
                textTime = textTime + "0";
            textTime = textTime + min.ToString() + ":";
            double sec = jumpTime - hours * 60 * 60 - min * 60;
            if (sec < 10)
                textTime = textTime + "0";
            textTime = textTime + sec.ToString("0.000");

            label_Info.Text = textTime;
        }

        private void btn_Save_Sim_Click(object sender, EventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.DefaultExt = "mns";
            file.Filter = "MNSim Files (*.mns)|*.mns";
            file.CheckFileExists = false;
            file.CheckPathExists = true;
            file.FileName = "default.mns";
            file.OverwritePrompt = true;
            file.Title = "Choose Settings File";
            file.ValidateNames = true;
            file.ShowDialog();


        }

        private void btn_RestartSim_Click(object sender, EventArgs e)
        {
            if (!(MessageBox.Show("Do you really want to restart this simulation?",
                        "RESTART SIM?", MessageBoxButtons.YesNo) == DialogResult.Yes))
                return;

            EventArgsMessage args = new EventArgsMessage("Restarting Simulation...");

            btn_Stop_Sim_Click(sender, args);
            btn_RunSim_Click(sender, e);
        }

        private void text_IOFileTag_TextChanged(object sender, EventArgs e)
        {
            if (text_IOFileTag.Text.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > -1)
            {
                label_Info.Text = "Invalid File Tag.";
                text_IOFileTag.BackColor = Color.Yellow;
            }
            else
            {
                label_Info.Text = "";
                text_IOFileTag.BackColor = Color.White;
            }
        }

        private void btn_ChooseIOFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (System.IO.Directory.Exists(text_IOFolder.Text))
                dialog.SelectedPath = text_IOFolder.Text;
            dialog.ShowDialog();
            text_IOFolder.Text = dialog.SelectedPath;
        }

        private void text_IOFolder_TextChanged(object sender, EventArgs e)
        {
            if (text_IOFolder.Text.IndexOfAny(System.IO.Path.GetInvalidPathChars()) > -1)
            {
                label_Info.Text = "Invalid Folder Selected.";
                text_IOFolder.BackColor = Color.Yellow;
            }
            else
            {
                label_Info.Text = "";
                text_IOFolder.BackColor = Color.White;
            }
        }
    }
}
