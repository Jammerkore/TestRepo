--dv =============================================
--dv Modified date: 12/9/2014
--dv Notes:
--dv Notes:		
--dv History: 12/9/2014 TT#1413-MD-stodd-Data Layer Request - SIZE_CURVE_LOAD_INFO
--dv ============================================= 
CREATE TABLE [dbo].[SIZE_CURVE_LOAD_INFO](
	 [PROCESS_RID] [int] NOT NULL
	,[CURVES_READ] [int] NULL
	,[CURVES_WITH_ERRORS] [int] NULL
	,[CURVES_CREATED] [int] NULL
	,[CURVES_MODIFIED] [int] NULL
	,[CURVES_REMOVED] [int] NULL
	,[GROUPS_READ] [int] NULL
	,[GROUPS_WITH_ERRORS] [int] NULL
	,[GROUPS_CREATED] [int] NULL
	,[GROUPS_MODIFIED] [int] NULL
	,[GROUPS_REMOVED] [int] NULL
) ON 'AUDIT' 

GO
