using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    public partial class ArchivesTabCB : Form
    {
        MiniSEM MainForm;

        private bool _ArchivesTabEnable = false;
        public bool ArchivesTabEnable
        {
            get { return _ArchivesTabEnable; }
            set { _ArchivesTabEnable = value; }
        }

       

        public ArchivesTabCB()
        {

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();
            //FormLocation();
        }

        public ArchivesTabCB(MiniSEM main)
        {
            MainForm = main;

            InitializeComponent();
        }

        private SEC.Nanoeye.Support.Controls.PaintPanel ppSingle;
        public SEC.Nanoeye.Support.Controls.PaintPanel PpSingle
        {
            get { return ppSingle; }
            set { ppSingle = value; }
        }


        private void FormClose(object sender, EventArgs e)
        {
            MainForm.ArchivesFormClose();
            this.Hide();
           
           
        }

        public void FormLocation()
        {
            //this.Location = new Point(MainForm.Right - this.Width, MainForm.Bottom - this.Height);
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.85), Cursor.Position.Y - (int)(this.Height + 10));

            

            //this.Location = new Point();
            
        }

        private void FormShown(object sender, EventArgs e)
        {
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.55), Cursor.Position.Y - (int)(this.Height + 10));
        }


        //private Bitmap _Thumnail = null;
        //public Bitmap Thumnail
        //{
        //    get
        //    {
        //        if (_Thumnail == null)
        //        {
        //            System.IO.MemoryStream ms = new System.IO.MemoryStream(_Thumbnail);
        //            _Thumnail = new Bitmap(ms);
        //        }
        //        return _Thumnail;
        //    }
        //}

        private Size _imageSize = new Size(1280, 960);
        public Size ImageSize
        {
            get { return _imageSize; }
            set
            {
                _imageSize = value;
            }
        }

        private void ArchivesCantrastValueChange(object sender, EventArgs e)
        {
            if (!_ArchivesTabEnable)
            {
                ppSingle.FormClientSizeChange(ppSingle.ClientSize);
                _ArchivesTabEnable = true;
            }
            
            
            ppSingle.BeginInit();
            ppSingle.ImportData(SEC.GenericSupport.Converter.ArrayFromBytearray(MainForm.ArchivesImageData, _imageSize.Width, _imageSize.Height));
           
            ppSingle.Contrast = ArchivesContrast.Value;

            ppSingle.EndInit();
        }

        private void ArchivesBrightnessValueChange(object sender, EventArgs e)
        {
            if (!_ArchivesTabEnable)
            {
                ppSingle.FormClientSizeChange(ppSingle.ClientSize);
                _ArchivesTabEnable = true;
            }

            ppSingle.BeginInit();
            ppSingle.ImportData(SEC.GenericSupport.Converter.ArrayFromBytearray(MainForm.ArchivesImageData, ppSingle.ImageSize.Width, ppSingle.ImageSize.Height));
            ppSingle.Brightness = ArchivesBrightness.Value;

            ppSingle.EndInit();
        }

        




    }
}
