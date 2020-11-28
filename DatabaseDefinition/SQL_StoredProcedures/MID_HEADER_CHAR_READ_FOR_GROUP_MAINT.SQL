--dv =============================================
--dv Create date: 3/18/2015
--dv Description: Reads all possible header characteristic values from a header characteristic group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_READ_FOR_GROUP_MAINT]
	@HCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Header characteristic values for this group
	SELECT 
		hc.HC_RID,
		hcg.HCG_RID,
		hcg.HCG_ID,
		hcg.HCG_LIST_IND,
		hcg.HCG_TYPE,
		CASE 		
			WHEN hcg.HCG_TYPE=1 THEN CONVERT(varchar(50), hc.DATE_VALUE, 101)
			WHEN hcg.HCG_TYPE=2 THEN CONVERT(varchar(50), hc.NUMBER_VALUE)
			WHEN hcg.HCG_TYPE=3 THEN CONVERT(varchar(50), hc.DOLLAR_VALUE)
			ELSE hc.TEXT_VALUE
		END  AS CHAR_VALUE

	FROM HEADER_CHAR_GROUP hcg
	INNER JOIN HEADER_CHAR hc ON hc.HCG_RID = hcg.HCG_RID 
	WHERE hcg.HCG_RID=@HCG_RID AND hcg.HCG_LIST_IND = '1'
	ORDER BY CHAR_VALUE

END

GO


