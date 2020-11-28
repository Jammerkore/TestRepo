--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the audit relieve intransit table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT]
	@PROCESS_RID int,
	@RECS_READ int, 
	@RECS_ACCEPTED int, 
	@RECS_WITH_ERRORS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO AUDIT_RELIEVE_INTRANSIT_INFO
		(
		PROCESS_RID, 
		RECS_READ, 
		RECS_ACCEPTED, 
		RECS_WITH_ERRORS
		)
    VALUES 
		(
		@PROCESS_RID, 
		@RECS_READ, 
		@RECS_ACCEPTED, 
		@RECS_WITH_ERRORS
		)

	SELECT @@ROWCOUNT;

END
GO
