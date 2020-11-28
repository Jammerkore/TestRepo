--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Clears last entry field on the audit header table based on the header rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_HEADER_UPDATE_LAST_ENTRY]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE AUDIT_HEADER 
	SET 
	LAST_ENTRY=0 
	WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;
END
GO
