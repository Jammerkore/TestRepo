--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Updates the completed and in progress flags for a table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_REMOVAL_ANALYSIS_UPDATE]
	@TABLE_NAME varchar(4000),
	@COMPLETED bit,
	@IN_PROGRESS bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE STORE_REMOVAL_ANALYSIS 
	SET 
		COMPLETED = @COMPLETED, 
		IN_PROGRESS = @IN_PROGRESS
	WHERE TABLE_LIST_ID IN (SELECT TABLE_LIST_ID FROM STORE_REMOVAL_TABLE_LIST WHERE TABLE_NAME = @TABLE_NAME)

	SELECT @@ROWCOUNT;
END
GO
