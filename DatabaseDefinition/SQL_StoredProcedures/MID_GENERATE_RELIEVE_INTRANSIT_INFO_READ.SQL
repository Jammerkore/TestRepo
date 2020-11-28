--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads rows from the generate relieve intransit info table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		PROCESS_RID, 
		COALESCE(HEADERS_TO_RELIEVE,0) HEADERS_TO_RELIEVE,
        COALESCE(FILES_GENERATED,0) FILES_GENERATED,
		COALESCE(TOTAL_ERRORS,0) TOTAL_ERRORS
    FROM GENERATE_RELIEVE_INTRANSIT_INFO
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
