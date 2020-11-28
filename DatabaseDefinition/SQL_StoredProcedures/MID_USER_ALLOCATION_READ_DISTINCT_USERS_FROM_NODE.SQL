--dv =============================================
--dv Create date:	10/30/2013
--dv Description:	Reads distinct users from the user allocation table based on the hierarchy node rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		au.USER_RID, 
		au.USER_NAME
	FROM USER_ALLOCATION ua, 
		 APPLICATION_USER au
	WHERE ua.HN_RID = @HN_RID
		  AND ua.USER_RID = au.USER_RID

END
GO
