--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads header characteristic lists
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_READ_LIST] 
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		hc.HC_RID, 
		hc.HCG_RID, 
		hc.TEXT_VALUE,
		hc.DATE_VALUE, 
		hc.NUMBER_VALUE, 
		hc.DOLLAR_VALUE
	FROM HEADER_CHAR hc, 
		 HEADER_CHAR_GROUP hcg
	WHERE hc.HCG_RID = hcg.HCG_RID
		  AND hcg.HCG_LIST_IND = '1'
	ORDER BY HCG_RID, HC_RID

END
GO