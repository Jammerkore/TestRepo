using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterBuilderListNodeContainer : UserControl
    {
        public filterBuilderListNodeContainer()
        {
            InitializeComponent(); ;
        }
        public filterBuilderListNodeContainer(filterManager manager, filterBuilderListNode ln)
        {
            InitializeComponent();
            this.manager = manager;
            uiNode = ln;
            //SetPosition();
        }
        public filterBuilderListNode uiNode;
        private filterManager manager;

        private const int indentation = 16;
        private const int pictureWidth = 16;

        public void SetPosition()
        {
            this.Controls.Add(uiNode);


            UpdatePosition();
            UpdateChildPicture();

        }
        private void UpdateChildPicture()
        {
            if (uiNode.conditionNode.IsRootLevel == false)
            {
                if (manager.currentFilter.IsLastChild(uiNode.conditionNode) == true)
                {
                    this.pChild.Visible = false;
                    this.pLastChild.Visible = true;
                }
                else
                {
                    this.pChild.Visible = true;
                    this.pLastChild.Visible = false;
                }
            }
            else
            {
                this.pChild.Visible = false;
                this.pLastChild.Visible = false;
            }
        }
        private void UpdatePosition()
        {
            if (uiNode.conditionNode.IsRootLevel == false)
            {
                this.Controls[0].Location = new System.Drawing.Point(indentation * (uiNode.conditionNode.NodeLevel - 1), 0);
                this.Controls[1].Location = new System.Drawing.Point(indentation * (uiNode.conditionNode.NodeLevel - 1), 0);
                this.Controls[2].Location = new System.Drawing.Point(indentation * (uiNode.conditionNode.NodeLevel - 1), 0);
                //set the position of the node control
                this.Controls[3].Location = new System.Drawing.Point(indentation * (uiNode.conditionNode.NodeLevel - 1) + pictureWidth + 1, 0);

                //Draw straight lines where needed
                for (int i = 2; i <= uiNode.conditionNode.NodeLevel - 1; i++)
                {
                    if (IsParentAtNodeLevelLastChild(i, uiNode.conditionNode) == false)
                    {
                        PictureBox p = new PictureBox();
                        p.Image = this.pStraight.Image;
                        p.Height = 16;
                        p.Width = 16;
                        Padding pd = p.Margin;
                        pd.All = 0;
                        p.Margin = pd;
                        this.Controls.Add(p);
                        p.Location = new System.Drawing.Point(indentation * (i - 1), 0);
                    }
                }

            }
            else
            {
                this.Controls[0].Location = new System.Drawing.Point(0, 0);
                this.Controls[1].Location = new System.Drawing.Point(0, 0);
                this.Controls[2].Location = new System.Drawing.Point(0, 0);
                this.Controls[3].Location = new System.Drawing.Point(0, 0);
            }
            //this.Controls[3].Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left);
        }

        private bool IsParentAtNodeLevelLastChild(int nodeLevel, ConditionNode cn)
        {
            ConditionNode parent = cn.Parent;

            while (parent.NodeLevel != nodeLevel)
            {
                parent = parent.Parent;
            }
            return manager.currentFilter.IsLastChild(parent);
        }


    }
}
