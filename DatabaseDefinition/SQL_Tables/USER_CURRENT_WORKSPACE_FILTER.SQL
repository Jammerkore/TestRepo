--dv =============================================
--dv Modified date: 10/16/2014
--dv Notes:	Cascade delete set for both user and filter, holds the current selected filter for the AWS and SWS
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[USER_CURRENT_WORKSPACE_FILTER] (
	"CURRENT_FILTER_RID" int IDENTITY(1,1) NOT NULL,
	"WORKSPACE_TYPE" int NOT NULL,
	"USER_RID" int NOT NULL,
	"FILTER_RID" int NOT NULL)
GO
