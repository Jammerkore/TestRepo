using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows.Controls
{
    public partial class SelectSingleHierarchyNodeControl : UserControl
    {
        public SelectSingleHierarchyNodeControl()
        {
            InitializeComponent();
            AllowEnchancedNodeSearching = true;
        }

        private SessionAddressBlock _SAB;
        private AllowedNodeLevel _allowedNodeLevel;

        public enum AllowedNodeLevel
        {
            Any,
            Style,
            Color,
            Size,
            NoSize //TT#1313-MD -jsobek -Header Filters
        }

        public bool AllowEnchancedNodeSearching { get; set; }

        //Begin TT#1349-MD -jsobek -Store Filters - Merchandise Selector - add new "No Size" option to prevent selection sizes
        //Read soft text messages
        public struct UIMessages
        {
            public static string InvalidMerchandiseNode = MIDText.GetTextOnly(eMIDTextCode.msg_SelectSingleHierarchyNode_InvalidMerchandiseNode); //"Invalid Merchandise Node"
            public static string OnlyStyle = ((int)eMIDTextCode.msg_SelectSingleHierarchyNode_OnlyStyle).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_SelectSingleHierarchyNode_OnlyStyle); //"Selected Merchandise must be at the Style Level."
            public static string OnlyColor = ((int)eMIDTextCode.msg_SelectSingleHierarchyNode_OnlyColor).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_SelectSingleHierarchyNode_OnlyColor); //"Selected Merchandise must be at the Color Level."
            public static string OnlySize = ((int)eMIDTextCode.msg_SelectSingleHierarchyNode_OnlySize).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_SelectSingleHierarchyNode_OnlySize); //"Selected Merchandise must be at the Size Level."
            public static string NoSize = ((int)eMIDTextCode.msg_SelectSingleHierarchyNode_NoSize).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_SelectSingleHierarchyNode_NoSize); //"Selected Merchandise cannot be at the Size Level."
        }
        //End TT#1349-MD -jsobek -Store Filters - Merchandise Selector - add new "No Size" option to prevent selection sizes

        public void LoadData(SessionAddressBlock SAB, AllowedNodeLevel allowedNodeLevel)
        {
            _SAB = SAB;
            _allowedNodeLevel = allowedNodeLevel;
        }

        public int GetNode()
        {
            if (this.txtHierarchyNode.Tag == null)
            {
                return -1;
            }
            else
            {
                HierarchyNodeProfile hnp = (HierarchyNodeProfile)this.txtHierarchyNode.Tag;
                return hnp.Key;
            }
        }

        //Begin TT#1313-MD -jsobek -Header Filters
        public int GetHierarchyRID()
        {
            if (this.txtHierarchyNode.Tag == null)
            {
                return -1;
            }
            else
            {
                HierarchyNodeProfile hnp = (HierarchyNodeProfile)this.txtHierarchyNode.Tag;
                return hnp.HierarchyRID;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters

        public string GetText()
        {
            return txtHierarchyNode.Text;
            
        }

        private void txtHierarchyNode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            string key = e.Button.Key;
        }
       
        private void txtHierarchyNode_DragDrop(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList;
            try
            {
               
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                     
                    int NodeRID = cbList.ClipboardProfile.Key;
                    HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(NodeRID, true, true);

                 
                    if (IsMerchandiseAllowed(hnp) == false)
                    {
                        return;
                    }

                    SetNodeText(hnp.Text);
                    this.txtHierarchyNode.Tag = hnp;

                    RaiseNodeChangedEvent(hnp.Key);
                
                    SetError(string.Empty);
                    this._textChanged = false; //TT#1416-MD -jsobek -Filters - Merchandise Node Validation
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //Begin TT#1349-MD -jsobek -Store Filters - Merchandise Selector - add new "No Size" option to prevent selection sizes
        private bool IsMerchandiseAllowed(HierarchyNodeProfile hnp)
        {
            bool isAllowed = true;
            if (_allowedNodeLevel == AllowedNodeLevel.Style && hnp.LevelType != eHierarchyLevelType.Style)
            {
                //MessageBox.Show("Only Style nodes can be dropped here.", "Invalid Merchandise Node", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(UIMessages.OnlyStyle, UIMessages.InvalidMerchandiseNode, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                isAllowed = false;
            }
            else if (_allowedNodeLevel == AllowedNodeLevel.Color && hnp.LevelType != eHierarchyLevelType.Color)
            {
                //MessageBox.Show("Only Color nodes can be dropped here.", "Invalid Merchandise Node", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(UIMessages.OnlyColor, UIMessages.InvalidMerchandiseNode, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                isAllowed = false;
            }
            else if (_allowedNodeLevel == AllowedNodeLevel.Size && hnp.LevelType != eHierarchyLevelType.Size)
            {
                //MessageBox.Show("Only Size nodes can be dropped here.", "Invalid Merchandise Node", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(UIMessages.OnlySize, UIMessages.InvalidMerchandiseNode, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                isAllowed = false;
            }
            else if (_allowedNodeLevel == AllowedNodeLevel.NoSize && hnp.LevelType == eHierarchyLevelType.Size)
            {
                //MessageBox.Show("Size nodes can not be dropped here.", "Invalid Merchandise Node", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(UIMessages.NoSize, UIMessages.InvalidMerchandiseNode, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                isAllowed = false;
            }
            return isAllowed;
        }
        //End TT#1349-MD -jsobek -Store Filters - Merchandise Selector - add new "No Size" option to prevent selection sizes

        private void txtHierarchyNode_DragEnter(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            try
            {

              

                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        //SetSecurity();
                        //if (FunctionSecurity.AllowUpdate ||
                        //    FunctionSecurity.AllowView)
                        //{
                        //    e.Effect = DragDropEffects.All;
                        //}
                        //else
                        //{
                        //    e.Effect = DragDropEffects.None;
                        //}
                        e.Effect = DragDropEffects.All;
                   


                    int X, Y;

                    // Get mouse position in client coordinates
                    Point p = PointToClient(Control.MousePosition);
                    int xPos, yPos;
                    int _spacing = 2;
                    int _imageHeight;
                    int _imageWidth;
                    MIDGraphics.BuildDragImage(cbList, imageListDrag, 0, _spacing,
                                Font, ForeColor, out _imageHeight, out _imageWidth);

                    xPos = _imageWidth / 2;
                    yPos = _imageHeight / 2;

                    // Begin dragging image
                    DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, xPos, yPos);



                    // calculate image position based on upper left coordinate of window, not the client area
                    X = p.X + SystemInformation.BorderSize.Width;
                    Y = p.Y + 0;// SystemInformation.CaptionHeight;
                    //if (Menu != null && Menu.MenuItems.Count > 0)
                    //{
                    //    Y += SystemInformation.MenuHeight;
                    //}

                    DragHelper.ImageList_DragEnter(Handle, X, Y);
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }

               




            
                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //private FunctionSecurityProfile _functionSecurity;
        //private bool NodeDropAllowViewOrUpdate()
        //{
            //return true;

            //if (_functionSecurity == null)
            //{
            //    _functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsNodePropertiesOverrides);
            //}

            //if (_functionSecurity.AllowUpdate || _functionSecurity.AllowView)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        //}

      

        private void txtHierarchyNode_DragOver(object sender, DragEventArgs e)
        {

                int X, Y;
                DragHelper.ImageList_DragLeave(Handle);

                // Get mouse position in client coordinates
                Point p = PointToClient(Control.MousePosition);

                // calculate image position based on upper left coordinate of window, not the client area
                X = p.X + SystemInformation.BorderSize.Width;
                Y = p.Y;// +SystemInformation.CaptionHeight;
                //if (Menu != null && Menu.MenuItems.Count > 0)
                //{
                //    Y += SystemInformation.MenuHeight;
                //}

                DragHelper.ImageList_DragMove(X, Y);

                DragHelper.ImageList_DragEnter(Handle, X, Y);
         
        }

        private void txtHierarchyNode_DragLeave(object sender, EventArgs e)
        {
            DragHelper.ImageList_DragLeave(Handle);
        }


       

        private void HandleException(Exception ex)
        {
            _hasError = true;  //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
            MessageBox.Show(ex.ToString(),"Error in clipboard data.",  MessageBoxButtons.OK, MessageBoxIcon.Error);
        }







        private bool _hasError = false;
        
        public bool _textChanged = false;
        //private bool _priorError = false;
        //private void txtHierarchyNode_Validating(object sender, CancelEventArgs e)
        //{
        //    string errorMessage;

        //    try
        //    {
        //        _hasError = false;      //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        //        if (txtHierarchyNode.Text.Trim() == string.Empty && txtHierarchyNode.Tag != null)
        //        {
        //            _hasError = true;  //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        //            ResetText();
        //            txtHierarchyNode.Tag = null;
        //           // NodeRID = Include.NoRID;
        //            RaiseNodeChangedEvent(Include.NoRID);
        //        }
        //        else
        //        {
        //            if (_textChanged)
        //            {
        //                _textChanged = false;

        //                HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeDataWithQualifiedID(txtHierarchyNode.Text, false); //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        //                if (hnp.Key == Include.NoRID)
        //                {
        //                    if (AllowEnchancedNodeSearching == true && (_allowedNodeLevel == AllowedNodeLevel.Any || _allowedNodeLevel == AllowedNodeLevel.Style))
        //                    {
        //                        hnp = _SAB.HierarchyServerSession.GetNodeDataFromBaseSearchString(txtHierarchyNode.Text);
        //                    }
        //                }
                 


        //                if (hnp.Key == Include.NoRID)
        //                {
        //                    _priorError = true;

        //                    errorMessage = string.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), txtHierarchyNode.Text);

        //                    _hasError = true;  //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        //                    SetError( errorMessage);

        //                    MessageBox.Show(errorMessage);

        //                    RaiseNodeChangedEvent(Include.NoRID);

        //                    if (e != null)
        //                    {
        //                        e.Cancel = true;
        //                    }
        //                }
        //                else
        //                {
        //                    if (IsMerchandiseAllowed(hnp) == false)
        //                    {
        //                        _hasError = true;  //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        //                        return;
        //                    }

        //                    SetText(hnp.Text);
        //                    txtHierarchyNode.Tag = hnp;
        //                    //NodeRID = hnp.Key;
        //                    RaiseNodeChangedEvent(hnp.Key);
        //                }
        //            }
        //            else if (_priorError)
        //            {
        //                _hasError = true;  //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        //                _priorError = false;
        //                if (txtHierarchyNode.Tag == null)
        //                {
        //                    ResetText();
        //                    RaiseNodeChangedEvent(Include.NoRID);
        //                }
        //                else
        //                {
        //                    SetText(((HierarchyNodeProfile)txtHierarchyNode.Tag).Text);
        //                    RaiseNodeChangedEvent(((HierarchyNodeProfile)txtHierarchyNode.Tag).Key);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}
        //protected void txtHierarchyNode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        //{
         
        //    _textChanged = true;
        //    SetError(string.Empty);
        //}

        //Begin TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        private void txtHierarchyNode_Validating(object sender, CancelEventArgs e)
        {
            if (_textChanged)
            {
                IsValid(raiseChangedEvent: true);
            }
        }
        //End TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 

        System.Windows.Forms.ErrorProvider _errorProvider = null;
    
        private void SetError(string s)
        {
            if (_errorProvider == null)
            {
                _errorProvider = new System.Windows.Forms.ErrorProvider(this);
            }
            _errorProvider.SetError(this.txtHierarchyNode, s);
        }

        protected void txtHierarchyNode_Validated(object sender, System.EventArgs e)
        {
            //_textChanged = false;
            //_priorError = false;
            SetError(string.Empty); 
        }

        private bool _settingText = false;
        private void txtHierarchyNode_TextChanged(object sender, EventArgs e)
        {
            if (_settingText == false)
            {
                _textChanged = true;
                RaiseIsDirtyEvent();
            }
        }
        private void ResetNodeText()
        {
            _settingText = true;
            txtHierarchyNode.Text = string.Empty;
            _settingText = false;
        }
        private void SetNodeText(string s)
        {
            _settingText = true;
            txtHierarchyNode.Text = s;
            _settingText = false;
        }
        public void SetNode(HierarchyNodeProfile hnp)
        {
            if (hnp.Text == null)
            {
                SetNodeText(string.Empty);
            }
            else
            {
                SetNodeText(hnp.Text);
            }
         
            txtHierarchyNode.Tag = hnp;
            this._textChanged = false;
        }

        private void txtHierarchyNode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //txtHierarchyNode_Validating(null, null);
                IsValid(raiseChangedEvent: true);
            }
        }
        //Begin TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        public bool IsValid(bool raiseChangedEvent)
        {
            bool isValid = true;
            //txtHierarchyNode_Validating(null, null);
            string errorMessage;

            try
            {
                _hasError = false;      //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
                if (txtHierarchyNode.Text.Trim() == string.Empty)
                {
                    //_hasError = true;  //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
                    //ResetNodeText();
                    txtHierarchyNode.Tag = null;
                    // NodeRID = Include.NoRID;
                    if (raiseChangedEvent)
                    {
                        RaiseNodeChangedEvent(Include.NoRID);
                    }
                }
                else
                {
                    if (_textChanged)
                    {
              

                        HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeDataWithQualifiedID(txtHierarchyNode.Text, false); //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
                        if (hnp.Key == Include.NoRID)
                        {
                            if (AllowEnchancedNodeSearching == true && (_allowedNodeLevel == AllowedNodeLevel.Any || _allowedNodeLevel == AllowedNodeLevel.Style))
                            {
                                hnp = _SAB.HierarchyServerSession.GetNodeDataFromBaseSearchString(txtHierarchyNode.Text);
                            }
                        }



                        if (hnp.Key == Include.NoRID)
                        {
      

                            errorMessage = string.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), txtHierarchyNode.Text);

                            _hasError = true;  //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
                            SetError(errorMessage);

                            MessageBox.Show(errorMessage);

                            if (raiseChangedEvent)
                            {
                                RaiseNodeChangedEvent(Include.NoRID);
                            }
                       
                        }
                        else
                        {
                            if (IsMerchandiseAllowed(hnp) == false)
                            {
                                _hasError = true;  //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 

                            }
                            else
                            {
                                _textChanged = false;
                                SetNodeText(hnp.Text);
                                txtHierarchyNode.Tag = hnp;
                                //NodeRID = hnp.Key;
                                if (raiseChangedEvent)
                                {
                                    RaiseNodeChangedEvent(hnp.Key);
                                }
                            }
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }








            if (_hasError)
            {
                isValid = false;
            }
            return isValid;
        }
        //End TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 

        public event NodeChangedEventHandler NodeChangedEvent;
        public virtual void RaiseNodeChangedEvent(int NodeRID)
        {
            if (NodeChangedEvent != null)
                NodeChangedEvent(this, new NodeChangedEventArgs(NodeRID));
        }
        public class NodeChangedEventArgs
        {
            public NodeChangedEventArgs(int NodeRID) { this.NodeRID = NodeRID; }
            public int NodeRID { get; private set; } // readonly
   
        }
        public delegate void NodeChangedEventHandler(object sender, NodeChangedEventArgs e);

        public event IsDirtyEventHandler IsDirtyEvent;
        public virtual void RaiseIsDirtyEvent()
        {
            if (IsDirtyEvent != null)
                IsDirtyEvent(this, new IsDirtyEventArgs());
        }
        public class IsDirtyEventArgs
        {
            public IsDirtyEventArgs() {  }

        }
        public delegate void IsDirtyEventHandler(object sender, IsDirtyEventArgs e);

    }
}
