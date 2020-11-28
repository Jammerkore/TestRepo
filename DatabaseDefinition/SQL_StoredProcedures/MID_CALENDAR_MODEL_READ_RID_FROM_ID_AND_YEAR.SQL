--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads calendar model rid based on the calendar model id and fiscal year
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR]
	@CM_ID varchar(50),
	@FISCAL_YEAR int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CM_RID 
	FROM CALENDAR_MODEL 
	WHERE CM_ID = @CM_ID
		  AND FISCAL_YEAR = @FISCAL_YEAR

END
GO
