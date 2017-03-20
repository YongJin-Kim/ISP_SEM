using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    public partial class Measurement_Tools : Form
    {
        private MiniSEM mainForm = null;

        public Measurement_Tools()
        {

            InitializeComponent();

        }

        public Measurement_Tools(MiniSEM main)
        {

            mainForm = main;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();
            
            

        }

        private SEC.Nanoeye.Support.Controls.PaintPanel ppSingle;
        public SEC.Nanoeye.Support.Controls.PaintPanel PpSingle
        {
            get { return ppSingle; }
            set { ppSingle = value; }
        }

        public ListBox MTList
        {
            get { return mtList; }
            set { mtList = value; }
        }

        

        private void FormShown(object sender, EventArgs e)
        {
            this.Location = new Point(Cursor.Position.X - (int)(this.Width *0.85), Cursor.Position.Y - (int)(this.Height + 10));
        }

        public void FormLcation()
        {
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.85), Cursor.Position.Y - (int)(this.Height + 10));

        }

        private void MTools_Click(object sender, EventArgs e)
        {
            Control con = sender as Control;

            switch (con.Name)
            {
                case "mtColor":
                    SEC.GUIelement.MeasuringTools.ItemBase ib = ppSingle.MTools.GetSelectItem();
                    if (ib != null)
                    {
                        ColorDialog cd = new ColorDialog();
                        cd.Color = ib.ItemColor;
                        if (cd.ShowDialog() == DialogResult.OK)
                        {
                            ib.ItemColor = cd.Color;
                            Properties.Settings.Default.MtoolsColor = cd.Color;
                        }
                    }
                    break;
                case "mtDeleteAll":
                    ppSingle.MTools.DeleteItemAll();
                    break;
                case "mtDeleteOne":
                    ppSingle.MTools.DeleteItem();
                    break;
                case "mtIAngular":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Angle);
                    break;
                case "mtIArea":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.ClosePath, mtText.Checked);
                    break;
                case "mtIArrow":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Arrow);
                    break;
                case "mtICancel":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.SelectNull();
                    break;
                case "mtIEllipse":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Ellipse, mtText.Checked);
                    break;
                case "mtILength":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.OpenPath, mtText.Checked);
                    break;
                case "mtILinear":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Line, mtText.Checked);
                    break;
                case "mtIRectangle":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Rectangle, mtText.Checked);
                    break;
                case "mtIText":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.TextBox);
                    break;
                case "mtIMarquios":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.MarquiosScale);
                    break;
                case "mtIPoint":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Point, mtText.Checked);
                    break;
                default:
                    throw new ArgumentException();
            }
            
        }

        private void mtList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ppSingle.MTools.SetSelectItem(mtList.SelectedItem as SEC.GUIelement.MeasuringTools.ItemBase);
        }

        private void MToolsText_CheckedChanged(object sender, EventArgs e)
        {
            SEC.GUIelement.MeasuringTools.ItemBase ib = ppSingle.MTools.GetSelectItem();
            if (ib != null)
            {
                ib.DrawText = mtText.Checked;
                ppSingle.MTools.SelectNull();
                ppSingle.MTools.SetSelectItem(ib);
                ppSingle.Invalidate();

                MTools_ItemChanged(ppSingle.MTools, EventArgs.Empty);
            }
        }
        void MTools_SelectedItemChanged(object sender, EventArgs e)
        {
            SEC.GUIelement.MeasuringTools.ItemBase ib = ppSingle.MTools.GetSelectItem();
            mtList.SelectedItem = ib;
            if (ib != null)
            {
                mtText.Checked = ib.DrawText;
            }
        }

        void MTools_ItemChanged(object sender, EventArgs e)
        {
            mtList.Items.Clear();
            foreach (SEC.GUIelement.MeasuringTools.ItemBase ib in ppSingle.MTools)
            {
                mtList.Items.Add(ib);
            }
            mtList.SelectedItem = ppSingle.MTools.GetSelectItem();
        }

        void FormClose(object sender, EventArgs e)
        {
            mainForm.MToolsClose();
            this.Hide();
        }

        private void MtoolsFontSize_TextChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.FontSize = Convert.ToInt16(MtoolsFontSize.Text);

            ppSingle.MTools.Font = new Font("Arial", Properties.Settings.Default.FontSize);
        }

        private void straightBtn_CheckedChanged(object sender, EventArgs e)
        {
            ppSingle.MTools.IsSymetric = straightBtn.Checked;
        }

        private void MtFontbtn_Click(object sender, EventArgs e)
        {
            FontDialog MtFont = new FontDialog();

            MtFont.Font = ppSingle.MTools.Font;

            if (MtFont.ShowDialog() == DialogResult.OK)
            {
                ppSingle.MTools.Font = MtFont.Font;

            }

            MtFontbtn.Checked = false;

        }
      
        
    }
}
