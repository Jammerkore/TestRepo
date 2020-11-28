using System;
using System.Data;
using System.IO;
using System.Globalization;

using Infragistics.Win.UltraWinGrid;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public partial class InfragisticsLayoutData : DataLayer
	{
		public InfragisticsLayoutData() : base()
		{
		}

		public InfragisticsLayoutData(string aConnectionString)
			: base(aConnectionString)
	    {
	    }

		/// <summary>
		/// Read an Infragistics layout from the database
		/// </summary>
		/// <param name="aUserRID">The record ID of the user</param>
		/// <param name="aLayoutID">The eLayoutID of the layout you want to read</param>
		/// <returns>An instance of the InfragisticsLayout class containing the length of the layout and the layout</returns>
		public InfragisticsLayout InfragisticsLayout_Read(int aUserRID, eLayoutID aLayoutID)
		{
			try
			{
				InfragisticsLayout gl = new InfragisticsLayout();
                DataTable dt = StoredProcedures.MID_INFRAGISTICS_LAYOUTS_READ.Read(_dba,
                                                                                   USER_RID: aUserRID,
                                                                                   LAYOUT_ID: (int)aLayoutID
                                                                                   );
				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
					gl.LayoutLength = Convert.ToInt32(dr["LAYOUT_SIZE"]);
					byte[] layoutBytes = new byte[gl.LayoutLength];
					object x = dr["LAYOUT_CONTENT"];
					layoutBytes = (byte[])dr["LAYOUT_CONTENT"];
					gl.LayoutStream = new MemoryStream(layoutBytes);
				}
				return gl;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Saves an Infragistics UltraGrid layout to the database.  This method will determine if the layout is to
		/// be inserted or updated
		/// </summary>
		/// <param name="aUserRID">The record ID of the user</param>
		/// <param name="aLayoutID">The eLayoutID of the layout you want to save</param>
		/// <param name="aGrid">The grid whose layout is to be saved.</param>
		public void InfragisticsLayout_Save(int aUserRID, eLayoutID aLayoutID, Infragistics.Win.UltraWinGrid.UltraGrid aGrid)
		{
			try
			{
				MemoryStream layoutStream = new MemoryStream(); 
				if (aGrid.DisplayLayout != null)
				{
					aGrid.DisplayLayout.Save(layoutStream, Infragistics.Win.UltraWinGrid.PropertyCategories.All);
					OpenUpdateConnection();
                    int count = StoredProcedures.MID_INFRAGISTICS_LAYOUTS_READ_COUNT.ReadRecordCount(_dba,
                                                                                                     USER_RID: aUserRID,
                                                                                                     LAYOUT_ID: (int)aLayoutID
                                                                                                     );
					if (count == 0)
					{
						InfragisticsLayout_Add(aUserRID, aLayoutID, layoutStream);
					}
					else
					{
						InfragisticsLayout_Update(aUserRID, aLayoutID, layoutStream);
					}
					CommitData();
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// This method saves layouts of non-UltraGrid Infragistics controls.
		/// </summary>
		/// <param name="aUserRID">The record ID of the user</param>
		/// <param name="aLayoutID">The eLayoutID of the layout you want to save</param>
		/// <param name="aLayoutStream">A Memory stream containing the layout</param>
		public void InfragisticsLayout_Save(int aUserRID, eLayoutID aLayoutID, MemoryStream aLayoutStream)
		{
			try
			{
				OpenUpdateConnection();
                int count = StoredProcedures.MID_INFRAGISTICS_LAYOUTS_READ_COUNT.ReadRecordCount(_dba,
                                                                                                     USER_RID: aUserRID,
                                                                                                     LAYOUT_ID: (int)aLayoutID
                                                                                                     );
				if (count == 0)
				{
					InfragisticsLayout_Add(aUserRID, aLayoutID, aLayoutStream);
				}
				else
				{
					InfragisticsLayout_Update(aUserRID, aLayoutID, aLayoutStream);
				}
				CommitData();

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}

		private void InfragisticsLayout_Add(int aUserRID, eLayoutID aLayoutID, MemoryStream aLayoutStream)
		{
			try
			{
				aLayoutStream.Position = 0;
				byte[] layoutBytes = new byte[aLayoutStream.Length];
				int streamLength = Convert.ToInt32(aLayoutStream.Length);
				aLayoutStream.Read(layoutBytes, 0, streamLength);

                StoredProcedures.MID_INFRAGISTICS_LAYOUTS_INSERT.Insert(_dba,
                                                                        USER_RID: aUserRID,
                                                                        LAYOUT_ID: (int)aLayoutID,
                                                                        LAYOUT_SIZE: (int)aLayoutStream.Length,
                                                                        LAYOUT_CONTENT: layoutBytes
                                                                        );

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void InfragisticsLayout_Update(int aUserRID, eLayoutID aLayoutID, MemoryStream aLayoutStream)
		{
			try
			{
				aLayoutStream.Position = 0;
				byte[] layoutBytes = new byte[aLayoutStream.Length];
				aLayoutStream.Read(layoutBytes, 0, (int)aLayoutStream.Length);

                StoredProcedures.MID_INFRAGISTICS_LAYOUTS_UPDATE.Update(_dba,
                                                                        USER_RID: aUserRID,
                                                                        LAYOUT_ID: (int)aLayoutID,
                                                                        LAYOUT_SIZE: (int)aLayoutStream.Length,
                                                                        LAYOUT_CONTENT: layoutBytes
                                                                        );

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Deletes and Infragistics control layout from the database
		/// </summary>
		/// <param name="aUserRID">The record ID of the user</param>
		/// <param name="aLayoutID">The eLayoutID of the layout you want to delete</param>
		public void InfragisticsLayout_Delete(int aUserRID, eLayoutID aLayoutID)
		{
			try
			{
				OpenUpdateConnection();
                StoredProcedures.MID_INFRAGISTICS_LAYOUTS_DELETE.Delete(_dba,
                                                                        USER_RID: aUserRID,
                                                                        LAYOUT_ID: (int)aLayoutID
                                                                        );
				CommitData();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Deletes and Infragistics control layout from the database
		/// </summary>
		/// <param name="aLayoutID">The eLayoutID of the layout you want to delete</param>
		public void InfragisticsLayout_Delete(eLayoutID aLayoutID)
		{
			try
			{
				OpenUpdateConnection();
                StoredProcedures.MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT.Delete(_dba, LAYOUT_ID: (int)aLayoutID);
				CommitData();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}

        //Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        /// <summary>
        /// List of Infragistics layouts from the database by userID
        /// </summary>
        /// <param name="aUserRID">The record ID of the user</param>
        /// <returns>An list of InfragisticsLayouts class containing the length of the layout and the layout</returns>
        public DataTable InfragisticsUserLayout_Read(int aUserRID)
        {
            try
            {
                InfragisticsLayout gl = new InfragisticsLayout();
                DataTable dt = StoredProcedures.MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER.Read(_dba, USER_RID: aUserRID);

                return dt;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Deletes all Infragistics control layouts from the database by userID
        /// </summary>
        /// <param name="aUserRID">The record ID of the user</param>
        /// <param name="aLayoutID">The eLayoutID of the layout you want to delete</param>
        public int InfragisticsUserLayout_Delete(int aUserRID)
        {
            int retRows;
            try
            {
                OpenUpdateConnection();
                retRows = StoredProcedures.MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER.Delete(_dba, USER_RID: aUserRID);

                CommitData();
                return retRows;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                Rollback();
                throw;
            }
            finally
            {
                if (ConnectionIsOpen)
                {
                    CloseUpdateConnection();
                }
            }
        }

        //End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts

	}

	public class InfragisticsLayout
	{
		private int _layoutLength = 0;
		private MemoryStream _layoutStream;

		/// <summary>
		/// Gets the length of the grid layout.
		/// </summary>
		public int LayoutLength 
		{
			get { return _layoutLength ; }
			set { _layoutLength = value ; }
		}

		/// <summary>
		/// Gets the grid layout.
		/// </summary>
		public MemoryStream LayoutStream 
		{
			get { return _layoutStream ; }
			set { _layoutStream = value ; }
		}

		public InfragisticsLayout()
		{
			
		}
	}
}
