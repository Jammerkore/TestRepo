--dv =============================================
--dv Create date:	12/04/2013
--dv Description:	Get the method list if it exists.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_LIST]
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
		METHOD_STATUS,
		coalesce(TEMPLATE_IND, '1') as TEMPLATE_IND
	FROM METHOD
	WHERE METHOD_RID > 1

END
GO
