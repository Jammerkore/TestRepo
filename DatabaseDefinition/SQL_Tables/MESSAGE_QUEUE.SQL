--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#2307 -JSmith -Incorrect Stock Values
--dv =============================================
CREATE TABLE [dbo].[MESSAGE_QUEUE](
	[MESSAGE_RID] [int] IDENTITY(1,1) NOT NULL,
	[MESSAGE_DATETIME] [datetime] NOT NULL,
	[MESSAGE_TO] [int] NOT NULL,
	[MESSAGE_FROM] [int] NULL,
	[MESSAGE_CODE] [int] NOT NULL,
	[MESSAGE_PROCESSING_PRIORITY] [int] NOT NULL,
	[MESSAGE_DETAILS] [varchar](5000) NULL
 CONSTRAINT [PK_MESSAGE_QUEUE] PRIMARY KEY CLUSTERED 
(
	[MESSAGE_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
