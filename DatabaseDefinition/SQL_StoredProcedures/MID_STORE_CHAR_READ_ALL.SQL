--dv =============================================
--dv Create date: 12/18/2013
--dv Description: Get all store characteristic values.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SC.SC_RID,
		SC.SCG_RID,
		SC.TEXT_VALUE,
		SC.DATE_VALUE,
		SC.NUMBER_VALUE,
		SC.DOLLAR_VALUE
	FROM STORE_CHAR SC
	ORDER BY SCG_RID, SC_RID

END
GO


