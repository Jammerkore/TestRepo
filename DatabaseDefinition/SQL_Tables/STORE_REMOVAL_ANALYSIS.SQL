--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#739-MD -stodd -delete stores
--dv ============================================= 
CREATE TABLE [dbo].[STORE_REMOVAL_ANALYSIS](
	[ANALYSIS_ID] [int] IDENTITY(1,1) NOT NULL,
	[TABLE_LIST_ID] [int] NOT NULL,
	[TOTAL_ROW_COUNT] [int] NOT NULL,
	[ROWS_TO_DELETE_COUNT] [int] NOT NULL,
	[ROWS_TO_COPY_COUNT]  AS ([TOTAL_ROW_COUNT]-[ROWS_TO_DELETE_COUNT]),
	[PERCENT_TO_DELETE]  AS (case when [TOTAL_ROW_COUNT]=(0) then (0) else CONVERT([float],[ROWS_TO_DELETE_COUNT],0)/CONVERT([float],[TOTAL_ROW_COUNT],0) end),
	[DO_DELETE_PROCESS] [bit] NULL,
	[IN_PROGRESS] [bit] default '0' not NULL,
	[COMPLETED] [bit] default '0' not NULL,
	[STORE_SET_IND] [bit] default 0 not NULL,
	[ANALYSIS_DATE] [datetime] NULL,
 CONSTRAINT [PK_STORE_REMOVAL_ANALYSIS] PRIMARY KEY CLUSTERED 
(
	[ANALYSIS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
