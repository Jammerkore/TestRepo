--dv =============================================
--dv Create date:	12/03/2013
--dv Description:	Get the method list from type and user if it exist.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_LIST_FROM_TYPE_AND_USER]
	@METHOD_TYPE_ID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID int = 4;

	SELECT 
		METHOD_RID,
		METHOD_NAME,
		METHOD_TYPE_ID,
		USER_RID,
		METHOD_DESCRIPTION,
		SG_RID,
		VIRTUAL_IND,
		METHOD_STATUS,
		coalesce(TEMPLATE_IND, '1') as TEMPLATE_IND
	FROM METHOD
	WHERE METHOD_TYPE_ID = @METHOD_TYPE_ID
		AND METHOD_RID > 1
		AND USER_RID IN (@USER_RID, @GlobalUserRID)

END
GO

