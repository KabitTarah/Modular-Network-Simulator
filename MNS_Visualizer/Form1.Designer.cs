namespace MNS_Visualizer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Panel = new System.Windows.Forms.Panel();
            this.cb_IPhysicalProcessor = new System.Windows.Forms.ComboBox();
            this.tab_INode = new System.Windows.Forms.TabControl();
            this.tab_DLL = new System.Windows.Forms.TabPage();
            this.CurrentDir = new System.Windows.Forms.Label();
            this.btn_LoadDLL = new System.Windows.Forms.Button();
            this.lb_DLLs = new System.Windows.Forms.ListBox();
            this.tab_File = new System.Windows.Forms.TabPage();
            this.btn_Settings_Load_Record_ = new System.Windows.Forms.Button();
            this.btn_Settings_Save_Record_ = new System.Windows.Forms.Button();
            this.btn_Settings_Load_File_ = new System.Windows.Forms.Button();
            this.btn_Settings_Save = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.settings_Description = new System.Windows.Forms.RichTextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.settings_PhysicalProcessor = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.settings_AppEventGenerator = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.settings_NodeType = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.settings_Deployer = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.settings_Seed = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.settings_Records = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.settings_FileName = new System.Windows.Forms.TextBox();
            this.tab_Run = new System.Windows.Forms.TabPage();
            this.cb_AppSetsSink = new System.Windows.Forms.CheckBox();
            this.label_Multirun2 = new System.Windows.Forms.Label();
            this.tb_Multirun = new System.Windows.Forms.TextBox();
            this.label_Multirun1 = new System.Windows.Forms.Label();
            this.cb_Multirun = new System.Windows.Forms.CheckBox();
            this.cb_GraphicsOff = new System.Windows.Forms.CheckBox();
            this.cb_randomSink = new System.Windows.Forms.CheckBox();
            this.selected_GraphicsRunning = new System.Windows.Forms.Label();
            this.btn_RunSim = new System.Windows.Forms.Button();
            this.selected_IDeployer = new System.Windows.Forms.Label();
            this.selected_ILocation = new System.Windows.Forms.Label();
            this.selected_IRandomizerFactory = new System.Windows.Forms.Label();
            this.selected_IApplicationEventGenerator = new System.Windows.Forms.Label();
            this.selected_IPhysicalProcessor = new System.Windows.Forms.Label();
            this.selected_INodeFactory = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tab_Statistics = new System.Windows.Forms.TabPage();
            this.textClick_ID = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.tab_IO = new System.Windows.Forms.TabPage();
            this.label40 = new System.Windows.Forms.Label();
            this.text_IOFileTag = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.text_IOFolder = new System.Windows.Forms.TextBox();
            this.btn_ChooseIOFolder = new System.Windows.Forms.Button();
            this.tab_INodeFactory = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_INode = new System.Windows.Forms.ComboBox();
            this.tab_IPhysicalProcessor = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tab_IApplicationEventGenerator = new System.Windows.Forms.TabPage();
            this.cb_IApplicationEventGenerator = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tab_IDeployer = new System.Windows.Forms.TabPage();
            this.cb_IDeployer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tab_IRandomizerFactory = new System.Windows.Forms.TabPage();
            this.cb_IRandomizerFactory = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tab_ILocation = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.text_FieldY2 = new System.Windows.Forms.TextBox();
            this.text_FieldX2 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.text_FieldY1 = new System.Windows.Forms.TextBox();
            this.text_FieldX1 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cb_ILocation = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.FPS = new System.Windows.Forms.TextBox();
            this.labelFPS = new System.Windows.Forms.Label();
            this.btnToggleGraphics = new System.Windows.Forms.Button();
            this.label_TimeScale = new System.Windows.Forms.Label();
            this.TimeScale = new System.Windows.Forms.TextBox();
            this.cb_InvertScale = new System.Windows.Forms.CheckBox();
            this.label_Info = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label_SimTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.btn_ResizetabControl = new System.Windows.Forms.Button();
            this.text_JumpStepSize = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelTimeFactor = new System.Windows.Forms.Label();
            this.btn_Save_Sim = new System.Windows.Forms.PictureBox();
            this.text_JumpHrs = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.text_JumpMin = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.text_JumpSec = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btn_Load_Sim = new System.Windows.Forms.Button();
            this.btn_RestartSim = new System.Windows.Forms.Button();
            this.btn_Back_Sim = new System.Windows.Forms.Button();
            this.btn_Pause_Sim = new System.Windows.Forms.Button();
            this.btn_Advance_Sim = new System.Windows.Forms.Button();
            this.btn_Stop_Sim = new System.Windows.Forms.Button();
            this.btn_Jump_Sim = new System.Windows.Forms.Button();
            this.backgroundWorker_RunSim = new System.ComponentModel.BackgroundWorker();
            this.tab_INode.SuspendLayout();
            this.tab_DLL.SuspendLayout();
            this.tab_File.SuspendLayout();
            this.tab_Run.SuspendLayout();
            this.tab_Statistics.SuspendLayout();
            this.tab_IO.SuspendLayout();
            this.tab_INodeFactory.SuspendLayout();
            this.tab_IPhysicalProcessor.SuspendLayout();
            this.tab_IApplicationEventGenerator.SuspendLayout();
            this.tab_IDeployer.SuspendLayout();
            this.tab_IRandomizerFactory.SuspendLayout();
            this.tab_ILocation.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save_Sim)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            this.Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel.Location = new System.Drawing.Point(12, 12);
            this.Panel.Name = "Panel";
            this.Panel.Padding = new System.Windows.Forms.Padding(5);
            this.Panel.Size = new System.Drawing.Size(986, 742);
            this.Panel.TabIndex = 0;
            // 
            // cb_IPhysicalProcessor
            // 
            this.cb_IPhysicalProcessor.BackColor = System.Drawing.Color.Yellow;
            this.cb_IPhysicalProcessor.FormattingEnabled = true;
            this.cb_IPhysicalProcessor.Location = new System.Drawing.Point(6, 23);
            this.cb_IPhysicalProcessor.Name = "cb_IPhysicalProcessor";
            this.cb_IPhysicalProcessor.Size = new System.Drawing.Size(226, 21);
            this.cb_IPhysicalProcessor.TabIndex = 14;
            this.cb_IPhysicalProcessor.SelectedIndexChanged += new System.EventHandler(this.cb_IPhysicalProcessor_SelectedIndexChanged);
            // 
            // tab_INode
            // 
            this.tab_INode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tab_INode.Controls.Add(this.tab_DLL);
            this.tab_INode.Controls.Add(this.tab_File);
            this.tab_INode.Controls.Add(this.tab_Run);
            this.tab_INode.Controls.Add(this.tab_Statistics);
            this.tab_INode.Controls.Add(this.tab_IO);
            this.tab_INode.Controls.Add(this.tab_INodeFactory);
            this.tab_INode.Controls.Add(this.tab_IPhysicalProcessor);
            this.tab_INode.Controls.Add(this.tab_IApplicationEventGenerator);
            this.tab_INode.Controls.Add(this.tab_IDeployer);
            this.tab_INode.Controls.Add(this.tab_IRandomizerFactory);
            this.tab_INode.Controls.Add(this.tab_ILocation);
            this.tab_INode.Location = new System.Drawing.Point(1014, 12);
            this.tab_INode.Multiline = true;
            this.tab_INode.Name = "tab_INode";
            this.tab_INode.SelectedIndex = 0;
            this.tab_INode.Size = new System.Drawing.Size(246, 549);
            this.tab_INode.TabIndex = 1;
            // 
            // tab_DLL
            // 
            this.tab_DLL.Controls.Add(this.CurrentDir);
            this.tab_DLL.Controls.Add(this.btn_LoadDLL);
            this.tab_DLL.Controls.Add(this.lb_DLLs);
            this.tab_DLL.Location = new System.Drawing.Point(4, 76);
            this.tab_DLL.Name = "tab_DLL";
            this.tab_DLL.Padding = new System.Windows.Forms.Padding(3);
            this.tab_DLL.Size = new System.Drawing.Size(238, 469);
            this.tab_DLL.TabIndex = 0;
            this.tab_DLL.Text = "DLLs";
            this.tab_DLL.UseVisualStyleBackColor = true;
            // 
            // CurrentDir
            // 
            this.CurrentDir.AutoSize = true;
            this.CurrentDir.Location = new System.Drawing.Point(6, 39);
            this.CurrentDir.Name = "CurrentDir";
            this.CurrentDir.Size = new System.Drawing.Size(86, 13);
            this.CurrentDir.TabIndex = 40;
            this.CurrentDir.Text = "Current Directory";
            // 
            // btn_LoadDLL
            // 
            this.btn_LoadDLL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LoadDLL.Location = new System.Drawing.Point(6, 6);
            this.btn_LoadDLL.Name = "btn_LoadDLL";
            this.btn_LoadDLL.Size = new System.Drawing.Size(101, 30);
            this.btn_LoadDLL.TabIndex = 39;
            this.btn_LoadDLL.Text = "Load DLL:";
            this.btn_LoadDLL.UseVisualStyleBackColor = true;
            this.btn_LoadDLL.Click += new System.EventHandler(this.btn_LoadDLL_Click);
            // 
            // lb_DLLs
            // 
            this.lb_DLLs.FormattingEnabled = true;
            this.lb_DLLs.HorizontalScrollbar = true;
            this.lb_DLLs.Location = new System.Drawing.Point(6, 55);
            this.lb_DLLs.Name = "lb_DLLs";
            this.lb_DLLs.Size = new System.Drawing.Size(226, 407);
            this.lb_DLLs.Sorted = true;
            this.lb_DLLs.TabIndex = 2;
            // 
            // tab_File
            // 
            this.tab_File.Controls.Add(this.btn_Settings_Load_Record_);
            this.tab_File.Controls.Add(this.btn_Settings_Save_Record_);
            this.tab_File.Controls.Add(this.btn_Settings_Load_File_);
            this.tab_File.Controls.Add(this.btn_Settings_Save);
            this.tab_File.Controls.Add(this.textBox1);
            this.tab_File.Controls.Add(this.settings_Description);
            this.tab_File.Controls.Add(this.label39);
            this.tab_File.Controls.Add(this.settings_PhysicalProcessor);
            this.tab_File.Controls.Add(this.label38);
            this.tab_File.Controls.Add(this.settings_AppEventGenerator);
            this.tab_File.Controls.Add(this.label37);
            this.tab_File.Controls.Add(this.settings_NodeType);
            this.tab_File.Controls.Add(this.label36);
            this.tab_File.Controls.Add(this.settings_Deployer);
            this.tab_File.Controls.Add(this.label35);
            this.tab_File.Controls.Add(this.settings_Seed);
            this.tab_File.Controls.Add(this.label34);
            this.tab_File.Controls.Add(this.label32);
            this.tab_File.Controls.Add(this.label31);
            this.tab_File.Controls.Add(this.settings_Records);
            this.tab_File.Controls.Add(this.label30);
            this.tab_File.Controls.Add(this.settings_FileName);
            this.tab_File.Location = new System.Drawing.Point(4, 76);
            this.tab_File.Name = "tab_File";
            this.tab_File.Padding = new System.Windows.Forms.Padding(3);
            this.tab_File.Size = new System.Drawing.Size(238, 469);
            this.tab_File.TabIndex = 10;
            this.tab_File.Text = "File";
            this.tab_File.UseVisualStyleBackColor = true;
            // 
            // btn_Settings_Load_Record_
            // 
            this.btn_Settings_Load_Record_.Enabled = false;
            this.btn_Settings_Load_Record_.FlatAppearance.BorderSize = 0;
            this.btn_Settings_Load_Record_.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Settings_Load_Record_.Image = ((System.Drawing.Image)(resources.GetObject("btn_Settings_Load_Record_.Image")));
            this.btn_Settings_Load_Record_.Location = new System.Drawing.Point(190, 439);
            this.btn_Settings_Load_Record_.Name = "btn_Settings_Load_Record_";
            this.btn_Settings_Load_Record_.Size = new System.Drawing.Size(25, 25);
            this.btn_Settings_Load_Record_.TabIndex = 100007;
            this.toolTip1.SetToolTip(this.btn_Settings_Load_Record_, "Load Settings Record");
            this.btn_Settings_Load_Record_.UseVisualStyleBackColor = true;
            // 
            // btn_Settings_Save_Record_
            // 
            this.btn_Settings_Save_Record_.Enabled = false;
            this.btn_Settings_Save_Record_.FlatAppearance.BorderSize = 0;
            this.btn_Settings_Save_Record_.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Settings_Save_Record_.Image = ((System.Drawing.Image)(resources.GetObject("btn_Settings_Save_Record_.Image")));
            this.btn_Settings_Save_Record_.Location = new System.Drawing.Point(23, 439);
            this.btn_Settings_Save_Record_.Name = "btn_Settings_Save_Record_";
            this.btn_Settings_Save_Record_.Size = new System.Drawing.Size(25, 25);
            this.btn_Settings_Save_Record_.TabIndex = 100006;
            this.toolTip1.SetToolTip(this.btn_Settings_Save_Record_, "Save Settings Record");
            this.btn_Settings_Save_Record_.UseVisualStyleBackColor = true;
            // 
            // btn_Settings_Load_File_
            // 
            this.btn_Settings_Load_File_.Enabled = false;
            this.btn_Settings_Load_File_.FlatAppearance.BorderSize = 0;
            this.btn_Settings_Load_File_.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Settings_Load_File_.Image = ((System.Drawing.Image)(resources.GetObject("btn_Settings_Load_File_.Image")));
            this.btn_Settings_Load_File_.Location = new System.Drawing.Point(208, 6);
            this.btn_Settings_Load_File_.Name = "btn_Settings_Load_File_";
            this.btn_Settings_Load_File_.Size = new System.Drawing.Size(25, 25);
            this.btn_Settings_Load_File_.TabIndex = 100001;
            this.toolTip1.SetToolTip(this.btn_Settings_Load_File_, "Choose File");
            this.btn_Settings_Load_File_.UseVisualStyleBackColor = true;
            // 
            // btn_Settings_Save
            // 
            this.btn_Settings_Save.Enabled = false;
            this.btn_Settings_Save.FlatAppearance.BorderSize = 0;
            this.btn_Settings_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Settings_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Settings_Save.Image")));
            this.btn_Settings_Save.Location = new System.Drawing.Point(178, 6);
            this.btn_Settings_Save.Name = "btn_Settings_Save";
            this.btn_Settings_Save.Size = new System.Drawing.Size(25, 25);
            this.btn_Settings_Save.TabIndex = 100000;
            this.toolTip1.SetToolTip(this.btn_Settings_Save, "Save Simulator Settings");
            this.btn_Settings_Save.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(23, 119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(192, 20);
            this.textBox1.TabIndex = 100004;
            this.textBox1.Text = "Empty Record 0";
            // 
            // settings_Description
            // 
            this.settings_Description.Location = new System.Drawing.Point(23, 298);
            this.settings_Description.Name = "settings_Description";
            this.settings_Description.Size = new System.Drawing.Size(192, 128);
            this.settings_Description.TabIndex = 100005;
            this.settings_Description.Text = "";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(9, 282);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(63, 13);
            this.label39.TabIndex = 79;
            this.label39.Text = "Description:";
            // 
            // settings_PhysicalProcessor
            // 
            this.settings_PhysicalProcessor.AutoSize = true;
            this.settings_PhysicalProcessor.Location = new System.Drawing.Point(20, 267);
            this.settings_PhysicalProcessor.Name = "settings_PhysicalProcessor";
            this.settings_PhysicalProcessor.Size = new System.Drawing.Size(74, 13);
            this.settings_PhysicalProcessor.TabIndex = 78;
            this.settings_PhysicalProcessor.Text = "Empty Record";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(9, 254);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(99, 13);
            this.label38.TabIndex = 77;
            this.label38.Text = "Physical Processor:";
            // 
            // settings_AppEventGenerator
            // 
            this.settings_AppEventGenerator.AutoSize = true;
            this.settings_AppEventGenerator.Location = new System.Drawing.Point(20, 239);
            this.settings_AppEventGenerator.Name = "settings_AppEventGenerator";
            this.settings_AppEventGenerator.Size = new System.Drawing.Size(74, 13);
            this.settings_AppEventGenerator.TabIndex = 76;
            this.settings_AppEventGenerator.Text = "Empty Record";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(9, 226);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(143, 13);
            this.label37.TabIndex = 75;
            this.label37.Text = "Application Event Generator:";
            // 
            // settings_NodeType
            // 
            this.settings_NodeType.AutoSize = true;
            this.settings_NodeType.Location = new System.Drawing.Point(20, 155);
            this.settings_NodeType.Name = "settings_NodeType";
            this.settings_NodeType.Size = new System.Drawing.Size(74, 13);
            this.settings_NodeType.TabIndex = 74;
            this.settings_NodeType.Text = "Empty Record";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(9, 142);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(78, 13);
            this.label36.TabIndex = 73;
            this.label36.Text = "Node Protocol:";
            // 
            // settings_Deployer
            // 
            this.settings_Deployer.AutoSize = true;
            this.settings_Deployer.Location = new System.Drawing.Point(20, 211);
            this.settings_Deployer.Name = "settings_Deployer";
            this.settings_Deployer.Size = new System.Drawing.Size(74, 13);
            this.settings_Deployer.TabIndex = 72;
            this.settings_Deployer.Text = "Empty Record";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(9, 198);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(49, 13);
            this.label35.TabIndex = 71;
            this.label35.Text = "Deployer";
            // 
            // settings_Seed
            // 
            this.settings_Seed.AutoSize = true;
            this.settings_Seed.Location = new System.Drawing.Point(20, 183);
            this.settings_Seed.Name = "settings_Seed";
            this.settings_Seed.Size = new System.Drawing.Size(74, 13);
            this.settings_Seed.TabIndex = 70;
            this.settings_Seed.Text = "Empty Record";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(9, 170);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(35, 13);
            this.label34.TabIndex = 69;
            this.label34.Text = "Seed:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(9, 105);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(38, 13);
            this.label32.TabIndex = 67;
            this.label32.Text = "Name:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 65);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(125, 13);
            this.label31.TabIndex = 4;
            this.label31.Text = "Choose Settings Record:";
            // 
            // settings_Records
            // 
            this.settings_Records.FormattingEnabled = true;
            this.settings_Records.Items.AddRange(new object[] {
            "Empty Settings 0",
            "Empty Settings 1",
            "Empty Settings 2",
            "Empty Settings 3",
            "Empty Settings 4",
            "Empty Settings 5",
            "Empty Settings 6",
            "Empty Settings 7",
            "Empty Settings 8",
            "Empty Settings 9"});
            this.settings_Records.Location = new System.Drawing.Point(6, 81);
            this.settings_Records.MaxDropDownItems = 10;
            this.settings_Records.Name = "settings_Records";
            this.settings_Records.Size = new System.Drawing.Size(226, 21);
            this.settings_Records.TabIndex = 100003;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(6, 24);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(67, 13);
            this.label30.TabIndex = 2;
            this.label30.Text = "Settings File:";
            // 
            // settings_FileName
            // 
            this.settings_FileName.Location = new System.Drawing.Point(6, 40);
            this.settings_FileName.Name = "settings_FileName";
            this.settings_FileName.Size = new System.Drawing.Size(226, 20);
            this.settings_FileName.TabIndex = 100002;
            // 
            // tab_Run
            // 
            this.tab_Run.Controls.Add(this.cb_AppSetsSink);
            this.tab_Run.Controls.Add(this.label_Multirun2);
            this.tab_Run.Controls.Add(this.tb_Multirun);
            this.tab_Run.Controls.Add(this.label_Multirun1);
            this.tab_Run.Controls.Add(this.cb_Multirun);
            this.tab_Run.Controls.Add(this.cb_GraphicsOff);
            this.tab_Run.Controls.Add(this.cb_randomSink);
            this.tab_Run.Controls.Add(this.selected_GraphicsRunning);
            this.tab_Run.Controls.Add(this.btn_RunSim);
            this.tab_Run.Controls.Add(this.selected_IDeployer);
            this.tab_Run.Controls.Add(this.selected_ILocation);
            this.tab_Run.Controls.Add(this.selected_IRandomizerFactory);
            this.tab_Run.Controls.Add(this.selected_IApplicationEventGenerator);
            this.tab_Run.Controls.Add(this.selected_IPhysicalProcessor);
            this.tab_Run.Controls.Add(this.selected_INodeFactory);
            this.tab_Run.Controls.Add(this.label6);
            this.tab_Run.Location = new System.Drawing.Point(4, 76);
            this.tab_Run.Name = "tab_Run";
            this.tab_Run.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Run.Size = new System.Drawing.Size(238, 469);
            this.tab_Run.TabIndex = 9;
            this.tab_Run.Text = "Run";
            this.tab_Run.UseVisualStyleBackColor = true;
            // 
            // cb_AppSetsSink
            // 
            this.cb_AppSetsSink.AutoSize = true;
            this.cb_AppSetsSink.Location = new System.Drawing.Point(28, 178);
            this.cb_AppSetsSink.Name = "cb_AppSetsSink";
            this.cb_AppSetsSink.Size = new System.Drawing.Size(161, 17);
            this.cb_AppSetsSink.TabIndex = 21;
            this.cb_AppSetsSink.Text = "Application Sets Sink Node?";
            this.toolTip1.SetToolTip(this.cb_AppSetsSink, "If unchecked, the furthest nodes will be used for the sink and event respectively" +
                    ".");
            this.cb_AppSetsSink.UseVisualStyleBackColor = true;
            // 
            // label_Multirun2
            // 
            this.label_Multirun2.AutoSize = true;
            this.label_Multirun2.Location = new System.Drawing.Point(132, 267);
            this.label_Multirun2.Name = "label_Multirun2";
            this.label_Multirun2.Size = new System.Drawing.Size(34, 13);
            this.label_Multirun2.TabIndex = 20;
            this.label_Multirun2.Text = "times.";
            // 
            // tb_Multirun
            // 
            this.tb_Multirun.BackColor = System.Drawing.Color.White;
            this.tb_Multirun.Location = new System.Drawing.Point(82, 264);
            this.tb_Multirun.Name = "tb_Multirun";
            this.tb_Multirun.Size = new System.Drawing.Size(47, 20);
            this.tb_Multirun.TabIndex = 19;
            this.tb_Multirun.Text = "1000";
            this.tb_Multirun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Multirun1
            // 
            this.label_Multirun1.AutoSize = true;
            this.label_Multirun1.Location = new System.Drawing.Point(48, 267);
            this.label_Multirun1.Name = "label_Multirun1";
            this.label_Multirun1.Size = new System.Drawing.Size(27, 13);
            this.label_Multirun1.TabIndex = 18;
            this.label_Multirun1.Text = "Run";
            // 
            // cb_Multirun
            // 
            this.cb_Multirun.AutoSize = true;
            this.cb_Multirun.Location = new System.Drawing.Point(30, 247);
            this.cb_Multirun.Name = "cb_Multirun";
            this.cb_Multirun.Size = new System.Drawing.Size(66, 17);
            this.cb_Multirun.TabIndex = 17;
            this.cb_Multirun.Text = "Multi-run";
            this.cb_Multirun.UseVisualStyleBackColor = true;
            // 
            // cb_GraphicsOff
            // 
            this.cb_GraphicsOff.AutoSize = true;
            this.cb_GraphicsOff.Location = new System.Drawing.Point(30, 224);
            this.cb_GraphicsOff.Name = "cb_GraphicsOff";
            this.cb_GraphicsOff.Size = new System.Drawing.Size(85, 17);
            this.cb_GraphicsOff.TabIndex = 16;
            this.cb_GraphicsOff.Text = "Graphics Off";
            this.cb_GraphicsOff.UseVisualStyleBackColor = true;
            // 
            // cb_randomSink
            // 
            this.cb_randomSink.AutoSize = true;
            this.cb_randomSink.Location = new System.Drawing.Point(29, 201);
            this.cb_randomSink.Name = "cb_randomSink";
            this.cb_randomSink.Size = new System.Drawing.Size(138, 17);
            this.cb_randomSink.TabIndex = 10;
            this.cb_randomSink.Text = "Randomized Sink Node";
            this.toolTip1.SetToolTip(this.cb_randomSink, "If unchecked, the furthest nodes will be used for the sink and event respectively" +
                    ".");
            this.cb_randomSink.UseVisualStyleBackColor = true;
            // 
            // selected_GraphicsRunning
            // 
            this.selected_GraphicsRunning.AutoSize = true;
            this.selected_GraphicsRunning.ForeColor = System.Drawing.Color.Green;
            this.selected_GraphicsRunning.Location = new System.Drawing.Point(27, 108);
            this.selected_GraphicsRunning.Name = "selected_GraphicsRunning";
            this.selected_GraphicsRunning.Size = new System.Drawing.Size(95, 13);
            this.selected_GraphicsRunning.TabIndex = 9;
            this.selected_GraphicsRunning.Text = "Graphics: Running";
            // 
            // btn_RunSim
            // 
            this.btn_RunSim.Location = new System.Drawing.Point(3, 137);
            this.btn_RunSim.Name = "btn_RunSim";
            this.btn_RunSim.Size = new System.Drawing.Size(226, 23);
            this.btn_RunSim.TabIndex = 8;
            this.btn_RunSim.Text = "Run Simulation";
            this.btn_RunSim.UseVisualStyleBackColor = true;
            this.btn_RunSim.Click += new System.EventHandler(this.btn_Run_Sim_Click);
            // 
            // selected_IDeployer
            // 
            this.selected_IDeployer.AutoSize = true;
            this.selected_IDeployer.ForeColor = System.Drawing.Color.Red;
            this.selected_IDeployer.Location = new System.Drawing.Point(27, 95);
            this.selected_IDeployer.Name = "selected_IDeployer";
            this.selected_IDeployer.Size = new System.Drawing.Size(129, 13);
            this.selected_IDeployer.TabIndex = 7;
            this.selected_IDeployer.Text = "IDeployer: None Selected";
            // 
            // selected_ILocation
            // 
            this.selected_ILocation.AutoSize = true;
            this.selected_ILocation.ForeColor = System.Drawing.Color.Red;
            this.selected_ILocation.Location = new System.Drawing.Point(27, 82);
            this.selected_ILocation.Name = "selected_ILocation";
            this.selected_ILocation.Size = new System.Drawing.Size(128, 13);
            this.selected_ILocation.TabIndex = 6;
            this.selected_ILocation.Text = "ILocation: None Selected";
            // 
            // selected_IRandomizerFactory
            // 
            this.selected_IRandomizerFactory.AutoSize = true;
            this.selected_IRandomizerFactory.ForeColor = System.Drawing.Color.Red;
            this.selected_IRandomizerFactory.Location = new System.Drawing.Point(27, 69);
            this.selected_IRandomizerFactory.Name = "selected_IRandomizerFactory";
            this.selected_IRandomizerFactory.Size = new System.Drawing.Size(178, 13);
            this.selected_IRandomizerFactory.TabIndex = 5;
            this.selected_IRandomizerFactory.Text = "IRandomizerFactory: None Selected";
            // 
            // selected_IApplicationEventGenerator
            // 
            this.selected_IApplicationEventGenerator.AutoSize = true;
            this.selected_IApplicationEventGenerator.ForeColor = System.Drawing.Color.Red;
            this.selected_IApplicationEventGenerator.Location = new System.Drawing.Point(27, 56);
            this.selected_IApplicationEventGenerator.Name = "selected_IApplicationEventGenerator";
            this.selected_IApplicationEventGenerator.Size = new System.Drawing.Size(214, 13);
            this.selected_IApplicationEventGenerator.TabIndex = 4;
            this.selected_IApplicationEventGenerator.Text = "IApplicationEventGenerator: None Selected";
            // 
            // selected_IPhysicalProcessor
            // 
            this.selected_IPhysicalProcessor.AutoSize = true;
            this.selected_IPhysicalProcessor.ForeColor = System.Drawing.Color.Red;
            this.selected_IPhysicalProcessor.Location = new System.Drawing.Point(27, 43);
            this.selected_IPhysicalProcessor.Name = "selected_IPhysicalProcessor";
            this.selected_IPhysicalProcessor.Size = new System.Drawing.Size(173, 13);
            this.selected_IPhysicalProcessor.TabIndex = 2;
            this.selected_IPhysicalProcessor.Text = "IPhysicalProcessor: None Selected";
            // 
            // selected_INodeFactory
            // 
            this.selected_INodeFactory.AutoSize = true;
            this.selected_INodeFactory.ForeColor = System.Drawing.Color.Red;
            this.selected_INodeFactory.Location = new System.Drawing.Point(27, 30);
            this.selected_INodeFactory.Name = "selected_INodeFactory";
            this.selected_INodeFactory.Size = new System.Drawing.Size(148, 13);
            this.selected_INodeFactory.TabIndex = 1;
            this.selected_INodeFactory.Text = "INodeFactory: None Selected";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Selected Modules:";
            // 
            // tab_Statistics
            // 
            this.tab_Statistics.Controls.Add(this.textClick_ID);
            this.tab_Statistics.Controls.Add(this.label41);
            this.tab_Statistics.Location = new System.Drawing.Point(4, 76);
            this.tab_Statistics.Name = "tab_Statistics";
            this.tab_Statistics.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Statistics.Size = new System.Drawing.Size(238, 469);
            this.tab_Statistics.TabIndex = 8;
            this.tab_Statistics.Text = "Statistics";
            this.tab_Statistics.UseVisualStyleBackColor = true;
            // 
            // textClick_ID
            // 
            this.textClick_ID.AutoSize = true;
            this.textClick_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textClick_ID.Location = new System.Drawing.Point(69, 14);
            this.textClick_ID.Name = "textClick_ID";
            this.textClick_ID.Size = new System.Drawing.Size(14, 13);
            this.textClick_ID.TabIndex = 1;
            this.textClick_ID.Text = "0";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(6, 14);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(50, 13);
            this.label41.TabIndex = 0;
            this.label41.Text = "Node ID:";
            // 
            // tab_IO
            // 
            this.tab_IO.Controls.Add(this.label40);
            this.tab_IO.Controls.Add(this.text_IOFileTag);
            this.tab_IO.Controls.Add(this.label33);
            this.tab_IO.Controls.Add(this.text_IOFolder);
            this.tab_IO.Controls.Add(this.btn_ChooseIOFolder);
            this.tab_IO.Location = new System.Drawing.Point(4, 76);
            this.tab_IO.Name = "tab_IO";
            this.tab_IO.Padding = new System.Windows.Forms.Padding(3);
            this.tab_IO.Size = new System.Drawing.Size(238, 469);
            this.tab_IO.TabIndex = 11;
            this.tab_IO.Text = "IO";
            this.tab_IO.UseVisualStyleBackColor = true;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(37, 53);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(48, 13);
            this.label40.TabIndex = 100005;
            this.label40.Text = "File Tag:";
            // 
            // text_IOFileTag
            // 
            this.text_IOFileTag.Location = new System.Drawing.Point(37, 69);
            this.text_IOFileTag.Name = "text_IOFileTag";
            this.text_IOFileTag.Size = new System.Drawing.Size(104, 20);
            this.text_IOFileTag.TabIndex = 100004;
            this.toolTip1.SetToolTip(this.text_IOFileTag, "Prepend output file names with this string.");
            this.text_IOFileTag.TextChanged += new System.EventHandler(this.text_IOFileTag_TextChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(34, 3);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(107, 13);
            this.label33.TabIndex = 100003;
            this.label33.Text = "Select Output Folder:";
            // 
            // text_IOFolder
            // 
            this.text_IOFolder.Location = new System.Drawing.Point(37, 19);
            this.text_IOFolder.Name = "text_IOFolder";
            this.text_IOFolder.Size = new System.Drawing.Size(192, 20);
            this.text_IOFolder.TabIndex = 100002;
            this.text_IOFolder.TextChanged += new System.EventHandler(this.text_IOFolder_TextChanged);
            // 
            // btn_ChooseIOFolder
            // 
            this.btn_ChooseIOFolder.FlatAppearance.BorderSize = 0;
            this.btn_ChooseIOFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ChooseIOFolder.Image = ((System.Drawing.Image)(resources.GetObject("btn_ChooseIOFolder.Image")));
            this.btn_ChooseIOFolder.Location = new System.Drawing.Point(6, 16);
            this.btn_ChooseIOFolder.Name = "btn_ChooseIOFolder";
            this.btn_ChooseIOFolder.Size = new System.Drawing.Size(25, 25);
            this.btn_ChooseIOFolder.TabIndex = 100001;
            this.toolTip1.SetToolTip(this.btn_ChooseIOFolder, "Save Simulator Settings");
            this.btn_ChooseIOFolder.UseVisualStyleBackColor = true;
            this.btn_ChooseIOFolder.Click += new System.EventHandler(this.btn_ChooseIOFolder_Click);
            // 
            // tab_INodeFactory
            // 
            this.tab_INodeFactory.Controls.Add(this.label2);
            this.tab_INodeFactory.Controls.Add(this.cb_INode);
            this.tab_INodeFactory.Location = new System.Drawing.Point(4, 76);
            this.tab_INodeFactory.Name = "tab_INodeFactory";
            this.tab_INodeFactory.Padding = new System.Windows.Forms.Padding(3);
            this.tab_INodeFactory.Size = new System.Drawing.Size(238, 469);
            this.tab_INodeFactory.TabIndex = 2;
            this.tab_INodeFactory.Text = "INodeFactory";
            this.tab_INodeFactory.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Select INodeFactory:";
            // 
            // cb_INode
            // 
            this.cb_INode.BackColor = System.Drawing.Color.Yellow;
            this.cb_INode.FormattingEnabled = true;
            this.cb_INode.Location = new System.Drawing.Point(3, 23);
            this.cb_INode.Name = "cb_INode";
            this.cb_INode.Size = new System.Drawing.Size(226, 21);
            this.cb_INode.TabIndex = 2;
            this.cb_INode.SelectedIndexChanged += new System.EventHandler(this.cb_INode_SelectedIndexChanged);
            // 
            // tab_IPhysicalProcessor
            // 
            this.tab_IPhysicalProcessor.Controls.Add(this.cb_IPhysicalProcessor);
            this.tab_IPhysicalProcessor.Controls.Add(this.label3);
            this.tab_IPhysicalProcessor.Location = new System.Drawing.Point(4, 76);
            this.tab_IPhysicalProcessor.Name = "tab_IPhysicalProcessor";
            this.tab_IPhysicalProcessor.Padding = new System.Windows.Forms.Padding(3);
            this.tab_IPhysicalProcessor.Size = new System.Drawing.Size(238, 469);
            this.tab_IPhysicalProcessor.TabIndex = 3;
            this.tab_IPhysicalProcessor.Text = "IPhysicalProcessor";
            this.tab_IPhysicalProcessor.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "Select IPhysicalProcessor:";
            // 
            // tab_IApplicationEventGenerator
            // 
            this.tab_IApplicationEventGenerator.Controls.Add(this.cb_IApplicationEventGenerator);
            this.tab_IApplicationEventGenerator.Controls.Add(this.label1);
            this.tab_IApplicationEventGenerator.Controls.Add(this.label8);
            this.tab_IApplicationEventGenerator.Location = new System.Drawing.Point(4, 76);
            this.tab_IApplicationEventGenerator.Name = "tab_IApplicationEventGenerator";
            this.tab_IApplicationEventGenerator.Padding = new System.Windows.Forms.Padding(3);
            this.tab_IApplicationEventGenerator.Size = new System.Drawing.Size(238, 469);
            this.tab_IApplicationEventGenerator.TabIndex = 1;
            this.tab_IApplicationEventGenerator.Text = "IApplicationEventGenerator";
            this.tab_IApplicationEventGenerator.UseVisualStyleBackColor = true;
            // 
            // cb_IApplicationEventGenerator
            // 
            this.cb_IApplicationEventGenerator.BackColor = System.Drawing.Color.Yellow;
            this.cb_IApplicationEventGenerator.FormattingEnabled = true;
            this.cb_IApplicationEventGenerator.Location = new System.Drawing.Point(6, 40);
            this.cb_IApplicationEventGenerator.Name = "cb_IApplicationEventGenerator";
            this.cb_IApplicationEventGenerator.Size = new System.Drawing.Size(226, 21);
            this.cb_IApplicationEventGenerator.TabIndex = 25;
            this.cb_IApplicationEventGenerator.SelectedIndexChanged += new System.EventHandler(this.cb_IApplicationEventGenerator_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 24;
            this.label1.Text = "Select";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(212, 17);
            this.label8.TabIndex = 23;
            this.label8.Text = "IApplicationEventGenerator:";
            // 
            // tab_IDeployer
            // 
            this.tab_IDeployer.Controls.Add(this.cb_IDeployer);
            this.tab_IDeployer.Controls.Add(this.label4);
            this.tab_IDeployer.Location = new System.Drawing.Point(4, 76);
            this.tab_IDeployer.Name = "tab_IDeployer";
            this.tab_IDeployer.Padding = new System.Windows.Forms.Padding(3);
            this.tab_IDeployer.Size = new System.Drawing.Size(238, 469);
            this.tab_IDeployer.TabIndex = 7;
            this.tab_IDeployer.Text = "IDeployer";
            this.tab_IDeployer.UseVisualStyleBackColor = true;
            // 
            // cb_IDeployer
            // 
            this.cb_IDeployer.BackColor = System.Drawing.Color.Yellow;
            this.cb_IDeployer.FormattingEnabled = true;
            this.cb_IDeployer.Location = new System.Drawing.Point(6, 23);
            this.cb_IDeployer.Name = "cb_IDeployer";
            this.cb_IDeployer.Size = new System.Drawing.Size(226, 21);
            this.cb_IDeployer.TabIndex = 16;
            this.cb_IDeployer.SelectedIndexChanged += new System.EventHandler(this.cb_IDeployer_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Select IDeployer:";
            // 
            // tab_IRandomizerFactory
            // 
            this.tab_IRandomizerFactory.Controls.Add(this.cb_IRandomizerFactory);
            this.tab_IRandomizerFactory.Controls.Add(this.label9);
            this.tab_IRandomizerFactory.Location = new System.Drawing.Point(4, 76);
            this.tab_IRandomizerFactory.Name = "tab_IRandomizerFactory";
            this.tab_IRandomizerFactory.Padding = new System.Windows.Forms.Padding(3);
            this.tab_IRandomizerFactory.Size = new System.Drawing.Size(238, 469);
            this.tab_IRandomizerFactory.TabIndex = 5;
            this.tab_IRandomizerFactory.Text = "IRandomizerFactory";
            this.tab_IRandomizerFactory.UseVisualStyleBackColor = true;
            // 
            // cb_IRandomizerFactory
            // 
            this.cb_IRandomizerFactory.BackColor = System.Drawing.Color.Yellow;
            this.cb_IRandomizerFactory.FormattingEnabled = true;
            this.cb_IRandomizerFactory.Location = new System.Drawing.Point(6, 23);
            this.cb_IRandomizerFactory.Name = "cb_IRandomizerFactory";
            this.cb_IRandomizerFactory.Size = new System.Drawing.Size(226, 21);
            this.cb_IRandomizerFactory.TabIndex = 25;
            this.cb_IRandomizerFactory.SelectedIndexChanged += new System.EventHandler(this.cb_IRandomizerFactory_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(207, 17);
            this.label9.TabIndex = 24;
            this.label9.Text = "Select IRandomizerFactory:";
            // 
            // tab_ILocation
            // 
            this.tab_ILocation.Controls.Add(this.label17);
            this.tab_ILocation.Controls.Add(this.text_FieldY2);
            this.tab_ILocation.Controls.Add(this.text_FieldX2);
            this.tab_ILocation.Controls.Add(this.label18);
            this.tab_ILocation.Controls.Add(this.label19);
            this.tab_ILocation.Controls.Add(this.label20);
            this.tab_ILocation.Controls.Add(this.label16);
            this.tab_ILocation.Controls.Add(this.text_FieldY1);
            this.tab_ILocation.Controls.Add(this.text_FieldX1);
            this.tab_ILocation.Controls.Add(this.label15);
            this.tab_ILocation.Controls.Add(this.label14);
            this.tab_ILocation.Controls.Add(this.label13);
            this.tab_ILocation.Controls.Add(this.label12);
            this.tab_ILocation.Controls.Add(this.cb_ILocation);
            this.tab_ILocation.Controls.Add(this.label5);
            this.tab_ILocation.Location = new System.Drawing.Point(4, 76);
            this.tab_ILocation.Name = "tab_ILocation";
            this.tab_ILocation.Padding = new System.Windows.Forms.Padding(3);
            this.tab_ILocation.Size = new System.Drawing.Size(238, 469);
            this.tab_ILocation.TabIndex = 6;
            this.tab_ILocation.Text = "ILocation";
            this.tab_ILocation.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(161, 131);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(14, 17);
            this.label17.TabIndex = 40;
            this.label17.Text = ")";
            // 
            // text_FieldY2
            // 
            this.text_FieldY2.BackColor = System.Drawing.Color.White;
            this.text_FieldY2.Location = new System.Drawing.Point(116, 131);
            this.text_FieldY2.Name = "text_FieldY2";
            this.text_FieldY2.Size = new System.Drawing.Size(45, 20);
            this.text_FieldY2.TabIndex = 41;
            this.text_FieldY2.Text = "6000";
            this.text_FieldY2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // text_FieldX2
            // 
            this.text_FieldX2.BackColor = System.Drawing.Color.White;
            this.text_FieldX2.Location = new System.Drawing.Point(62, 131);
            this.text_FieldX2.Name = "text_FieldX2";
            this.text_FieldX2.Size = new System.Drawing.Size(45, 20);
            this.text_FieldX2.TabIndex = 39;
            this.text_FieldX2.Text = "6000";
            this.text_FieldX2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(106, 134);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(13, 17);
            this.label18.TabIndex = 44;
            this.label18.Text = ",";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(49, 131);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 17);
            this.label19.TabIndex = 43;
            this.label19.Text = "(";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(28, 115);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(63, 13);
            this.label20.TabIndex = 42;
            this.label20.Text = "Final Corner";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(161, 86);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(14, 17);
            this.label16.TabIndex = 38;
            this.label16.Text = ")";
            // 
            // text_FieldY1
            // 
            this.text_FieldY1.BackColor = System.Drawing.Color.White;
            this.text_FieldY1.Location = new System.Drawing.Point(116, 86);
            this.text_FieldY1.Name = "text_FieldY1";
            this.text_FieldY1.Size = new System.Drawing.Size(45, 20);
            this.text_FieldY1.TabIndex = 34;
            this.text_FieldY1.Text = "0";
            this.text_FieldY1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // text_FieldX1
            // 
            this.text_FieldX1.BackColor = System.Drawing.Color.White;
            this.text_FieldX1.Location = new System.Drawing.Point(62, 86);
            this.text_FieldX1.Name = "text_FieldX1";
            this.text_FieldX1.Size = new System.Drawing.Size(45, 20);
            this.text_FieldX1.TabIndex = 33;
            this.text_FieldX1.Text = "0";
            this.text_FieldX1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(106, 89);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(13, 17);
            this.label15.TabIndex = 37;
            this.label15.Text = ",";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(49, 86);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 17);
            this.label14.TabIndex = 36;
            this.label14.Text = "(";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 70);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "Initial Corner";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 17);
            this.label12.TabIndex = 31;
            this.label12.Text = "Field Definition";
            // 
            // cb_ILocation
            // 
            this.cb_ILocation.BackColor = System.Drawing.Color.Yellow;
            this.cb_ILocation.FormattingEnabled = true;
            this.cb_ILocation.Location = new System.Drawing.Point(6, 23);
            this.cb_ILocation.Name = "cb_ILocation";
            this.cb_ILocation.Size = new System.Drawing.Size(226, 21);
            this.cb_ILocation.TabIndex = 30;
            this.cb_ILocation.SelectedIndexChanged += new System.EventHandler(this.cb_ILocation_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 17);
            this.label5.TabIndex = 29;
            this.label5.Text = "Select ILocation:";
            // 
            // FPS
            // 
            this.FPS.Location = new System.Drawing.Point(1048, 733);
            this.FPS.Name = "FPS";
            this.FPS.Size = new System.Drawing.Size(45, 20);
            this.FPS.TabIndex = 14;
            this.FPS.Text = "15";
            this.FPS.TextChanged += new System.EventHandler(this.FPS_TextChanged);
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Location = new System.Drawing.Point(1015, 736);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(27, 13);
            this.labelFPS.TabIndex = 30;
            this.labelFPS.Text = "FPS";
            // 
            // btnToggleGraphics
            // 
            this.btnToggleGraphics.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleGraphics.Location = new System.Drawing.Point(1128, 731);
            this.btnToggleGraphics.Name = "btnToggleGraphics";
            this.btnToggleGraphics.Size = new System.Drawing.Size(132, 23);
            this.btnToggleGraphics.TabIndex = 15;
            this.btnToggleGraphics.Text = "Stop Graphics";
            this.btnToggleGraphics.UseVisualStyleBackColor = true;
            this.btnToggleGraphics.Click += new System.EventHandler(this.btnToggleGraphics_Click);
            // 
            // label_TimeScale
            // 
            this.label_TimeScale.AutoSize = true;
            this.label_TimeScale.Location = new System.Drawing.Point(1015, 707);
            this.label_TimeScale.Name = "label_TimeScale";
            this.label_TimeScale.Size = new System.Drawing.Size(99, 13);
            this.label_TimeScale.TabIndex = 33;
            this.label_TimeScale.Text = "Real Sec : Sim Sec";
            // 
            // TimeScale
            // 
            this.TimeScale.Location = new System.Drawing.Point(1128, 704);
            this.TimeScale.Name = "TimeScale";
            this.TimeScale.Size = new System.Drawing.Size(45, 20);
            this.TimeScale.TabIndex = 12;
            this.TimeScale.Text = "5";
            this.TimeScale.TextChanged += new System.EventHandler(this.TimeScale_TextChanged);
            // 
            // cb_InvertScale
            // 
            this.cb_InvertScale.AutoSize = true;
            this.cb_InvertScale.Location = new System.Drawing.Point(1180, 706);
            this.cb_InvertScale.Name = "cb_InvertScale";
            this.cb_InvertScale.Size = new System.Drawing.Size(83, 17);
            this.cb_InvertScale.TabIndex = 13;
            this.cb_InvertScale.Text = "Invert Scale";
            this.cb_InvertScale.UseVisualStyleBackColor = true;
            // 
            // label_Info
            // 
            this.label_Info.AutoSize = true;
            this.label_Info.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label_Info.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Info.ForeColor = System.Drawing.Color.Crimson;
            this.label_Info.Location = new System.Drawing.Point(2, 0);
            this.label_Info.Name = "label_Info";
            this.label_Info.Size = new System.Drawing.Size(242, 13);
            this.label_Info.TabIndex = 99999;
            this.label_Info.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label_Info);
            this.panel1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Location = new System.Drawing.Point(1014, 563);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(246, 17);
            this.panel1.TabIndex = 36;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1084, 622);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 99999;
            this.label11.Text = "Sim Time:";
            // 
            // label_SimTime
            // 
            this.label_SimTime.AutoSize = true;
            this.label_SimTime.Location = new System.Drawing.Point(1143, 622);
            this.label_SimTime.Name = "label_SimTime";
            this.label_SimTime.Size = new System.Drawing.Size(43, 13);
            this.label_SimTime.TabIndex = 99999;
            this.label_SimTime.Text = "0:00:00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "IRandomizerFactory";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(28, 69);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(137, 13);
            this.label22.TabIndex = 4;
            this.label22.Text = "IApplicationEventGenerator";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(28, 56);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(39, 13);
            this.label23.TabIndex = 3;
            this.label23.Text = "IModel";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(28, 43);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(96, 13);
            this.label24.TabIndex = 2;
            this.label24.Text = "IPhysicalProcessor";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(27, 30);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(71, 13);
            this.label25.TabIndex = 1;
            this.label25.Text = "INodeFactory";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(3, 3);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(95, 13);
            this.label26.TabIndex = 0;
            this.label26.Text = "Selected Modules:";
            // 
            // btn_ResizetabControl
            // 
            this.btn_ResizetabControl.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btn_ResizetabControl.Location = new System.Drawing.Point(1253, 1);
            this.btn_ResizetabControl.Name = "btn_ResizetabControl";
            this.btn_ResizetabControl.Size = new System.Drawing.Size(19, 20);
            this.btn_ResizetabControl.TabIndex = 41;
            this.btn_ResizetabControl.Text = "ç";
            this.btn_ResizetabControl.UseVisualStyleBackColor = true;
            this.btn_ResizetabControl.Click += new System.EventHandler(this.btn_ResizetabControl_Click);
            // 
            // text_JumpStepSize
            // 
            this.text_JumpStepSize.Location = new System.Drawing.Point(1128, 678);
            this.text_JumpStepSize.Name = "text_JumpStepSize";
            this.text_JumpStepSize.Size = new System.Drawing.Size(45, 20);
            this.text_JumpStepSize.TabIndex = 11;
            this.text_JumpStepSize.Text = "10.000";
            this.text_JumpStepSize.TextChanged += new System.EventHandler(this.text_JumpStepSize_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.label21.Location = new System.Drawing.Point(1015, 681);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(105, 13);
            this.label21.TabIndex = 99999;
            this.label21.Text = "Fwd/Rev Step (Sec)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1038, 635);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 13);
            this.label10.TabIndex = 99999;
            this.label10.Text = "Actual Time Factor:";
            // 
            // labelTimeFactor
            // 
            this.labelTimeFactor.AutoSize = true;
            this.labelTimeFactor.Location = new System.Drawing.Point(1143, 635);
            this.labelTimeFactor.Name = "labelTimeFactor";
            this.labelTimeFactor.Size = new System.Drawing.Size(19, 13);
            this.labelTimeFactor.TabIndex = 99999;
            this.labelTimeFactor.Text = "15";
            this.labelTimeFactor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btn_Save_Sim
            // 
            this.btn_Save_Sim.Enabled = false;
            this.btn_Save_Sim.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save_Sim.Image")));
            this.btn_Save_Sim.Location = new System.Drawing.Point(1, 751);
            this.btn_Save_Sim.Name = "btn_Save_Sim";
            this.btn_Save_Sim.Size = new System.Drawing.Size(24, 24);
            this.btn_Save_Sim.TabIndex = 56;
            this.btn_Save_Sim.TabStop = false;
            this.toolTip1.SetToolTip(this.btn_Save_Sim, "Save Simulator Settings");
            this.btn_Save_Sim.Visible = false;
            this.btn_Save_Sim.Click += new System.EventHandler(this.btn_Save_Sim_Click);
            // 
            // text_JumpHrs
            // 
            this.text_JumpHrs.Location = new System.Drawing.Point(1128, 652);
            this.text_JumpHrs.Name = "text_JumpHrs";
            this.text_JumpHrs.Size = new System.Drawing.Size(26, 20);
            this.text_JumpHrs.TabIndex = 7;
            this.text_JumpHrs.Text = "0";
            this.text_JumpHrs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.text_JumpHrs.TextChanged += new System.EventHandler(this.text_JumpHrs_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(1152, 655);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(10, 13);
            this.label27.TabIndex = 58;
            this.label27.Text = ":";
            this.label27.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // text_JumpMin
            // 
            this.text_JumpMin.Location = new System.Drawing.Point(1159, 652);
            this.text_JumpMin.Name = "text_JumpMin";
            this.text_JumpMin.Size = new System.Drawing.Size(26, 20);
            this.text_JumpMin.TabIndex = 8;
            this.text_JumpMin.Text = "00";
            this.text_JumpMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.text_JumpMin.TextChanged += new System.EventHandler(this.text_JumpMin_TextChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(1184, 655);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(10, 13);
            this.label28.TabIndex = 60;
            this.label28.Text = ":";
            this.label28.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // text_JumpSec
            // 
            this.text_JumpSec.Location = new System.Drawing.Point(1191, 652);
            this.text_JumpSec.Name = "text_JumpSec";
            this.text_JumpSec.Size = new System.Drawing.Size(39, 20);
            this.text_JumpSec.TabIndex = 9;
            this.text_JumpSec.Text = "00.000";
            this.text_JumpSec.TextChanged += new System.EventHandler(this.text_JumpSec_TextChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(1015, 655);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(86, 13);
            this.label29.TabIndex = 99999;
            this.label29.Text = "Go To Sim Time:";
            // 
            // btn_Load_Sim
            // 
            this.btn_Load_Sim.FlatAppearance.BorderSize = 0;
            this.btn_Load_Sim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Load_Sim.Image = ((System.Drawing.Image)(resources.GetObject("btn_Load_Sim.Image")));
            this.btn_Load_Sim.Location = new System.Drawing.Point(1044, 582);
            this.btn_Load_Sim.Name = "btn_Load_Sim";
            this.btn_Load_Sim.Size = new System.Drawing.Size(25, 25);
            this.btn_Load_Sim.TabIndex = 99999;
            this.toolTip1.SetToolTip(this.btn_Load_Sim, "Load Simulator Settings");
            this.btn_Load_Sim.UseVisualStyleBackColor = true;
            this.btn_Load_Sim.Visible = false;
            // 
            // btn_RestartSim
            // 
            this.btn_RestartSim.Enabled = false;
            this.btn_RestartSim.FlatAppearance.BorderSize = 0;
            this.btn_RestartSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_RestartSim.Image = ((System.Drawing.Image)(resources.GetObject("btn_RestartSim.Image")));
            this.btn_RestartSim.Location = new System.Drawing.Point(1014, 582);
            this.btn_RestartSim.Name = "btn_RestartSim";
            this.btn_RestartSim.Size = new System.Drawing.Size(25, 25);
            this.btn_RestartSim.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btn_RestartSim, "Restart simulation using same settings");
            this.btn_RestartSim.UseVisualStyleBackColor = true;
            this.btn_RestartSim.Click += new System.EventHandler(this.btn_RestartSim_Click);
            // 
            // btn_Back_Sim
            // 
            this.btn_Back_Sim.Enabled = false;
            this.btn_Back_Sim.FlatAppearance.BorderSize = 0;
            this.btn_Back_Sim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Back_Sim.Image = ((System.Drawing.Image)(resources.GetObject("btn_Back_Sim.Image")));
            this.btn_Back_Sim.Location = new System.Drawing.Point(1100, 582);
            this.btn_Back_Sim.Name = "btn_Back_Sim";
            this.btn_Back_Sim.Size = new System.Drawing.Size(25, 25);
            this.btn_Back_Sim.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btn_Back_Sim, "Reverse Step");
            this.btn_Back_Sim.UseVisualStyleBackColor = true;
            this.btn_Back_Sim.Click += new System.EventHandler(this.btn_Back_Sim_Click);
            // 
            // btn_Pause_Sim
            // 
            this.btn_Pause_Sim.FlatAppearance.BorderSize = 0;
            this.btn_Pause_Sim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Pause_Sim.Image = ((System.Drawing.Image)(resources.GetObject("btn_Pause_Sim.Image")));
            this.btn_Pause_Sim.Location = new System.Drawing.Point(1130, 582);
            this.btn_Pause_Sim.Name = "btn_Pause_Sim";
            this.btn_Pause_Sim.Size = new System.Drawing.Size(25, 25);
            this.btn_Pause_Sim.TabIndex = 4;
            this.btn_Pause_Sim.Tag = "run";
            this.toolTip1.SetToolTip(this.btn_Pause_Sim, "Pause/Play Sim");
            this.btn_Pause_Sim.UseVisualStyleBackColor = true;
            this.btn_Pause_Sim.Click += new System.EventHandler(this.btn_Run_Sim_Click);
            // 
            // btn_Advance_Sim
            // 
            this.btn_Advance_Sim.Enabled = false;
            this.btn_Advance_Sim.FlatAppearance.BorderSize = 0;
            this.btn_Advance_Sim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Advance_Sim.Image = ((System.Drawing.Image)(resources.GetObject("btn_Advance_Sim.Image")));
            this.btn_Advance_Sim.Location = new System.Drawing.Point(1160, 582);
            this.btn_Advance_Sim.Name = "btn_Advance_Sim";
            this.btn_Advance_Sim.Size = new System.Drawing.Size(25, 25);
            this.btn_Advance_Sim.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btn_Advance_Sim, "Forward Step");
            this.btn_Advance_Sim.UseVisualStyleBackColor = true;
            this.btn_Advance_Sim.Click += new System.EventHandler(this.btn_Advance_Sim_Click);
            // 
            // btn_Stop_Sim
            // 
            this.btn_Stop_Sim.Enabled = false;
            this.btn_Stop_Sim.FlatAppearance.BorderSize = 0;
            this.btn_Stop_Sim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Stop_Sim.Image = ((System.Drawing.Image)(resources.GetObject("btn_Stop_Sim.Image")));
            this.btn_Stop_Sim.Location = new System.Drawing.Point(1236, 582);
            this.btn_Stop_Sim.Name = "btn_Stop_Sim";
            this.btn_Stop_Sim.Size = new System.Drawing.Size(25, 25);
            this.btn_Stop_Sim.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btn_Stop_Sim, "Stop & Reset Simulation");
            this.btn_Stop_Sim.UseVisualStyleBackColor = true;
            this.btn_Stop_Sim.Click += new System.EventHandler(this.btn_Stop_Sim_Click);
            // 
            // btn_Jump_Sim
            // 
            this.btn_Jump_Sim.Enabled = false;
            this.btn_Jump_Sim.FlatAppearance.BorderSize = 0;
            this.btn_Jump_Sim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Jump_Sim.Image = ((System.Drawing.Image)(resources.GetObject("btn_Jump_Sim.Image")));
            this.btn_Jump_Sim.Location = new System.Drawing.Point(1236, 649);
            this.btn_Jump_Sim.Name = "btn_Jump_Sim";
            this.btn_Jump_Sim.Size = new System.Drawing.Size(25, 25);
            this.btn_Jump_Sim.TabIndex = 10;
            this.toolTip1.SetToolTip(this.btn_Jump_Sim, "Go To Sim Time");
            this.btn_Jump_Sim.UseVisualStyleBackColor = true;
            this.btn_Jump_Sim.Click += new System.EventHandler(this.btn_Jump_Sim_Click);
            // 
            // backgroundWorker_RunSim
            // 
            this.backgroundWorker_RunSim.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_RunSim_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 766);
            this.Controls.Add(this.btn_Jump_Sim);
            this.Controls.Add(this.btn_Stop_Sim);
            this.Controls.Add(this.btn_Advance_Sim);
            this.Controls.Add(this.btn_Pause_Sim);
            this.Controls.Add(this.btn_Back_Sim);
            this.Controls.Add(this.btn_RestartSim);
            this.Controls.Add(this.btn_Load_Sim);
            this.Controls.Add(this.btn_Save_Sim);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.text_JumpSec);
            this.Controls.Add(this.text_JumpMin);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.text_JumpHrs);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.labelTimeFactor);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.text_JumpStepSize);
            this.Controls.Add(this.btn_ResizetabControl);
            this.Controls.Add(this.label_SimTime);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cb_InvertScale);
            this.Controls.Add(this.label_TimeScale);
            this.Controls.Add(this.TimeScale);
            this.Controls.Add(this.btnToggleGraphics);
            this.Controls.Add(this.labelFPS);
            this.Controls.Add(this.FPS);
            this.Controls.Add(this.tab_INode);
            this.Controls.Add(this.Panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(1280, 800);
            this.MinimumSize = new System.Drawing.Size(1278, 766);
            this.Name = "Form1";
            this.Text = "MNSim Visualizer";
            this.tab_INode.ResumeLayout(false);
            this.tab_DLL.ResumeLayout(false);
            this.tab_DLL.PerformLayout();
            this.tab_File.ResumeLayout(false);
            this.tab_File.PerformLayout();
            this.tab_Run.ResumeLayout(false);
            this.tab_Run.PerformLayout();
            this.tab_Statistics.ResumeLayout(false);
            this.tab_Statistics.PerformLayout();
            this.tab_IO.ResumeLayout(false);
            this.tab_IO.PerformLayout();
            this.tab_INodeFactory.ResumeLayout(false);
            this.tab_INodeFactory.PerformLayout();
            this.tab_IPhysicalProcessor.ResumeLayout(false);
            this.tab_IPhysicalProcessor.PerformLayout();
            this.tab_IApplicationEventGenerator.ResumeLayout(false);
            this.tab_IApplicationEventGenerator.PerformLayout();
            this.tab_IDeployer.ResumeLayout(false);
            this.tab_IDeployer.PerformLayout();
            this.tab_IRandomizerFactory.ResumeLayout(false);
            this.tab_IRandomizerFactory.PerformLayout();
            this.tab_ILocation.ResumeLayout(false);
            this.tab_ILocation.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save_Sim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.TabControl tab_INode;
        private System.Windows.Forms.TabPage tab_DLL;
        private System.Windows.Forms.TabPage tab_IApplicationEventGenerator;
        private System.Windows.Forms.TabPage tab_INodeFactory;
        private System.Windows.Forms.TabPage tab_IPhysicalProcessor;
        private System.Windows.Forms.TextBox FPS;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.Button btnToggleGraphics;
        private System.Windows.Forms.Label label_TimeScale;
        private System.Windows.Forms.TextBox TimeScale;
        private System.Windows.Forms.CheckBox cb_InvertScale;
        private System.Windows.Forms.Label label_Info;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label_SimTime;
        private System.Windows.Forms.TabPage tab_IRandomizerFactory;
        private System.Windows.Forms.TabPage tab_ILocation;
        private System.Windows.Forms.TabPage tab_IDeployer;
        private System.Windows.Forms.Label CurrentDir;
        private System.Windows.Forms.Button btn_LoadDLL;
        private System.Windows.Forms.ListBox lb_DLLs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_INode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_IPhysicalProcessor;
        private System.Windows.Forms.ComboBox cb_IDeployer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_IApplicationEventGenerator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cb_IRandomizerFactory;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox text_FieldY2;
        private System.Windows.Forms.TextBox text_FieldX2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox text_FieldY1;
        private System.Windows.Forms.TextBox text_FieldX1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cb_ILocation;
        private System.Windows.Forms.TabPage tab_Statistics;
        private System.Windows.Forms.TabPage tab_Run;
        private System.Windows.Forms.Label selected_INodeFactory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label selected_IDeployer;
        private System.Windows.Forms.Label selected_ILocation;
        private System.Windows.Forms.Label selected_IRandomizerFactory;
        private System.Windows.Forms.Label selected_IApplicationEventGenerator;
        private System.Windows.Forms.Label selected_IPhysicalProcessor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btn_RunSim;
        private System.Windows.Forms.Button btn_ResizetabControl;
        private System.Windows.Forms.TabPage tab_File;
        private System.Windows.Forms.Label selected_GraphicsRunning;
        private System.Windows.Forms.TextBox text_JumpStepSize;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelTimeFactor;
        private System.Windows.Forms.PictureBox btn_Save_Sim;
        private System.Windows.Forms.TextBox text_JumpHrs;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox text_JumpMin;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox text_JumpSec;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ComboBox settings_Records;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox settings_FileName;
        private System.Windows.Forms.Label settings_Seed;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label settings_NodeType;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label settings_Deployer;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label settings_PhysicalProcessor;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label settings_AppEventGenerator;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.RichTextBox settings_Description;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_Load_Sim;
        private System.Windows.Forms.Button btn_RestartSim;
        private System.Windows.Forms.Button btn_Back_Sim;
        private System.Windows.Forms.Button btn_Pause_Sim;
        private System.Windows.Forms.Button btn_Advance_Sim;
        private System.Windows.Forms.Button btn_Stop_Sim;
        private System.Windows.Forms.Button btn_Jump_Sim;
        private System.Windows.Forms.Button btn_Settings_Load_File_;
        private System.Windows.Forms.Button btn_Settings_Save;
        private System.Windows.Forms.Button btn_Settings_Save_Record_;
        private System.Windows.Forms.Button btn_Settings_Load_Record_;
        private System.Windows.Forms.TabPage tab_IO;
        private System.Windows.Forms.TextBox text_IOFolder;
        private System.Windows.Forms.Button btn_ChooseIOFolder;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox text_IOFileTag;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.CheckBox cb_randomSink;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label textClick_ID;
        private System.Windows.Forms.Label label_Multirun2;
        private System.Windows.Forms.TextBox tb_Multirun;
        private System.Windows.Forms.Label label_Multirun1;
        private System.Windows.Forms.CheckBox cb_Multirun;
        private System.Windows.Forms.CheckBox cb_GraphicsOff;
        private System.ComponentModel.BackgroundWorker backgroundWorker_RunSim;
        private System.Windows.Forms.CheckBox cb_AppSetsSink;
    }
}

