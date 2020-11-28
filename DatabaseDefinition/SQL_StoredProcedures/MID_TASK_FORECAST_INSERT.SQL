--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a forecast task
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_INSERT]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int, 
	@FORECAST_SEQUENCE int, 
	@HN_RID int,
	@FV_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO TASK_FORECAST
		(
		TASKLIST_RID, 
		TASK_SEQUENCE, 
		FORECAST_SEQUENCE, 
		HN_RID, 
		FV_RID
		)
	VALUES
		(
		@TASKLIST_RID, 
		@TASK_SEQUENCE, 
		@FORECAST_SEQUENCE, 
		@HN_RID,
		@FV_RID
		)

	SELECT @@ROWCOUNT;

END
GO