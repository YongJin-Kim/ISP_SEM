using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	public abstract class TableBase : ControlValueBase, ITable
	{
		public abstract int IndexMinimum { get; }

		public abstract int IndexMaximum { get; }

		public abstract int SelectedIndex { get; set; }

		public abstract object SeletedItem { get; set; }

		public abstract int Length { get; }

		public abstract object[] this[int index] { get; set; }

		public event EventHandler  SelectedIndexChanged;
		protected virtual void OnSelectedIndexChanged()
		{
			if (!_IsInited) { return; }

			if (SelectedIndexChanged != null)
			{
				SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  TableChanged;
		protected virtual void OnTableChanged()
		{
			if (!_IsInited) { return; }
			if (TableChanged != null)
			{
				TableChanged(this, EventArgs.Empty);
			}
		}

		public abstract void TableSet(object[,] values);

		public abstract object[,] TableGet();

		public abstract void TableAppend(object[] values);

		public abstract void TableRemove(object key);

		public abstract void SetStyle(int index, ref object value);

		public virtual void TableChange(object preKey, object[] values)
		{
			BeginInit();

			TableRemove(preKey);
			TableAppend(values);

			EndInit();
		}

		public override void BeginInit()
		{
			throw new NotImplementedException();
		}

		public override void EndInit(bool sync)
		{
			throw new NotImplementedException();
		}

		public override void Sync()
		{
			throw new NotImplementedException();
		}

		public override bool Validate()
		{
			throw new NotImplementedException();
		}
	}
}
