using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoeyeSEM.Settings
{
	public interface ISettingManager
	{
		bool Inited{get;}

		void Save(string fileName);
		/// <summary>
		/// 특정 파일에서 각종 설정을 불러 온다.
		/// </summary>
		/// <param name="fileName">파일 명</param>
		void Load(string fileName);
		void InitSetting();

		#region Scanner
		SEC.Nanoeye.NanoImage.SettingScanner ScannerLoad(string name);

		void ScannerSave(SEC.Nanoeye.NanoImage.SettingScanner set);
		string[] ScannerList();
		#endregion

		#region Column
		void ColumnCreate(string name);

		bool ColumnDelete(string name);

		string[] ColumnList();

		void ColumnLoad(string name, SEC.Nanoeye.NanoColumn.ISEMController column, ColumnOnevalueMode mode);

		void ColumnSave(SEC.Nanoeye.NanoColumn.ISEMController column, ColumnOnevalueMode mode);

		void ColumnOneSave(SECtype.IValue icv, ColumnOnevalueMode mode);

		void ColumnOneLoad(SECtype.IValue icv, ColumnOnevalueMode mode);

		void ColumnNameChagne(SECtype.IController column, string newName);
		#endregion

        #region Stage
        void StageCreate(string name);

        void StageLoad(string name);

        void StageSave(string name);

        #endregion
    }
}
