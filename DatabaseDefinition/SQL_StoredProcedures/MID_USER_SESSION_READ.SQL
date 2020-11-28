--dv =============================================
--dv Create date: 12/13/2013
--dv Description: Reads user sessions based on user and status
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_SESSION_READ]
	@USER_RID int,
	@SESSION_STATUS int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM USER_SESSION 
	WHERE USER_RID = @USER_RID 
		  AND SESSION_STATUS = @SESSION_STATUS

END
GO