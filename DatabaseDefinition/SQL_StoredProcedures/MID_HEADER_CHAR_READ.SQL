--dv =============================================
--dv Create date:	1/20/2014
--dv Description:	Reads characteristics for the given header rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_READ]
	@HDR_RID int
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
	FROM HEADER_CHAR_JOIN hcj, 
		 HEADER_CHAR hc
	WHERE hcj.HDR_RID = @HDR_RID
		  AND hc.HC_RID = hcj.HC_RID
	ORDER BY hcj.HDR_RID, hcj.HC_RID

END
GO