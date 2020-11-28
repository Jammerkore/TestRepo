--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Reads enqueue data for forecast models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_READ_FOR_MODEL]
	@ENQUEUE_TYPE int,
	@RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		me.RID, 
		me.USER_RID,
		CASE au.USER_FULLNAME WHEN '' THEN au.USER_NAME ELSE au.USER_FULLNAME END as USER_FULLNAME
	FROM MID_ENQUEUE me, APPLICATION_USER au
	WHERE ENQUEUE_TYPE = @ENQUEUE_TYPE
		  AND RID = @RID
		  AND me.USER_RID=au.USER_RID

END
GO
