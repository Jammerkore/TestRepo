--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the store eligibility criteria load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT]
	@PROCESS_RID int,
	@RECS_READ int, 
	@RECS_WITH_ERRORS int, 
	@CRITERIA_ADDED_UPDATED int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO STORE_ELIGIBILITY_CRITERIA_LOAD_INFO
		(
		PROCESS_RID, 
		RECS_READ, 
		RECS_WITH_ERRORS, 
		CRITERIA_ADDED_UPDATED
		) 
	VALUES
		(
		@PROCESS_RID, 
		@RECS_READ, 
		@RECS_WITH_ERRORS, 
		@CRITERIA_ADDED_UPDATED
		) 

	SELECT @@ROWCOUNT;

END
GO
