--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Reads enqueue data for the product hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_READ_FOR_HIERARCHY]
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HIERARCHY_TYPE int = 5;

	SELECT 
		me.PH_RID, 
		me.USER_RID,
		CASE au.USER_FULLNAME WHEN '' THEN au.USER_NAME ELSE au.USER_FULLNAME END as USER_FULLNAME
		FROM MID_ENQUEUE me, APPLICATION_USER au 
		WHERE ENQUEUE_TYPE = @HIERARCHY_TYPE
			  AND PH_RID = @PH_RID
			  AND me.USER_RID=au.USER_RID

END
GO
