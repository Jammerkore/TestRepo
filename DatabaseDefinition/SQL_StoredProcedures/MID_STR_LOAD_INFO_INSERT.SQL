--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Inserts a row into the store load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STR_LOAD_INFO_INSERT]
	@PROCESS_RID int,
	@STORE_RECS int,
	@STORES_CREATED int,
	@STORES_MODIFIED int, 
	@STORES_DELETED int, 
	@STORES_RECOVERED int, 
	@RECS_WITH_ERRORS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO STR_LOAD_INFO
		(
		PROCESS_RID, 
		STORE_RECS, 
		STORES_CREATED, 
		STORES_MODIFIED, 
		STORES_DELETED, 
		STORES_RECOVERED, 
		RECS_WITH_ERRORS
		)
	VALUES
		(
		@PROCESS_RID, 
		@STORE_RECS, 
		@STORES_CREATED, 
		@STORES_MODIFIED, 
		@STORES_DELETED, 
		@STORES_RECOVERED, 
		@RECS_WITH_ERRORS
		)

	SELECT @@ROWCOUNT;

END
GO
