using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SECtype = SEC.GenericSupport.DataType;
using SECstage = SEC.Nanoeye.NanoStage;
using SEC.Nanoeye;

namespace SEC.Nanoeye.NanoeyeSEM.Settings.AIOsem
{
    class Manager4500P : ISettingManager
    {
        private SettingData setting;

        private bool _Inited = false;
        public bool Inited
        {
            get { return _Inited; }
        }

        public void Save(string fileName)
        {
            setting.AcceptChanges();

            string dir = System.IO.Path.GetDirectoryName(fileName);
            if (!System.IO.Directory.Exists(dir)) { System.IO.Directory.CreateDirectory(dir); }

            StreamWriter sw = new StreamWriter(fileName, false);
            setting.WriteXml(sw);
            sw.Flush();
            sw.Dispose();

            Trace.WriteLine("Setting file save to " + fileName, "Setting");
        }

        /// <summary>
        /// 특정 파일에서 각종 설정을 불러 온다.
        /// </summary>
        /// <param name="fileName">파일 명</param>
        public void Load(string fileName)
        {
            Trace.Assert(fileName != null);

            InitSetting();

            if (System.IO.File.Exists(fileName))
            {
                Trace.WriteLine("Setting file load from " + fileName, "Setting");
                setting.ReadXml(fileName);
            }
            else
            {
                Trace.WriteLine("Setting file is not exist from " + fileName, "Setting");
                ColumnCreate("NA");
            }

            List<string> scanList = new List<string>();

            foreach (SettingData.ScanRow row in setting.Scan)
            {
                scanList.Add(row.Name);
            }

            foreach (string scan in SystemInfoBinder.ScanNames)
            {
                if (!scanList.Contains(scan))
                {
                    SettingData.ScanRow row = setting.Scan.NewScanRow();
                    row.Name = scan;
                    setting.Scan.AddScanRow(row);
                }
            }
            setting.Scan.AcceptChanges();
        }

        public void InitSetting()
        {
            setting = new SettingData();
            _Inited = true;
        }

        #region Scanner
        public SEC.Nanoeye.NanoImage.SettingScanner ScannerLoad(string name)
        {
            Trace.Assert(_Inited);

            SettingData.ScanRow row = setting.Scan.FindByName(name);

            if (row == null) { throw new ArgumentException("Undefined Scan setting."); }

            SEC.Nanoeye.NanoImage.SettingScanner set = new SEC.Nanoeye.NanoImage.SettingScanner();

            set.Name = row.Name;
            set.BlurLevel = row.AverageFilter;
            set.AiDifferential = row.Bipolar;
            set.AiChannel = row.Channel;
            set.AoMaximum = (float)row.DeflexMax;
            set.AoMinimum = (float)row.DeflexMin;
            set.AverageLevel = row.FrameAverage;
            set.FrameHeight = row.FrameHeight;
            set.FrameWidth = row.FrameWith;
            set.ImageHeight = row.ImageHeight;
            set.ImageWidth = row.ImageWidth;
            set.ImageLeft = row.ImageX;
            set.ImageTop = row.ImageY;
            set.PropergationDelay = row.PorpergationDelay;
            set.SampleComposite = row.SampleMixture;
            set.AiClock = row.SamplingFreq;
            set.AoClock = row.DeflexFreq;
            set.RatioX = row.ScanRatioX;
            set.RatioY = row.ScanRatioY;
            set.ShiftX = row.ScanShiftX;
            set.ShiftY = row.ScanShiftY;
            set.PaintX = (float)row.DisplayX;
            set.PaintY = (float)row.DisplayY;
            set.PaintWidth = (float)row.DisplayWidth;
            set.PaintHeight = (float)row.DisplayHeight;
            set.LineAverage = row.LineAverage;

            return set;
        }

        public void ScannerSave(SEC.Nanoeye.NanoImage.SettingScanner set)
        {
            SettingData.ScanRow sr = setting.Scan.FindByName(set.Name);

            if (sr == null)
            {
                sr = setting.Scan.NewScanRow();
                sr.Name = set.Name;
                setting.Scan.AddScanRow(sr);
            }

            sr.AverageFilter = set.BlurLevel;
            sr.Bipolar = set.AiDifferential;
            sr.Channel = set.AiChannel;
            sr.DeflexMax = set.AoMaximum;
            sr.DeflexMin = set.AoMinimum;
            sr.FrameAverage = set.AverageLevel;
            sr.FrameHeight = set.FrameHeight;
            sr.FrameWith = set.FrameWidth;
            sr.ImageHeight = set.ImageHeight;
            sr.ImageWidth = set.ImageWidth;
            sr.ImageX = set.ImageLeft;
            sr.ImageY = set.ImageTop;
            sr.PorpergationDelay = set.PropergationDelay;
            sr.SampleMixture = set.SampleComposite;
            sr.SamplingFreq = (int)(set.AiClock);
            sr.DeflexFreq = (int)set.AoClock;
            sr.ScanRatioX = set.RatioX;
            sr.ScanRatioY = set.RatioY;
            sr.ScanShiftX = set.ShiftX;
            sr.ScanShiftY = set.ShiftY;
            sr.DisplayX = set.PaintX;
            sr.DisplayY = set.PaintY;
            sr.DisplayWidth = set.PaintWidth;
            sr.DisplayHeight = set.PaintHeight;
            sr.LineAverage = set.LineAverage;
            sr.AcceptChanges();
        }

        public string[] ScannerList()
        {
            List<string> result = new List<string>();

            foreach (SettingData.ScanRow row in setting.Scan)
            {
                result.Add(row.Name);
            }

            return result.ToArray();
        }
        #endregion

        #region Column
        public void ColumnCreate(string name)
        {
            Trace.Assert(name != null);
            Trace.Assert(_Inited);

            SettingData.ColumnRow crr = setting.Column.FindByAlias(name);
            if (crr != null)
            {
                throw new ArgumentException("There is same name setting.", "name");
            }

            SettingData.ColumnRow cr = setting.Column.NewColumnRow();
            cr.Alias = name;
            setting.Column.AddColumnRow(cr);
            cr.AcceptChanges();
            setting.Column.AcceptChanges();
        }

        public bool ColumnDelete(string name)
        {
            Trace.Assert(name != null);
            Trace.Assert(_Inited);

            SettingData.ColumnRow dr = setting.Column.FindByAlias(name);

            if (dr != null)
            {
                setting.Column.RemoveColumnRow(dr);
                setting.Column.AcceptChanges();
                setting.AcceptChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string[] ColumnList()
        {
            List<string> result = new List<string>();
            foreach (SettingData.ColumnRow cr in setting.Column)
            {
                if (cr.RowState == System.Data.DataRowState.Deleted) { continue; }
                result.Add(cr.Alias);
            }
            return result.ToArray();
        }

        public void ColumnLoad(string name, SEC.Nanoeye.NanoColumn.ISEMController column, ColumnOnevalueMode mode)
        {
            Trace.Assert(_Inited);
            Trace.Assert(column != null);
            Trace.Assert(name != null);

            SettingData.ColumnRow cr = setting.Column.FindByAlias(name);
            if (cr == null)
            {
                throw new ArgumentException("There is no setting.", "name");
            }

            column.Name = cr.Alias;
            column.HVtext = cr.HVtext;

            ((SECtype.IControlDouble)column["HvElectronGun"]).Value = cr.fHV_Gun;
            //((SECtype.IControlDouble)column["ScanAmplitudeX"]).Value = 0.707d;
            //((SECtype.IControlDouble)column["ScanAmplitudeY"]).Value = 0.707d;
            //((SECtype.IControlDouble)column["ScanAmpX"]).Value = cr.ScanAmpX;
            //((SECtype.IControlDouble)column["ScanAmpY"]).Value = cr.ScanAmpY;

            //((SECtype.IControlInt)column["ScanFeedbackMode"]).Value = 0;
            //((SECtype.IControlDouble)column["ScanMagnificationX"]).Value = 0;
            //((SECtype.IControlDouble)column["ScanMagnificationY"]).Value = 0;
            //((SECtype.IControlDouble)column["ScanRotation"]).Value = 0;
            ((SECtype.IControlBool)column["LensCondenser1WobbleEnable"]).Value = false;
            ((SECtype.IControlDouble)column["LensCondenser1WobbleAmplitude"]).Value = 0;
            ((SECtype.IControlDouble)column["LensCondenser1WobbleFrequence"]).Value = 0;


            ((SECtype.IControlDouble)column["LensCondenser2"]).Value = cr.fLens_CL2;
            ;
            ((SECtype.IControlBool)column["LensCondenser2WobbleEnable"]).Value = false;
            ((SECtype.IControlDouble)column["LensCondenser2WobbleAmplitude"]).Value = 0;
            ((SECtype.IControlDouble)column["LensCondenser2WobbleFrequence"]).Value = 0;

            //((SECtype.IControlDouble)column["LensObjectFine"]).Value = 0.5d;
            //((SECtype.IControlInt)column["LensObjectDirection"]).Value = 0;
            ((SECtype.IControlBool)column["LensObjectWobbleEnable"]).Value = false;
            ((SECtype.IControlDouble)column["LensObjectWobbleAmplitude"]).Value = 0;
            ((SECtype.IControlDouble)column["LensObjectWobbleFrequence"]).Value = 0;

            //((SECtype.IControlDouble)column["BeamShiftX"]).Value = 0;
            //((SECtype.IControlDouble)column["BeamShiftY"]).Value = 0;

            ((SECtype.IControlDouble)column["StigXab"]).Value = cr.rSA_XAB;
            ((SECtype.IControlDouble)column["StigXcd"]).Value = cr.rSA_XCD;
            ((SECtype.IControlBool)column["StigXWobbleEnable"]).Value = false;
            ((SECtype.IControlDouble)column["StigXWobbleAmplitude"]).Value = 0;
            ((SECtype.IControlDouble)column["StigXWobbleFrequence"]).Value = 0;

            ((SECtype.IControlDouble)column["StigYab"]).Value = cr.rSA_YAB;
            ((SECtype.IControlDouble)column["StigYcd"]).Value = cr.rSA_YCD;
            ((SECtype.IControlBool)column["StigYWobbleEnable"]).Value = false;
            ((SECtype.IControlDouble)column["StigYWobbleAmplitude"]).Value = 0;
            ((SECtype.IControlDouble)column["StigYWobbleFrequence"]).Value = 0;


            ((SECtype.IControlDouble)column["GunAlignX"]).Value = cr.rGA_X;
            ((SECtype.IControlDouble)column["GunAlignY"]).Value = cr.rGA_Y;


            ((SECtype.IControlDouble)column["LensObjectCoarse"]).Minimum = cr.fLens_OlMin;
            ((SECtype.IControlDouble)column["LensObjectCoarse"]).Maximum = cr.fLens_OlMax;

            ControllerWDTableGet(column, cr);
            ControllerMagTableGet(column, cr);

            //new Controller 추가 부분
            ((SECtype.IControlDouble)column["ScanGeoXA"]).Value = cr.ScanGeoXA;
            ((SECtype.IControlDouble)column["ScanGeoXB"]).Value = cr.ScanGeoXB;
            ((SECtype.IControlDouble)column["ScanGeoYA"]).Value = cr.ScanGeoYA;
            ((SECtype.IControlDouble)column["ScanGeoYB"]).Value = cr.ScanGeoYB;

            ((SECtype.IControlDouble)column["Scan_AmpXA"]).Value = cr.Scan_AmpXA;
            ((SECtype.IControlDouble)column["Scan_AmpXB"]).Value = cr.Scan_AmpXB;

            ((SECtype.IControlDouble)column["Scan_AmpYA"]).Value = cr.Scan_AmpYA;
            ((SECtype.IControlDouble)column["Scan_AmpYB"]).Value = cr.Scan_AmpYB;

            ((SECtype.IControlDouble)column["VariableX"]).Value = cr.VariableX; 
            ((SECtype.IControlDouble)column["VariableY"]).Value = cr.VariableY;


            SECtype.IControlDouble icb;
            switch (mode)
            {
                case ColumnOnevalueMode.Factory:
                    ((SECtype.IControlDouble)column["HvFilament"]).Value = cr.fHV_Filament;
                    ((SECtype.IControlDouble)column["HvGrid"]).Value = cr.fHV_Grid;
                    ((SECtype.IControlDouble)column["HvCollector"]).Value = cr.fHV_CLT;
                    ((SECtype.IControlDouble)column["HvPmt"]).Value = cr.fHV_PMT;

                    icb = ((SECtype.IControlDouble)column["LensCondenser1"]);
                    icb.BeginInit();
                    icb.Value = cr.fLens_CL1Value;
                    icb.Maximum = cr.fLens_CL1Max;
                    icb.Minimum = cr.fLens_CL1Min;
                    icb.EndInit();

                    //((SECtype.IControlDouble)column["StigX"]).Value = cr.rStig_X;
                    //((SECtype.IControlDouble)column["StigY"]).Value = cr.rStig_Y;
                    break;
                case ColumnOnevalueMode.Run:

                    ((SECtype.IControlDouble)column["HvFilament"]).Value = cr.rHV_Filament;
                    ((SECtype.IControlDouble)column["HvGrid"]).Value = cr.rHV_Grid;
                    ((SECtype.IControlDouble)column["HvCollector"]).Value = cr.rHV_CLT;
                    ((SECtype.IControlDouble)column["HvPmt"]).Value = cr.rHV_PMT;

                    icb = ((SECtype.IControlDouble)column["LensCondenser1"]);
                    icb.BeginInit();
                    icb.Value = cr.rSpotSize;
                    icb.Maximum = cr.fLens_CL1Max;
                    icb.Minimum = cr.fLens_CL1Min;
                    icb.EndInit();

                    ((SECtype.IControlDouble)column["StigX"]).Value = cr.rStig_X;
                    ((SECtype.IControlDouble)column["StigY"]).Value = cr.rStig_Y;

                    ((SECtype.IControlDouble)column["LensObjectCoarse"]).Value = cr.rWD_Selected * ((SECtype.IControlDouble)column["LensObjectCoarse"]).Precision;
                    try
                    {
                        ((SECtype.ITable)column["ScanMagSplineTable"]).SelectedIndex = ((SECtype.ITable)column["ScanMagSplineTable"]).IndexMinimum;
                    }
                    catch { }
                    break;
            }
        }

        public void ColumnSave(SEC.Nanoeye.NanoColumn.ISEMController column, ColumnOnevalueMode mode)
        {
            Trace.Assert(_Inited);
            Trace.Assert(column != null);

            SettingData.ColumnRow cr = setting.Column.FindByAlias(column.Name);
            if (cr == null)
            {
                //cr = setting.Column.NewColumnRow();
                //cr.Alias = column.Name;
                //setting.Column.AddColumnRow(cr);
                return;
            }

            cr.rSA_XAB = ((SECtype.IControlDouble)column["StigXab"]).Value;
            cr.rSA_XCD = ((SECtype.IControlDouble)column["StigXcd"]).Value;
            cr.rSA_YAB = ((SECtype.IControlDouble)column["StigYab"]).Value;
            cr.rSA_YCD = ((SECtype.IControlDouble)column["StigYcd"]).Value;
            cr.rGA_X = ((SECtype.IControlDouble)column["GunAlignX"]).Value;
            cr.rGA_Y = ((SECtype.IControlDouble)column["GunAlignY"]).Value;

            cr.rHV_Filament = ((SECtype.IControlDouble)column["HvFilament"]).Value;
            cr.rHV_Grid = ((SECtype.IControlDouble)column["HvGrid"]).Value;
            cr.rHV_CLT = ((SECtype.IControlDouble)column["HvCollector"]).Value;
            cr.rHV_PMT = ((SECtype.IControlDouble)column["HvPmt"]).Value;

            cr.rSpotSize = ((SECtype.IControlDouble)column["LensCondenser1"]).Value;

            cr.rStig_X = ((SECtype.IControlDouble)column["StigX"]).Value;
            cr.rStig_Y = ((SECtype.IControlDouble)column["StigY"]).Value;

            cr.rWD_Selected = (int)(((SECtype.IControlDouble)column["LensObjectCoarse"]).Value / ((SECtype.IControlDouble)column["LensObjectCoarse"]).Precision);


            cr.rSA_XAB = ((SECtype.IControlDouble)column["StigXab"]).Value;
            cr.rSA_XCD = ((SECtype.IControlDouble)column["StigXcd"]).Value;
            ////((SECtype.IControlBool)column["StigXWobbleEnable"]).Value = false;
            ////((SECtype.IControlDouble)column["StigXWobbleAmplitude"]).Value = 0;
            ////((SECtype.IControlDouble)column["StigXWobbleFrequence"]).Value = 0;

            cr.rSA_YAB = ((SECtype.IControlDouble)column["StigYab"]).Value;
            cr.rSA_YCD = ((SECtype.IControlDouble)column["StigYcd"]).Value;
            //cr.ScanAmpX = ((SECtype.IControlDouble)column["ScanAmpX"]).Value;
            //cr.ScanAmpY = ((SECtype.IControlDouble)column["ScanAmpY"]).Value;


            ////((SECtype.IControlBool)column["StigYWobbleEnable"]).Value = false;
            ////((SECtype.IControlDouble)column["StigYWobbleAmplitude"]).Value = 0;
            ////((SECtype.IControlDouble)column["StigYWobbleFrequence"]).Value = 0;


            //new Controller 추가 부분
            cr.ScanGeoXA = ((SECtype.IControlDouble)column["ScanGeoXA"]).Value;
            cr.ScanGeoXB = ((SECtype.IControlDouble)column["ScanGeoXB"]).Value;
            cr.ScanGeoYA = ((SECtype.IControlDouble)column["ScanGeoYA"]).Value;
            cr.ScanGeoYB = ((SECtype.IControlDouble)column["ScanGeoYB"]).Value;

            cr.Scan_AmpXA = ((SECtype.IControlDouble)column["Scan_AmpXA"]).Value;
            cr.Scan_AmpXB = ((SECtype.IControlDouble)column["Scan_AmpXB"]).Value;

            cr.Scan_AmpYA = ((SECtype.IControlDouble)column["Scan_AmpYA"]).Value;
            cr.Scan_AmpYB = ((SECtype.IControlDouble)column["Scan_AmpYB"]).Value;

            cr.VariableX = ((SECtype.IControlDouble)column["VariableX"]).Value;
            cr.VariableY = ((SECtype.IControlDouble)column["VariableY"]).Value;



            switch (mode)
            {
                case ColumnOnevalueMode.Factory:
                    cr.HVtext = column.HVtext;

                    cr.fHV_Gun = ((SECtype.IControlDouble)column["HvElectronGun"]).Value;
                    cr.fLens_CL2 = ((SECtype.IControlDouble)column["LensCondenser2"]).Value;

                    cr.fHV_Filament = ((SECtype.IControlDouble)column["HvFilament"]).Value;
                    cr.fHV_Grid = ((SECtype.IControlDouble)column["HvGrid"]).Value;
                    cr.fHV_CLT = ((SECtype.IControlDouble)column["HvCollector"]).Value;
                    cr.fHV_PMT = ((SECtype.IControlDouble)column["HvPmt"]).Value;

                    cr.fLens_CL1Value = ((SECtype.IControlDouble)column["LensCondenser1"]).Value;
                    cr.fLens_CL1Max = ((SECtype.IControlDouble)column["LensCondenser1"]).Maximum;
                    cr.fLens_CL1Min = ((SECtype.IControlDouble)column["LensCondenser1"]).Minimum;

                    cr.fLens_OlMax = ((SECtype.IControlDouble)column["LensObjectCoarse"]).Maximum;
                    cr.fLens_OlMin = ((SECtype.IControlDouble)column["LensObjectCoarse"]).Minimum;
                    break;
                case ColumnOnevalueMode.Run:
                    break;
            }

            ControllerWDTableSave(column, cr);
            ControllerMagTableSave(column, cr);
        }

        public void ColumnOneSave(SECtype.IValue icv, ColumnOnevalueMode mode)
        {
            SettingData.ColumnRow cr = setting.Column.FindByAlias((icv.Owner as SECtype.IController).Name);

            switch (icv.Name)
            {
                case "HvElectronGun":
                    if (mode == ColumnOnevalueMode.Factory) { cr.fHV_Gun = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.Run) { }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "HvFilament":
                    if (mode == ColumnOnevalueMode.Factory) { cr.fHV_Filament = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rHV_Filament = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "HvGrid":
                    if (mode == ColumnOnevalueMode.Factory) { cr.fHV_Grid = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rHV_Grid = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "HvCollector":
                    if (mode == ColumnOnevalueMode.Factory) { cr.fHV_CLT = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rHV_CLT = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { cr.rES_CLT = (icv as SECtype.IControlDouble).Value; }
                    break;
                case "HvPmt":
                    if (mode == ColumnOnevalueMode.Factory) { cr.fHV_PMT = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rHV_PMT = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { cr.rES_PMT = (icv as SECtype.IControlDouble).Value; }
                    break;
                case "LensCondenser1":
                    if (mode == ColumnOnevalueMode.Factory)
                    {
                        cr.fLens_CL1Value = (icv as SECtype.IControlDouble).Value;
                        cr.fLens_CL1Max = (icv as SECtype.IControlDouble).Maximum;
                        cr.fLens_CL1Min = (icv as SECtype.IControlDouble).Minimum;
                    }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rSpotSize = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { cr.rES_CL1 = (icv as SECtype.IControlDouble).Value; }
                    break;
                case "LensCondenser2":
                    if (mode == ColumnOnevalueMode.Factory) { cr.fLens_CL2 = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.Run) { }
                    else if (mode == ColumnOnevalueMode.External) { cr.rES_CL2 = (icv as SECtype.IControlDouble).Value; }
                    break;
                case "BeamShiftX":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rBS_X = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "BeamShiftY":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rBS_Y = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigXab":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rSA_XAB = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigXcd":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rSA_XCD = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigYab":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rSA_YAB = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigYcd":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rSA_YCD = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "GunAlignX":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rGA_X = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "GunAlignY":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rGA_Y = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigX":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rStig_X = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigY":
                    if (mode == ColumnOnevalueMode.Factory) { }
                    else if (mode == ColumnOnevalueMode.Run) { cr.rStig_Y = (icv as SECtype.IControlDouble).Value; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "LensWDtableSpline":
                    if (mode == ColumnOnevalueMode.Factory) { ControllerWDTableSave(icv.Owner as SEC.Nanoeye.NanoColumn.ISEMController, cr); }
                    else if (mode == ColumnOnevalueMode.Run) { }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "ScanMagSplineTable":
                    if (mode == ColumnOnevalueMode.Factory) { ControllerMagTableSave(icv.Owner as SEC.Nanoeye.NanoColumn.ISEMController, cr); }
                    else if (mode == ColumnOnevalueMode.Run) { }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;

                //case "ScanAmpX":
                //    if (mode == ColumnOnevalueMode.Factory) { cr.ScanAmpX = (icv as SECtype.IControlDouble).Value; }
                //    else if (mode == ColumnOnevalueMode.Run) { }
                //    else if (mode == ColumnOnevalueMode.External) { cr.ScanAmpX = (icv as SECtype.IControlDouble).Value; }
                //    break;
                //case "ScanAmpY":
                //    if (mode == ColumnOnevalueMode.Factory) { cr.ScanAmpY = (icv as SECtype.IControlDouble).Value; }
                //    else if (mode == ColumnOnevalueMode.Run) { }
                //    else if (mode == ColumnOnevalueMode.External) { cr.ScanAmpY = (icv as SECtype.IControlDouble).Value; }
                //    break;

            }
        }

        public void ColumnOneLoad(SECtype.IValue icv, ColumnOnevalueMode mode)
        {
            SettingData.ColumnRow cr = setting.Column.FindByAlias((icv.Owner as SECtype.IController).Name);

            if (cr == null)
            {
                return;
            }

            SECtype.IControlDouble icd = icv as SECtype.IControlDouble;

            switch (icv.Name)
            {
                case "LensObjectFine":
                    icd.Value = (icd.Maximum + icd.Minimum) / 2;
                    break;
                case "HvElectronGun":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.fHV_Gun; }
                    else if (mode == ColumnOnevalueMode.Run) { }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "HvFilament":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.fHV_Filament; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rHV_Filament; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "HvGrid":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.fHV_Grid; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rHV_Grid; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "HvCollector":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.fHV_CLT; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rHV_CLT; }
                    else if (mode == ColumnOnevalueMode.External) { icd.Value = cr.rES_CLT; }
                    break;
                case "HvPmt":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.fHV_PMT; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rHV_PMT; }
                    else if (mode == ColumnOnevalueMode.External) { icd.Value = cr.rES_PMT; }
                    break;
                case "LensCondenser1":
                    icd.BeginInit();
                    if (mode == ColumnOnevalueMode.Factory)
                    {
                        icd.Value = cr.fLens_CL1Value;
                        icd.Maximum = cr.fLens_CL1Max;
                        icd.Minimum = cr.fLens_CL1Min;
                    }
                    else if (mode == ColumnOnevalueMode.Run)
                    {
                        icd.Value = cr.rSpotSize;
                        icd.Maximum = cr.fLens_CL1Max;
                        icd.Minimum = cr.fLens_CL1Min;
                    }
                    else if (mode == ColumnOnevalueMode.External)
                    {
                        icd.Value = cr.rES_CL1;
                        icd.Maximum = icd.DefaultMax;
                        icd.Minimum = icd.DefaultMin;
                    }
                    icd.EndInit();
                    break;
                case "LensCondenser2":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.fLens_CL2; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.fLens_CL2; }
                    else if (mode == ColumnOnevalueMode.External) { icd.Value = cr.rES_CL2; }
                    break;
                case "BeamShiftX":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0.5; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rBS_X; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "BeamShiftY":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0.5; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rBS_Y; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigXab":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rSA_XAB; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigXcd":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rSA_XCD; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigYab":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rSA_YAB; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigYcd":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rSA_XCD; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "GunAlignX":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.rGA_X; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rGA_X; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "GunAlignY":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.rGA_Y; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rGA_Y; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigX":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0.5; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rStig_X; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "StigY":
                    if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0.5; }
                    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.rStig_Y; }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "LensWDtableSpline":
                    if (mode == ColumnOnevalueMode.Factory) { ControllerWDTableGet((icv.Owner) as SEC.Nanoeye.NanoColumn.ISEMController, cr); }
                    else if (mode == ColumnOnevalueMode.Run) { }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;
                case "ScanMagSplineTable":
                    if (mode == ColumnOnevalueMode.Factory) { ControllerMagTableGet((icv.Owner) as SEC.Nanoeye.NanoColumn.ISEMController, cr); }
                    else if (mode == ColumnOnevalueMode.Run) { }
                    else if (mode == ColumnOnevalueMode.External) { }
                    break;

                //case "ScanAmpX":
                //    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.ScanAmpX; }
                //    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.ScanAmpX; }
                //    else if (mode == ColumnOnevalueMode.External) { icd.Value = cr.ScanAmpX; }
                //    break;

                //case "ScanAmpY":
                //    if (mode == ColumnOnevalueMode.Factory) { icd.Value = cr.ScanAmpY; }
                //    else if (mode == ColumnOnevalueMode.Run) { icd.Value = cr.ScanAmpY; }
                //    else if (mode == ColumnOnevalueMode.External) { icd.Value = cr.ScanAmpY; }
                //    break;
            }
        }

        #region Table
        private void ControllerMagTableGet(SEC.Nanoeye.NanoColumn.ISEMController con, SettingData.ColumnRow cfr)
        {
            string magTable = cfr.Magnification;

            if (magTable == null) { return; }

            string[] magList = magTable.Split('\n');

            object[,] objs = new object[magList.Length - 1, 4];
            for (int i = 0; i < magList.Length - 1; i++)
            {
                string[] magItem = magList[i].Split(',');
                objs[i, 0] = int.Parse(magItem[0]);
                objs[i, 1] = double.Parse(magItem[1]);
                objs[i, 2] = double.Parse(magItem[2]);
                objs[i, 3] = int.Parse(magItem[3]);
            }

            int maxMag = (int)cfr.MaxMagnification;
            object obj = ((object)maxMag);

            ((SECtype.ITable)con["ScanMagSplineTable"]).TableSet(objs);
            ((SECtype.ITable)con["ScanMagSplineTable"]).SetStyle((int)SEC.Nanoeye.NanoColumn.EnumIControlTableSetStyle.Scan_Mag_Maximum_Set, ref obj);
        }

        private void ControllerWDTableGet(SEC.Nanoeye.NanoColumn.ISEMController con, SettingData.ColumnRow cfr)
        {
            string wdTable = cfr.WD;

            if (wdTable == null) { return; }

            string[] wdList = wdTable.Split('\n');

            object[,] objs = new object[wdList.Length - 1, 5];
            for (int i = 0; i < wdList.Length - 1; i++)
            {
                string[] wdItem = wdList[i].Split(',');
                if (wdItem.Length != 5)
                {
                    Trace.WriteLine("Invalid WD table value. - " + wdList[i], "Setting");
                }
                else
                {
                    try
                    {
                        objs[i, 0] = int.Parse(wdItem[0]);	// wd
                        objs[i, 1] = int.Parse(wdItem[1]);	// obj / obj_precision
                        objs[i, 2] = double.Parse(wdItem[2]);	// Mag Constant
                        objs[i, 3] = int.Parse(wdItem[3]);	// Scan Rotation / SR_precision
                        objs[i, 4] = int.Parse(wdItem[4]);	// Beam Shift Rotation / BS_precision
                    }
                    catch (Exception ex)
                    {
                        SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
                        objs[i, 0] = null;
                        Trace.WriteLine("Invalid WD table value. - " + wdList[i], "Setting");
                    }
                }
            }

            SEC.Nanoeye.NanoColumn.I4000M column = con as SEC.Nanoeye.NanoColumn.I4000M;
            (column.LensWDtable as SEC.Nanoeye.NanoColumn.Lens.IWDSplineObjBase).TableSet(objs);
        }

        private void ControllerWDTableSave(SEC.Nanoeye.NanoColumn.ISEMController con, SettingData.ColumnRow cfr)
        {
            SEC.Nanoeye.NanoColumn.I4000M column = con as SEC.Nanoeye.NanoColumn.I4000M;

            object[,] objs = (column.LensWDtable as SEC.Nanoeye.NanoColumn.Lens.IWDSplineObjBase).TableGet();

            if (objs == null) { return; }

            string str = "";

            for (int i = 0; i < objs.GetLength(0); i++)
            {
                str += (string.Format("{0:D},{1:D},{2:F6},{3:D},{4:D}\n",
                                        (int)objs[i, 0], (int)objs[i, 1], (double)objs[i, 2], (int)objs[i, 3], (int)objs[i, 4]));
            }

            cfr.WD = str;
        }

        private void ControllerMagTableSave(SEC.Nanoeye.NanoColumn.ISEMController con, SettingData.ColumnRow cfr)
        {
            object[,] value = ((SECtype.ITable)con["ScanMagSplineTable"]).TableGet();

            if (value == null) { return; }

            string magTable = "";

            for (int cnt = 0; cnt < value.GetLength(0); cnt++)
            {
                magTable += string.Format("{0:D},{1:F6},{2:F6},{3:D}\n",
                    (int)value[cnt, 0], (double)value[cnt, 1], (double)value[cnt, 2], (int)value[cnt, 3]);
            }
            cfr.Magnification = magTable;

            object obj = new object();
            ((SECtype.ITable)con["ScanMagSplineTable"]).SetStyle((int)SEC.Nanoeye.NanoColumn.EnumIControlTableSetStyle.Scan_Mag_Maximum_Get, ref obj);
            cfr.MaxMagnification = (int)obj;
        }
        #endregion

        public void ColumnNameChagne(SEC.GenericSupport.DataType.IController column, string newName)
        {
            Trace.Assert(_Inited);

            SettingData.ColumnRow dr = setting.Column.FindByAlias(column.Name);

            dr.Alias = newName;
            column.Name = newName;
        }

        #endregion


        public void StageSave(string name)
        {
            throw new NotImplementedException();
        }

        public void StageLoad(string name)
        {
            throw new NotImplementedException();
        }

        public void StageCreate(string name)
        {
            throw new NotImplementedException();
        }
    }
}
