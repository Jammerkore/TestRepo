using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetail.DataCommon
{
    //public enum PlanChainLadderViewGridComponentAppearances
    //{
    //    Normal,
    //    Ineligible,
    //    Locked,
    //    Negative,
    //    NegativeEditable,
    //    Editable
    //}
    //public class PlanChainLadderViewGridCellComponent
    //{
    //    public PlanChainLadderViewGridComponentAppearances cellAppearance = PlanChainLadderViewGridComponentAppearances.Normal;
    //    public bool canEditCell = false;
    //}
    //public class PlanChainLadderViewRowComponent
    //{
    //    public List<PlanChainLadderViewGridCellComponent> columnComponentList = new List<PlanChainLadderViewGridCellComponent>();
    //}
    //public class PlanChainLadderViewGridComponent
    //{
    //    public List<PlanChainLadderViewRowComponent> rowComponentList = new List<PlanChainLadderViewRowComponent>();
    //}
    //public delegate void PlanChainLadderResolveSingleCellFromComponentDelegate(int rowIndex, int columnIndex, PlanChainLadderViewGridCellComponent c);
    public delegate bool IsChainLadderCellValueNegative(int rowIndex, int columnIndex);
    public delegate bool IsChainLadderCellIneligible(int rowIndex, int columnIndex);
    public delegate bool IsChainLadderCellLocked(int rowIndex, int columnIndex);
    public delegate bool IsChainLadderCellEditable(int rowIndex, int columnIndex);
    public delegate bool IsChainLadderCellBasis(int rowIndex, int columnIndex);
    //public delegate string GetCellDisplayFormattedValue(int rowIndex, int columnIndex, double unformattedValue);
    
    public delegate List<int> GetSelectedHeaderRIDsFromWorkspace();


    //public class BasisToolTip
    //{
    //    public int columnIndex;
    //    public string toolTip;
    //    public string basisHeaderName;
    //    public int basisHeaderSequence;
    //    public bool isDisplayed = true;
    //    public BasisToolTip(int columnIndex, string toolTip, string basisHeaderName, int basisHeaderSequence)
    //    {
    //        this.columnIndex = columnIndex;
    //        this.toolTip = toolTip;
    //        this.basisHeaderName = basisHeaderName;
    //        this.basisHeaderSequence = basisHeaderSequence;
    //    }
 
    //}

}
