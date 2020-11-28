--dv =============================================
--dv Create date:	12/03/2013
--dv Description:	Get the method list from type if it exists.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_LIST_FROM_TYPE]
	@METHOD_TYPE_ID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		METHOD_RID,
		METHOD_NAME,
		METHOD_TYPE_ID,
		USER_RID,
		METHOD_DESCRIPTION,
		SG_RID,
		VIRTUAL_IND,
		METHOD_STATUS
	FROM METHOD
	WHERE METHOD_TYPE_ID = @METHOD_TYPE_ID
		AND METHOD_RID > 1

END
GO