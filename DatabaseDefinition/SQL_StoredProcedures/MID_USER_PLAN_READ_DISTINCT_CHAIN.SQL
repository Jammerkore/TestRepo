--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Returns a distinct User 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_READ_DISTINCT_CHAIN]
	@CHAIN_HN_RID int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		au.USER_RID, 
		au.USER_NAME
    FROM USER_PLAN up, 
		 APPLICATION_USER au 
	WHERE up.CHAIN_HN_RID = @CHAIN_HN_RID 
		  AND up.USER_RID = au.USER_RID

END
GO