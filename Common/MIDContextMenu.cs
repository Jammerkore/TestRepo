using System;

namespace MIDRetail.Common
{
	/// <summary>
	/// Summary description for MIDContextMenu.
	/// </summary>
	public class MIDContextMenu
	{
		public MIDContextMenu()
		{
			//
			// TODO: Add constructor logic here
			//
//			ButtonTool btInsertBefore						= new ButtonTool("btInsertBefore");
//			btInsertBefore.SharedProps.Caption				= MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before);
//			btInsertBefore.SharedProps.Shortcut			= Shortcut.CtrlShiftB;
//			//				btInsertBefore.SharedProps.AppearancesSmall.Appearance.Image	= 27;
//			this.utmMethodsGrid.Tools.Add(btInsertBefore);
//
//			ButtonTool btInsertAfter				= new ButtonTool("btInsertAfter");
//			btInsertAfter.SharedProps.Caption		= MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after);
//			btInsertAfter.SharedProps.Shortcut		= Shortcut.CtrlShiftA;
//			ilMethodsGrid.Images.Add(Image.FromFile("C:\\scmvs2017\\allocation\\Graphics\\Right.bmp"));
//			this.utmMethodsGrid.ImageListSmall = this.ilMethodsGrid;
//			//				btInsertAfter.SharedProps.AppearancesLarge.Appearance.Image	= Image.FromFile(MIDConfigurationManager.AppSettings["ApplicationRoot"] + Include.MIDGraphicsDir + "\\Right.bmp");
//			//				btInsertAfter.SharedProps.AppearancesSmall.Appearance.Image	= Image.FromFile(MIDConfigurationManager.AppSettings["ApplicationRoot"] + Include.MIDGraphicsDir + "\\Right.bmp");
//			//				btInsertAfter.SharedProps.AppearancesLarge.Appearance.Image	= Image.FromFile("C:\\scmvs2017\\allocation\\Graphics\\Right.bmp");
//			//				btInsertAfter.SharedProps.AppearancesSmall.Appearance.Image	= Image.FromFile("C:\\scmvs2017\\allocation\\Graphics\\Right.bmp");
//			btInsertAfter.SharedProps.AppearancesSmall.Appearance.Image	= 0;
//			this.utmMethodsGrid.Tools.Add(btInsertAfter);
//
//			ButtonTool btMoveUp						= new ButtonTool("btMoveUp");
//			btMoveUp.SharedProps.Caption			= "Move Up";
//			btMoveUp.SharedProps.Shortcut		= Shortcut.CtrlShiftU;
//			//				btInsertAfter.SharedProps.AppearancesSmall.Appearance.Image	= 28;
//			this.utmMethodsGrid.Tools.Add(btMoveUp);
//
//			ButtonTool btMoveDown						= new ButtonTool("btMoveDown");
//			btMoveDown.SharedProps.Caption			= "Move Down";
//			btMoveDown.SharedProps.Shortcut		= Shortcut.CtrlShiftD;
//			//				btInsertAfter.SharedProps.AppearancesSmall.Appearance.Image	= 28;
//			this.utmMethodsGrid.Tools.Add(btMoveDown);
//
//			PopupMenuTool methodsGridMenuTool		= new PopupMenuTool("mnuMethodsGrid");
//
//			methodsGridMenuTool.SharedProps.Caption	= "Tearoff Menu";
//			methodsGridMenuTool.AllowTearaway		= true;
//
//			PopupMenuTool insertMenuTool							= new PopupMenuTool("mnuInsert");
//
//			insertMenuTool.SharedProps.Caption						= MIDText.GetTextOnly(eMIDTextCode.lbl_Insert);
//			insertMenuTool.Settings.ToolAppearance.BackColor		= Color.Transparent;
//			//				insertMenuTool.Settings.Appearance.BackColor			= SystemColors.ControlDark;
//			//				insertMenuTool.Settings.Appearance.BackColor2			= SystemColors.Control;
//			insertMenuTool.Settings.Appearance.BackGradientStyle	= GradientStyle.HorizontalBump;
//			//				insertMenuTool.SharedProps.AppearancesLarge.Appearance.Image	= Image.FromFile("C:\\scmvs2017\\allocation\\Graphics\\Right.bmp");
//			//				insertMenuTool.SharedProps.AppearancesSmall.Appearance.Image	= Image.FromFile("C:\\scmvs2017\\allocation\\Graphics\\Right.bmp");
//			insertMenuTool.SharedProps.AppearancesSmall.Appearance.Image	= 0;
//
//
//			// Add the mnuMethodsGrid menu tool to the root tools collection.
//			this.utmMethodsGrid.Tools.Add(methodsGridMenuTool);
//			this.utmMethodsGrid.Tools.Add(insertMenuTool);
//
//
//			// Add some tools to the menu.
//			insertMenuTool.Tools.Add(this.utmMethodsGrid.Tools["btInsertBefore"]);
//			insertMenuTool.Tools.Add(this.utmMethodsGrid.Tools["btInsertAfter"]);
//
//			methodsGridMenuTool.Tools.Add(this.utmMethodsGrid.Tools["mnuInsert"]);
//			methodsGridMenuTool.Tools.Add(this.utmMethodsGrid.Tools["btMoveUp"]);
//			methodsGridMenuTool.Tools.Add(this.utmMethodsGrid.Tools["btMoveDown"]);
		}

		// this causes the context menu to be displayed
//		private void ugMethods_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//		{
//			try
//			{
//				//				if (e.Button == MouseButtons.Right)
//				//					this.utmMethodsGrid.ShowPopup("mnuMethodsGrid", System.Windows.Forms.Cursor.Position);
//			}
//			catch( Exception ex )
//			{
//				HandleException(ex);
//			}
//		}
	}
}
