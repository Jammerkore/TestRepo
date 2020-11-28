--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads all header characteristics
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_READ_ALL] 
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
	ORDER BY HCG_RID, HC_RID

END
GO