--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the color code load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_LOAD_INFO_INSERT]
	@PROCESS_RID int,
	@RECS_READ int, 
	@RECS_WITH_ERRORS int,
	@CODES_CREATED int, 
	@CODES_MODIFIED int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO COLOR_CODE_LOAD_INFO
		(
		PROCESS_RID, 
		RECS_READ, 
		RECS_WITH_ERRORS,
		CODES_CREATED, 
		CODES_MODIFIED
		) 
	VALUES
		(
		@PROCESS_RID, 
		@RECS_READ, 
		@RECS_WITH_ERRORS,
		@CODES_CREATED, 
		@CODES_MODIFIED
		) 

	SELECT @@ROWCOUNT;

END
GO
