using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;


namespace SEC.Nanoeye.Controls
{
    internal class BitmapButtonDesigner : ControlDesigner
    {
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
        }

        protected override void PostFilterProperties(IDictionary properties)
        {
            HideProperty(properties, "FlatAppearance");
            HideProperty(properties, "FlatStyle");
            //HideProperty(properties, "Image");
            //HideProperty(properties, "ImageList");
            //HideProperty(properties, "ImageAlign");
            //HideProperty(properties, "ImageIndex");
            //HideProperty(properties, "ImageKey");
            HideProperty(properties, "BackgroundImageLayout");
            HideProperty(properties, "BackgroundImage");
            //HideProperty(properties, "BackColor");
            HideProperty(properties, "UseVisualStyleBackColor");
            HideProperty(properties, "AutoSize");
			HideProperty(properties, "TabStop");

            base.PostFilterProperties(properties);
        }

        private void HideProperty(IDictionary properties, string name)
        {
            PropertyDescriptor pd =
                properties[name] as PropertyDescriptor;

            pd = TypeDescriptor.CreateProperty(
                pd.ComponentType,
                pd,
                new Attribute[3] { 
                new BrowsableAttribute(false),
                new EditorBrowsableAttribute(EditorBrowsableState.Never),
                new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)});
            
            properties[pd.Name] = pd;
        }
    }
}
