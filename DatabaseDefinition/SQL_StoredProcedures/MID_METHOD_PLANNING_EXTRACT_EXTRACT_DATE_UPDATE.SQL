--dv =============================================
--dv Create date:	2/23/2018
--dv Description:	Updates a row into the method planning extract table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE]
	@METHOD_RID int,
	@EXTRACT_DATE datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD_PLANNING_EXTRACT 
	SET
		EXTRACT_DATE = @EXTRACT_DATE 
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT

END
GO