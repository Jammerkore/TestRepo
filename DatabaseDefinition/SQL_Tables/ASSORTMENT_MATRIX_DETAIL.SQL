--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[ASSORTMENT_MATRIX_DETAIL] (
	[HDR_RID] [int] NOT NULL,
	[HDR_PACK_RID] [int] NULL CONSTRAINT [DF_ASSORTMENT_MATRIX_DETAIL_HDR_PACK_RID]  DEFAULT ((-1)),
	[COLOR_CODE_RID] [int] NULL CONSTRAINT [DF_ASSORTMENT_MATRIX_DETAIL_COLOR_CODE_RID]  DEFAULT ((-1)),
	[SGL_RID] [int] NOT NULL CONSTRAINT [DF_ASSORTMENT_MATRIX_DETAIL_SGL_RID]  DEFAULT ((-1)),
	[GRADE] [int] NULL,
    [UNITS] [int] NOT NULL,
    [LOCKED] [char] NOT NULL,
	[AVERAGE_UNITS] [float] default 0 NOT NULL)
GO
