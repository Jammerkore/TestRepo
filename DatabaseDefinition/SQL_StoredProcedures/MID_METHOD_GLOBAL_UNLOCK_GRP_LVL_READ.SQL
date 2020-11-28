--dv =============================================
--dv Create date:	02/5/2014
--dv Description:	Returns a Group Level Method Global Unlock
--dv History:		9/11/2014 TT#1296-MD -jsobek -Recieve error when trying to open an existing Global Lock Method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COALESCE(SGL_RID, '-1') SGL_RID
    FROM METHOD_GLOBAL_UNLOCK_GRP_LVL 
    WHERE 
		METHOD_RID = @METHOD_RID

END
GO