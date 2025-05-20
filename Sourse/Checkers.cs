using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Checkers
{
    public class MainForm : System.Windows.Forms.Form
	{
		#region Export dll functions
		[DllImport("Checkers.dll")] public unsafe static extern void LibManualMove(ref int* n, int X1, int Y1, int X2, int Y2);
		[DllImport("Checkers.dll")] public unsafe static extern void LibAutoMove(ref int* n);
		[DllImport("Checkers.dll")] public unsafe static extern void LibNew(ref int* n);
		[DllImport("Checkers.dll")] public unsafe static extern void LibSetMaster(int W, int B);
		[DllImport("Checkers.dll")] public unsafe static extern void LibSetLevel(int L);
		[DllImport("Checkers.dll")] public unsafe static extern int LibReMaster();
		[DllImport("Checkers.dll")] public unsafe static extern void LibSetField(ref int* n);
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region My private constants
		private const int bShift = 20;
		private const int bSize = 50;
		private const string levelStr = "Уровень: ";
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region My private variables
		private bool auto;
		private Color rotate;
		private string fileName;
		private int timeError = 7;
		private Cursor[] Cursorss;
		private bool FlagGOver = false;
		private Dot[] DMov = new Dot[2];
		private Field field = new Field();
		private DataSet ds = new DataSet(); 
		private  DateTime[] time = new DateTime[2];
		private Color[] Colorss = new Color[]{Color.Bisque, Color.Maroon};
		private string[] ResStr = new string[]{"  Белые выиграли    ", "  Черные выиграли    ", "Ничья после 300 ходов"};
		// -------*********----------********---------
		private Thread t;
		private static MainForm MF;
		private static ThreadStart ths = new ThreadStart(ComputersGame);
		private DataGridTableStyle ts;
		private DataGridColumnStyle cs;
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region Windows Form components
			private DataTable stat;
			private DataGrid gstat;
			private Panel panField;
			private System.Windows.Forms.MainMenu mainMnu;
			private System.Windows.Forms.MenuItem mnuFile;
			private System.Windows.Forms.MenuItem mnuGame;
			private System.Windows.Forms.MenuItem mnuView;
			private System.Windows.Forms.MenuItem mnuViewRotate;
			private System.Windows.Forms.ImageList imgListFigures;
			private System.Windows.Forms.Label lblStat;
			private System.Windows.Forms.MenuItem mnuGameNew;
			private System.Windows.Forms.MenuItem mnuViewStat;
			private System.Windows.Forms.MenuItem mnuViewClock;
			private System.Windows.Forms.MenuItem mnuFileSave;
			private System.Windows.Forms.MenuItem mnuFileLoad;
			private System.Windows.Forms.MenuItem mnuFileR1;
			private System.Windows.Forms.MenuItem mnuFileExit;
			private System.Windows.Forms.MenuItem mnuGameR1;
			private System.Windows.Forms.MenuItem mnuGameLow;
			private System.Windows.Forms.MenuItem mnuGameMedium;
			private System.Windows.Forms.MenuItem mnuGameHigh;
			private System.Windows.Forms.MenuItem mnuGameR2;
			private System.Windows.Forms.MenuItem mnuGameCW;
			private System.Windows.Forms.MenuItem mnuGameCB;
			private System.Windows.Forms.MenuItem mnuGameCWB;
			private System.Windows.Forms.MenuItem mnuGameNO;
			private System.Windows.Forms.Label lblLevel;
			private System.Windows.Forms.Label lblClockW;
			private System.Windows.Forms.Label lblClockB;
			private System.Windows.Forms.Timer timer;
			private System.Windows.Forms.StatusBar statusBar;
			private System.Windows.Forms.StatusBarPanel statusBarPanel;
		private System.Windows.Forms.MenuItem mnuViewAutoRotate;
		private System.Windows.Forms.MenuItem mnuViewR1;
		private System.Windows.Forms.MenuItem mnuViewR2;
			private System.ComponentModel.IContainer components;
			#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region mainfoprm & dispose
		public MainForm(){InitializeComponent();}

		// ---------- * ---------------- * --------------- * ------------	
		protected override void Dispose( bool disposing )
		{
			if( disposing ){if (components != null) components.Dispose();}
			base.Dispose( disposing );
		}
		#endregion
		// ---------- * ---------------- * --------------- * ------------	
		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.panField = new System.Windows.Forms.Panel();
			this.imgListFigures = new System.Windows.Forms.ImageList(this.components);
			this.mainMnu = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileSave = new System.Windows.Forms.MenuItem();
			this.mnuFileLoad = new System.Windows.Forms.MenuItem();
			this.mnuFileR1 = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuGame = new System.Windows.Forms.MenuItem();
			this.mnuGameNew = new System.Windows.Forms.MenuItem();
			this.mnuGameR1 = new System.Windows.Forms.MenuItem();
			this.mnuGameLow = new System.Windows.Forms.MenuItem();
			this.mnuGameMedium = new System.Windows.Forms.MenuItem();
			this.mnuGameHigh = new System.Windows.Forms.MenuItem();
			this.mnuGameR2 = new System.Windows.Forms.MenuItem();
			this.mnuGameCB = new System.Windows.Forms.MenuItem();
			this.mnuGameCW = new System.Windows.Forms.MenuItem();
			this.mnuGameCWB = new System.Windows.Forms.MenuItem();
			this.mnuGameNO = new System.Windows.Forms.MenuItem();
			this.mnuView = new System.Windows.Forms.MenuItem();
			this.mnuViewRotate = new System.Windows.Forms.MenuItem();
			this.mnuViewR1 = new System.Windows.Forms.MenuItem();
			this.mnuViewAutoRotate = new System.Windows.Forms.MenuItem();
			this.mnuViewR2 = new System.Windows.Forms.MenuItem();
			this.mnuViewStat = new System.Windows.Forms.MenuItem();
			this.mnuViewClock = new System.Windows.Forms.MenuItem();
			this.stat = new System.Data.DataTable();
			this.gstat = new System.Windows.Forms.DataGrid();
			this.lblStat = new System.Windows.Forms.Label();
			this.lblLevel = new System.Windows.Forms.Label();
			this.lblClockW = new System.Windows.Forms.Label();
			this.lblClockB = new System.Windows.Forms.Label();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
			((System.ComponentModel.ISupportInitialize)(this.stat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gstat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
			this.SuspendLayout();
			// 
			// panField
			// 
			this.panField.BackColor = System.Drawing.Color.DimGray;
			this.panField.Location = new System.Drawing.Point(8, 24);
			this.panField.Name = "panField";
			this.panField.Size = new System.Drawing.Size(440, 440);
			this.panField.TabIndex = 0;
			// 
			// imgListFigures
			// 
			this.imgListFigures.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imgListFigures.ImageSize = new System.Drawing.Size(50, 50);
			this.imgListFigures.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListFigures.ImageStream")));
			this.imgListFigures.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// mainMnu
			// 
			this.mainMnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFile,
																					this.mnuGame,
																					this.mnuView});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFileSave,
																					this.mnuFileLoad,
																					this.mnuFileR1,
																					this.mnuFileExit});
			this.mnuFile.Text = "Файл";
			// 
			// mnuFileSave
			// 
			this.mnuFileSave.Index = 0;
			this.mnuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mnuFileSave.Text = "Сохранить партию";
			this.mnuFileSave.Click += new System.EventHandler(this.mnuFileSave_Click);
			// 
			// mnuFileLoad
			// 
			this.mnuFileLoad.Index = 1;
			this.mnuFileLoad.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuFileLoad.Text = "Загрузить партию";
			this.mnuFileLoad.Click += new System.EventHandler(this.mnuFileLoad_Click);
			// 
			// mnuFileR1
			// 
			this.mnuFileR1.Index = 2;
			this.mnuFileR1.Text = "-";
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Index = 3;
			this.mnuFileExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.mnuFileExit.Text = "Выход";
			this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			// 
			// mnuGame
			// 
			this.mnuGame.Index = 1;
			this.mnuGame.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuGameNew,
																					this.mnuGameR1,
																					this.mnuGameLow,
																					this.mnuGameMedium,
																					this.mnuGameHigh,
																					this.mnuGameR2,
																					this.mnuGameCB,
																					this.mnuGameCW,
																					this.mnuGameCWB,
																					this.mnuGameNO});
			this.mnuGame.Text = "Игра";
			// 
			// mnuGameNew
			// 
			this.mnuGameNew.Index = 0;
			this.mnuGameNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.mnuGameNew.Text = "Новая партия";
			this.mnuGameNew.Click += new System.EventHandler(this.mnuGameNew_Click);
			// 
			// mnuGameR1
			// 
			this.mnuGameR1.Index = 1;
			this.mnuGameR1.Text = "-";
			// 
			// mnuGameLow
			// 
			this.mnuGameLow.Index = 2;
			this.mnuGameLow.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
			this.mnuGameLow.Text = "Уровень Низкий";
			this.mnuGameLow.Click += new System.EventHandler(this.mnuGameLow_Click);
			// 
			// mnuGameMedium
			// 
			this.mnuGameMedium.Index = 3;
			this.mnuGameMedium.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
			this.mnuGameMedium.Text = "Уровень Средний";
			this.mnuGameMedium.Click += new System.EventHandler(this.mnuGameMedium_Click);
			// 
			// mnuGameHigh
			// 
			this.mnuGameHigh.Index = 4;
			this.mnuGameHigh.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
			this.mnuGameHigh.Text = "Уровень Высокий";
			this.mnuGameHigh.Click += new System.EventHandler(this.mnuGameHigh_Click);
			// 
			// mnuGameR2
			// 
			this.mnuGameR2.Index = 5;
			this.mnuGameR2.Text = "-";
			// 
			// mnuGameCB
			// 
			this.mnuGameCB.Index = 6;
			this.mnuGameCB.Shortcut = System.Windows.Forms.Shortcut.CtrlF5;
			this.mnuGameCB.Text = "Игрок - Компьютер";
			this.mnuGameCB.Click += new System.EventHandler(this.mnuGameCB_Click);
			// 
			// mnuGameCW
			// 
			this.mnuGameCW.Index = 7;
			this.mnuGameCW.Shortcut = System.Windows.Forms.Shortcut.CtrlF6;
			this.mnuGameCW.Text = "Компьютер - Игрок";
			this.mnuGameCW.Click += new System.EventHandler(this.mnuGameCW_Click);
			// 
			// mnuGameCWB
			// 
			this.mnuGameCWB.Index = 8;
			this.mnuGameCWB.Shortcut = System.Windows.Forms.Shortcut.CtrlF7;
			this.mnuGameCWB.Text = "Компьютер - Компьютер";
			this.mnuGameCWB.Click += new System.EventHandler(this.mnuGameCWB_Click);
			// 
			// mnuGameNO
			// 
			this.mnuGameNO.Index = 9;
			this.mnuGameNO.Shortcut = System.Windows.Forms.Shortcut.CtrlF8;
			this.mnuGameNO.Text = "Игрок - Игрок";
			this.mnuGameNO.Click += new System.EventHandler(this.mnuGameNO_Click);
			// 
			// mnuView
			// 
			this.mnuView.Index = 2;
			this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuViewRotate,
																					this.mnuViewR1,
																					this.mnuViewAutoRotate,
																					this.mnuViewR2,
																					this.mnuViewStat,
																					this.mnuViewClock});
			this.mnuView.Text = "Вид";
			// 
			// mnuViewRotate
			// 
			this.mnuViewRotate.Index = 0;
			this.mnuViewRotate.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.mnuViewRotate.Text = "Развернуть доску";
			this.mnuViewRotate.Click += new System.EventHandler(this.mnuViewRotate_Click);
			// 
			// mnuViewR1
			// 
			this.mnuViewR1.Index = 1;
			this.mnuViewR1.Text = "-";
			// 
			// mnuViewAutoRotate
			// 
			this.mnuViewAutoRotate.Checked = true;
			this.mnuViewAutoRotate.Index = 2;
			this.mnuViewAutoRotate.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.mnuViewAutoRotate.Text = "Разворачивать авоматически";
			this.mnuViewAutoRotate.Click += new System.EventHandler(this.mnuViewAutoRotate_Click);
			// 
			// mnuViewR2
			// 
			this.mnuViewR2.Index = 3;
			this.mnuViewR2.Text = "-";
			// 
			// mnuViewStat
			// 
			this.mnuViewStat.Index = 4;
			this.mnuViewStat.Shortcut = System.Windows.Forms.Shortcut.CtrlF9;
			this.mnuViewStat.Text = "Статистика";
			this.mnuViewStat.Click += new System.EventHandler(this.mnuViewStat_Click);
			// 
			// mnuViewClock
			// 
			this.mnuViewClock.Index = 5;
			this.mnuViewClock.Shortcut = System.Windows.Forms.Shortcut.CtrlF10;
			this.mnuViewClock.Text = "Часы";
			this.mnuViewClock.Click += new System.EventHandler(this.mnuViewClock_Click);
			// 
			// gstat
			// 
			this.gstat.BackColor = System.Drawing.Color.DimGray;
			this.gstat.BackgroundColor = System.Drawing.Color.DimGray;
			this.gstat.CaptionVisible = false;
			this.gstat.ColumnHeadersVisible = false;
			this.gstat.SetDataBinding(stat, "");
			this.gstat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.gstat.ForeColor = System.Drawing.Color.Chartreuse;
			this.gstat.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
			this.gstat.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.gstat.Location = new System.Drawing.Point(464, 24);
			//this.gstat.Name = "gstat";
			this.gstat.ReadOnly = true;
			this.gstat.SelectionForeColor = System.Drawing.Color.Red;
			this.gstat.Size = new System.Drawing.Size(160, 440);
			this.gstat.TabIndex = 0;
			// 
			// lblStat
			// 
			this.lblStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblStat.ForeColor = System.Drawing.Color.Blue;
			this.lblStat.Location = new System.Drawing.Point(464, 2);
			this.lblStat.Name = "lblStat";
			this.lblStat.Size = new System.Drawing.Size(160, 20);
			this.lblStat.TabIndex = 4;
			this.lblStat.Text = "Статистика";
			this.lblStat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblLevel
			// 
			this.lblLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblLevel.ForeColor = System.Drawing.Color.Blue;
			this.lblLevel.Location = new System.Drawing.Point(125, 2);
			this.lblLevel.Name = "lblLevel";
			this.lblLevel.Size = new System.Drawing.Size(208, 20);
			this.lblLevel.TabIndex = 5;
			this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblClockW
			// 
			this.lblClockW.BackColor = System.Drawing.Color.Black;
			this.lblClockW.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblClockW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic), System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblClockW.ForeColor = System.Drawing.Color.Chartreuse;
			this.lblClockW.Location = new System.Drawing.Point(16, 4);
			this.lblClockW.Name = "lblClockW";
			this.lblClockW.Size = new System.Drawing.Size(90, 16);
			this.lblClockW.TabIndex = 6;
			this.lblClockW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblClockB
			// 
			this.lblClockB.BackColor = System.Drawing.Color.Black;
			this.lblClockB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblClockB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic), System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblClockB.ForeColor = System.Drawing.Color.Chartreuse;
			this.lblClockB.Location = new System.Drawing.Point(350, 4);
			this.lblClockB.Name = "lblClockB";
			this.lblClockB.Size = new System.Drawing.Size(90, 16);
			this.lblClockB.TabIndex = 7;
			this.lblClockB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 473);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusBarPanel});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(634, 24);
			this.statusBar.TabIndex = 8;
			// 
			// statusBarPanel
			// 
			this.statusBarPanel.Width = 624;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(634, 497);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.statusBar,
																		  this.lblClockB,
																		  this.lblClockW,
																		  this.lblLevel,
																		  this.lblStat,
																		  this.gstat,
																		  this.panField});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMnu;
			this.Name = "MainForm";
			this.Text = "SPGPU - Checkers";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.stat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gstat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
			this.ResumeLayout(false);

		}
			#endregion
		// ---------- * ---------------- * --------------- * ------------	
		#region Main
		[MTAThread]
		static void Main(){Application.Run(new MainForm());}
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region private functions
		private string Zerro(int n){return (n < 10 ? "0" : "") + n;}
		// ---------- * ---------------- * --------------- * ------------	
		private void ReColor(ref Color c){c = c == Color.White ? Color.Black : Color.White;}
		// ---------- * ---------------- * --------------- * ------------	
		private int Rotating(int V, int Max, bool R){return R ? (Max - V) : V;}
		// ---------- * ---------------- * --------------- * ------------
		private int ToIndex(Dot d) {return (field.FColor(d) == Color.White ? 1 : 0) + (field.FKing(d) ? 1 : 0) * 2;}
		// ---------- * ---------------- * --------------- * ------------	
		private void GameOver()
		{
			MessageBox.Show("\n     Конец игры...\n" + ResStr[(field.Master == Color.Black) ? 0 : 1] + "\n"
			, "Конец игры..."); FlagGOver = true; timer.Enabled = false; 
		}
		// ---------- * ---------------- * --------------- * ------------	
		private unsafe string FromDLL(eEvent e, Dot[] Ds)
		{
			int a = 0; string res = ""; int* arr = &a; 
			switch (e)
			{
				case eEvent.eManual:	LibManualMove(ref arr, Ds[0].X, Ds[0].Y, Ds[1].X, Ds[1].Y); break;
				case eEvent.eNew:		LibNew(ref arr); break;
				case eEvent.eAuto:		LibAutoMove(ref arr); break;
				case eEvent.eSet:		
					LibNew(ref arr);
					for(int j = 0; j < 8; j++) for(int i = 0; i < 8; i++) arr[i + j * 8] = (field.mField[i, j]);
					LibSetField(ref arr); break;
			}
			for(int j = 0; j < 8; j++) for(int i = 0; i < 8; i++) field.mField[i, j] = arr[i + j * 8];
			for(int i = 64; i < 75; i++) res += (char)(arr[i]);
			return res;
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void Statistic(string str)
		{
			string
				s1 = ((char)('a' + str[0])).ToString(),
				s2 = ((char)('1' + str[1])).ToString(),
				s3 = ((char)('a' + str[5])).ToString(),
				s4 = ((char)('1' + str[6])).ToString(),
				si = s1 + s2 + str.Substring(2, 3) + s3 + s4; 
					 
			if(field.Master != Color.White) 
			{
				stat.Rows[stat.Rows.Count - 1]["Black"] = si;
				stat.Rows[stat.Rows.Count - 1]["FBlack"] = field.ToString();
			}
			else 
			{
				DataRow row = stat.NewRow(); 
				row["#"] = (stat.Rows.Count + 1) + ".   "; 
				row["White"] = si; row["FWhite"] = field.ToString();
				row["FBlack"] = "Not";
				stat.Rows.Add(row);
			};
			int sf = (field.Master == Color.White) ? 0 : 1;
			gstat.CurrentCell = new DataGridCell(stat.Rows.Count - 1, 1 + sf);
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void RePaint()
		{
			bool comp = LibReMaster() == 1; 
			foreach(Control cnt in panField.Controls)
			{
				string tag = cnt.Tag.ToString();
				if(cnt.GetType().Name == "PictureBox")
				{
					int shift = Rotating((int)cnt.Tag, 63, rotate == Color.White);
					((PictureBox)cnt).Image = null; Dot d = Dot.FromArray(shift);
					int K = 0; if(field.FErre(d)) {field.Set(d, field.Get(d) - 100); K = 8; if(!field.Figure(d))((PictureBox)cnt).BackColor = Color.OrangeRed;} 
					else if(field.FErrs(d)) {field.Set(d, field.Get(d) - 50); K = 4; if(!field.Figure(d))((PictureBox)cnt).BackColor = Color.Orange;}  
					if(field.Figure(d)) ((PictureBox)cnt).Image = imgListFigures.Images[ToIndex(d) + K];
					((PictureBox)cnt).Cursor = (field.Figure(d) && (field.FColor(d) == field.Master)) ? Cursors.Hand : Cursors.Default;
					if(comp) ((PictureBox)cnt).Cursor = (FlagGOver) ? Cursors.Default : Cursors.WaitCursor;
				}
				else
				{	
					string line = tag.Substring(0, 1);
					int shift = Rotating(int.Parse(tag.Substring(1, 1)), 7, rotate == Color.White);
					if(line == "l" || line == "r") ((Label)cnt).Text = (1 + shift).ToString();
					else ((Label)cnt).Text = ((char)('h' - shift)).ToString();
				}
			}
			Refresh();
		}
		// ---------- * ---------------- * --------------- * ------------
		private static void ComputersGame() 
		{
			foreach(MenuItem mi in MF.mnuGame.MenuItems) mi.Enabled = false;
			foreach(MenuItem mi in MF.mnuFile.MenuItems) mi.Enabled = false;
			MF.mnuGameNew.Enabled = true; MF.mnuFileExit.Enabled = true;
			Thread currentThread = Thread.CurrentThread; MF.auto = true;
			while(LibReMaster() == 1) //Пока текущий ход выполняет компьютер:
			{	
				string str = MF.FromDLL(eEvent.eAuto, MF.DMov); 
				if(!MF.auto || MF.FlagGOver) {MF.FromDLL(eEvent.eNew, MF.DMov); MF.RePaint(); break;}
				MF.Statistic(str.Substring(2, 7)); 
				MF.field.Master = (MF.field.Master == Color.White) ? Color.Black : Color.White; 
				if(MF.mnuViewAutoRotate.Checked) MF.rotate = (str[0] == 0) ? Color.White : Color.Black; MF.RePaint();
				if(str[1] == (char)3) MF.GameOver();
			} 
			foreach(MenuItem mi in MF.mnuGame.MenuItems) mi.Enabled = true;
			foreach(MenuItem mi in MF.mnuFile.MenuItems) mi.Enabled = true;
		} // - ComputersGame

		// ---------- * ---------------- * --------------- * ------------
		private void StartAuto() 
		{
			if(LibReMaster() == 1) timer.Enabled = true; t = new Thread(ths); t.Start();
		} // - StartAuto
		// ---------- * ---------------- * --------------- * ------------
		protected void AddStyle()
		{
			//gstat.TableStyles.Clear();
			ts = new DataGridTableStyle();
			ts.AlternatingBackColor = System.Drawing.Color.DimGray;
			ts.BackColor = System.Drawing.Color.DimGray;
			ts.ForeColor = System.Drawing.Color.Chartreuse;
			ts.SelectionBackColor = System.Drawing.Color.DimGray;
			ts.SelectionForeColor = System.Drawing.Color.Chartreuse;
			ts.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
			ts.RowHeadersVisible = false;
			// cs
			cs = new DataGridTextBoxColumn();
			cs.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			cs.MappingName = "#"; cs.Width = 30;
			ts.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "White"; cs.Width = 55;
			ts.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.NullText = " ";
			cs.MappingName = "Black"; cs.Width = 55;
			ts.GridColumnStyles.Add(cs);

			gstat.TableStyles.Add(ts);
		}
		// ---------- * ---------------- * --------------- * ------------	
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region My delegats
		private void TakeFigure(object sender, MouseEventArgs e)
		{
			bool comp = LibReMaster() == 1;
			int shift = Rotating((int)((PictureBox)sender).Tag, 63, rotate == Color.White); Dot d = Dot.FromArray(shift);
			if(!field.Figure(d) || field.FColor(d) != field.Master || comp) {DMov[0] = new Dot(); return;}
			((PictureBox)sender).Image = null; DMov[0] = d;
			((PictureBox)sender).Cursor = Cursorss[ToIndex(d)];
			((PictureBox)sender).BackColor = Color.OrangeRed; timer.Enabled = true;
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void PlaceFigure(object sender, MouseEventArgs e)
		{
			this.ActiveControl = panField;
			char res = (char) 100; bool ok = false;
			if(!DMov[0].Successful){DMov[1] = new Dot(); return;}
			int nX = Rotating(e.X / bSize - (e.X >= 0 ? 0 : 1), 0, rotate != Color.White);
			int nY = Rotating(e.Y / bSize - (e.Y >= 0 ? 0 : 1), 0, rotate == Color.White);
			DMov[1] = new Dot(DMov[0].X + nX, DMov[0].Y + nY);
			if(DMov[1].Successful && field.Get(DMov[1]) == 1)
			{
				string str = FromDLL(eEvent.eManual, DMov); res = str[1];
				if(res == (char)0 || res == (char)3)
				{
					
						Statistic(str.Substring(2, 7)); 
						field.Master = (field.Master == Color.White) ? Color.Black : Color.White; 
						if(mnuViewAutoRotate.Checked) rotate = (str[0] == 0) ? Color.White : Color.Black;// break;
						if(res == (char)3) GameOver(); else ok = true; 
				}
				else if(res == (char)1) timeError = 0;
				else if(res == (char)2) timeError = 0;

				
			}
			((PictureBox)sender).Cursor = Cursors.Default; 
			((PictureBox)sender).BackColor = Colorss[1]; 
			DMov[0] = new Dot(); DMov[1] = new Dot(); RePaint();
			if(ok) StartAuto();
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void MainForm_Load(object sender, System.EventArgs e)
		{
			#region My Cursors
//			try
//			{
				Cursorss = new Cursor[]
				{new Cursor(GetType(), "BP.cur"), 
				new Cursor(GetType(), "WP.cur"),
				new Cursor(GetType(), "BK.cur"), 
				new Cursor(GetType(), "WK.cur"),};
//			}
//			catch
//			{
//				MessageBox.Show("Не обнаружены необходимые комноненты (4)\nпопробуйте переустановить приложение..."
//					, "Start Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//				this.Close();
//			}
			#endregion
			#region My Data
			stat.Columns.Add("#");stat.Columns.Add("White"); stat.Columns.Add("Black");  
			stat.Columns.Add("FWhite"); stat.Columns.Add("FBlack"); AddStyle();
			#endregion
			#region My View
			lblLevel.Text = levelStr + "Средний"; rotate  = Color.White;
			mnuViewStat.Checked = mnuViewClock.Checked = true;
			mnuGameCB.Checked = mnuGameMedium.Checked = true;
			lblClockB.Text = lblClockW.Text = "00 : 00 : 00 : 0";
			for(int i = 0; i < 8; i++){for(int j = 0; j < 8; j++)
				{
					#region PictureBox
					PictureBox p = new PictureBox();
					p.Tag = i * 8 + (7 - j);
					p.Size = new Size(bSize, bSize);
					p.BorderStyle = BorderStyle.FixedSingle;
					p.MouseUp += new MouseEventHandler(PlaceFigure);
					p.MouseDown += new MouseEventHandler(TakeFigure);
					p.BackColor = Colorss[((i + j) % 2 == 1) ? 1 : 0];
					p.Location = new Point(j * bSize + bShift, i * bSize + bShift);
					panField.Controls.AddRange(new System.Windows.Forms.Control[]{p});
					#endregion
				}
				#region Label Bottom
				Label l = new Label();
				l.ForeColor = Color.Aqua;
				l.Tag = "b" + i.ToString();
				l.BackColor = Color.DimGray;
				l.Size = new Size(bSize, bShift);
				l.Location = new Point(i * bSize + bShift, 0);
				l.TextAlign = ContentAlignment.MiddleCenter;
				l.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, ((System.Byte)(204)));
				panField.Controls.AddRange(new System.Windows.Forms.Control[]{l});
				#endregion
				#region Label Top
				l = new Label();
				l.ForeColor = Color.Aqua;
				l.Tag = "t" + i.ToString();
				l.BackColor = Color.DimGray;
				l.Size = new Size(bSize, bShift);
				l.Location = new Point(i * bSize + bShift, 420);
				l.TextAlign = ContentAlignment.MiddleCenter;
				l.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, ((System.Byte)(204)));
				panField.Controls.AddRange(new System.Windows.Forms.Control[]{l});
				#endregion
				#region Label Left
				l = new Label();
				l.ForeColor = Color.Aqua;
				l.Tag = "l" + i.ToString();
				l.BackColor = Color.DimGray;
				l.Size = new Size(bShift, bSize);
				l.TextAlign = ContentAlignment.MiddleCenter;
				l.Location = new Point(0, i * bSize + bShift);
				l.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, ((System.Byte)(204)));
				panField.Controls.AddRange(new System.Windows.Forms.Control[]{l});
				#endregion
				#region  Label Right
				l = new Label();
				l.ForeColor = Color.Aqua;
				l.Tag = "r" + i.ToString();
				l.BackColor = Color.DimGray;
				l.Size = new Size(bShift, bSize);
				l.TextAlign = ContentAlignment.MiddleCenter;
				l.Location = new Point(420, i * bSize + bShift);
				l.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, ((System.Byte)(204)));
				panField.Controls.AddRange(new System.Windows.Forms.Control[]{l});
				#endregion
			}
			#endregion
			MF = this; RePaint();
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuFileExit_Click(object sender, System.EventArgs e){this.Close();}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuGameNew_Click(object sender, System.EventArgs e)
		{
			time = new DateTime[2]; timer.Enabled = false; FromDLL(eEvent.eNew, DMov); 
			field.Master = Color.White; RePaint(); stat.Rows.Clear(); auto = false;
			lblClockB.Text = lblClockW.Text = "00 : 00 : 00 : 0"; FlagGOver = false; StartAuto();
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuGameLow_Click(object sender, System.EventArgs e)
		{
			mnuGameMedium.Checked = false; mnuGameHigh.Checked = false;
			mnuGameLow.Checked = true; lblLevel.Text = levelStr + "Низкий";
			LibSetLevel((int)eLevel.llow);
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuGameMedium_Click(object sender, System.EventArgs e)
		{
			mnuGameLow.Checked = false; mnuGameHigh.Checked = false;
			mnuGameMedium.Checked = true; lblLevel.Text = levelStr + "Средний";
			LibSetLevel((int)eLevel.lmedium);
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuGameHigh_Click(object sender, System.EventArgs e)
		{
			mnuGameLow.Checked = false; mnuGameMedium.Checked = false;
			mnuGameHigh.Checked = true; lblLevel.Text = levelStr + "Высокий";
			LibSetLevel((int)eLevel.lhigh);
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuGameCB_Click(object sender, System.EventArgs e)
		{
			mnuGameCW.Checked = false; mnuGameCWB.Checked = false;
			mnuGameNO.Checked = false; mnuGameCB.Checked = true;
			LibSetMaster(0, 1); rotate = Color.White; RePaint(); StartAuto();
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuGameCW_Click(object sender, System.EventArgs e)
		{
			mnuGameCB.Checked = false; mnuGameCWB.Checked = false;
			mnuGameNO.Checked = false; mnuGameCW.Checked = true;
			LibSetMaster(1, 0); rotate = Color.Black; RePaint(); StartAuto();
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuGameCWB_Click(object sender, System.EventArgs e)
		{
			mnuGameCB.Checked = false; mnuGameCW.Checked = false;
			mnuGameNO.Checked = false; mnuGameCWB.Checked = true;
			LibSetMaster(1, 1); RePaint(); StartAuto();
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuGameNO_Click(object sender, System.EventArgs e)
		{
			mnuGameCB.Checked = false; mnuGameCW.Checked = false;
			mnuGameCWB.Checked = false; mnuGameNO.Checked = true;
			LibSetMaster(0, 0);
		}
		// ---------- * ---------------- * --------------- * ------------
		private void mnuViewRotate_Click(object sender, System.EventArgs e)
		{	
			ReColor(ref rotate); RePaint();
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuViewStat_Click(object sender, System.EventArgs e)
		{
			if(mnuViewStat.Checked) Width -= 178; else Width += 178;
			statusBarPanel.Width = Width; mnuViewStat.Checked = !mnuViewStat.Checked;
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void mnuViewClock_Click(object sender, System.EventArgs e)
		{
			mnuViewClock.Checked = !mnuViewClock.Checked;
			lblClockB.Visible = lblClockW.Visible = mnuViewClock.Checked;
		}
		// ---------- * ---------------- * --------------- * ------------	
		private void timer_Tick(object sender, System.EventArgs e)
		{
			int i = field.Master == Color.White ? 0 : 1;
			time[i] = time[i].AddMilliseconds(100); 
			string tm = Zerro(time[i].Hour) + " : " + Zerro(time[i].Minute) + " : " + Zerro(time[i].Second) + " : " + Zerro(time[i].Millisecond / 10);
			if(i == 0)lblClockW.Text = tm; else lblClockB.Text = tm;
			if(timeError < 7) timeError++;
			if(timeError == 6) {
				foreach(Control cnt in panField.Controls)
				{
					string tag = cnt.Tag.ToString();
					if(cnt.GetType().Name == "PictureBox")
					{
						int shift = (int)cnt.Tag; Dot d = Dot.FromArray(shift);
						((PictureBox)cnt).BackColor = Colorss[field.FBlack(d) ? 1 : 0];
					}
				}
				RePaint();}
		}

		// ---------- * ---------------- * --------------- * ------------
		private void mnuFileSave_Click(object sender, System.EventArgs e)
		{
			FileDialog dlg = (FileDialog) new SaveFileDialog();
			dlg.InitialDirectory = Environment.CurrentDirectory; 	// Узнаем текущую директорию
			dlg.Filter = "Save files (*.sav)|*.sav"; dlg.Title = "Сохранение партии";
			dlg.FileName = fileName; dlg.RestoreDirectory = true; 	// Перед закрытием восстановим текущую директорию
			if(dlg.ShowDialog() != DialogResult.OK) return; else fileName = dlg.FileName;
			IFormatter fmt = new BinaryFormatter();	// Создаем форматизатор двоичного файла
			ds.Tables.Clear(); ds.Tables.Add(stat);
			try 
			{
				Stream s = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);//==== Открываем файл для записи
				fmt.Serialize(s, ds); s.Close();	// Просим форматизатор сериализовать набор данных. Закрываем файл
				Text = fileName.Substring (fileName.LastIndexOf('\\') + 1);// Запоминаем путь к текущему документу. Заменяем заголовок окна (на имя файла)
			}
			catch (Exception ex) {MessageBox.Show(ex.ToString());}
		}
		// ---------- * ---------------- * --------------- * ------------
		private void mnuViewAutoRotate_Click(object sender, System.EventArgs e)
		{
			mnuViewAutoRotate.Checked = !mnuViewAutoRotate.Checked;
		}
		// ---------- * ---------------- * --------------- * ------------
		private void mnuFileLoad_Click(object sender, System.EventArgs e)
		{
			FileDialog dlg = (FileDialog) new OpenFileDialog();
			dlg.InitialDirectory = Environment.CurrentDirectory; 	// Узнаем текущую директорию
			dlg.Filter = "Save files (*.sav)|*.sav"; dlg.Title = "Загрузка партии";
			dlg.FileName = fileName; dlg.RestoreDirectory = true; 	// Перед закрытием восстановим текущую директорию
			if(dlg.ShowDialog() != DialogResult.OK) return; else fileName = dlg.FileName;
			IFormatter fmt = new BinaryFormatter();	// Создаем форматизатор двоичного файла
			ds.Tables.Clear(); 
			try 
			{
				Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);//==== Открываем файл для записи
				ds = (DataSet)fmt.Deserialize(s); s.Close(); ds.Dispose();// Просим форматизатор сериализовать набор данных. Закрываем файл
				stat = ds.Tables[0]; gstat.SetDataBinding(stat, ""); //AddStyle(); //gstat.Refresh(); 
				if((string)stat.Rows[stat.Rows.Count - 1]["FBlack"] != "Not") field.FromString((string)stat.Rows[stat.Rows.Count - 1]["FBlack"]);
				else field.FromString((string)stat.Rows[stat.Rows.Count - 1]["FWhite"]); RePaint(); FromDLL(eEvent.eSet, DMov); 
			}
			catch (FileNotFoundException) {MessageBox.Show("Не найден файл: " + fileName, "Загрузка партии");}
			
		}
		// ---------- * ---------------- * --------------- * ------------
		#endregion
	}
	// -----*******--------********--------********--------********-------
	enum eEvent : byte {eManual, eNew, eAuto, eSet};	// Запросы в dll	
	enum eLevel : int {llow = 2, lmedium = 3, lhigh = 4}; //Уровни игры
	// -----*******--------********--------********--------********-------
}
