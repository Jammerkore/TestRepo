using System;
using System.Collections.Generic;
using System.Data; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.DataCommon; 
using MIDRetail.Data;	


namespace Logility.ROWeb
{
    public partial class ROGlobalOptions
    {
        private DataTable GetBasisLabelOrder()
        {
            BasisLabelOrderRowHandler rowHandler = BasisLabelOrderRowHandler.GetInstance();
            DataTable dt = new DataTable("Basis Label Order");

            rowHandler.AddUITableColumns(dt);

            foreach (DataRow drIn in _dtBasisLabels.Rows)
            {
                DataRow dr = dt.NewRow();

                rowHandler.TranslateDBRowToUI(drIn, dr);

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataTable GetBasisLabels()
        {
            DataTable dt = BuildBasisLabelsDataTable();

            if (_GlobalOptionsProfile.Key != Include.NoRID)
            {
                AddBasisLabelsData(dt);
            }

            return dt;
        }

        private DataTable BuildBasisLabelsDataTable()
        {
            DataTable dt = new DataTable("Basis Labels");

            dt.Columns.Add("Checked", typeof(bool));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("ID", typeof(int));

            return dt;
        }

        private void AddBasisLabelsData(DataTable dt)
        {
            var labelSet = GetBasisLabelTypeSet();
            ProfileList labels = GetBasisLabelProfileList();

            foreach (BasisLabelTypeProfile labelType in labels)
            {
                DataRow dr = dt.NewRow();

                dr["Checked"] = labelSet.Contains(labelType.BasisLabelType);
                dr["Name"] = labelType.BasisLabelName;
                dr["ID"] = labelType.Key;

                dt.Rows.Add(dr);
            }
        }

        private void UpdateBasisLabels(GlobalOptions opts, DataTable dtBasisLabelsOrder)
        {
            DeleteAllBasisLabelRows(opts);
            AddBasisLabelRows(opts, dtBasisLabelsOrder);
        }

        private void DeleteAllBasisLabelRows(GlobalOptions opts)
        {
            foreach (Object obj in Enum.GetValues(typeof(eBasisLabelType)))
            {
                int enumVal = Convert.ToInt32(obj);

                opts.GetBasisLabelInfo_Delete(_iSystemOptionsRID, enumVal);
            }
        }

        private void AddBasisLabelRows(GlobalOptions opts, DataTable dtBasisLabelsOrder)
        {
            BasisLabelOrderRowHandler rowHandler = BasisLabelOrderRowHandler.GetInstance();

            // Ignore SeqNum on save - that way it's reset to a range of 0 to N-1.
            for (int iRowIndex = 0; iRowIndex < dtBasisLabelsOrder.Rows.Count; ++iRowIndex)
            {
                DataRow dr = dtBasisLabelsOrder.Rows[iRowIndex];

                rowHandler.ParseUIRow(dr);
                rowHandler.AddDBRow(opts, _iSystemOptionsRID, iRowIndex);
            }
        }

        private HashSet<eBasisLabelType> GetBasisLabelTypeSet()
        {
            BasisLabelOrderRowHandler rowHandler = BasisLabelOrderRowHandler.GetInstance();
            var labelSet = new HashSet<eBasisLabelType>();

            foreach (DataRow drIn in _dtBasisLabels.Rows)
            {
                rowHandler.ParseDBRow(drIn);
                eBasisLabelType basisLabel = (eBasisLabelType)rowHandler.iRID;
                labelSet.Add(basisLabel);
            }

            return labelSet;
        }

        private ProfileList GetBasisLabelProfileList()
        {
            ProfileList basisLabelList = new ProfileList(eProfileType.BasisLabelType);
            Array values = Enum.GetValues(typeof(eBasisLabelType));
            string[] names = Enum.GetNames(typeof(eBasisLabelType));

            for (int i = 0; i < names.Length; i++)
            {
                eBasisLabelType enumVal = (eBasisLabelType) values.GetValue(i);
                BasisLabelTypeProfile bltp = new BasisLabelTypeProfile(Convert.ToInt32(enumVal));

                bltp.BasisLabelName = names[i].Replace("_", " ");
                bltp.BasisLabelType = enumVal;

                basisLabelList.Add(bltp);
            }

            return basisLabelList;
        }

        private class BasisLabelTypeProfile : Profile
        {
            //=======
            // FIELDS
            //=======

            private string _name;
            private eBasisLabelType _type;

            //=============
            // CONSTRUCTORS
            //=============

            /// <summary>
            /// Creates a new instance of BasisLabelTypeProfile.
            /// </summary>
            /// <param name="aKey">
            /// The integer that identifies the logical RID of this coordinate.
            /// </param>
            public BasisLabelTypeProfile(int iKey)
                : base(iKey)
            {
            }

            //===========
            // PROPERTIES
            //===========

            /// <summary>
            /// Returns the eProfileType of this profile.
            /// </summary>
            override public eProfileType ProfileType
            {
                get
                {
                    return eProfileType.BasisLabelType;
                }
            }

            public eBasisLabelType BasisLabelType
            {
                get
                {
                    return _type;
                }
                set
                {
                    _type = value;
                }
            }

            public string BasisLabelName
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }
        }
    }

    public class BasisLabelOrderRowHandler : DBRowHandler
    {
        private static BasisLabelOrderRowHandler _Instance;

        public static BasisLabelOrderRowHandler GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new BasisLabelOrderRowHandler();
            }

            return _Instance;
        }

        private string _sName;
        private int _seqNum;

        private TypedColumnHandler<string> _NameColumnHandler = new TypedColumnHandler<string>("Name", eMIDTextCode.Unassigned, true, string.Empty);
		private TypedDBColumnHandler<int> _SeqNumColumnHandler = new TypedDBColumnHandler<int>("LABEL_SEQ", "SeqNum", eMIDTextCode.Unassigned, false, -1);

        protected BasisLabelOrderRowHandler()
            : base("LABEL_TYPE", "ID", eMIDTextCode.Unassigned)
        {
            _aColumnHandlers = new ColumnHandler[] { _RIDColumnHandler, _NameColumnHandler, _SeqNumColumnHandler };
        }

        public override void TranslateDBRowToUI(DataRow drDB, DataRow drUI)
        {
            ParseDBRow(drDB);
            _RIDColumnHandler.SetUIColumn(drUI, iRID);
            _NameColumnHandler.SetUIColumn(drUI, _sName);
            _SeqNumColumnHandler.SetUIColumn(drUI, _seqNum);
        }

        public override void ParseUIRow(DataRow dr)
        {
            base.ParseDataRow(dr, false);
            _sName = _NameColumnHandler.ParseUIColumn(dr);
            _seqNum = _SeqNumColumnHandler.ParseUIColumn(dr);
        }

        public override void ParseDBRow(DataRow dr)
        {
            base.ParseDataRow(dr, true);

            eBasisLabelType basisLabel = (eBasisLabelType)iRID;

            _sName = basisLabel.ToString().Replace('_', ' ');
            _seqNum = _SeqNumColumnHandler.ParseColumn(dr, true);
        }

        protected override void ParseDataRow(DataRow dr, bool bIsDBRow)
        {
            throw new NotImplementedException();
        }

        public void AddDBRow(GlobalOptions opts, int iSystemOptionsRID, int iRowIndex)
        {
            opts.GetBasisLabelInfo_Insert(iSystemOptionsRID, iRID, iRowIndex);
        }
    }
}
