--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads rows from the audit relieve intransit table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		PROCESS_RID, 
		RECS_READ, 
		RECS_ACCEPTED, 
		RECS_WITH_ERRORS 
	FROM AUDIT_RELIEVE_INTRANSIT_INFO
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
