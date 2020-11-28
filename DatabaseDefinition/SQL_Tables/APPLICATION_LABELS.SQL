--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
CREATE TABLE [dbo].[APPLICATION_LABELS](
	[APPLICATION_LABEL_TYPE] [int] NOT NULL,
	[SEQUENCE] [int] NOT NULL,
	[APPLICATION_LABEL_HEADING] [int] NOT NULL,
CONSTRAINT [PK_APPLICATION_LABELS] PRIMARY KEY CLUSTERED 
(
	[APPLICATION_LABEL_TYPE] ASC,
	[SEQUENCE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An enum representing the type of query this is.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APPLICATION_LABELS', @level2type=N'COLUMN',@level2name=N'APPLICATION_LABEL_TYPE'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order these heading should be ordered in.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APPLICATION_LABELS', @level2type=N'COLUMN',@level2name=N'SEQUENCE'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The soft text number used to get the lable for this column heading.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'APPLICATION_LABELS', @level2type=N'COLUMN',@level2name=N'APPLICATION_LABEL_HEADING'

GO
