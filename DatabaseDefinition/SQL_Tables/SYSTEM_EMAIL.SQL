--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
--dv ============================================= 
CREATE TABLE [dbo].[SYSTEM_EMAIL](
	[SYSTEM_EMAIL_RID] [int] IDENTITY(1,1) NOT NULL,
	[SYSTEM_EMAIL_FILTER] [varchar](4000) NOT NULL,
	[SYSTEM_EMAIL_FROM] [varchar](500) NULL,
	[SYSTEM_EMAIL_TO] [varchar](500) NULL,
	[SYSTEM_EMAIL_CC] [varchar](500) NULL,
	[SYSTEM_EMAIL_BCC] [varchar](500) NULL,
	[SYSTEM_EMAIL_SUBJECT] [varchar](500) NULL,
	[SYSTEM_EMAIL_BODY] [varchar](4000) NULL,
	[SYSTEM_EMAIL_ATTACHMENT_FILENAME] [varchar](255) NULL,
	[CREATED_BY_USER_RID] [int] NOT NULL,
	[CREATED_DATETIME] [datetime] NOT NULL,
	[LAST_MODIFIED_BY_USER_RID] [int] NOT NULL,
	[LAST_MODIFIED_DATETIME] [datetime] NOT NULL,
 CONSTRAINT [PK_SYSTEM_EMAIL] PRIMARY KEY CLUSTERED 
(
	[SYSTEM_EMAIL_RID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
