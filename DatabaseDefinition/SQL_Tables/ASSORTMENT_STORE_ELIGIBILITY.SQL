--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
CREATE TABLE [dbo].[ASSORTMENT_STORE_ELIGIBILITY](
	[HDR_RID] [int] NOT NULL,
	[ST_RID] [int] NOT NULL,
	[ELIGIBLE] [char](1) NOT NULL,
 CONSTRAINT [PK_ASSORTMENT_STORE_ELIGIBILITY] PRIMARY KEY CLUSTERED 
(
	[HDR_RID] ASC,
	[ST_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
