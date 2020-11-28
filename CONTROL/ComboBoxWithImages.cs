using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
    public partial class ComboBoxWithImages : ComboBox
    {
        private ImageList imageList;
        public ImageList ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }

        public ComboBoxWithImages()
        {
            InitializeComponent();
            DrawMode = DrawMode.OwnerDrawFixed;
            //SetStyle(ControlStyles.UserPaint, true);
        }

        //protected override void OnPaint(PaintEventArgs ea)
        //{
            
        //    base.OnPaint(ea);
        //}

        protected override void OnDrawItem(DrawItemEventArgs ea)
        {
            ea.DrawBackground();
            ea.DrawFocusRectangle();

            ComboBoxWithImagesItem item;
            Size imageSize = imageList.ImageSize;
            Rectangle bounds = ea.Bounds;

            try
            {
                item = (ComboBoxWithImagesItem)Items[ea.Index];

                if (item.ImageIndex != -1)
                {
                    imageList.Draw(ea.Graphics, bounds.Left + (item.Indent * imageSize.Width), bounds.Top,
                                    item.ImageIndex);
                    ea.Graphics.DrawString(item.Text, ea.Font, new
                                        SolidBrush(ea.ForeColor), bounds.Left + (item.Indent * imageSize.Width) + imageSize.Width, bounds.Top);
                }
                else
                {
                    ea.Graphics.DrawString(item.Text, ea.Font, new
                        SolidBrush(ea.ForeColor), bounds.Left + (item.Indent * imageSize.Width), bounds.Top);
                }
            }
            catch
            {
                if (ea.Index != -1)
                {
                    ea.Graphics.DrawString(Items[ea.Index].ToString(), ea.Font, new
                            SolidBrush(ea.ForeColor), bounds.Left, bounds.Top);
                }
                else
                {
                    ea.Graphics.DrawString(Text, ea.Font, new
                        SolidBrush(ea.ForeColor), bounds.Left, bounds.Top);
                }
            }

            base.OnDrawItem(ea);
        }
    }

    public class ComboBoxWithImagesItem
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private int _imageIndex;
        public int ImageIndex
        {
            get { return _imageIndex; }
            set { _imageIndex = value; }
        }

        private int _indent;
        public int Indent
        {
            get { return _indent; }
            set { _indent = value; }
        }

        public ComboBoxWithImagesItem()
            : this("")
        {
        }

        public ComboBoxWithImagesItem(string text)
            : this(text, -1)
        {
        }

        public ComboBoxWithImagesItem(string aText, int aImageIndex)
        {
            _text = aText;
            _imageIndex = aImageIndex;
        }

        /// <summary>
        /// Creates an image of the class
        /// </summary>
        /// <param name="aText">The test to use for the item.</param>
        /// <param name="aImageIndex">The image index to use for the item.</param>
        /// <param name="aIndent">The number of positions to indent the item.</param>
        public ComboBoxWithImagesItem(string aText, int aImageIndex, int aIndent)
        {
            _text = aText;
            _imageIndex = aImageIndex;
            _indent = aIndent;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
