--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads all multi-headers along with the link characteristic value.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_MULTI_WITH_LINK_CHAR] 
	@HEADER_LINK_CHARACTERISTIC int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		h.HDR_RID, 
		h.HDR_ID, 
		h.HDR_GROUP_RID, 
		h.InUse, 
		h.MultiHeader,
        COALESCE(hc.HCG_RID, -1) HCG_RID, 
		COALESCE(hc.HC_RID, -1) HC_RID
	FROM VW_GET_HEADERS h 
		 LEFT OUTER JOIN HEADER_CHAR_JOIN hcj ON h.HDR_RID = hcj.HDR_RID
         LEFT OUTER JOIN HEADER_CHAR hc ON hcj.HC_RID = hc.HC_RID AND hc.HCG_RID = @HEADER_LINK_CHARACTERISTIC AND hc.HC_RID = hcj.HC_RID
    WHERE (h.InUse = '1' or h.MultiHeader = '1')
    ORDER BY h.HDR_GROUP_RID 

END
GO
