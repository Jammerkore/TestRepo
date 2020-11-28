--dv =============================================
--dv Create date:	11/29/2013
--dv Description:	Delete Custom OLLRid ForiegnKeys
--dv				from the HEADER and USER_PLAN tables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID]
	@OLL_RID int

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD 
	SET 
		CUSTOM_OLL_RID = null
	WHERE CUSTOM_OLL_RID = @OLL_RID

	UPDATE USER_PLAN 
	SET 
		CUSTOM_OLL_RID = null
	WHERE CUSTOM_OLL_RID = @OLL_RID

	UPDATE OTS_PLAN 
	SET 
		OLL_RID = null
	WHERE OLL_RID = @OLL_RID

	SELECT @@ROWCOUNT;
END
GO









