--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Updates rows for the calendar model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_UPDATE] 
	@CM_RID int,
	@CM_ID varchar(50),
	@START_DATE datetime,
	@FISCAL_YEAR int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE CALENDAR_MODEL 
	SET 
		CM_ID = @CM_ID, 
		START_DATE = @START_DATE,
        FISCAL_YEAR = @FISCAL_YEAR
    WHERE CM_RID = @CM_RID

	SELECT @@ROWCOUNT;
END
GO