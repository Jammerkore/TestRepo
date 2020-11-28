--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Updates periods for the calendar model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_PERIODS_UPDATE] 
	@CM_RID int,
	@CMP_SEQUENCE int,
	@CMP_TYPE int,
	@CMP_ID varchar(50),
	@CMP_ABBREVIATION varchar(10),
	@NO_OF_TIME_PERIODS int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE CALENDAR_MODEL_PERIODS 
	SET 
		CMP_ID = @CMP_ID,
		CMP_ABBREVIATION = @CMP_ABBREVIATION, 
		NO_OF_TIME_PERIODS = @NO_OF_TIME_PERIODS
	WHERE CM_RID = @CM_RID 
		  AND CMP_SEQUENCE = @CMP_SEQUENCE 
		  AND CMP_TYPE = @CMP_TYPE

	SELECT @@ROWCOUNT;

END
GO