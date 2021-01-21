--dv =============================================
--dv Create date:	12/04/2013
--dv Description:	Get the method list from type, user and name if it exists.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME]
	@METHOD_NAME varchar(50),
	@METHOD_TYPE_ID int,
	@USER_RID int
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
	WHERE METHOD_NAME = @METHOD_NAME
		AND METHOD_TYPE_ID = @METHOD_TYPE_ID
		AND METHOD_RID > 1
		AND USER_RID = @USER_RID

END
GO