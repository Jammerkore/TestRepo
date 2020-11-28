--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[DETAIL_INFO](
	[APPLICATION_LABEL_TYPE] [int] NOT NULL,
	[SEQUENCE] [int] NOT NULL,
	[SEVERITY] [char](10) NOT NULL,
	[REFERENCE_TEXT] [varchar](4000) NOT NULL,
CONSTRAINT [PK_DETAIL_INFO] PRIMARY KEY CLUSTERED 
(
	[APPLICATION_LABEL_TYPE] ASC,
	[SEQUENCE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An enum representing the type of query this is.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DETAIL_INFO', @level2type=N'COLUMN',@level2name=N'APPLICATION_LABEL_TYPE'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the necessity of removing this reference if one wants to delete.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DETAIL_INFO', @level2type=N'COLUMN',@level2name=N'SEVERITY'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The parameterized query needed to get the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DETAIL_INFO', @level2type=N'COLUMN',@level2name=N'REFERENCE_TEXT'

GO
