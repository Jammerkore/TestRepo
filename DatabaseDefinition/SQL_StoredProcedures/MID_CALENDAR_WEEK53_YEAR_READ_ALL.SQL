--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads all rows from the CALENDAR_WEEK53 table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_WEEK53_YEAR_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		WEEK53_FISCAL_YEAR, 
		CM_RID, 
		CMP_SEQUENCE, 
		OFFSET_ID 
	FROM CALENDAR_WEEK53_YEAR

END
GO
