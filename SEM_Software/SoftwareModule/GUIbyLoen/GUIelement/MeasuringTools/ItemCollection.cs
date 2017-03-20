using System;
using System.Collections.Generic;
using System.Text;

namespace SEC.GUIelement.MeasuringTools
{
	internal class ItemCollection : List<ItemBase>
	{
		private MToolsManager m_Owner;

		public ItemCollection(MToolsManager owner)
		{
			m_Owner = owner;
		}

		public new void Insert(int index, ItemBase item)
		{
			item.Parent = m_Owner;
			//item.ItemColor = m_Owner.ItemColor;
            item.ItemColor = m_Owner.Color;

			base.Insert(index, item);
		}

		public new void Remove(ItemBase item)
		{
			base.Remove(item);
		}

		public new void RemoveAt(int index)
		{
			base.RemoveAt(index);
		}

		public new ItemBase this[int index]
		{
			get { return base[index]; }
			set
			{
				base[index] = value;

				if (base[index] != null)
				{
					base[index].Parent = m_Owner;
				}
			}
		}

		public new void Add(ItemBase item)
		{
			item.Parent = m_Owner;
            item.ItemColor = m_Owner.Color;
			//item.ItemColor = m_Owner.ItemColor;
			item.IsSelected = true;

			base.Add(item);
		}		
	}
}
