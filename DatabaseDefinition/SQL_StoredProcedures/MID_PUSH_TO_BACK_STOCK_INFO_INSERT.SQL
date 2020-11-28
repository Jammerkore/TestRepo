--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the push to back stock info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PUSH_TO_BACK_STOCK_INFO_INSERT]
	@PROCESS_RID int,
	@HDRS_READ int, 
	@HDRS_WITH_ERRORS int,
	@HDRS_PROCESSED int, 
	@HDRS_SKIPPED int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PUSH_TO_BACK_STOCK_INFO
		(
		PROCESS_RID, 
		HDRS_READ, 
		HDRS_WITH_ERRORS,
		HDRS_PROCESSED, 
		HDRS_SKIPPED
		) 
	VALUES
		(
		@PROCESS_RID, 
		@HDRS_READ, 
		@HDRS_WITH_ERRORS,
		@HDRS_PROCESSED, 
		@HDRS_SKIPPED
		)

	SELECT @@ROWCOUNT;

END
GO
