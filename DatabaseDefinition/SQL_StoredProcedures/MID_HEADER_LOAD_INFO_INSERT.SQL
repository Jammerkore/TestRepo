--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the header load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_LOAD_INFO_INSERT]
	@PROCESS_RID int,
	@RECS_READ int, 
	@RECS_WITH_ERRORS int,
	@HDRS_CREATED int, 
	@HDRS_MODIFIED int, 
	@HDRS_REMOVED int, 
	@HDRS_RESET int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO HEADER_LOAD_INFO
		(
		PROCESS_RID, 
		RECS_READ, 
		RECS_WITH_ERRORS,
		HDRS_CREATED, 
		HDRS_MODIFIED, 
		HDRS_REMOVED, 
		HDRS_RESET
		) 
	VALUES
		(
		@PROCESS_RID, 
		@RECS_READ, 
		@RECS_WITH_ERRORS,
		@HDRS_CREATED, 
		@HDRS_MODIFIED, 
		@HDRS_REMOVED, 
		@HDRS_RESET
		) 	

	SELECT @@ROWCOUNT;

END
GO
