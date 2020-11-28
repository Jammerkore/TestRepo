--dv =============================================
--dv Created date: 11/5/2014
--dv Notes:	Stores entries for executing custom conversion code during a db upgrade
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[APPLICATION_DB_CONVERSION_QUEUE](
	[QUEUE_RID] [int] IDENTITY(1,1) NOT NULL,
	[CC_FUNCTION_NAME] [varchar](50) NOT NULL,
	[IS_DEFERRED] [bit] NOT NULL
)
GO


