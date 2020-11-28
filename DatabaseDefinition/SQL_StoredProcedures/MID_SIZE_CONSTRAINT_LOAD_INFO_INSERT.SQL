--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the size constraint load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT]
	@PROCESS_RID int,
	@RECS_READ int, 
	@RECS_WITH_ERRORS int,
	@MODELS_CREATED int, 
	@MODELS_MODIFIED int, 
	@MODELS_REMOVED int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SIZE_CONSTRAINT_LOAD_INFO
		(
		PROCESS_RID, 
		RECS_READ, 
		RECS_WITH_ERRORS,
		MODELS_CREATED, 
		MODELS_MODIFIED, 
		MODELS_REMOVED
		) 
	VALUES
		(
		@PROCESS_RID, 
		@RECS_READ, 
		@RECS_WITH_ERRORS,
		@MODELS_CREATED, 
		@MODELS_MODIFIED, 
		@MODELS_REMOVED
		)

	SELECT @@ROWCOUNT;

END
GO
