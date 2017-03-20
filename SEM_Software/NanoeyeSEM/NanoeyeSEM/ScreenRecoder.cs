using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

using System.IO;

//using SEC.MultiMedia.ScreenRecorder;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class ScreenRecorder : Form
	{
        MiniSEM MainForm;

		/// <summary>
		/// 동영상 저장을 할 window
		/// </summary>
		public Control ImageWindow { get; set; }

        private bool _RecorDingEnable = false;
        public bool RecorDingEnable
        {
            get { return _RecorDingEnable; }
            set { _RecorDingEnable = value; }

        }

		private SEC.MultiMedia.ScreenRecorder.Recorder recoder;
        

		public ScreenRecorder()
		{
			InitializeComponent();

			// 90도나 270도 로테이트된 것을 처리 할 수 없으므로 아래 방법을 사용 할 수 없음.
			//rotateCb.DataSource = System.Enum.GetValues(typeof(RotateFlipType));

			rotateCb.Items.Clear();
			rotateCb.Items.Add(RotateFlipType.RotateNoneFlipNone);
			rotateCb.Items.Add(RotateFlipType.RotateNoneFlipX);
			rotateCb.Items.Add(RotateFlipType.RotateNoneFlipY);
			rotateCb.Items.Add(RotateFlipType.RotateNoneFlipXY);
			rotateCb.SelectedIndex = 0;
		}

        public ScreenRecorder(MiniSEM main)
        {
            MainForm = main;

            InitializeComponent();

            // 90도나 270도 로테이트된 것을 처리 할 수 없으므로 아래 방법을 사용 할 수 없음.
            //rotateCb.DataSource = System.Enum.GetValues(typeof(RotateFlipType));

            rotateCb.Items.Clear();
            rotateCb.Items.Add(RotateFlipType.RotateNoneFlipNone);
            rotateCb.Items.Add(RotateFlipType.RotateNoneFlipX);
            rotateCb.Items.Add(RotateFlipType.RotateNoneFlipY);
            rotateCb.Items.Add(RotateFlipType.RotateNoneFlipXY);
            rotateCb.SelectedIndex = 0;
        }

		protected override void OnLoad(EventArgs e)
		{
			TextManager.Instance.DefineText(this);

			base.OnLoad(e);
		}


        
        

		private void file_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.AddExtension = true;
			sfd.DefaultExt = "avi";
			sfd.Filter = "avi files (*.avi)|*.avi";
			if (sfd.ShowDialog(this) != DialogResult.OK)
			{
				sfd.Dispose();
				return;
			}
			filename.Text = sfd.FileName;
		}

		Timer runtimer;
		Stopwatch watch;

		private void run_Click(object sender, EventArgs e)
		{
			if (ImageWindow == null)
			{
				throw new ArgumentException("ImageWindow가 정의 되지 않음");
			}
			if (run.Checked)
			{	// 저장 중이라면
				recoder.Stop();

				run.Checked = false;
                

				//TextManager.TextStruct ts = TextManager.Instance.GetString("RECODER_Start");
				//run.Font = ts.Font;
				//run.Text = ts.Text;
                run.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_record_enable;
             
				statusTSSL.Text = "Stopped...";
				runtimer.Stop();
				runtimer.Dispose();


				file.Enabled = true;
				filename.Enabled = true;
				areaImage.Enabled = true;
				areaMain.Enabled = true;
				areaScreen.Enabled = true;
				fpsValue.Enabled = true;

                MainForm.RecordingEnable = false;

			}
			else
			{
				string filePath = filename.Text;
                run.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_record_select;
                MainForm.RecordingEnable = true;

				if (filePath == "")
				{
					file_Click(null, EventArgs.Empty);
					filePath = filename.Text;
					if (filePath == "") { return; }
				}

				if (!filePath.Contains(@"\"))
				{
					filePath = @"C:\" + filePath;
				}

				string directory = filePath.Remove(filePath.LastIndexOf('\\'));
				//string directory = filePath;
				if (!Directory.Exists(directory))
				{
					MessageBox.Show(SystemInfoBinder.Default.MainForm, TextManager.Instance.GetString("Message_DirectoryInvalid").Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				float fps = (float)(fpsValue.Value);
				
				recoder = new SEC.MultiMedia.ScreenRecorder.Recorder();
				recoder.RotateFlip = (RotateFlipType)(rotateCb.SelectedItem);
				try
				{
					if (areaImage.Checked)
					{
						if (!recoder.Start(ImageWindow, filePath, fps, true))
						{
							recoder = null;
							return;
						}
					}
					else if (areaMain.Checked)
					{
						//if ( !recoder.Start((Control)Owner, filePath, fps) ) {
						if (!recoder.Start(Owner, filePath, fps, true))
						{
							recoder = null;
							return;
						}
					}
					else
					{ // areaScreen.checked
						if (!recoder.Start(Screen.FromControl(Owner).Bounds, filePath, fps))
						{
							recoder = null;
							return;
						}
					}
				}
				catch (Exception ex)
				{
					SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
					MessageBox.Show(SystemInfoBinder.Default.MainForm, TextManager.Instance.GetString("Message_ScreenRecoderStartFail").Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					recoder = null;
					return;
				}

				run.Checked = true;

				//TextManager.TextStruct ts =TextManager.Instance.GetString("RECODER_Stop");
				//run.Text = ts.Text;
				//run.Font = ts.Font;

                statusTSSL.ForeColor = Color.FromArgb(50, 235, 251);
                filename.ForeColor = Color.FromArgb(255, 255, 255);
				statusTSSL.Text = "Recording...";

				watch = new Stopwatch();
				watch.Start();

				runtimer = new Timer();
				runtimer.Tick += new EventHandler(runtimer_Tick);
				runtimer.Interval = 500;
				runtimer.Start();

				file.Enabled = false;
				filename.Enabled = false;
				areaImage.Enabled = false;
				areaMain.Enabled = false;
				areaScreen.Enabled = false;
				fpsValue.Enabled = false;
			}

            SEC.GenericSupport.SingleForm.RecordingEnable(run.Checked);
            //SEC.GenericSupport.SingleForm.RecordChang();
            
		}

		void runtimer_Tick(object sender, EventArgs e)
		{
            runtimeTSSL.ForeColor = Color.FromArgb(50, 235, 251);
			runtimeTSSL.Text = watch.Elapsed.ToString();
		}

        private void FormShown(object sender, EventArgs e)
        {
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.9), Cursor.Position.Y - (int)(this.Height + 10));
        
        }

        private void FormClose(object sender, EventArgs e)
        {
            MainForm.RecordBtn_Close();
            this.Hide();
        }

	}
}
