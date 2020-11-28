--dv =============================================
--dv Create date:	10/17/2014
--dv Description:	Reads characteristics for a list of header rids
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_READ_FOR_HEADER_LIST]
	@HDR_RID_LIST HDR_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		hcj.HDR_RID, 
		hcj.HC_RID,
		hc.HCG_RID, 
		hc.TEXT_VALUE, 
		hc.DATE_VALUE, 
		hc.NUMBER_VALUE, 
		hc.DOLLAR_VALUE
	FROM HEADER_CHAR_JOIN hcj 
		 INNER JOIN HEADER_CHAR hc ON hc.HC_RID = hcj.HC_RID
	WHERE hcj.HDR_RID IN (SELECT HDR_RID FROM @HDR_RID_LIST)
	ORDER BY hcj.HDR_RID, hcj.HC_RID

END
GO