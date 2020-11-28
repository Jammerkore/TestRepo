--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Clears last entry field on the audit header table based on the header rid and component type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE]
	@HDR_RID int,
	@COMPONENT_TYPE1 int,
	@COMPONENT_TYPE2 int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE AUDIT_HEADER 
	SET 
	LAST_ENTRY=0 
	WHERE HDR_RID = @HDR_RID
		  AND (HEADER_COMPONENT_TYPE = @COMPONENT_TYPE1 OR HEADER_COMPONENT_TYPE = @COMPONENT_TYPE2)

	SELECT @@ROWCOUNT;

END
GO
