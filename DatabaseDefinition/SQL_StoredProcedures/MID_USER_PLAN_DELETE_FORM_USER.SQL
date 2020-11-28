--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Deletes a user plan based on the user provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_DELETE_FORM_USER] 
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM USER_PLAN 
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;

END
GO