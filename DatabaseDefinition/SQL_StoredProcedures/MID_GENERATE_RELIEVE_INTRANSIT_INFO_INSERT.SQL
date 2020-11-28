--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the generate relieve intransit info table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT]
	@PROCESS_RID int,
	@HEADERS_TO_RELIEVE int, 
	@FILES_GENERATED int,
	@TOTAL_ERRORS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO GENERATE_RELIEVE_INTRANSIT_INFO
		(
		PROCESS_RID, 
		HEADERS_TO_RELIEVE, 
		FILES_GENERATED,
		TOTAL_ERRORS
		) 
	VALUES
		(
		@PROCESS_RID, 
		@HEADERS_TO_RELIEVE, 
		@FILES_GENERATED,
		@TOTAL_ERRORS
		)
	
	SELECT @@ROWCOUNT;

END
GO
