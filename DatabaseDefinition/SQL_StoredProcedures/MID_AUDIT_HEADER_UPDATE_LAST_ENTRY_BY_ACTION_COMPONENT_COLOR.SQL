--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Clears last entry field on the audit header table based on the header rid, component type, and pack/color name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR]
	@HDR_RID int,
	@ACTION_OR_METHOD_LIST ACTION_OR_METHOD_TYPE READONLY,
	@COMPONENT_TYPE int,
	@PACK_OR_COLOR_NAME varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE AUDIT_HEADER 
	SET 
	LAST_ENTRY=0 
	WHERE HDR_RID = @HDR_RID
		  AND 
		  (
			METHOD_TYPE IN (SELECT ACTION_OR_METHOD_VALUE FROM @ACTION_OR_METHOD_LIST)
			OR
			ACTION_TYPE IN (SELECT ACTION_OR_METHOD_VALUE FROM @ACTION_OR_METHOD_LIST)
		  )
		  AND HEADER_COMPONENT_TYPE = @COMPONENT_TYPE AND PACK_OR_COLOR_NAME = @PACK_OR_COLOR_NAME

	SELECT @@ROWCOUNT;

END
GO
