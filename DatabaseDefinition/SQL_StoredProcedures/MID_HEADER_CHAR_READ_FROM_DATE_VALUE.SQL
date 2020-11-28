--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads header characteristics
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_READ_FROM_DATE_VALUE] 
	@HCG_RID INT,
	@DATE_VALUE DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		HC_RID, 
		HCG_RID, 
		TEXT_VALUE,
		DATE_VALUE, 
		NUMBER_VALUE, 
		DOLLAR_VALUE
	FROM HEADER_CHAR
	WHERE HCG_RID = @HCG_RID
		  AND DATE_VALUE = @DATE_VALUE
	ORDER BY HCG_RID, HC_RID

END
GO