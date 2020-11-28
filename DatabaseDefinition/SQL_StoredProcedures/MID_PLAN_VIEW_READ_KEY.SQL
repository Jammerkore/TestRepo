--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Reads a plan view key for the user and view id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_READ_KEY]
	@USER_RID int,
	@VIEW_ID varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM PLAN_VIEW 
	WHERE USER_RID = @USER_RID 
		AND VIEW_ID = @VIEW_ID

END
GO